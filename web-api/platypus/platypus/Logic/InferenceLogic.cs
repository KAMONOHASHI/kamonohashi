using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    public class InferenceLogic : PlatypusLogicBase, IInferenceLogic
    {
        private readonly IInferenceHistoryRepository inferenceHistoryRepository;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly ISlackLogic slackLogic;
        private readonly IResourceMonitorLogic resourceMonitorLogic;
        private readonly ITenantRepository tenantRepository;
        private readonly IUnitOfWork unitOfWork;

        public InferenceLogic(
            IInferenceHistoryRepository inferenceHistoryRepository,
            IClusterManagementLogic clusterManagementLogic,
            ISlackLogic slackLogic,
            IResourceMonitorLogic resourceMonitorLogic,
            ITenantRepository tenantRepository,
            IUnitOfWork unitOfWork,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.inferenceHistoryRepository = inferenceHistoryRepository;
            this.clusterManagementLogic = clusterManagementLogic;
            this.slackLogic = slackLogic;
            this.resourceMonitorLogic = resourceMonitorLogic;
            this.tenantRepository = tenantRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 推論履歴コンテナを削除し、ステータスを変更する。
        /// </summary>
        /// <param name="inferenceHistory">対象学習履歴</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public async Task ExitAsync(InferenceHistory inferenceHistory, ContainerStatus status, bool force)
        {
            // コンテナの生存確認
            if (inferenceHistory.GetStatus().Exist())
            {
                var tenant = tenantRepository.Get(inferenceHistory.TenantId);
                var info = await clusterManagementLogic.GetContainerDetailsInfoAsync(inferenceHistory.Key, tenant.Name, force);

                // コンテナ削除の前に、DBの更新を先に実行
                await inferenceHistoryRepository.UpdateStatusAsync(inferenceHistory.Id, status, info.CreatedAt, DateTime.Now, force);

                // ノード情報取得
                var node = info.NodeName != null
                    ? (await clusterManagementLogic.GetAllNodesAsync()).FirstOrDefault(x => x.Name == info.NodeName)
                    : null;

                // ジョブ実行履歴追加
                AddJobHistory(inferenceHistory, node, tenant, info, status.Key);

                // 実コンテナ削除の結果は確認せず、DBの更新を先に確定する（コンテナがいないなら、そのまま消しても問題ない想定）
                unitOfWork.Commit();

                if (info.Status.Exist())
                {
                    // 再確認してもまだ存在していたら、コンテナ削除
                    await clusterManagementLogic.DeleteContainerAsync(
                        ContainerType.Inferencing, inferenceHistory.Key, tenant.Name, force);

                    // 通知処理
                    slackLogic.InformJobResult(inferenceHistory);
                }
            }
            else
            {
                await inferenceHistoryRepository.UpdateStatusAsync(inferenceHistory.Id, status, force);

                // DBの更新を確定する
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// ジョブ実行履歴を追加する
        /// </summary>
        /// <param name="inferenceHistory">対象推論履歴</param>
        /// <param name="node">実行ノード</param>
        /// <param name="tenant">実行テナント</param>
        /// <param name="info">対象コンテナ詳細情報</param>
        /// <param name="status">ステータス</param>
        public void AddJobHistory(InferenceHistory inferenceHistory, NodeInfo node, Tenant tenant, ContainerDetailsInfo info, string status)
        {
            var resourceJob = new ResourceJob
            {
                TenantId = tenant.Id,
                TenantName = tenant.Name,
                NodeName = node?.Name ?? "",
                NodeCpu = (int)(node?.Cpu ?? 0),
                NodeMemory = (int)(node?.Memory ?? 0),
                NodeGpu = node?.Gpu ?? 0,
                ContainerName = inferenceHistory.Key,
                Cpu = inferenceHistory.Cpu,
                Memory = inferenceHistory.Memory,
                Gpu = inferenceHistory.Gpu,
                JobCreatedAt = inferenceHistory.CreatedAt,
                JobStartedAt = inferenceHistory.StartedAt ?? info?.CreatedAt,
                JobCompletedAt = inferenceHistory.CompletedAt ?? DateTime.Now,
                Status = status,
            };
            resourceMonitorLogic.AddJobHistory(resourceJob);
        }
    }
}

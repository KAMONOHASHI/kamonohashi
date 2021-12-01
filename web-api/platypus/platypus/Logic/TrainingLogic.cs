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
    public class TrainingLogic : PlatypusLogicBase, ITrainingLogic
    {
        private readonly ITrainingHistoryRepository trainingHistoryRepository;
        private readonly ITensorBoardContainerRepository tensorBoardContainerRepository;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IResourceMonitorLogic resourceMonitorLogic;
        private readonly ITenantRepository tenantRepository;
        private readonly IUnitOfWork unitOfWork;

        public TrainingLogic(
            ITrainingHistoryRepository trainingHistoryRepository,
            ITensorBoardContainerRepository tensorBoardContainerRepository,
            IClusterManagementLogic clusterManagementLogic,
            IResourceMonitorLogic resourceMonitorLogic,
            ITenantRepository tenantRepository,
            IUnitOfWork unitOfWork,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.trainingHistoryRepository = trainingHistoryRepository;
            this.tensorBoardContainerRepository = tensorBoardContainerRepository;
            this.clusterManagementLogic = clusterManagementLogic;
            this.resourceMonitorLogic = resourceMonitorLogic;
            this.tenantRepository = tenantRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 学習履歴コンテナを削除し、ステータスを変更する。
        /// </summary>
        /// <param name="trainingHistory">対象学習履歴</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public async Task ExitAsync(TrainingHistory trainingHistory, ContainerStatus status, bool force)
        {
            // コンテナの生存確認
            if (trainingHistory.GetStatus().Exist())
            {
                var tenant = tenantRepository.Get(trainingHistory.TenantId);
                var info = await clusterManagementLogic.GetContainerDetailsInfoAsync(trainingHistory.Key, tenant.Name, force);

                // コンテナ削除の前に、DBの更新を先に実行
                await trainingHistoryRepository.UpdateStatusAsync(trainingHistory.Id, status, info.CreatedAt, DateTime.Now, force);

                // ノード情報の取得
                var node = info.NodeName != null
                    ? (await clusterManagementLogic.GetAllNodesAsync()).FirstOrDefault(x => x.Name == info.NodeName)
                    : null;

                // ジョブ実行履歴追加
                AddJobHistory(trainingHistory, node, tenant, info, status.Key);

                // 実コンテナ削除の結果は確認せず、DBの更新を先に確定する（コンテナがいないなら、そのまま消しても問題ない想定）
                unitOfWork.Commit();

                if (info.Status.Exist())
                {
                    // 再確認してもまだ存在していたら、コンテナ削除
                    await clusterManagementLogic.DeleteContainerAsync(
                        ContainerType.Training, trainingHistory.Key, tenant.Name, force);
                }
            }
            else
            {
                await trainingHistoryRepository.UpdateStatusAsync(trainingHistory.Id, status, force);

                // DBの更新を確定する
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// TensorBoardコンテナを削除する。
        /// </summary>
        /// <param name="container">対象コンテナ</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public async Task DeleteTensorBoardAsync(TensorBoardContainer container, bool force)
        {
            //TensorBoardコンテナを削除する。
            var tenant = tenantRepository.Get(container.TenantId);
            await clusterManagementLogic.DeleteContainerAsync(ContainerType.TensorBoard, container.Name, tenant.Name, force);

            //結果に関わらず、DBからコンテナ情報を消す
            tensorBoardContainerRepository.Delete(container, force);

            unitOfWork.Commit();
        }

        /// <summary>
        /// ジョブ実行履歴を追加する
        /// </summary>
        /// <param name="trainingHistory">対象学習履歴</param>
        /// <param name="node">実行ノード</param>
        /// <param name="tenant">実行テナント</param>
        /// <param name="info">対象コンテナ詳細情報</param>
        /// <param name="status">ステータス</param>
        public void AddJobHistory(TrainingHistory trainingHistory, NodeInfo node, Tenant tenant, ContainerDetailsInfo info, string status)
        {
            var resourceJob = new ResourceJob
            {
                TenantId = tenant.Id,
                TenantName = tenant.Name,
                NodeName = node?.Name ?? "",
                NodeCpu = (int)(node?.Cpu ?? 0),
                NodeMemory = (int)(node?.Memory ?? 0),
                NodeGpu = node?.Gpu ?? 0,
                ContainerName = trainingHistory.Key,
                Cpu = trainingHistory.Cpu,
                Memory = trainingHistory.Memory,
                Gpu = trainingHistory.Gpu,
                JobCreatedAt = trainingHistory.CreatedAt,
                JobStartedAt = trainingHistory.StartedAt ?? info?.CreatedAt,
                JobCompletedAt = trainingHistory.CompletedAt ?? DateTime.Now,
                Status = status,
            };
            resourceMonitorLogic.AddJobHistory(resourceJob);
        }

    }
}

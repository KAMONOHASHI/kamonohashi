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
    public class NotebookLogic : PlatypusLogicBase, INotebookLogic
    {
        private readonly INotebookHistoryRepository notebookHistoryRepository;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IResourceMonitorLogic resourceMonitorLogic;
        private readonly ITenantRepository tenantRepository;
        private readonly IUnitOfWork unitOfWork;

        public NotebookLogic(
            INotebookHistoryRepository notebookHistoryRepository,
            IClusterManagementLogic clusterManagementLogic,
            IResourceMonitorLogic resourceMonitorLogic,
            ITenantRepository tenantRepository,
            IUnitOfWork unitOfWork,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.notebookHistoryRepository = notebookHistoryRepository;
            this.clusterManagementLogic = clusterManagementLogic;
            this.resourceMonitorLogic = resourceMonitorLogic;
            this.tenantRepository = tenantRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// ノートブック履歴コンテナを削除し、ステータスを変更する。
        /// </summary>
        /// <param name="notebookHistory">対象ノートブック履歴</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public async Task ExitAsync(NotebookHistory notebookHistory, ContainerStatus status, bool force)
        {
            // コンテナの生存確認
            if (notebookHistory.GetStatus().Exist())
            {
                var tenant = tenantRepository.Get(notebookHistory.TenantId);
                var info = await clusterManagementLogic.GetContainerDetailsInfoAsync(notebookHistory.Key, tenant.Name, force);

                // コンテナ削除の前に、DBの更新を先に実行
                await notebookHistoryRepository.UpdateStatusAsync(notebookHistory.Id, status, DateTime.Now, info.CreatedAt, force);

                // ジョブ実行履歴追加
                var node = info.NodeName != null
                    ? (await clusterManagementLogic.GetAllNodesAsync()).FirstOrDefault(x => x.Name == info.NodeName)
                    : null;

                AddJobHistory(notebookHistory, node, tenant, info, status);

                // 実コンテナ削除の結果は確認せず、DBの更新を確定する（コンテナがいないなら、そのまま消しても問題ない想定）
                unitOfWork.Commit();

                if (info.Status.Exist())
                {
                    // 再確認してもまだ存在していたら、コンテナ削除

                    await clusterManagementLogic.DeleteContainerAsync(
                        ContainerType.Notebook, notebookHistory.Key, tenant.Name, force);
                }
            }
            else
            {
                await notebookHistoryRepository.UpdateStatusAsync(notebookHistory.Id, status, force);

                // DBの更新を確定する
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// ジョブ実行履歴を追加する
        /// </summary>
        /// <param name="notebookHistory">対象ノートブック履歴</param>
        /// <param name="node">実行ノード</param>
        /// <param name="tenant">実行テナント</param>
        /// <param name="info">対象コンテナ詳細情報</param>
        /// <param name="status">ステータス</param>
        public void AddJobHistory(NotebookHistory notebookHistory, NodeInfo node, Tenant tenant, ContainerDetailsInfo info, ContainerStatus status)
        {
            var resourceJob = new ResourceJob
            {
                TenantId = tenant.Id,
                TenantName = tenant.Name,
                NodeName = node?.Name ?? "",
                NodeCpu = (int)(node?.Cpu ?? 0),
                NodeMemory = (int)(node?.Memory ?? 0),
                NodeGpu = node?.Gpu ?? 0,
                ContainerName = notebookHistory.Key,
                Cpu = notebookHistory.Cpu,
                Memory = notebookHistory.Memory,
                Gpu = notebookHistory.Gpu,
                JobCreatedAt = notebookHistory.StartedAt ?? notebookHistory.CreatedAt,
                JobStartedAt = notebookHistory.JobStartedAt ?? info?.CreatedAt,
                JobCompletedAt = notebookHistory.CompletedAt ?? DateTime.Now,
                Status = status.Key,
            };
            resourceMonitorLogic.AddJobHistory(resourceJob);
        }
    }
}

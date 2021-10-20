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
    /// <summary>
    /// 前処理ロジッククラス
    /// </summary>
    /// <seealso cref="Nssol.Platypus.Logic.Interfaces.IPreprocessLogic" />
    public class PreprocessLogic : PlatypusLogicBase, IPreprocessLogic
    {
        private IPreprocessHistoryRepository preprocessHistoryRepository;
        private readonly IResourceMonitorLogic resourceMonitorLogic;
        private readonly ITenantRepository tenantRepository;
        private IDataLogic dataLogic;
        private IStorageLogic storageLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PreprocessLogic(
            IPreprocessHistoryRepository preprocessHistoryRepository,
            IResourceMonitorLogic resourceMonitorLogic,
            ITenantRepository tenantRepository,
            IDataLogic dataLogic,
            IStorageLogic storageLogic,
            IClusterManagementLogic clusterManagementLogic,
            IUnitOfWork unitOfWork,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.preprocessHistoryRepository = preprocessHistoryRepository;
            this.resourceMonitorLogic = resourceMonitorLogic;
            this.tenantRepository = tenantRepository;
            this.dataLogic = dataLogic;
            this.storageLogic = storageLogic;
            this.clusterManagementLogic = clusterManagementLogic;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 前処理コンテナを削除し、ステータスを変更する。
        /// 削除可否の判断が必要なら、呼び出しもとで判定すること。
        /// </summary>
        /// <param name="preprocessHistory">対象前処理履歴</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public async Task<bool> DeleteAsync(PreprocessHistory preprocessHistory, bool force)
        {
            // 前処理結果を削除
            bool result = true;
            foreach (var outputDataId in preprocessHistoryRepository.GetPreprocessOutputs(preprocessHistory.Id))
            {
                //1件でも失敗したら結果はfalse。ただし、エラーが出ても最後まで消し切る。
                result &= await dataLogic.DeleteDataAsync(outputDataId);
            }

            // 前処理履歴の削除
            preprocessHistoryRepository.Delete(preprocessHistory, force);

            // 結果に関わらずコミット
            unitOfWork.Commit();

            var status = ContainerStatus.Convert(preprocessHistory.Status);
            if (status.Exist())
            {
                // ジョブ実行履歴追加
                var tenant = tenantRepository.Get(preprocessHistory.TenantId);
                var info = await clusterManagementLogic.GetContainerDetailsInfoAsync(preprocessHistory.Name, tenant.Name, force);
                var node = info.NodeName != null
                    ? (await clusterManagementLogic.GetAllNodesAsync()).FirstOrDefault(x => x.Name == info.NodeName)
                    : null;
                AddJobHistory(preprocessHistory, node, tenant, info, status.Key);
                unitOfWork.Commit();

                //コンテナが動いていれば、停止する
                await clusterManagementLogic.DeleteContainerAsync(
                    ContainerType.Preprocessing, preprocessHistory.Name, tenant.Name, force);
            }

            // ストレージ内の前処理データを削除する
            await storageLogic.DeleteResultsAsync(ResourceType.PreprocContainerAttachedFiles, preprocessHistory.Id);

            return result;
        }

        /// <summary>
        /// ジョブ実行履歴を追加する
        /// </summary>
        /// <param name="preprocessHistory">対象前処理履歴</param>
        /// <param name="node">実行ノード</param>
        /// <param name="tenant">実行テナント</param>
        /// <param name="info">対象コンテナ詳細情報</param>
        /// <param name="status">ステータス</param>
        public void AddJobHistory(PreprocessHistory preprocessHistory, NodeInfo node, Tenant tenant, ContainerDetailsInfo info, string status)
        {
            var resourceJob = new ResourceJob
            {
                TenantId = tenant.Id,
                TenantName = tenant.Name,
                NodeName = node?.Name ?? "",
                NodeCpu = (int)(node?.Cpu ?? 0),
                NodeMemory = (int)(node?.Memory ?? 0),
                NodeGpu = node?.Gpu ?? 0,
                ContainerName = preprocessHistory.Name,
                Cpu = preprocessHistory.Cpu ?? 0,
                Memory = preprocessHistory.Memory ?? 0,
                Gpu = preprocessHistory.Gpu ?? 0,
                JobCreatedAt = preprocessHistory.CreatedAt,
                JobStartedAt = preprocessHistory.StartedAt ?? info?.CreatedAt,
                JobCompletedAt = preprocessHistory.CompletedAt ?? DateTime.Now,
                Status = status,
            };
            resourceMonitorLogic.AddJobHistory(resourceJob);
        }
    }
}

using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models.TenantModels;
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
        private IDataLogic dataLogic;
        private IStorageLogic storageLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PreprocessLogic(
            IPreprocessHistoryRepository preprocessHistoryRepository,
            IDataLogic dataLogic,
            IStorageLogic storageLogic,
            IClusterManagementLogic clusterManagementLogic,
            IUnitOfWork unitOfWork,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.preprocessHistoryRepository = preprocessHistoryRepository;
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
                //コンテナが動いていれば、停止する
                await clusterManagementLogic.DeleteContainerAsync(
                    ContainerType.Preprocessing, preprocessHistory.Name, CurrentUserInfo.SelectedTenant.Name, force);
            }

            // ストレージ内の前処理データを削除する
            await storageLogic.DeleteResultsAsync(ResourceType.PreprocContainerAttachedFiles, preprocessHistory.Id);

            return result;
        }
    }
}

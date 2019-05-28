using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    public class InferenceLogic : PlatypusLogicBase, IInferenceLogic
    {
        private readonly IInferenceHistoryRepository inferenceHistoryRepository;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IUnitOfWork unitOfWork;

        public InferenceLogic(
            IInferenceHistoryRepository inferenceHistoryRepository,
            IClusterManagementLogic clusterManagementLogic,
            IUnitOfWork unitOfWork,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.inferenceHistoryRepository = inferenceHistoryRepository;
            this.clusterManagementLogic = clusterManagementLogic;
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
            //コンテナの生存確認
            if (inferenceHistory.GetStatus().Exist())
            {
                var info = await clusterManagementLogic.GetContainerDetailsInfoAsync(inferenceHistory.Key, CurrentUserInfo.SelectedTenant.Name, force);

                if (info.Status.Exist())
                {
                    //再確認してもまだ存在していたら、コンテナ削除
                    await clusterManagementLogic.DeleteContainerAsync(
                        ContainerType.Training, inferenceHistory.Key, CurrentUserInfo.SelectedTenant.Name, force);
                }

                await inferenceHistoryRepository.UpdateStatusAsync(inferenceHistory.Id, status, info.CreatedAt, DateTime.Now, force);
            }
            else
            {
                await inferenceHistoryRepository.UpdateStatusAsync(inferenceHistory.Id, status, force);
            }

            //実コンテナ削除の結果は確認せず、DBの更新を確定する（コンテナがいないなら、そのまま消しても問題ない想定）
            unitOfWork.Commit();
        }
    }
}

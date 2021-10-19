using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System.Threading.Tasks;
using Nssol.Platypus.Models;
using Nssol.Platypus.Infrastructure.Infos;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// Job系の操作で、<see cref="Controllers.spa.TrainingController"/>以外からも使用したい処理をまとめたロジック
    /// </summary>
    public interface ITrainingLogic
    {
        /// <summary>
        /// 学習履歴コンテナを削除し、ステータスを変更する。
        /// </summary>
        /// <param name="trainingHistory">対象学習履歴</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task ExitAsync(TrainingHistory trainingHistory, ContainerStatus status, bool force);

        /// <summary>
        /// TensorBoardコンテナを削除する。
        /// </summary>
        /// <param name="container">対象コンテナ</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task DeleteTensorBoardAsync(TensorBoardContainer container, bool force);

        /// <summary>
        /// ジョブ実行履歴を追加する
        /// </summary>
        /// <param name="trainingHistory">対象学習履歴</param>
        /// <param name="tenant">対象テナント</param>
        /// <param name="info">対象コンテナの詳細情報</param>
        /// <param name="status">変更後のステータス</param>
        Task AddJobHistory(TrainingHistory trainingHistory, Tenant tenant, ContainerDetailsInfo info, ContainerStatus status);
    }
}

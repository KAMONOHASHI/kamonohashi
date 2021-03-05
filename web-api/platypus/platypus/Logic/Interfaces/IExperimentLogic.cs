using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// Job系の操作で、<see cref="Controllers.spa.ExperimentController"/>以外からも使用したい処理をまとめたロジック
    /// </summary>
    public interface IExperimentLogic
    {
        /// <summary>
        /// 実験履歴コンテナを削除し、ステータスを変更する。
        /// </summary>
        /// <param name="experimentHistory">対象実験履歴</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task ExitAsync(ExperimentHistory experimentHistory, ContainerStatus status, bool force);

        /// <summary>
        /// 前処理コンテナを削除し、ステータスを変更する。
        /// </summary>
        /// <param name="history"></param>
        /// <param name="status"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        Task ExitPreprocessAsync(ExperimentPreprocessHistory history, ContainerStatus status, bool force);

        /// <summary>
        /// TensorBoardコンテナを削除する。
        /// </summary>
        /// <param name="container">対象コンテナ</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task DeleteTensorBoardAsync(ExperimentTensorBoardContainer container, bool force);
    }
}

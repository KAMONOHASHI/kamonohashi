using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// 実験履歴テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    /// TODO 前処理分も追加
    public interface IExperimentHistoryRepository : IRepositoryForTenant<ExperimentHistory>
    {
        /// <summary>
        /// 全実験履歴（データセットを含む）を取得します。
        /// </summary>
        IQueryable<ExperimentHistory> GetAllIncludeDataSet();

        /// <summary>
        /// 全実験履歴（データセット、親実験を含む）を並べ替えありで取得します。
        /// </summary>
        IQueryable<ExperimentHistory> GetAllIncludeDataSetAndParentWithOrdering();

        /// <summary>
        /// 全実験履歴の名前とIDのみ取得する
        /// </summary>
        Task<IEnumerable<ExperimentHistory>> GetAllNameAsync();

        /// <summary>
        /// 指定された実験履歴IDの実験履歴エンティティ（全ての外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        Task<ExperimentHistory> GetIncludeAllAsync(long id);

        /// <summary>
        /// データセットIDに紐づく実験履歴が存在するかチェックします。
        /// </summary>
        /// <param name="datasetId">データセットID</param>
        /// <returns>True:存在する　False:存在しない</returns>
        Task<bool> ExistsByDataSetIdAsync(long datasetId);

        /// <summary>
        /// 検索条件に合致するデータを一件取得します。
        /// </summary>
        /// <param name="where">検索条件</param>
        /// <param name="force">選択中のテナント以外も対象とするか</param>
        ExperimentHistory Find(Expression<Func<ExperimentHistory, bool>> where, bool force);

        /// <summary>
        /// 実験履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, bool force);

        /// <summary>
        /// 実験履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="startedAt">開始日時</param>
        /// <param name="completedAt">停止日時</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, DateTime? startedAt, DateTime completedAt, bool force);

    }
}

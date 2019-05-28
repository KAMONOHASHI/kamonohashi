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
    /// 推論履歴テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IInferenceHistoryRepository : IRepositoryForTenant<InferenceHistory>
    {
        /// <summary>
        /// 全推論履歴（データセットを含む）を取得します。
        /// </summary>
        IQueryable<InferenceHistory> GetAllIncludeDataSet();
        /// <summary>
        /// 全推論履歴（データセットを含む）を並べ替えありで取得します。
        /// </summary>
        IQueryable<InferenceHistory> GetAllIncludeDataSetWithOrdering();

        /// <summary>
        /// 全推論履歴の名前とIDのみ取得する
        /// </summary>
        Task<IEnumerable<InferenceHistory>> GetAllNameAsync();

        /// <summary>
        /// 指定された推論履歴IDの学習履歴エンティティ（全ての外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        Task<InferenceHistory> GetIncludeAllAsync(long id);

        /// <summary>
        /// データセットIDに紐づく推論履歴が存在するかチェックします。
        /// </summary>
        /// <param name="datasetId">データセットID</param>
        /// <returns>True:存在する　False:存在しない</returns>
        Task<bool> ExistsByDataSetIdAsync(long datasetId);

        /// <summary>
        /// 検索条件に合致するデータを一件取得します。
        /// </summary>
        InferenceHistory Find(Expression<Func<InferenceHistory, bool>> where, bool force);

        /// <summary>
        /// 推論履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        Task UpdateStatusAsync(long id, ContainerStatus status, bool force);

        /// <summary>
        /// 推論履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        Task UpdateStatusAsync(long id, ContainerStatus status, DateTime? startedAt, DateTime completedAt, bool force);

        #region 添付ファイル操作

        /// <summary>
        /// 指定したIDの推論履歴に紐づく全ての添付ファイルを取得します。
        /// </summary>
        Task<IEnumerable<InferenceHistoryAttachedFile>> GetAllAttachedFilesAsync(long id);

        /// <summary>
        /// 指定したIDの推論履歴添付ファイルを取得します。
        /// </summary>
        Task<InferenceHistoryAttachedFile> GetAttachedFileAsync(long id);

        /// <summary>
        /// 指定したIDの推論履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        Task<bool> ExistsAttachedFileAsync(long id, string fileName);

        /// <summary>
        /// 推論履歴添付ファイルを追加します。
        /// </summary>
        void AddAttachedFile(InferenceHistoryAttachedFile file);

        /// <summary>
        /// 指定した推論履歴添付ファイルを削除します。
        /// </summary>
        void DeleteAttachedFile(InferenceHistoryAttachedFile file);
        #endregion

    }
}

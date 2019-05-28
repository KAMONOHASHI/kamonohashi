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
    /// 学習履歴テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface ITrainingHistoryRepository : IRepositoryForTenant<TrainingHistory>
    {
        /// <summary>
        /// 全学習履歴（データセットを含む）を取得します。
        /// </summary>
        IQueryable<TrainingHistory> GetAllIncludeDataSet();
        /// <summary>
        /// 全学習履歴（データセットを含む）を並べ替えありで取得します。
        /// </summary>
        IQueryable<TrainingHistory> GetAllIncludeDataSetWithOrdering();

        /// <summary>
        /// 全学習履歴の名前とIDのみ取得する
        /// </summary>
        Task<IEnumerable<TrainingHistory>> GetAllNameAsync();

        /// <summary>
        /// 指定された学習履歴IDの学習履歴エンティティ（全ての外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        Task<TrainingHistory> GetIncludeAllAsync(long id);

        /// <summary>
        /// データセットIDに紐づく学習履歴が存在するかチェックします。
        /// </summary>
        /// <param name="datasetId">データセットID</param>
        /// <returns>True:存在する　False:存在しない</returns>
        Task<bool> ExistsByDataSetIdAsync(long datasetId);

        /// <summary>
        /// 検索条件に合致するデータを一件取得します。
        /// </summary>
        TrainingHistory Find(Expression<Func<TrainingHistory, bool>> where, bool force);

        /// <summary>
        /// 学習履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        Task UpdateStatusAsync(long id, ContainerStatus status, bool force);

        /// <summary>
        /// 学習履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        Task UpdateStatusAsync(long id, ContainerStatus status, DateTime? startedAt, DateTime completedAt, bool force);

        #region 添付ファイル操作

        /// <summary>
        /// 指定したIDの学習履歴に紐づく全ての添付ファイルを取得します。
        /// </summary>
        Task<IEnumerable<TrainingHistoryAttachedFile>> GetAllAttachedFilesAsync(long id);

        /// <summary>
        /// 指定したIDの学習履歴添付ファイルを取得します。
        /// </summary>
        Task<TrainingHistoryAttachedFile> GetAttachedFileAsync(long id);

        /// <summary>
        /// 指定したIDの学習履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        Task<bool> ExistsAttachedFileAsync(long id, string fileName);

        /// <summary>
        /// 学習履歴添付ファイルを追加します。
        /// </summary>
        void AddAttachedFile(TrainingHistoryAttachedFile file);

        /// <summary>
        /// 指定した学習履歴添付ファイルを削除します。
        /// </summary>
        void DeleteAttachedFile(TrainingHistoryAttachedFile file);
        #endregion
    }
}

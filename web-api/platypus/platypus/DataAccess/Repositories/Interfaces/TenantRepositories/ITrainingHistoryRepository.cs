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
        /// 全学習履歴（データセット、親学習を含む）を並べ替えありで取得します。
        /// </summary>
        IQueryable<TrainingHistory> GetAllIncludeDataSetAndParentWithOrdering();

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
        /// <param name="where">検索条件</param>
        /// <param name="force">選択中のテナント以外も対象とするか</param>
        TrainingHistory Find(Expression<Func<TrainingHistory, bool>> where, bool force);

        /// <summary>
        /// 学習履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, bool force);

        /// <summary>
        /// 学習履歴のメモを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <param name="memo">memo</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateMemoAsync(long id, string memo, bool force);

        /// <summary>
        /// 学習履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="startedAt">開始日時</param>
        /// <param name="completedAt">停止日時</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, DateTime? startedAt, DateTime completedAt, bool force);

        /// <summary>
        /// 派生した学習履歴を取得する
        /// </summary>
        /// <param name="id">親学習ID</param>
        Task<IEnumerable<TrainingHistoryParentMap>> GetChildrenAsync(long id);

        /// <summary>
        /// 学習履歴に親学習を紐づける
        /// </summary>
        /// <param name="trainingHistory">学習履歴</param>
        /// <param name="parent">親学習履歴</param>
        TrainingHistoryParentMap AttachParentAsync(TrainingHistory trainingHistory, TrainingHistory parent);
        #region 添付ファイル操作

        /// <summary>
        /// 指定したIDの学習履歴に紐づく全ての添付ファイルを取得します。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <returns>添付ファイル一覧</returns>
        Task<IEnumerable<TrainingHistoryAttachedFile>> GetAllAttachedFilesAsync(long id);

        /// <summary>
        /// 指定したIDの学習履歴添付ファイルを取得します。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <returns>添付ファイル</returns>
        Task<TrainingHistoryAttachedFile> GetAttachedFileAsync(long id);

        /// <summary>
        /// 指定したIDの学習履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns> True:登録済み　False:未登録</returns>
        Task<bool> ExistsAttachedFileAsync(long id, string fileName);

        /// <summary>
        /// 学習履歴添付ファイルを追加します。
        /// </summary>
        /// <param name="file">追加対象のファイル</param>
        void AddAttachedFile(TrainingHistoryAttachedFile file);

        /// <summary>
        /// 指定した学習履歴添付ファイルを削除します。
        /// </summary>
        /// <param name="file">削除対象のファイル</param>
        void DeleteAttachedFile(TrainingHistoryAttachedFile file);
        #endregion
    }
}

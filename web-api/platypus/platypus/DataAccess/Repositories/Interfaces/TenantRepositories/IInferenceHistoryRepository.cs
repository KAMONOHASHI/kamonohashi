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
        /// 全推論履歴（データセット、親学習・親推論を含む）を並べ替えありで取得します。
        /// </summary>
        IQueryable<InferenceHistory> GetAllIncludeDataSetAndParentWithOrdering();

        /// <summary>
        /// 全推論履歴の名前とIDのみ取得する
        /// </summary>
        Task<IEnumerable<InferenceHistory>> GetAllNameAsync();

        /// <summary>
        /// 指定された推論履歴IDの推論履歴エンティティ（全ての外部参照を含む）を取得します。
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
        /// <param name="where">検索条件</param>
        /// <param name="force">選択中のテナント以外も対象とするか</param>
        InferenceHistory Find(Expression<Func<InferenceHistory, bool>> where, bool force);

        /// <summary>
        /// 推論履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, bool force);

        /// <summary>
        /// 推論履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="startedAt">開始日時</param>
        /// <param name="completedAt">停止日時</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, DateTime? startedAt, DateTime completedAt, bool force);

        /// <summary>
        /// 指定したIDの学習履歴を利用した推論履歴を取得する
        /// </summary>
        /// <param name="id">マウントされた学習ID</param>
        Task<IEnumerable<InferenceHistoryParentMap>> GetMountedTrainingAsync(long id);
        /// <summary>
        /// 指定したIDの推論履歴を利用した推論履歴を取得する
        /// </summary>
        /// <param name="id">マウントされた推論ID</param>
        Task<IEnumerable<InferenceHistoryParentInferenceMap>> GetMountedInferenceAsync(long id);
        /// <summary>
        /// 推論履歴に親学習を紐づける
        /// </summary>
        /// <param name="history">推論履歴</param>
        /// <param name="parent">親学習履歴</param>
        InferenceHistoryParentMap AttachParentAsync(InferenceHistory history, TrainingHistory parent);

        /// <summary>
        /// 推論履歴に親推論を紐づける
        /// </summary>
        /// <param name="history">推論履歴</param>
        /// <param name="parent">親推論履歴</param>
        InferenceHistoryParentInferenceMap AttachParentInferenceAsync(InferenceHistory history, InferenceHistory parent);
        #region 添付ファイル操作

        /// <summary>
        /// 指定したIDの推論履歴に紐づく全ての添付ファイルを取得します。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        /// <returns>添付ファイル一覧</returns>
        Task<IEnumerable<InferenceHistoryAttachedFile>> GetAllAttachedFilesAsync(long id);

        /// <summary>
        /// 指定したIDの推論履歴添付ファイルを取得します。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        /// <returns>添付ファイル</returns>
        Task<InferenceHistoryAttachedFile> GetAttachedFileAsync(long id);

        /// <summary>
        /// 指定したIDの推論履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns> True:登録済み　False:未登録</returns>
        Task<bool> ExistsAttachedFileAsync(long id, string fileName);

        /// <summary>
        /// 推論履歴添付ファイルを追加します。
        /// </summary>
        /// <param name="file">追加対象のファイル</param>
        void AddAttachedFile(InferenceHistoryAttachedFile file);

        /// <summary>
        /// 指定した推論履歴添付ファイルを削除します。
        /// </summary>
        /// <param name="file">削除対象のファイル</param>
        void DeleteAttachedFile(InferenceHistoryAttachedFile file);
        #endregion

    }
}

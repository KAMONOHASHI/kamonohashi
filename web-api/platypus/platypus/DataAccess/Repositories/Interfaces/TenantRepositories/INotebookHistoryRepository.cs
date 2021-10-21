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
    /// ノートブック履歴テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface INotebookHistoryRepository : IRepositoryForTenant<NotebookHistory>
    {
        /// <summary>
        /// 全ノートブック履歴を並べ替えありで取得する
        /// </summary>
        IQueryable<NotebookHistory> GetAllWithOrdering();

        /// <summary>
        /// 全ノートブック履歴の名前とIDのみ取得する
        /// </summary>
        Task<IEnumerable<NotebookHistory>> GetAllNameAsync();

        /// <summary>
        /// 指定されたノートブック履歴IDのノートブック履歴エンティティ（全ての外部参照を含む）を取得する
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        Task<NotebookHistory> GetIncludeAllAsync(long id);

        /// <summary>
        /// テナント横断で全データ（外部参照を含む）をすべて取得する。（取得結果はキャッシュされない）
        /// ソートはIDの逆順。
        /// </summary>
        Task<IEnumerable<NotebookHistory>> GetAllIncludeTenantAsNoTrackingAsync();

        /// <summary>
        /// 検索条件に合致するデータを一件取得する
        /// </summary>
        /// <param name="where">検索条件</param>
        /// <param name="force">選択中のテナント以外も対象とするか</param>
        NotebookHistory Find(Expression<Func<NotebookHistory, bool>> where, bool force);

        /// <summary>
        /// ノートブック履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, bool force);

        /// <summary>
        /// ノートブック履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="completedAt">停止日時</param>
        /// <param name="jobStartedAt">アサイン日時</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, DateTime completedAt, DateTime? jobStartedAt, bool force);

        /// <summary>
        /// ノートブック履歴IDに親学習履歴IDを紐づける
        /// </summary>
        /// <param name="notebookHistory">ノートブック履歴</param>
        /// <param name="parent">親学習履歴</param>
        NotebookHistoryParentTrainingMap AttachParentToNotebookAsync(NotebookHistory notebookHistory, TrainingHistory parent);

        /// <summary>
        /// ノートブック履歴IDに紐づいている親学習履歴IDを解除する
        /// </summary>
        /// <param name="notebookHistory">ノートブック履歴</param>
        void DetachParentToNotebookAsync(NotebookHistory notebookHistory);

        /// <summary>
        /// ノートブック履歴IDに親推論履歴IDを紐づける
        /// </summary>
        /// <param name="notebookHistory">ノートブック履歴</param>
        /// <param name="parentInference">親推論履歴</param>
        NotebookHistoryParentInferenceMap AttachParentInferenceToNotebookAsync(NotebookHistory notebookHistory, InferenceHistory parentInference);

        /// <summary>
        /// ノートブック履歴IDに紐づいている親推論履歴IDを解除する
        /// </summary>
        /// <param name="notebookHistory">ノートブック履歴</param>
        void DetachParentInferenceToNotebookAsync(NotebookHistory notebookHistory);
    }
}

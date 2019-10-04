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
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, DateTime completedAt, bool force);
    }
}

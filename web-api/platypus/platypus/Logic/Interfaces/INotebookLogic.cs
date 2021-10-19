using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// Job系の操作で、<see cref="Controllers.spa.NotebookController"/>以外からも使用したい処理をまとめたロジック
    /// </summary>
    public interface INotebookLogic
    {
        /// <summary>
        /// ノートブック履歴コンテナを削除し、ステータスを変更する。
        /// </summary>
        /// <param name="notebookHistory">対象ノートブック履歴</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task ExitAsync(NotebookHistory notebookHistory, ContainerStatus status, bool force);

        /// <summary>
        /// ジョブ実行履歴を追加する
        /// </summary>
        /// <param name="notebookHistory">対象ノートブック履歴</param>
        /// <param name="node">実行ノード</param>
        /// <param name="tenant">実行テナント</param>
        /// <param name="info">対象コンテナ詳細情報</param>
        /// <param name="status">ステータス</param>
        void AddJobHistory(NotebookHistory notebookHistory, NodeInfo node, Tenant tenant, ContainerDetailsInfo info, ContainerStatus status);
    }
}

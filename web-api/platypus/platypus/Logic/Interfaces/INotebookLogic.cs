using Nssol.Platypus.Infrastructure;
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
    }
}

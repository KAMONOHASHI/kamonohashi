using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// 前処理ロジックインターフェイス
    /// </summary>
    public interface IPreprocessLogic
    {
        /// <summary>
        /// 前処理コンテナを削除し、ステータスを変更する。
        /// 削除可否の判断が必要なら、呼び出しもとで判定すること。
        /// </summary>
        /// <param name="preprocessHistory">対象前処理履歴</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task<bool> DeleteAsync(PreprocessHistory preprocessHistory, bool force);

        /// <summary>
        /// ジョブ実行履歴を追加する
        /// </summary>
        /// <param name="preprocessHistory">対象前処理履歴</param>
        /// <param name="node">実行ノード</param>
        /// <param name="tenant">実行テナント</param>
        /// <param name="info">対象コンテナ詳細情報</param>
        /// <param name="status">ステータス</param>
        void AddJobHistory(PreprocessHistory preprocessHistory, NodeInfo node, Tenant tenant, ContainerDetailsInfo info, string status);
    }
}

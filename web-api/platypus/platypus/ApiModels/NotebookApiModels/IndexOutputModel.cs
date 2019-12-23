using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.NotebookApiModels
{
    /// <summary>
    /// ノートブック履歴のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : SimpleOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="history">ノートブック履歴</param>
        public IndexOutputModel(NotebookHistory history) : base(history)
        {
        }
    }
}

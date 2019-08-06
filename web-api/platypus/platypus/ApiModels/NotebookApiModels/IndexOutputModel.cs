using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.NotebookApiModels
{
    /// <summary>
    /// ノートブック履歴のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : SimpleOutputModel
    {
        public IndexOutputModel(NotebookHistory history) : base(history)
        {
        }
    }
}

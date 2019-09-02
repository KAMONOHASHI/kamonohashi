using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.NodeApiModels
{
    /// <summary>
    /// ノード情報のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(Node node) : base(node)
        {
            Id = node.Id;
            Name = node.Name;
            Memo = node.Memo;
            Partition = node.Partition;
            AccessLevel = node.AccessLevel;
            AccessLevelStr = node.AccessLevel.ToString();
            TensorBoardEnabled = node.TensorBoardEnabled;
            NotebookEnabled = node.NotebookEnabled;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// パーティション
        /// </summary>
        public string Partition { get; set; }

        /// <summary>
        /// アクセスレベル
        /// </summary>
        public NodeAccessLevel AccessLevel { get; set; }

        /// <summary>
        /// アクセスレベルの表示名
        /// </summary>
        public string AccessLevelStr { get; set; }

        /// <summary>
        /// TensorBoardコンテナの実行可否
        /// </summary>
        public bool TensorBoardEnabled { get; set; }

        /// <summary>
        /// Notebookコンテナの実行可否
        /// </summary>
        public bool NotebookEnabled { get; set; }
    }
}

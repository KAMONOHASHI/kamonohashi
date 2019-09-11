using Nssol.Platypus.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// クラスタノード
    /// </summary>
    public class Node : ModelBase
    {
        /// <summary>
        /// ノード名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// ノードのアクセスレベル。
        /// アクセス権がないテナントはこのノードに新規にコンテナを立てることができなくなる。
        /// アクセルレベルが下がっても、実行中のコンテナを殺すことはしない。
        /// </summary>
        [Required]
        public NodeAccessLevel AccessLevel { get; set; }

        /// <summary>
        /// パーティションラベル
        /// </summary>
        public string Partition { get; set; }

        /// <summary>
        /// TensorBoardの起動を許可するか
        /// </summary>
        public bool TensorBoardEnabled { get; set; }

        /// <summary>
        /// Notebookの起動を許可するか
        /// </summary>
        public bool NotebookEnabled { get; set; }

        /// <summary>
        /// 利用可能か。
        /// メンテなどでクラスタから外した場合はFalseになる。
        /// </summary>
        public bool Enable
        {
            get {
                return AccessLevel != NodeAccessLevel.Disabled;
            }
        }
    }
}

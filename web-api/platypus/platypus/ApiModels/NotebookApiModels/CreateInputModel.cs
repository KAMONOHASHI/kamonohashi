using Nssol.Platypus.ApiModels.Components;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.NotebookApiModels
{
    /// <summary>
    /// ノートブック新規実行モデル
    /// </summary>
    public class CreateInputModel
    {
        /// <summary>
        /// ノートブック名
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        
        /// <summary>
        /// コンテナ情報
        /// </summary>
        public ContainerImageInputModel ContainerImage { get; set; }

        /// <summary>
        /// データセットID
        /// </summary>
        public long? DataSetId { get; set; }

        /// <summary>
        /// 親学習履歴ID
        /// </summary>
        public IEnumerable<long> ParentIds { get; set; }

        /// <summary>
        /// ノートブックモデルGit情報
        /// </summary>
        public GitCommitInputModel GitModel { get; set; }

        /// <summary>
        /// 追加環境変数
        /// </summary>
        public Dictionary<string, string> Options { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        [Required]
        public int? Cpu { get; set; }
        
        /// <summary>
        /// メモリ数(GiB)
        /// </summary>
        [Required]
        public int? Memory { get; set; }
        
        /// <summary>
        /// GPU数
        /// </summary>
        [Required]
        public int? Gpu { get; set; }
        
        /// <summary>
        /// パーティション
        /// </summary>
        public string Partition { get; set; }
        
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// コンテナの生存期間(秒)
        /// </summary>
        [Required]
        public int? ExpiresIn { get; set; }
    }
}

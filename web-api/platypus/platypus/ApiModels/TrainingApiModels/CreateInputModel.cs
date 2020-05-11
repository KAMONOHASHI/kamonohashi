using Nssol.Platypus.ApiModels.Components;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    public class CreateInputModel
    {
        /// <summary>
        /// 識別名
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        /// <summary>
        /// コンテナ情報
        /// </summary>
        [Required]
        public ContainerImageInputModel ContainerImage { get; set; }

        /// <summary>
        /// データセットID
        /// </summary>
        [Required]
        public long? DataSetId { get; set; }

        /// <summary>
        /// 親学習履歴ID
        /// </summary>
        public IEnumerable<long> ParentIds { get; set; }

        /// <summary>
        /// 学習モデルGit情報
        /// </summary>
        [Required]
        public GitCommitInputModel GitModel { get; set; }

        /// <summary>
        /// ジョブ実行コマンド
        /// </summary>
        [Required]
        public string EntryPoint { get; set; }

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
        /// 開放ポート番号
        /// </summary>
        public List<int> Ports { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// zip圧縮するか否か。
        /// true：zip圧縮する　false：zip圧縮しない
        /// </summary>
        public bool Zip { get; set; }
    }
}

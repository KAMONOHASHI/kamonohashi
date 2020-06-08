using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.PreprocessingApiModels
{
    /// <summary>
    /// 前処理履歴の実行入力モデル
    /// </summary>
    public class RunPreprocessHistoryInputModel
    {
        /// <summary>
        /// 元データID
        /// </summary>
        [Required]
        public long? DataId { get; set; }

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
        /// メモリ数(GB)
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
    }
}

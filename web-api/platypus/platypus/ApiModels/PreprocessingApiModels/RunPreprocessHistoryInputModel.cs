using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.PreprocessingApiModels
{
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
    }
}

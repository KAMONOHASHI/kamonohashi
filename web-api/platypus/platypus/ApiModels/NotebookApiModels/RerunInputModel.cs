using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.NotebookApiModels
{
    /// <summary>
    /// ノートブック再起動モデル
    /// </summary>
    public class RerunInputModel
    {
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
        /// コンテナの生存期間(秒)
        /// </summary>
        [Required]
        public int? ExpiresIn { get; set; }
    }
}

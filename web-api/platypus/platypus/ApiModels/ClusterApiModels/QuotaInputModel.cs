using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.ClusterApiModels
{
    /// <summary>
    /// クォータ設定情報入力モデル
    /// </summary>
    public class QuotaInputModel
    {
        /// <summary>
        /// テナントID
        /// </summary>
        [Required]
        public long? TenantId { get; set; }

        /// <summary>
        /// CPUコア数。未指定、あるいは0を指定した場合は無制限。
        /// </summary>
        public int Cpu { get; set; }

        /// <summary>
        /// メモリ（GB）。未指定、あるいは0を指定した場合は無制限。
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// GPU数。未指定、あるいは0を指定した場合は無制限。
        /// </summary>
        public int Gpu { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// テナントとEksの対応付け中間テーブル。
    /// <seealso cref="Node"/>
    /// <seealso cref="Eks"/>
    /// </summary>
    public class TenantEksMap : ModelBase
    {
        /// <summary>
        /// ノードID
        /// </summary>
        [Required]
        public long TenantId { get; set; }

        /// <summary>
        /// KESのID
        /// </summary>
        [Required]
        public long EksId { get; set; }

        /// <summary>
        /// ノード
        /// </summary>
        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }

        /// <summary>
        /// テナント
        /// </summary>
        [ForeignKey(nameof(EksId))]
        public virtual Eks Eks { get; set; }
    }
}

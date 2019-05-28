using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// ノードとテナントの対応付け中間テーブル。
    /// <seealso cref="Node"/>
    /// </summary>
    public class NodeTenantMap : ModelBase
    {
        /// <summary>
        /// ノードID
        /// </summary>
        [Required]
        public long NodeId { get; set; }

        /// <summary>
        /// テナントID
        /// </summary>
        [Required]
        public long TenantId { get; set; }

        /// <summary>
        /// ノード
        /// </summary>
        [ForeignKey(nameof(NodeId))]
        public virtual Node Node { get; set; }

        /// <summary>
        /// テナント
        /// </summary>
        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }
    }
}

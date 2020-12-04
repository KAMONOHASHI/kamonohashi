using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// テンプレートとテナントの対応付け中間テーブル。
    /// <seealso cref="Template"/>
    /// </summary>
    public class TemplateTenantMap : ModelBase
    {
        /// <summary>
        /// テンプレートID
        /// </summary>
        [Required]
        public long TemplateId { get; set; }

        /// <summary>
        /// テナントID
        /// </summary>
        [Required]
        public long TenantId { get; set; }

        /// <summary>
        /// テンプレート
        /// </summary>
        [ForeignKey(nameof(TemplateId))]
        public virtual ModelTemplate Template{ get; set; }

        /// <summary>
        /// テナント
        /// </summary>
        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }
    }
}

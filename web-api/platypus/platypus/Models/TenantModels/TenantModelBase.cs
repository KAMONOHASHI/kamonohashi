using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.Models.TenantModels
{
    public abstract class TenantModelBase : ModelBase
    {
        /// <summary>
        /// テナントID
        /// </summary>
        [Required]
        public long TenantId { get; set; }

        /// <summary>
        /// テナント。
        /// <see cref="Nssol.Platypus.DataAccess.Core.RepositoryForTenantBase{TModel}"/> の各メソッドは、デフォルトではTenantをIncludeしていないので、使用する際は個別に対応のこと。
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }
    }
}

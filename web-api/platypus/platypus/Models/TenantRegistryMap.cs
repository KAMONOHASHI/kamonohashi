using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// テナントとレジストリをマッピングするテーブル
    /// </summary>
    public class TenantRegistryMap : ModelBase
    {
        /// <summary>
        /// テナントID
        /// </summary>
        [Required]
        public long TenantId { get; set; }

        /// <summary>
        /// レジストリID
        /// </summary>
        /// <remarks>
        /// 将来的にテナント管理者へレジストリ情報の編集権限を与えられるようにした場合、
        /// 管理者側で編集可能なものと負荷の物を区別するために用いる予定
        /// </remarks>
        [Required]
        public long RegistryId { get; set; }

        /// <summary>
        /// 編集可否
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// テナント
        /// </summary>
        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }

        /// <summary>
        /// レジストリ
        /// </summary>
        [ForeignKey(nameof(RegistryId))]
        public virtual Registry Registry { get; set; }
    }
}

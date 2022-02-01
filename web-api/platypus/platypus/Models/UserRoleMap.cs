using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// ユーザとロールの紐づけ
    /// </summary>
    public class UserRoleMap : ModelBase
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// ロールID
        /// </summary>
        [Required]
        public long RoleId { get; set; }

        /// <summary>
        /// UserTenantMap ID。
        /// <see cref="Role"/>がシステムロールの場合、この値はNULLになって、テナントに関係なくそのロールが必ず付与される。
        /// <see cref="Role"/>がテナントロールの場合、この値は非NULLになる。
        /// その場合、<see cref="UserId"/>と<see cref="TenantMap"/>のUserIdは一致している必要があり、
        /// <see cref="Role"/>のテナントIDが非NULLなら<see cref="TenantMap"/>のテナントIDと一致している必要がある。
        /// </summary>
        public long? TenantMapId { get; set; }

        /// <summary>
        /// ユーザ
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// ロール
        /// </summary>
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }

        /// <summary>
        /// テナントとのマッピング情報
        /// </summary>
        [ForeignKey(nameof(TenantMapId))]
        public virtual UserTenantMap TenantMap { get; set; }
    }
}

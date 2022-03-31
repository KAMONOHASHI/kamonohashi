using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// ユーザグループとテナントテーブルの関係を管理するテーブル
    /// </summary>
    public class UserGroupTenantMap : ModelBase
    {
        /// <summary>
        /// ユーザグループID
        /// </summary>
        [Required]
        public long UserGroupId { get; set; }

        /// <summary>
        /// テナントID
        /// </summary>
        [Required]
        public long TenantId { get; set; }

        /// <summary>
        /// ユーザグループ
        /// </summary>
        [ForeignKey(nameof(UserGroupId))]
        public virtual UserGroup UserGroup { get; set; }

        /// <summary>
        /// テナント
        /// </summary>
        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }
    }
}
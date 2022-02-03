using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// ユーザグループとロールテーブルの関係を管理するテーブル
    /// </summary>
    public class UserGroupRoleMap: ModelBase
    {
        /// <summary>
        /// ユーザグループID
        /// </summary>
        [Required]
        public long UserGroupId { get; set; }

        /// <summary>
        /// ロールID
        /// </summary>
        [Required]
        public long RoleId { get; set; }

        /// <summary>
        /// ユーザグループ
        /// </summary>
        [ForeignKey(nameof(UserGroupId))]
        public virtual UserGroup UserGroup { get; set; }

        /// <summary>
        /// ロール
        /// </summary>
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }
    }
}
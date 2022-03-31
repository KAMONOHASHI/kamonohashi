using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// ユーザグループを管理するテーブル
    /// </summary>
    public class UserGroup : ModelBase
    {
        /// <summary>
        /// ユーザグループを識別するための名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// ユーザグループに関する情報をメモとして記述できるようにする。
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 対象ユーザグループがグループか、OUか。
        /// </summary>
        /// <remarks>
        /// true：グループ false：OU
        /// </remarks>
        [Required]
        public bool IsGroup { get; set; }

        /// <summary>
        /// 対象ユーザグループのDN(識別名:DistinguishedName)情報
        /// </summary>
        [Required]
        public string Dn { get; set; }

        /// <summary>
        /// 対象ユーザグループのDN情報の直接的（直下）が対象か、間接的も許可するか。
        /// </summary>
        /// <remarks>
        /// true：直接 false：間接
        /// </remarks>
        [Required]
        public bool IsDirect { get; set; }

        /// <summary>
        /// ロールマッピング情報
        /// </summary>
        public virtual ICollection<UserGroupRoleMap> RoleMaps{ get; set; }
    }
}
using Nssol.Platypus.Infrastructure.Infos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// 権限テーブル
    /// </summary>
    public class Role : ModelBase
    {
        /// <summary>
        /// ロール名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 表示名
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// 並び順。小さいほど前に来る。
        /// </summary>
        [Required]
        public int SortOrder { get; set; }

        /// <summary>
        /// テナントID。
        /// この値が設定されている場合、<see cref="IsSystemRole"/>はTrueにできない
        /// </summary>
        public long? TenantId { get; set; }

        /// <summary>
        /// 管理用ロールか。
        /// ここがfalseの場合、<see cref="MenuRoleMap"/>で<see cref="MenuItemInfo.MenuType"/>がAdminのものと紐づけられない。
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// テナント
        /// </summary>
        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }
    }
}

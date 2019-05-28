using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Infrastructure;

namespace Nssol.Platypus.ApiModels.RoleApiModels
{
    /// <summary>
    /// ノード情報のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(Role role) : base(role)
        {
            Id = role.Id;
            Name = role.Name;
            DisplayName = role.DisplayName;
            IsSystemRole = role.IsSystemRole;
            TenantId = role.TenantId;
            SortOrder = role.SortOrder;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 管理者用ロールか
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// 紐づけられているテナントID。
        /// </summary>
        /// <remarks>
        /// <see cref="IsSystemRole"/>がTrueの場合は、必ずNULL
        /// </remarks>
        public long? TenantId { get; set; }
        /// <summary>
        /// 並び順
        /// </summary>
        public int SortOrder { get; set; }
    }
}

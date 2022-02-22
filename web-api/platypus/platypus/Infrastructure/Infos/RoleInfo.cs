using Nssol.Platypus.Models;
using System.Collections.Generic;

namespace Nssol.Platypus.Infrastructure.Infos
{
    /// <summary>
    /// ロール情報
    /// </summary>
    public class RoleInfo
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RoleInfo() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="role">ロール</param>
        public RoleInfo(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            DisplayName = role.DisplayName;
            IsCustomed = role.TenantId != null;
            SortOrder = role.SortOrder;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="map">ユーザロールマップ</param>
        public RoleInfo(UserRoleMap map)
        {
            Id = map.Role.Id;
            Name = map.Role.Name;
            DisplayName = map.Role.DisplayName;
            IsCustomed = map.Role.TenantId != null;
            SortOrder = map.Role.SortOrder;
            IsOrigin = map.IsOrigin;
            UserGroupTanantMapIdLists = map.UserGroupTenantMapIdList;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ロール名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ロール表示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// カスタムロールか
        /// </summary>
        public bool IsCustomed { get; set; }

        /// <summary>
        /// 並び順。小さいほど前に来る。
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// KQI上での紐づけであればtrue
        /// </summary>
        public bool IsOrigin { get; set; }

        /// <summary>
        /// ユーザグループIDリスト
        /// </summary>
        public List<long> UserGroupTanantMapIdLists { get; set; }
    }
}

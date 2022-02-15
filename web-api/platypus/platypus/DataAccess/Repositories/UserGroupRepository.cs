using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.DataAccess.Repositories
{
    /// <summary>
    /// UserGroup関連テーブルにアクセスするためのリポジトリ。
    /// </summary>
    public class UserGroupRepository : RepositoryBase<UserGroup>, IUserGroupRepository
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private ILogger<UserRepository> logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataContext">DBコンテキスト</param>
        /// <param name="logger">ロガー</param>
        public UserGroupRepository(
            CommonDbContext dataContext,
            ILogger<UserRepository> logger
            ) : base(dataContext)
        {
            this.logger = logger;
        }

        /// <summary>
        /// ユーザグループが紐づいている全テナント情報を取得する。
        /// </summary>
        public IEnumerable<Tenant> GetTenantAllWithUserGroups()
        {
            return GetModelAll<Tenant>()
                .Include(t => t.UserGroupMaps)
                .ThenInclude(map => map.UserGroup)
                .ThenInclude(map => map.RoleMaps)
                .Where(t => t.UserGroupMaps.Count > 0)
                .ToList();
        }

        /// <summary>
        /// 指定したIDのユーザグループ情報をロール情報付きで取得する。
        /// </summary>
        /// <param name="id">ユーザグループID</param>
        public UserGroup GetUserGroupById(long id)
        {
            return GetAll()
                .Include(u => u.RoleMaps)
                .ThenInclude(map => map.Role)
                .FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// テナントに紐づく全ユーザグループ情報を取得する。
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        public IEnumerable<UserGroup> GetUserGroupsAllFromTenant(long tenantId)
        {
            return GetModelAll<UserGroupTenantMap>()
                .Include(map => map.UserGroup)
                .Where(map => map.TenantId == tenantId)
                .Select(map => map.UserGroup);
        }

        /// <summary>
        /// ユーザグループにロールマップ情報を紐づける
        /// </summary>
        /// <param name="userGroup">ユーザグループ</param>
        /// <param name="roles">ロール</param>
        public void AttachRoleMap(UserGroup userGroup, IEnumerable<Role> roles)
        {
            userGroup.RoleMaps = new List<UserGroupRoleMap>();

            // UserGroupRoleMapから該当グループのレコードを削除する
            DeleteModelAll<UserGroupRoleMap>(map => map.UserGroup.Id == userGroup.Id);

            foreach (var role in roles)
            {
                var map = new UserGroupRoleMap()
                {
                    UserGroup = userGroup,
                    RoleId = role.Id
                };
                userGroup.RoleMaps.Add(map);
            }
        }

        /// <summary>
        /// テナントとユーザグループを紐づける
        /// </summary>
        /// <param name="tenant">テナント</param>
        /// <param name="userGroup">ユーザグループ</param>
        public void AttachUserGroupToTenant(Tenant tenant, UserGroup userGroup)
        {
            var map = new UserGroupTenantMap()
            {
                Tenant = tenant,
                UserGroupId = userGroup.Id
            };
            AddModel(map);
        }

        /// <summary>
        /// テナントとユーザグループの紐づけを解除する。
        /// </summary>
        /// <param name="tenant">テナント</param>
        /// <param name="userGroup">ユーザグループ</param>
        public void DetachUserGroupFromTenant(Tenant tenant, UserGroup userGroup)
        {
            DeleteModelAll<UserGroupTenantMap>(map => map.TenantId == tenant.Id && map.UserGroupId == userGroup.Id);
        }
    }
}

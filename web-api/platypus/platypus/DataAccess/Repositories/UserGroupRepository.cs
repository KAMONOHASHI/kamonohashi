using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.DataAccess.Repositories
{
    public class UserGroupRepository : RepositoryBase<UserGroup>, IUserGroupRepository
    {
        private ILogger<UserRepository> logger;

        public UserGroupRepository(
            CommonDbContext dataContext,
            ILogger<UserRepository> logger
            ) : base(dataContext)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 全ユーザグループ情報をロール情報付きで取得する。
        /// </summary>
        public IEnumerable<UserGroup> GetUserGroupsAllWithRoles()
        {
            return GetAll()
                .Include(u => u.RoleMaps)
                .ThenInclude(ur => ur.Role)
                .OrderBy(u => u.Id);
        }

        /// <summary>
        /// テナントに紐づく全ユーザグループ情報を取得する。
        /// </summary>
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
        public void AttachUserGroupToTenant(Tenant tenant, UserGroup userGroup)
        {
            var map = new UserGroupTenantMap()
            {
                Tenant = tenant,
                UserGroupId = userGroup.Id
            };
            //tenant.UserGroupMaps.Add(map);
            AddModel<UserGroupTenantMap>(map);
        }

        /// <summary>
        /// テナントとユーザグループの紐づけを解除する。
        /// </summary>
        public void DetachUserGroupFromTenant(Tenant tenant, UserGroup userGroup)
        {
            DeleteModelAll<UserGroupTenantMap>(map => map.TenantId == tenant.Id && map.UserGroupId == userGroup.Id);
        }
    }
}

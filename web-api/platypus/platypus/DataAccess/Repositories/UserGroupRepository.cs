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
        private IRoleRepository roleRepository;
        private ILogger<UserRepository> logger;

        public UserGroupRepository(
            IRoleRepository roleRepository,
            CommonDbContext dataContext,
            ILogger<UserRepository> logger
            ) : base(dataContext)
        {
            this.roleRepository = roleRepository;
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
                .OrderBy(u => u.Id)
                .ToList();
        }

        /// <summary>
        /// ユーザグループにロールマップ情報を紐づける
        /// </summary>
        public async void AttachRoleMap(UserGroup userGroup, IEnumerable<long> roles)
        {
            userGroup.RoleMaps = new List<UserGroupRoleMap>();

            // UserGroupRoleMapから該当グループのレコードを削除する
            DeleteModelAll<UserGroupRoleMap>(map => map.UserGroup.Id == userGroup.Id);

            foreach (var roleId in roles)
            {
                // ロールの存在チェック
                Role role = await roleRepository.GetRoleAsync(roleId);
                if (role == null)
                {
                    continue;
                }
                // システムロールは登録できないようにする
                if (role.IsSystemRole)
                {
                    continue;
                    //throw new UnauthorizedAccessException("tttt");
                }

                var map = new UserGroupRoleMap()
                {
                    UserGroup = userGroup,
                    Role = role
                };
                userGroup.RoleMaps.Add(map);
            }
        }
    }
}

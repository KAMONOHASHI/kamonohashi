using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System.Collections.Generic;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    public interface IUserGroupRepository : IRepository<UserGroup>
    {
        /// <summary>
        /// 全ユーザグループ情報をロール情報付きで取得する。
        /// </summary>
        IEnumerable<UserGroup> GetUserGroupsAllWithRoles();

        /// <summary>
        /// ユーザグループにロールマップ情報を紐づける
        /// </summary>
        void AttachRoleMap(UserGroup userGroup, IEnumerable<Role> roles);
    }
}

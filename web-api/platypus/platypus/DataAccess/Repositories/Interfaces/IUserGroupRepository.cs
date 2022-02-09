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
        /// 指定したIDのユーザグループ情報をロール情報付きで取得する。
        /// </summary>
        UserGroup GetUserGroupById(long id);

        /// <summary>
        /// テナントに紐づく全ユーザグループ情報を取得する。
        /// </summary>
        IEnumerable<UserGroup> GetUserGroupsAllFromTenant(long tenantId);

        /// <summary>
        /// ユーザグループにロールマップ情報を紐づける
        /// </summary>
        void AttachRoleMap(UserGroup userGroup, IEnumerable<Role> roles);

        /// <summary>
        /// テナントとユーザグループを紐づける
        /// </summary>
        void AttachUserGroupToTenant(Tenant tenant, UserGroup userGroup);

        /// <summary>
        /// テナントとユーザグループの紐づけを解除する。
        /// </summary>
        void DetachUserGroupFromTenant(Tenant tenant, UserGroup userGroup);
    }
}

using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System.Collections.Generic;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// UserGroup関連テーブルにアクセスするためのリポジトリインターフェース。
    /// </summary>
    public interface IUserGroupRepository : IRepository<UserGroup>
    {
        /// <summary>
        /// ユーザグループが紐づいている全テナント情報を取得する。
        /// </summary>
        IEnumerable<Tenant> GetTenantAllWithUserGroups();

        /// <summary>
        /// 指定したIDに紐づくテナント情報を取得する。
        /// </summary>
        /// <param name="userGroupId">ユーザグループID</param>
        IEnumerable<Tenant> GetTenantsByUserGroup(long userGroupId);

        /// <summary>
        /// 指定したIDのユーザグループ情報をロール情報付きで取得する。
        /// </summary>
        /// <param name="id">ユーザグループID</param>
        UserGroup GetUserGroupById(long id);

        /// <summary>
        /// テナントに紐づく全ユーザグループ情報を取得する。
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        IEnumerable<UserGroup> GetUserGroupsAllFromTenant(long tenantId);

        /// <summary>
        /// ユーザグループにロールマップ情報を紐づける
        /// </summary>
        /// <param name="userGroup">ユーザグループ</param>
        /// <param name="roles">ロール</param>
        void AttachRoleMap(UserGroup userGroup, IEnumerable<Role> roles);

        /// <summary>
        /// テナントとユーザグループを紐づける
        /// </summary>
        /// <param name="tenant">テナント</param>
        /// <param name="userGroup">ユーザグループ</param>
        void AttachUserGroupToTenant(Tenant tenant, UserGroup userGroup);

        /// <summary>
        /// テナントとユーザグループの紐づけを解除する。
        /// </summary>
        /// <param name="tenant">テナント</param>
        /// <param name="userGroup">ユーザグループ</param>
        void DetachUserGroupFromTenant(Tenant tenant, UserGroup userGroup);
    }
}

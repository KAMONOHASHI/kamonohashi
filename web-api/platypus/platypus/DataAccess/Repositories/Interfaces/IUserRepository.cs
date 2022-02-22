using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        #region User

        /// <summary>
        /// 全ユーザ情報をテナント付きで取得する
        /// </summary>
        IEnumerable<User> GetAllUsersWithTenant();

        /// <summary>
        /// sidに紐づく全ての情報を取得する。
        /// 指定したユーザが存在しない場合、NULLが返る。
        /// </summary>
        User GetUser(string accountName);

        /// <summary>
        /// 指定した別名からユーザ名を取得する。
        /// 指定した別名のユーザが存在しない場合、NULLが返る。
        /// </summary>
        /// <param name="nameAlias">別名</param>
        String GetUserName(string nameAlias);

        /// <summary>
        /// 指定したユーザ情報に、所属テナント・ロール情報を紐づけ、<see cref="UserInfo"/>オブジェクトとして取得する
        /// ユーザの存在チェックはしない。
        /// </summary>
        UserInfo GetUserInfo(User user);

        /// <summary>
        /// ユーザをテナント＆ロール付きで取得する。
        /// 指定したユーザが存在しない場合、NULLが返る。
        /// </summary>
        Task<UserInfo> GetUserInfoAsync(string accountName);

        /// <summary>
        /// ユーザを追加する
        /// </summary>
        void AddUser(User user);

        /// <summary>
        /// LDAPユーザを新規追加する。
        /// LDAPユーザはテナント選択ができないため、Sandboxテナントにロールなしで紐づける。
        /// </summary>
        void AddLdapUser(string userName);

        /// <summary>
        /// ユーザにサンドボックステナントを紐づける
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        void AttachSandbox(User user);

        /// <summary>
        /// ユーザを削除する
        /// </summary>
        void DeleteUser(User user);
        #endregion

        #region TenantUser

        /// <summary>
        /// 指定したテナントに所属しているユーザを取得する。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        IEnumerable<User> GetUsers(long tenantId);

        /// <summary>
        /// 指定したテナントにLdap経由で所属しているユーザを取得する。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        IEnumerable<User> GetLdapUsers(long tenantId);

        /// <summary>
        /// 指定したユーザが所属しているLDAPで参加したテナントを取得する。
        /// </summary>
        IEnumerable<Tenant> GetTenantByUser(long userId);

        /// <summary>
        /// 指定したユーザが当該テナントに所属しているか
        /// </summary>
        Task<bool> IsMemberAsync(long userId, long tenantId);

        /// <summary>
        /// 指定したユーザが当該テナントにKQI経由で所属しているか
        /// </summary>
        bool IsOriginMember(long userId, long tenantId);

        /// <summary>
        /// 指定したユーザが所属しているテナントを取得する。
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="tenantId">テナントID</param>
        public UserTenantMap FindUserTenantMap(long userId, long tenantId);

        /// <summary>
        /// ユーザをテナントに所属させる。
        /// ユーザIDやテナントIDの存在チェックは行わない。
        /// 結果として、作成したすべての<see cref="UserTenantRegistryMap"/>を返す。
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        /// <param name="tenantId">対象テナントID</param>
        /// <param name="roles">テナントロール</param>
        /// <param name="isOrigin">KQI上での紐づけならtrue</param>
        /// <param name="userGroupIds">ユーザグループIDs</param>
        /// <exception cref="ArgumentException"><paramref name="roles"/>にシステムロールが含まれていたり、別テナント用のロールが含まれていた場合</exception>
        IEnumerable<UserTenantRegistryMap> AttachTenant(User user, long tenantId, IEnumerable<Role> roles, bool isOrigin, List<long> userGroupIds);

        /// <summary>
        /// すでにユーザがテナントに所属しているときテナント所属情報を更新する。
        /// ユーザIDやテナントIDの存在チェックは行わない。
        /// 結果として、作成したすべての<see cref="UserTenantRegistryMap"/>を返す。
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        /// <param name="tenantId">対象テナントID</param>
        /// <param name="roles">テナントロール</param>
        /// <param name="isOrigin">KQI上での紐づけならtrue</param>
        /// <param name="userGroupIds">ユーザグループIDs</param>
        /// <exception cref="ArgumentException"><paramref name="roles"/>にシステムロールが含まれていたり、別テナント用のロールが含まれていた場合</exception>
        IEnumerable<UserTenantRegistryMap> UpdateTenant(User user, long tenantId, IEnumerable<Role> roles, bool isOrigin, List<long> userGroupIds);

        /// <summary>
        /// ユーザをテナントから外す。
        /// ユーザIDやテナントIDの存在チェック、および所属済みかのチェックは行わない。
        /// </summary>
        /// <param name="userId">対象ユーザID</param>
        /// <param name="tenantId">対象テナントID</param>
        /// <param name="temporary">一時的な削除で再度紐づけなおす場合はtrue</param>
        void DetachTenant(long userId, long tenantId, bool temporary);

        /// <summary>
        /// 指定したテナントについて、ユーザのロールを変更する。
        /// ユーザIDやテナントIDの存在チェック、および所属済みかのチェックは行わない。
        /// </summary>
        /// <param name="userId">対象ユーザID</param>
        /// <param name="tenantId">対象テナントID</param>
        /// <param name="roles">テナントロール</param>
        /// <param name="isOrigin">KQI上での紐づけならtrue</param>
        /// <exception cref="ArgumentException"><paramref name="roles"/>にシステムロールが含まれていたり、別テナント用のロールが含まれていた場合</exception>
        void ChangeTenantRole(long userId, long tenantId, IEnumerable<Role> roles, bool isOrigin);

        /// <summary>
        /// 指定したテナントについて、ユーザのLdap経由で付与されたロール情報を更新する。
        /// </summary>
        /// <param name="userId">>対象ユーザID</param>
        /// <param name="tenantId">対象テナントID</param>
        void UpdateLdapRole(long userId, long tenantId);

        /// <summary>
        /// 指定したユーザ、テナントに対するクラスタトークンを取得する
        /// </summary>
        string GetClusterToken(long userId, long tenantId);

        /// <summary>
        /// 指定したユーザ、テナントに対するクラスタトークンを登録する
        /// </summary>
        void SetClusterToken(long userId, long tenantId, string token);

        /// <summary>
        /// 指定したユーザに別名を付与する
        /// </summary>
        Task<User> SetAliasAsync(long userId, string nameAlias);
        #endregion
    }
}

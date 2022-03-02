using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// ユーザグループロジッククラス。
    /// <para>
    /// ADアクセスは以下理由からサービス層には切り出さず、ロジック層内で行う。
    /// <list type="bullet">
    /// <item>認証系をAD以外に変更する予定がない</item>
    /// <item>処理が少ない</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <seealso cref="Nssol.Platypus.Logic.Interfaces.IUserGroupLogic" />
    public class UserGroupLogic : PlatypusLogicBase, IUserGroupLogic
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUserGroupRepository userGroupRepository;
        private readonly ActiveDirectoryOptions adOptions;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserGroupLogic(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserGroupRepository userGroupRepository,
            ICommonDiLogic commonDiLogic,
            IOptions<ActiveDirectoryOptions> adOptions) : base(commonDiLogic)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userGroupRepository = userGroupRepository;
            this.adOptions = adOptions.Value;
        }

        /// <summary>
        /// 所属しているLdapグループから所属テナントを更新する
        /// </summary>
        /// <param name="entry">LDAPエントリ</param>
        /// <param name="user">ユーザ</param>
        /// <param name="LdapUserName">LDAPサーバ認証用ユーザ名</param>
        /// <param name="password">LDAPサーバ認証用パスワード</param>
        public async Task AddTenantFromGroup(LdapEntry entry, User user, string LdapUserName, string password)
        {
            // 削除用にグループ経由で参加したテナントを取得する
            var deleteTenants = userRepository.GetTenantByUser(user.Id).ToList();

            // ユーザ情報の取得
            var ldapGroups = entry.getAttribute("memberOf") != null ? entry.getAttribute("memberOf").StringValueArray : Array.Empty<string>();
            var dn = entry.getAttribute("distinguishedName").StringValue;

            // DNから先頭の"CN= ,"を排除
            var ou = Regex.Replace(dn, @"CN=.*?,", "");

            // ユーザグループが紐づいている全テナント取得
            var tenants = userGroupRepository.GetTenantAllWithUserGroups();

            foreach (var tenant in tenants)
            {
                var roleIds = new List<long>();
                var userGroupTenantMapIds = new List<long>();
                foreach (var userGroupMap in tenant.UserGroupMaps)
                {
                    if (userGroupMap.UserGroup.IsGroup)
                    {
                        // グループの時の判定処理
                        if (userGroupMap.UserGroup.IsDirect)
                        {
                            // 直接検索のとき
                            foreach (var ldapGroup in ldapGroups)
                            {
                                // ldapから取得したグループのDnとユーザグループのDnを比較
                                if (ldapGroup == userGroupMap.UserGroup.Dn)
                                {
                                    // ロール情報取得
                                    roleIds.AddRange(userGroupMap.UserGroup.RoleMaps.Select(map => map.RoleId).ToList());
                                    userGroupTenantMapIds.Add(userGroupMap.Id);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // 間接検索のとき
                            try
                            {
                                // Ldap接続
                                using (var conn = new LdapConnection())
                                {
                                    conn.Connect(adOptions.Server, adOptions.Port);
                                    var loginDN = $"{LdapUserName}@{adOptions.Domain}";
                                    conn.Bind(loginDN, password);

                                    string searchFilter = string.Format(adOptions.LdapGroupFilter, user.Name, userGroupMap.UserGroup.Dn);
                                    var result = conn.Search(
                                        this.adOptions.BaseDn,
                                        LdapConnection.SCOPE_SUB,
                                        searchFilter,
                                        Array.Empty<string>(),
                                        false
                                    );

                                    // 検索でヒットしなかったときはここで例外が吐かれる
                                    result.next();

                                    // ロール情報取得
                                    roleIds.AddRange(userGroupMap.UserGroup.RoleMaps.Select(map => map.RoleId).ToList());
                                    userGroupTenantMapIds.Add(userGroupMap.Id);
                                }
                            }
                            catch (LdapException e)
                            {
                                // ここは何もしない
                            }
                        }
                    }
                    else
                    {
                        // OUの時の判定処理
                        if (userGroupMap.UserGroup.IsDirect)
                        {
                            // 直接のとき
                            if (ou == userGroupMap.UserGroup.Dn)
                            {
                                // ロール情報取得
                                roleIds.AddRange(userGroupMap.UserGroup.RoleMaps.Select(map => map.RoleId).ToList());
                                userGroupTenantMapIds.Add(userGroupMap.Id);
                            }
                        }
                        else
                        {
                            // 間接のとき
                            // ユーザのDNとOUのDNを部分一致で比較する
                            if (Regex.IsMatch(dn, $@".*{userGroupMap.UserGroup.Dn}$"))
                            {
                                // ロール情報取得
                                roleIds.AddRange(userGroupMap.UserGroup.RoleMaps.Select(map => map.RoleId).ToList());
                                userGroupTenantMapIds.Add(userGroupMap.Id);
                            }
                            else
                            {
                                // memberOfのDNとOUのDNを部分一致で比較する
                                foreach(var ldapGroup in ldapGroups)
                                {
                                    if (Regex.IsMatch(ldapGroup, $@".*{userGroupMap.UserGroup.Dn}$"))
                                    {
                                        // ロール情報取得
                                        roleIds.AddRange(userGroupMap.UserGroup.RoleMaps.Select(map => map.RoleId).ToList());
                                        userGroupTenantMapIds.Add(userGroupMap.Id);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                // 配列が空ではないとき、テナントに参加する
                if (roleIds.Count > 0)
                {
                    // 既にテナントに参加しているかチェック
                    if (!await userRepository.IsMemberAsync(user.Id, tenant.Id))
                    {
                        // ロールの重複削除
                        roleIds = roleIds.Distinct().ToList();

                        //ロールについての存在＆入力チェック
                        var roles = new List<Role>();

                        foreach (long roleId in roleIds)
                        {
                            var role = await roleRepository.GetRoleAsync(roleId);
                            if (role == null)
                            {
                                //ロールがない
                                continue;
                            }
                            if (role.IsSystemRole)
                            {
                                //システムロールをテナントロールとして追加しようとしている
                                continue;
                            }
                            roles.Add(role);
                        }
                        // テナント参加
                        userRepository.AttachTenant(user, tenant.Id, roles, false, userGroupTenantMapIds);
                        // ログ出力
                        LogInformation($"ユーザ{user.Name}をテナント{tenant.Name}へ紐づけました。");
                    }
                    else
                    {
                        // UserGroupTenantMapIdsカラムの更新
                        userRepository.UpdateTenant(user, tenant.Id, null, false, userGroupTenantMapIds);
                        // ロールの更新
                        userRepository.UpdateLdapRole(user.Id, tenant.Id);

                        deleteTenants.Remove(tenant);
                    }
                }
            }
            // 残ったものは削除
            foreach (var deleteTenant in deleteTenants)
            {
                // KQI経由で参加している場合はテナント脱退しない
                if(userRepository.IsOriginMember(user.Id, deleteTenant.Id))
                {
                    // UserGroupTenantMapIdsカラムをnullにする
                    userRepository.UpdateTenant(user, deleteTenant.Id, null, false, null);
                    // ロールの更新
                    userRepository.UpdateLdapRole(user.Id, deleteTenant.Id);
                }
                else
                {
                    userRepository.DetachTenant(user.Id, deleteTenant.Id, false);
                }
            }
        }
    }
}

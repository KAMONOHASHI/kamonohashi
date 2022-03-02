using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        /// <summary>
        /// テナントリポジトリ
        /// </summary>
        private ITenantRepository tenantRepository;
        /// <summary>
        /// ロールリポジトリ
        /// </summary>
        private IRoleRepository roleRepository;
        private ILogger<UserRepository> logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserRepository(
            ITenantRepository tenantRepository,
            IRoleRepository roleRepository,
            CommonDbContext dataContext,
            ILogger<UserRepository> logger
            ) : base(dataContext)
        {
            this.tenantRepository = tenantRepository;
            this.roleRepository = roleRepository;
            this.logger = logger;
        }

        #region User

        /// <summary>
        /// 全ユーザ情報をテナント付きで取得する
        /// </summary>
        public IEnumerable<User> GetAllUsersWithTenant()
        {
            return GetAll().Include(u => u.TenantMaps).OrderBy(u => u.Name).ToList();
        }

        /// <summary>
        /// sidに紐づくユーザを取得する。
        /// 指定したユーザが存在しない場合、NULLが返る。
        /// </summary>
        public User GetUser(string accountName)
        {
            var user = Find(u => u.Name == accountName);
            return user;
        }

        /// <summary>
        /// 指定した別名からユーザ名を取得する。
        /// 指定した別名のユーザが存在しない場合、Unknownを返す。
        /// </summary>
        /// <param name="nameAlias">別名</param>
        public String GetUserName(string nameAlias)
        {
            var user = GetAll()
                .AsEnumerable()
                .FirstOrDefault(u => string.IsNullOrEmpty(u.Alias) == false
                    && u.Alias.Contains(nameAlias, StringComparison.CurrentCulture));
            if (user == null)
            {
                return "Unknown";
            }
            return user.Name;
        }

        /// <summary>
        /// 指定したユーザ情報に、所属テナント・ロール情報を紐づけ、<see cref="UserInfo"/>オブジェクトとして取得する
        /// ユーザの存在チェックはしない。
        /// </summary>
        public UserInfo GetUserInfo(User user)
        {
            if (user == null)
            {
                return null;
            }
            UserInfo userInfo;
            if (user.TenantMaps == null)
            {
                var tmpUser = GetAll().Include(u => u.TenantMaps).FirstOrDefault(u => u.Id == user.Id);
                userInfo = AttachRelations(tmpUser);
            }
            else
            {
                userInfo = AttachRelations(user);
            }
            return userInfo;
        }

        /// <summary>
        /// ユーザをテナント＆ロール付きで取得する。
        /// 指定したユーザが存在しない場合、NULLが返る。
        /// </summary>
        public async Task<UserInfo> GetUserInfoAsync(string accountName)
        {
            var user = await GetAll().Include(u => u.TenantMaps).FirstOrDefaultAsync(u => u.Name == accountName);
            if (user == null)
            {
                return null;
            }
            var userInfo = AttachRelations(user);
            return userInfo;
        }

        /// <summary>
        /// 指定されたUser情報と、キャッシュされたRole/Tenantデータから、UserInfoを作成する
        /// </summary>
        private UserInfo AttachRelations(User user)
        {
            var userInfo = new UserInfo()
            {
                Id = user.Id,
                Name = user.Name,
                Alias = user.Alias,
                ServiceType = user.ServiceType,
                SlackUrl = user.SlackUrl,
                Mention = user.Mention
            };

            userInfo.SystemRoles = roleRepository.GetSystemRoles(user.Id);

            var tenantDic = new Dictionary<Tenant, List<RoleInfo>>();

            Dictionary<long, List<RoleInfo>> roles = roleRepository.GetTenantRolesDictionary(user.Id);

            // マッピング情報を一件ずつ参照して、引数のuserを更新していく
            foreach (UserTenantMap mapping in user.TenantMaps)
            {
                Tenant tenant = tenantRepository.Get(mapping.TenantId);
                if (tenant == null)
                {
                    //マップにあるテナント情報が存在しない＝キャッシュとの間に不整合がある、と見なす
                    tenantRepository.Refresh();
                    tenant = tenantRepository.Get(mapping.TenantId); //リフレッシュしてやり直し
                }

                if (roles.ContainsKey(tenant.Id))
                {
                    tenantDic.Add(tenant, roles[tenant.Id]);
                }
                else
                {
                    //所属しているけどロールが一つもない状態
                    tenantDic.Add(tenant, new List<RoleInfo>());
                }

                //デフォルトテナントと一致していたら、userInfoに登録
                if (tenant.Id == user.DefaultTenantId)
                {
                    userInfo.DefaultTenant = tenant;
                }
            }

            userInfo.TenantDic = tenantDic;
            return userInfo;
        }

        /// <summary>
        /// ユーザを追加する
        /// </summary>
        public void AddUser(User user)
        {
            Add(user);
        }

        /// <summary>
        /// LDAPユーザを新規追加する。
        /// LDAPユーザはテナント選択ができないため、Sandboxテナントにロールなしで紐づける。
        /// </summary>
        public void AddLdapUser(string userName)
        {
            var user = new User()
            {
                Name = userName,
                ServiceType = AuthServiceType.Ldap
            };
            AttachSandbox(user);
            Add(user);
        }

        /// <summary>
        /// ユーザにサンドボックステナントを紐づける
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        public void AttachSandbox(User user)
        {
            var tenant = tenantRepository.GetFromTenantName(ApplicationConst.DefaultFirstTenantName);

            //ロールを紐づける。userとresearcherをデフォルトにするが、どっちも存在しなければ最初の一個。
            var attachedRoles = new List<Role>();
            var roles = roleRepository.GetCommonTenantRolesAsync().Result;
            if (roles.Any(r => r.Name == "users"))
            {
                attachedRoles.Add(roles.First(r => r.Name == "users"));
            }
            if (roles.Any(r => r.Name == "researchers"))
            {
                attachedRoles.Add(roles.First(r => r.Name == "researchers"));
            }
            if (attachedRoles.Count == 0)
            {
                attachedRoles.Add(roles.First());
            }

            AttachTenant(user, tenant.Id, attachedRoles, true, null);
            user.DefaultTenantId = tenant.Id;
        }

        /// <summary>
        /// ユーザを削除する。
        /// 紐づいている<see cref="UserRoleMap"/>、<see cref="UserTenantMap"/>もすべて削除する。
        /// </summary>
        public void DeleteUser(User user)
        {
            //まずはロールを外す
            DeleteModelAll<UserRoleMap>(map => map.UserId == user.Id);
            //続いてテナントから外す
            DeleteModelAll<UserTenantMap>(map => map.UserId == user.Id);
            //最後にユーザ自身を消す
            Delete(user);
        }

        #endregion

        #region UserTenant

        /// <summary>
        /// 指定したテナントに所属しているユーザを取得する。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        public IEnumerable<User> GetUsers(long tenantId)
        {
            return GetModelAll<UserTenantMap>().Where(map => map.TenantId == tenantId).Include(map => map.User).Select(map => map.User);
        }

        /// <summary>
        /// 指定したテナントにLdap経由で所属しているユーザを取得する。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        public IEnumerable<User> GetLdapUsers(long tenantId)
        {
            return GetModelAll<UserTenantMap>().Where(map => map.TenantId == tenantId && map.UserGroupTenantMapIds != null).Select(map => map.User);
        }

        /// <summary>
        /// 指定したユーザが所属しているLDAPで参加したテナントを取得する。
        /// </summary>
        public IEnumerable<Tenant> GetTenantByUser(long userId)
        {
            return GetModelAll<UserTenantMap>().Where(map => map.UserId == userId && map.UserGroupTenantMapIds != null).Include(map => map.Tenant).Select(map => map.Tenant);
        }

        /// <summary>
        /// 指定したユーザが当該テナントに所属しているか
        /// </summary>
        public async Task<bool> IsMemberAsync(long userId, long tenantId)
        {
            return await ExistsModelAsync<UserTenantMap>(map => map.UserId == userId && map.TenantId == tenantId);
        }

        /// <summary>
        /// 指定したユーザが当該テナントにKQI経由で所属しているか
        /// </summary>
        public bool IsOriginMember(long userId, long tenantId)
        {
            return GetModelAll<UserTenantMap>().FirstOrDefault(map => map.UserId == userId && map.TenantId == tenantId).IsOrigin;
        }
        
        /// <summary>
        /// 指定したユーザIDとテナントIDのマップを取得する。
        /// </summary>
        public UserTenantMap FindUserTenantMap(long userId, long tenantId)
        {
            return FindModel<UserTenantMap>(map => map.UserId == userId && map.TenantId == tenantId);
        }

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
        public IEnumerable<UserTenantRegistryMap> AttachTenant(User user, long tenantId, IEnumerable<Role> roles, bool isOrigin, List<long> userGroupIds)
        {
            var tenantMap = new UserTenantMap()
            {
                TenantId = tenantId,
                User = user,
                IsOrigin = isOrigin,
                UserGroupTenantMapIds = userGroupIds == null ? null : JsonConvert.SerializeObject(userGroupIds)
            };
            AddModel<UserTenantMap>(tenantMap);
            if (roles != null)
            {
                if (isOrigin)
                {
                    foreach (var role in roles)
                    {
                        if (role == null)
                        {
                            continue;
                        }
                        if (role.IsSystemRole)
                        {
                            //Adminロールを特定テナントに所属させようとしている
                            throw new UnauthorizedAccessException($"The tenant role {role.Name} is not assigned to a user as a system role.");
                        }

                        var roleMap = new UserRoleMap()
                        {
                            RoleId = role.Id,
                            TenantMap = tenantMap,
                            User = user,
                            IsOrigin = isOrigin,
                            UserGroupTenantMapIds = null,
                        };

                        AddModel<UserRoleMap>(roleMap);
                    }
                }
                else
                {
                    // ユーザグループからロールを取得
                    var userGroupRoleDictionary = new Dictionary<long, List<long>>();
                    foreach (var userGroupId in userGroupIds)
                    {
                        // ユーザグループの取得
                        var userGroupMap = GetModelAll<UserGroupTenantMap>().Include(map => map.UserGroup).ThenInclude(u => u.RoleMaps).FirstOrDefault(map => map.Id == userGroupId);

                        foreach (var roleId in userGroupMap.UserGroup.RoleMaps.Select(map => map.RoleId))
                        {
                            // 既に紐づいているかチェック
                            if (userGroupRoleDictionary.ContainsKey(roleId))
                            {
                                userGroupRoleDictionary[roleId].Add(userGroupId);
                            }
                            else
                            {
                                userGroupRoleDictionary.Add(roleId, new List<long> { userGroupId });
                            }
                        }
                    }
                    // ロールマップに登録
                    foreach (var ldapRoleId in userGroupRoleDictionary.Keys)
                    {
                        var roleMap = new UserRoleMap()
                        {
                            RoleId = ldapRoleId,
                            TenantMap = tenantMap,
                            User = user,
                            IsOrigin = isOrigin,
                            UserGroupTenantMapIds = JsonConvert.SerializeObject(userGroupRoleDictionary[ldapRoleId])
                        };
                        AddModel<UserRoleMap>(roleMap);
                    }
                }
            }

            //まずはGitの登録
            //テナントに紐づいているすべてのGitを取得
            var GitMaps = FindModelAll<TenantGitMap>(m => m.TenantId == tenantId).Include(m => m.Git).ToList();
            foreach (var GitMap in GitMaps)
            {
                UserTenantGitMap utrMap = new UserTenantGitMap()
                {
                    TenantGitMap = GitMap,
                    User = user,
                    IsOrigin = isOrigin,
                    UserGroupTenantMapIds = userGroupIds == null ? null : JsonConvert.SerializeObject(userGroupIds)
                };

                // 既存の認証情報存在チェック
                var existMap = GetModelAll<UserTenantGitMap>()
                    .Where(m => m.UserId == user.Id && m.TenantGitMapId == GitMap.Id).FirstOrDefault();
                if (existMap != null)
                {
                    // 既存の認証情報が存在する場合、再設定する
                    utrMap.GitToken = existMap.GitToken;
                }
                else
                {
                    // UserTenantGitMap において userId と TenantGitMapId のペアが存在しなければ、エントリ新規追加のためログに出力する
                    LogDebug($"UserTenantGitMap エントリの新規追加 : UserId={user.Id}, TenantGitMapId={GitMap.Id}, TenantId={tenantId}, GitId={GitMap.GitId}");
                }

                AddModel<UserTenantGitMap>(utrMap);
            }

            //続いてレジストリの登録
            //レジストリ登録はクラスタ管理サービスへも影響するので、作成したMapを全て返す

            List<UserTenantRegistryMap> maps = new List<UserTenantRegistryMap>();

            //テナントに紐づいているすべてのレジストリを取得
            var registryMaps = FindModelAll<TenantRegistryMap>(m => m.TenantId == tenantId).Include(m => m.Registry).ToList();
            foreach (var registryMap in registryMaps)
            {
                UserTenantRegistryMap utrMap = new UserTenantRegistryMap()
                {
                    TenantRegistryMap = registryMap,
                    User = user,
                    IsOrigin = isOrigin,
                    UserGroupTenantMapIds = userGroupIds == null ? null : JsonConvert.SerializeObject(userGroupIds)
                };

                // 既存の認証情報存在チェック
                var existMap = GetModelAll<UserTenantRegistryMap>()
                    .Where(m => m.UserId == user.Id && m.TenantRegistryMapId == registryMap.Id).FirstOrDefault();
                if (existMap != null)
                {
                    // 既存の認証情報が存在する場合、再設定する
                    utrMap.RegistryUserName = existMap.RegistryUserName;
                    utrMap.RegistryPassword = existMap.RegistryPassword;
                }

                AddModel<UserTenantRegistryMap>(utrMap);
                maps.Add(utrMap);
            }
            return maps;
        }

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
        public IEnumerable<UserTenantRegistryMap> UpdateTenant(User user, long tenantId, IEnumerable<Role> roles, bool isOrigin, List<long> userGroupIds)
        {
            // 引数のユーザグループを加工して準備
            if (userGroupIds != null && userGroupIds.Count() > 0)
            {
                userGroupIds = userGroupIds.Distinct().ToList();
            }

            // ユーザとテナントの紐づけを更新
            var userTenantMap = FindUserTenantMap(user.Id, tenantId);
            if (isOrigin)
            {
                // KQI上での紐づけとする場合、trueを設定する。
                userTenantMap.IsOrigin = true;
            }
            else
            {
                // ユーザグループ経由での紐づけ情報を更新
                userTenantMap.UserGroupTenantMapIds =
                    userGroupIds != null && userGroupIds.Count() > 0
                    ? JsonConvert.SerializeObject(userGroupIds)
                    : null;
            }

            // ロールについての処理
            if (roles != null)
            {
                // 更新前のユーザとロールの紐づけ一覧を取得しておく。
                var currentRoleMaps = GetModelAll<UserRoleMap>().Where(m => m.TenantMapId == userTenantMap.Id);

                foreach (var role in roles)
                {
                    if (role == null)
                    {
                        continue;
                    }
                    if (role.IsSystemRole)
                    {
                        //Adminロールを特定テナントに所属させようとしている
                        throw new UnauthorizedAccessException($"The tenant role {role.Name} is not assigned to a user as a system role.");
                    }
                    // ユーザとロールの紐づけを更新
                    var userRoleMap = FindModel<UserRoleMap>(m => m.TenantMapId == userTenantMap.Id && m.RoleId == role.Id);
                    if (userRoleMap != null)
                    {
                        // 紐づけ情報が存在する場合更新する
                        if (isOrigin)
                        {
                            // KQI上での紐づけとする場合、trueを設定する。
                            userRoleMap.IsOrigin = true;
                        }
                        else
                        {
                            userRoleMap.UserGroupTenantMapIds = userGroupIds != null && userGroupIds.Count() > 0 ? JsonConvert.SerializeObject(userGroupIds) : null;
                        }

                        // 編集前の情報から対象を除く。
                        currentRoleMaps = currentRoleMaps.Where(m => m.Id != userRoleMap.Id);
                    }
                    else
                    {
                        // 紐づけ情報が存在しない場合新規登録する
                        UserRoleMap roleMap;
                        if (isOrigin)
                        {
                            roleMap = new UserRoleMap()
                            {
                                RoleId = role.Id,
                                TenantMap = userTenantMap,
                                User = user,
                                IsOrigin = isOrigin,
                            };
                        }
                        else
                        {
                            roleMap = new UserRoleMap()
                            {
                                RoleId = role.Id,
                                TenantMap = userTenantMap,
                                User = user,
                                IsOrigin = isOrigin,
                                UserGroupTenantMapIds =
                                    userGroupIds != null && userGroupIds.Count() > 0
                                    ? JsonConvert.SerializeObject(userGroupIds)
                                    : null
                            };
                        }

                        AddModel<UserRoleMap>(roleMap);
                    }
                }
                // 残ったものは不要なロールのため削除する
                if (currentRoleMaps != null && currentRoleMaps.Count() > 0)
                {
                    foreach (var roleMap in currentRoleMaps)
                    {
                        // KQI上だけの紐づけの場合削除する。（ユーザグループ経由での紐づけがあれば残すためfalseを設定する。）
                        if (string.IsNullOrEmpty(roleMap.UserGroupTenantMapIds))
                        {
                            DeleteModel<UserRoleMap>(roleMap);
                        }
                        else
                        {
                            roleMap.IsOrigin = false;
                        }
                    }
                }
            }

            // Gitの更新
            // テナントに紐づいているすべてのGitを取得
            var GitMaps = FindModelAll<TenantGitMap>(m => m.TenantId == tenantId).Include(m => m.Git).ToList();
            // 更新前のユーザとテナントGitの紐づけ一覧を取得しておく。
            var currentUserTenantGitMaps = GetModelAll<UserTenantGitMap>().Include(m => m.TenantGitMap).Where(m => m.UserId == user.Id && m.TenantGitMap.TenantId == tenantId);
            foreach (var GitMap in GitMaps)
            {
                // ユーザとGitの紐づけを更新
                var userTenantGitMap = FindModel<UserTenantGitMap>(m => m.TenantGitMapId == GitMap.Id && m.UserId == user.Id);
                if (userTenantGitMap != null)
                {
                    // 紐づけ情報が存在する場合更新する
                    if (isOrigin)
                    {
                        // KQI上での紐づけとする場合、trueを設定する。
                        userTenantGitMap.IsOrigin = true;
                    }
                    else
                    {
                        userTenantGitMap.UserGroupTenantMapIds =
                            userGroupIds != null && userGroupIds.Count() > 0
                            ? JsonConvert.SerializeObject(userGroupIds)
                            : null;
                    }

                    // 編集前の情報から対象を除く。
                    currentUserTenantGitMaps = currentUserTenantGitMaps.Where(m => m.Id != userTenantGitMap.Id);
                }
                else
                {
                    // 紐づけ情報が存在しない場合新規登録する
                    UserTenantGitMap utrMap = new UserTenantGitMap()
                    {
                        TenantGitMap = GitMap,
                        User = user,
                        IsOrigin = isOrigin,
                        UserGroupTenantMapIds =
                            userGroupIds != null && userGroupIds.Count() > 0 
                            ? JsonConvert.SerializeObject(userGroupIds) 
                            : null
                    };
                    // UserTenantGitMap において userId と TenantGitMapId のペアが存在しなければ、エントリ新規追加のためログに出力する
                    LogDebug($"UserTenantGitMap エントリの新規追加 : UserId={user.Id}, TenantGitMapId={GitMap.Id}, TenantId={tenantId}, GitId={GitMap.GitId}");
                    AddModel<UserTenantGitMap>(utrMap);
                }
            }
            // 残ったものは不要なUserTenantGitMapのため削除する
            if (currentUserTenantGitMaps != null && currentUserTenantGitMaps.Count() > 0)
            {
                foreach (var gitMap in currentUserTenantGitMaps)
                {
                    // KQI上だけの紐づけの場合削除する。（ユーザグループ経由での紐づけがあれば残すためfalseを設定する。）
                    if (string.IsNullOrEmpty(gitMap.UserGroupTenantMapIds))
                    {
                        DeleteModel<UserTenantGitMap>(gitMap);
                    }
                    else
                    {
                        gitMap.IsOrigin = false;
                    }
                }
            }

            // 続いてレジストリの更新
            // レジストリはクラスタ管理サービスへも影響するので、作成したMapを全て返す

            List<UserTenantRegistryMap> maps = new List<UserTenantRegistryMap>();

            // テナントに紐づいているすべてのレジストリを取得
            var registryMaps = FindModelAll<TenantRegistryMap>(m => m.TenantId == tenantId).Include(m => m.Registry).ToList();
            // 更新前のユーザとテナントレジストリの紐づけ一覧を取得しておく。
            var currentUserTenantRegistryMaps = GetModelAll<UserTenantRegistryMap>().Where(m => m.UserId == user.Id).Include(m => m.TenantRegistryMap).Where(m => m.UserId == user.Id && m.TenantRegistryMap.TenantId == tenantId);
            foreach (var registryMap in registryMaps)
            {
                // ユーザとレジストリの紐づけを更新
                var userTenantRegistryMap = FindModel<UserTenantRegistryMap>(m => m.TenantRegistryMapId == registryMap.Id && m.UserId == user.Id);
                if (userTenantRegistryMap != null)
                {
                    // 紐づけ情報が存在する場合更新する
                    if (isOrigin)
                    {
                        // KQI上での紐づけとする場合、trueを設定する。
                        userTenantRegistryMap.IsOrigin = true;
                    }
                    else
                    {
                        userTenantRegistryMap.UserGroupTenantMapIds =
                            userGroupIds != null && userGroupIds.Count() > 0
                            ? JsonConvert.SerializeObject(userGroupIds)
                            : null;
                    }

                    // 編集前の情報から対象を除く。
                    currentUserTenantRegistryMaps = currentUserTenantRegistryMaps.Where(m => m.Id != userTenantRegistryMap.Id);

                    maps.Add(userTenantRegistryMap);
                }
                else
                {
                    // 紐づけ情報が存在しない場合新規登録する
                    UserTenantRegistryMap utrMap = new UserTenantRegistryMap()
                    {
                        TenantRegistryMap = registryMap,
                        User = user,
                        IsOrigin = isOrigin,
                        UserGroupTenantMapIds =
                            userGroupIds != null && userGroupIds.Count() > 0 
                            ? JsonConvert.SerializeObject(userGroupIds) 
                            : null
                    };

                    // UserTenantRegistryMap において userId と TenantRegistryMapId のペアが存在しなければ、エントリ新規追加のためログに出力する
                    LogDebug($"UserTenantRegistryMap エントリの新規追加 : UserId={user.Id}, TenantRegistryMapId={registryMap.Id}, TenantId={tenantId}, RegistryId={registryMap.RegistryId}");
                    AddModel<UserTenantRegistryMap>(utrMap);
                    maps.Add(utrMap);
                }
            }
            // 残ったものは不要なUserTenantRegistryMapのため削除する
            if (currentUserTenantRegistryMaps != null && currentUserTenantRegistryMaps.Count() > 0)
            {
                foreach (var registryMap in currentUserTenantRegistryMaps)
                {
                    // KQI上だけの紐づけの場合削除する。（ユーザグループ経由での紐づけがあれば残すためfalseを設定する。）
                    if (string.IsNullOrEmpty(registryMap.UserGroupTenantMapIds))
                    {
                        DeleteModel<UserTenantRegistryMap>(registryMap);
                    }
                    else
                    {
                        registryMap.IsOrigin = false;
                    }
                }
            }

            return maps;
        }

        /// <summary>
        /// ユーザをテナントから外す。
        /// ユーザIDやテナントIDの存在チェック、および所属済みかのチェックは行わない。
        /// </summary>
        /// <param name="userId">対象ユーザID</param>
        /// <param name="tenantId">対象テナントID</param>
        /// <param name="temporary">一時的な削除で再度紐づけなおす場合はtrue</param>
        public void DetachTenant(long userId, long tenantId, bool temporary)
        {
            //レジストリとの紐づけ情報を削除
            var registryMapIds = FindModelAll<TenantRegistryMap>(map => map.TenantId == tenantId).Select(map => map.Id);
            DeleteModelAll<UserTenantRegistryMap>(map => map.UserId == userId && registryMapIds.Contains(map.TenantRegistryMapId));

            //Gitとの紐づけ情報を削除
            var gitMapIds = FindModelAll<TenantGitMap>(map => map.TenantId == tenantId).Select(map => map.Id);
            DeleteModelAll<UserTenantGitMap>(map => map.UserId == userId && gitMapIds.Contains(map.TenantGitMapId));

            UserTenantMap tenantMap = FindUserTenantMap(userId, tenantId);

            //まずは既存のロールをすべて削除する
            DeleteModelAll<UserRoleMap>(map => map.TenantMapId == tenantMap.Id);

            //その後、テナントから外す
            DeleteModel<UserTenantMap>(tenantMap);
        }

        /// <summary>
        /// 指定したユーザテナントマップからKQIとの紐づけを解除する。
        /// </summary>
        /// <param name="user">>対象ユーザ</param>
        /// <param name="tenantId">対象テナントID</param>
        public void DetachOriginTenant(User user, long tenantId)
        {
            // マップを取得
            var userTenantMap = FindUserTenantMap(user.Id, tenantId);
            var userRoleMaps = GetModelAll<UserRoleMap>().Where(map => map.TenantMapId == userTenantMap.Id && map.IsOrigin);
            var userTenantGitMaps = GetModelAll<UserTenantGitMap>().Include(m => m.TenantGitMap).Where(m => m.UserId == user.Id && m.TenantGitMap.TenantId == tenantId);
            var userTenantRegistryMaps = GetModelAll<UserTenantRegistryMap>().Where(m => m.UserId == user.Id).Include(m => m.TenantRegistryMap).Where(m => m.UserId == user.Id && m.TenantRegistryMap.TenantId == tenantId);

            // KQI上での紐づけがないときは何もしない
            if (!userTenantMap.IsOrigin)
            {
                return;
            }
            // ユーザグループの紐づけがないときはテナントから脱退する
            if(userTenantMap.UserGroupTenantMapIds == null)
            {
                DetachTenant(user.Id, tenantId, false);
                return;
            }

            // それぞれのマップのisOriginをfalseに更新
            userTenantMap.IsOrigin = false;
            foreach (var userTenantGitMap in userTenantGitMaps)
            {
                userTenantGitMap.IsOrigin = false;
            }
            foreach (var userTenantRegistryMap in userTenantRegistryMaps)
            {
                userTenantRegistryMap.IsOrigin = false;
            }

            // ロールの更新
            foreach (var userRoleMap in userRoleMaps)
            {
                if (userRoleMap.UserGroupTenantMapIds == null)
                {
                    // ユーザグループの紐づけがないロールは削除する
                    DeleteModel<UserRoleMap>(userRoleMap);
                }
                else
                {
                    // ユーザグループの紐づけがあるロールを更新
                    userRoleMap.IsOrigin = false;
                }
            }
        }

        /// <summary>
        /// 指定したユーザとテナントのマップから指定したユーザグループとの紐づけを解除する。
        /// ユーザグループとの紐づけがなくなった場合はテナントから脱退する。
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        /// <param name="tenantId">対象テナントID</param>
        /// <param name="userGroupId">対象ユーザグループID</param>
        public void DetachUserGroup(User user, long tenantId, long userGroupId)
        {
            // マップを取得
            var tenantMap = FindUserTenantMap(user.Id, tenantId);

            // ユーザグループテナントマップIDを取得
            var deleteId = GetModelAll<UserGroupTenantMap>().FirstOrDefault(map => map.UserGroupId == userGroupId && map.TenantId == tenantId).Id;

            // ユーザグループとの紐づけを解除
            var userGroupTenantMapList = tenantMap.UserGroupTenantMapIdList;
            userGroupTenantMapList.Remove(deleteId);

            if(!tenantMap.IsOrigin && userGroupTenantMapList.Count == 0)
            {
                // KQIとの紐づけがなく、ユーザグループとの紐づけもなくなった場合はテナントから脱退する
                DetachTenant(user.Id, tenantId, false);
            }
            else
            {
                // ユーザグループとの紐づけ情報を更新
                UpdateTenant(user, tenantId, null, false, userGroupTenantMapList);
                // ロール情報を更新
                UpdateLdapRole(user.Id, tenantId);
            }
        }

        /// <summary>
        /// 指定したテナントについて、ユーザのロールを変更する。
        /// ユーザIDやテナントIDの存在チェック、および所属済みかのチェックは行わない。
        /// </summary>
        /// <param name="userId">対象ユーザID</param>
        /// <param name="tenantId">対象テナントID</param>
        /// <param name="roles">テナントロール</param>
        /// <param name="isOrigin">KQI上での紐づけならtrue</param>
        /// <exception cref="ArgumentException"><paramref name="roles"/>にシステムロールが含まれていたり、別テナント用のロールが含まれていた場合</exception>
        public void ChangeTenantRole(long userId, long tenantId, IEnumerable<Role> roles, bool isOrigin)
        {
            var tenantMap = FindUserTenantMap(userId, tenantId);
            
            if (isOrigin)
            {
                // 付与するロールがないとき、KQI上での紐づけがないと判断する。
                if (roles == null || roles.Count() < 1)
                {
                    tenantMap.IsOrigin = false;
                }
                else
                {
                    tenantMap.IsOrigin = true;
                }
            }

            // 更新前のユーザとロールの紐づけ一覧を取得しておく。
            var currentRoleMaps = GetModelAll<UserRoleMap>().Where(m => m.TenantMapId == tenantMap.Id);
            foreach (var role in roles)
            {
                if (role.IsSystemRole)
                {
                    //Adminロールを特定テナントに所属させようとしている
                    throw new UnauthorizedAccessException($"The tenant role {role.Name} is not assigned to user {userId} as a system role.");
                }
                // ユーザとロールの紐づけを更新
                var userRoleMap = FindModel<UserRoleMap>(m => m.TenantMapId == tenantMap.Id && m.RoleId == role.Id);
                if (userRoleMap != null)
                {
                    // 紐づけ情報が存在する場合更新する
                    if (isOrigin)
                    {
                        // KQI上での紐づけとする場合、trueを設定する。
                        userRoleMap.IsOrigin = true;
                    }
                    List<long> usrRoleMapTmpGroupIds = new List<long>();
                    if (userRoleMap.UserGroupTenantMapIdList != null && userRoleMap.UserGroupTenantMapIdList.Count() > 0)
                    {
                        usrRoleMapTmpGroupIds.AddRange(userRoleMap.UserGroupTenantMapIdList);
                        usrRoleMapTmpGroupIds = usrRoleMapTmpGroupIds.Distinct().ToList();
                    }
                    userRoleMap.UserGroupTenantMapIds = usrRoleMapTmpGroupIds != null && usrRoleMapTmpGroupIds.Count() > 0 ? JsonConvert.SerializeObject(usrRoleMapTmpGroupIds) : null;
                    // 編集前の情報から対象を除く。
                    currentRoleMaps = currentRoleMaps.Where(m => m.Id != userRoleMap.Id);
                }
                else
                {
                    // 紐づけ情報が存在しない場合新規登録する
                    var roleMap = new UserRoleMap()
                    {
                        RoleId = role.Id,
                        TenantMap = tenantMap,
                        UserId = userId,
                        IsOrigin = isOrigin,
                        UserGroupTenantMapIds = null
                    };
                    AddModel<UserRoleMap>(roleMap);
                }
            }
            // 残ったものは不要なロールのため削除する
            if (currentRoleMaps != null && currentRoleMaps.Count() > 0)
            {
                foreach (var roleMap in currentRoleMaps)
                {
                    // KQI上だけの紐づけの場合削除する。（ユーザグループ経由での紐づけがあれば残すためfalseを設定する。）
                    if (string.IsNullOrEmpty(roleMap.UserGroupTenantMapIds))
                    {
                        DeleteModel<UserRoleMap>(roleMap);
                    }
                    else
                    {
                        roleMap.IsOrigin = false;
                    }
                }
            }

            // テナントに紐づいているすべてのGitを取得
            var GitMaps = FindModelAll<TenantGitMap>(m => m.TenantId == tenantId).ToList();
            foreach (var GitMap in GitMaps)
            {
                // ユーザとGitの紐づけを更新
                var userTenantGitMap = FindModel<UserTenantGitMap>(m => m.TenantGitMapId == GitMap.Id && m.UserId == userId);
                if (userTenantGitMap != null)
                {
                    // ユーザとテナントの紐づけの値で更新する。
                    userTenantGitMap.IsOrigin = tenantMap.IsOrigin;
                }
            }
            // テナントに紐づいているすべてのレジストリを取得
            var registryMaps = FindModelAll<TenantRegistryMap>(m => m.TenantId == tenantId).ToList();
            foreach (var registryMap in registryMaps)
            {
                // ユーザとレジストリの紐づけを更新
                var userTenantRegistryMap = FindModel<UserTenantRegistryMap>(m => m.TenantRegistryMapId == registryMap.Id && m.UserId == userId);
                if (userTenantRegistryMap != null)
                {
                    // ユーザとテナントの紐づけの値で更新する。
                    userTenantRegistryMap.IsOrigin = tenantMap.IsOrigin;
                }
            }
        }

        /// <summary>
        /// 指定したテナントについて、ユーザのLdap経由で付与されたロール情報を更新する。
        /// </summary>
        /// <param name="userId">>対象ユーザID</param>
        /// <param name="tenantId">対象テナントID</param>
        public async void UpdateLdapRole(long userId, long tenantId)
        {
            UserTenantMap tenantMap = FindUserTenantMap(userId, tenantId);
            var currentroleMaps = GetModelAll<UserRoleMap>().Where(map => map.TenantMapId == tenantMap.Id).ToList();

            // ユーザグループからロールを取得
            var userGroupRoleDictionary = new Dictionary<long, List<long>>();
            foreach (var userGroupTenantMapId in tenantMap.UserGroupTenantMapIdList)
            {
                // ユーザグループの取得
                var userGroupMap = GetModelAll<UserGroupTenantMap>().Include(map => map.UserGroup).ThenInclude(u => u.RoleMaps).FirstOrDefault(map => map.Id == userGroupTenantMapId);

                if (userGroupMap == null)
                {
                    var userGroupTenantMapIdList = tenantMap.UserGroupTenantMapIdList;
                    userGroupTenantMapIdList.Remove(userGroupTenantMapId);
                    // マップ情報が取れない場合は各テーブルのユーザグループテナントマップ情報を更新する
                    UpdateTenant(await GetByIdAsync(userId), tenantId, null, false, userGroupTenantMapIdList);
                    continue;
                }

                foreach (var roleId in userGroupMap.UserGroup.RoleMaps.Select(map => map.RoleId))
                {
                    // 既に紐づいているかチェック
                    if (userGroupRoleDictionary.ContainsKey(roleId))
                    {
                        userGroupRoleDictionary[roleId].Add(userGroupTenantMapId);
                    }
                    else
                    {
                        userGroupRoleDictionary.Add(roleId, new List<long> { userGroupTenantMapId });
                    }
                }
            }

            // ロールマップに登録
            foreach (var roleId in userGroupRoleDictionary.Keys)
            {
                // ユーザとロールの紐づけを更新
                var userRoleMap = currentroleMaps.Find(map => map.TenantMapId == tenantMap.Id && map.RoleId == roleId);
                if (userRoleMap != null)
                {
                    // UserGroupTenantMapIdsの更新
                    userRoleMap.UserGroupTenantMapIds = JsonConvert.SerializeObject(userGroupRoleDictionary[roleId]);

                    currentroleMaps.Remove(userRoleMap);
                }
                else
                {
                    // 紐づけ情報が存在しない場合新規登録する
                    var roleMap = new UserRoleMap()
                    {
                        RoleId = roleId,
                        TenantMap = tenantMap,
                        UserId = userId,
                        IsOrigin = false,
                        UserGroupTenantMapIds = JsonConvert.SerializeObject(userGroupRoleDictionary[roleId])
                    };
                    AddModel<UserRoleMap>(roleMap);
                }
            }

            // 削除対象のロールを削除
            foreach (var roleMap in currentroleMaps)
            {
                if (!roleMap.IsOrigin)
                {
                    DeleteModel<UserRoleMap>(roleMap);
                }
                else
                {
                    // KQIの紐づけが残っているときはカラムをnullに更新する
                    roleMap.UserGroupTenantMapIds = null;
                }
            }
        }

        /// <summary>
        /// 指定したユーザ、テナントに対するクラスタトークンを取得する
        /// </summary>
        public string GetClusterToken(long userId, long tenantId)
        {
            var userTenantMap = FindUserTenantMap(userId, tenantId);
            return userTenantMap.ClusterToken;
        }

        /// <summary>
        /// 指定したユーザ、テナントに対するクラスタトークンを登録する
        /// </summary>
        public void SetClusterToken(long userId, long tenantId, string token)
        {
            var userTenantMap = FindUserTenantMap(userId, tenantId);
            userTenantMap.ClusterToken = token;
        }

        /// <summary>
        /// 指定したユーザに別名を付与する
        /// </summary>
        public async Task<User> SetAliasAsync(long userId, string nameAlias)
        {
            var user = await GetByIdAsync(userId);
            user.Alias = nameAlias;
            return user;
        }
        #endregion

        /// <summary>
        /// デバッグログ
        /// </summary>
        private void LogDebug(string message)
        {
            LogUtil.WriteLLog(logger.LogDebug, "-", "-", message);
        }
    }
}

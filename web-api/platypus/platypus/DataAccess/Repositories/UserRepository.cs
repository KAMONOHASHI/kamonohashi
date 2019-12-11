using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
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
        /// 指定した別名のユーザが存在しない場合、NULLが返る。
        /// </summary>
        /// <param name="nameAlias">別名</param>
        public String GetUserName(string nameAlias)
        {
            var user = Find(u => u.Alias.Contains(nameAlias));
            if (user == null)
            {
                return "!Unknown User!";
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
                ServiceType = user.ServiceType
            };

            userInfo.SystemRoles = roleRepository.GetSystemRoles(user.Id);

            var tenantDic = new Dictionary<Tenant, List<Role>>();

            Dictionary<long, List<Role>> roles = roleRepository.GetTenantRolesDictionary(user.Id);

            // マッピング情報を一件ずつ参照して、引数のuserを更新していく
            foreach (UserTenantMap mapping in user.TenantMaps)
            {
                Tenant tenant = tenantRepository.Get(mapping.TenantId);
                if(tenant == null)
                {
                    //マップにあるテナント情報が存在しない＝キャッシュとの間に不整合がある、と見なす
                    tenantRepository.Refresh();
                    tenant = tenantRepository.Get(mapping.TenantId); //リフレッシュしてやり直し
                }

                if(roles.ContainsKey(tenant.Id))
                {
                    tenantDic.Add(tenant, roles[tenant.Id]);
                }
                else
                {
                    //所属しているけどロールが一つもない状態
                    tenantDic.Add(tenant, new List<Role>());
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
            AttachSandbox(user, true);
            Add(user);
        }

        /// <summary>
        /// ユーザにサンドボックステナントを紐づける
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        /// <param name="isCreate">ユーザ新規作成時であればtrue</param>
        public void AttachSandbox(User user, bool isCreate)
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
            if(attachedRoles.Count == 0)
            {
                attachedRoles.Add(roles.First());
            }

            AttachTenant(user, tenant.Id, attachedRoles, isCreate);
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
        /// 指定したユーザが当該テナントに所属しているか
        /// </summary>
        public async Task<bool> IsMemberAsync(long userId, long tenantId)
        {
            return await ExistsModelAsync<UserTenantMap>(map => map.UserId == userId && map.TenantId == tenantId);
        }

        /// <summary>
        /// ユーザをテナントに所属させる。
        /// ユーザIDやテナントIDの存在チェックは行わない。
        /// 結果として、作成したすべての<see cref="UserTenantRegistryMap"/>を返す。
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        /// <param name="tenantId">対象テナントID</param>
        /// <param name="roles">テナントロール</param>
        /// <param name="isCreate">ユーザ新規作成時であればtrue</param>
        /// <exception cref="ArgumentException"><paramref name="roles"/>にシステムロールが含まれていたり、別テナント用のロールが含まれていた場合</exception>
        public IEnumerable<UserTenantRegistryMap> AttachTenant(User user, long tenantId, IEnumerable<Role> roles, bool isCreate)
        {
            var tenantMap = new UserTenantMap()
            {
                TenantId = tenantId,
                User = user
            };
            AddModel<UserTenantMap>(tenantMap);
            if (roles != null)
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
                        User = user
                    };
                    AddModel<UserRoleMap>(roleMap);
                }
            }

            if (isCreate == false) //新規作成時はIDが0の状態なので、判定しない
            {
                //まずはGitの登録
                //テナントに紐づいているすべてのGitを取得
                var GitMaps = FindModelAll<TenantGitMap>(m => m.TenantId == tenantId).Include(m => m.Git);
                foreach (var GitMap in GitMaps)
                {
                    UserTenantGitMap utrMap = new UserTenantGitMap()
                    {
                        TenantGitMap = GitMap,
                        UserId = user.Id
                    };

                    //認証情報が空欄のものをはじく
                    var existMap = GetModelAll<UserTenantGitMap>().Include(m => m.TenantGitMap)
                        .Where(m => m.UserId == user.Id && m.TenantGitMap.GitId == GitMap.GitId && !String.IsNullOrEmpty(m.GitToken)).FirstOrDefault();
                    if (existMap != null && !String.IsNullOrEmpty(existMap.GitToken))
                    {
                        utrMap.GitToken = existMap.GitToken;
                    }
                    
                    // UserTenantGitMap において userId と TenantGitMapId のペアが存在しなければエントリ新規追加
                    var utrMapCount = GetModelAll<UserTenantGitMap>().Where(m => m.UserId == user.Id && m.TenantGitMapId == GitMap.Id).Count();
                    if (utrMapCount == 0)
                    {
                        LogDebug($"UserTenantGitMap エントリの新規追加 : UserId={user.Id}, TenantGitMapId={GitMap.Id}, TenantId={tenantId}, GitId={GitMap.GitId}");
                    }
                    AddModel<UserTenantGitMap>(utrMap);
                }

                //続いてレジストリの登録
                //レジストリ登録はクラスタ管理サービスへも影響するので、作成したMapを全て返す
               
                List<UserTenantRegistryMap> maps = new List<UserTenantRegistryMap>();

                //テナントに紐づいているすべてのレジストリを取得
                var registryMaps = FindModelAll<TenantRegistryMap>(m => m.TenantId == tenantId).Include(m => m.Registry);
                foreach (var registryMap in registryMaps)
                {
                    UserTenantRegistryMap utrMap = new UserTenantRegistryMap()
                    {
                        TenantRegistryMap = registryMap,
                        UserId = user.Id
                    };

                    //認証情報が空欄のものをはじく
                    var existMap = GetModelAll<UserTenantRegistryMap>().Include(m => m.TenantRegistryMap)
                        .Where(m => m.UserId == user.Id && m.TenantRegistryMap.RegistryId == registryMap.RegistryId && !String.IsNullOrEmpty(m.RegistryPassword)).FirstOrDefault();
                    if (existMap != null && !String.IsNullOrEmpty(existMap.RegistryPassword))
                    {
                        utrMap.RegistryUserName = existMap.RegistryUserName;
                        utrMap.RegistryPassword = existMap.RegistryPassword;
                    }

                    AddModel<UserTenantRegistryMap>(utrMap);
                    maps.Add(utrMap);
                }
                return maps;
            }
            return null;
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

            UserTenantMap tenantMap = FindModel<UserTenantMap>(map => map.UserId == userId && map.TenantId == tenantId);
            
            //まずは既存のロールをすべて削除する
            DeleteModelAll<UserRoleMap>(map => map.TenantMapId == tenantMap.Id);

            //その後、テナントから外す
            DeleteModel<UserTenantMap>(tenantMap);
        }

        /// <summary>
        /// 指定したテナントについて、ユーザのロールを変更する。
        /// ユーザIDやテナントIDの存在チェック、および所属済みかのチェックは行わない。
        /// </summary>
        /// <param name="userId">対象ユーザID</param>
        /// <param name="tenantId">対象テナントID</param>
        /// <param name="roles">テナントロール</param>
        /// <exception cref="ArgumentException"><paramref name="roles"/>にシステムロールが含まれていたり、別テナント用のロールが含まれていた場合</exception>
        public void ChangeTenantRole(long userId, long tenantId, IEnumerable<Role> roles)
        {
            UserTenantMap tenantMap = FindModel<UserTenantMap>(map => map.UserId == userId && map.TenantId == tenantId);

            //まずは既存のロールをすべて削除する
            DeleteModelAll<UserRoleMap>(map => map.TenantMapId == tenantMap.Id);

            foreach (var role in roles)
            {
                if (role.IsSystemRole)
                {
                    //Adminロールを特定テナントに所属させようとしている
                    throw new UnauthorizedAccessException($"The tenant role {role.Name} is not assigned to user {userId} as a system role.");
                }
                var roleMap = new UserRoleMap()
                {
                    RoleId = role.Id,
                    TenantMapId = tenantMap.Id,
                    UserId = userId
                };
                AddModel<UserRoleMap>(roleMap);
            }
        }

        /// <summary>
        /// 指定したユーザ、テナントに対するクラスタトークンを取得する
        /// </summary>
        public string GetClusterToken(long userId, long tenantId)
        {
            var userTenantMap = FindModel<UserTenantMap>(map => map.UserId == userId && map.TenantId == tenantId);
            return userTenantMap.ClusterToken;
        }

        /// <summary>
        /// 指定したユーザ、テナントに対するクラスタトークンを登録する
        /// </summary>
        public void SetClusterToken(long userId, long tenantId, string token)
        {
            var userTenantMap = FindModel<UserTenantMap>(map => map.UserId == userId && map.TenantId == tenantId);
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

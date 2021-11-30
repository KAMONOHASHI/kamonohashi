using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories
{
    /// <summary>
    /// ロールリポジトリ
    /// 
    /// ロール情報はキャッシュしている
    /// </summary>
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        /// <summary>
        /// ロール情報をキャッシュするためのインメモリキャッシュ
        /// </summary>
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// ロール情報のキャッシュ期間
        /// </summary>
        private TimeSpan cacheSpan = new TimeSpan(1, 0, 0, 0);

        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILogger<RoleRepository> logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RoleRepository(
            IMemoryCache memoryCache,
            CommonDbContext dataContext,
            ILogger<RoleRepository> logger) : base(dataContext)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        /// <summary>
        /// キャッシュを破棄する
        /// </summary>
        public void Refresh()
        {
            base.ClearCache<Role>(memoryCache, logger);
            base.ClearCache<MenuRoleMap>(memoryCache, logger);
        }

        /// <summary>
        /// 全ロールを取得する
        /// </summary>
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            //メモリキャッシュからロールを取得
            //多重読み込みしてもエラーは起きないので、ロックは取らない
            return await base.GetFromCacheAsync<Role>(memoryCache, cacheSpan, logger, (r => r.SortOrder));
        }

        /// <summary>
        /// Idからロールを取得する。
        /// 対応するロールが見つからない場合はNULLを返す。
        /// </summary>
        /// <param name="id">ロールID</param>
        public async Task<Role> GetRoleAsync(long id)
        {
            return (await GetAllRolesAsync()).FirstOrDefault(d => d.Id == id);
        }

        /// <summary>
        /// ロールを新規に追加する
        /// </summary>
        /// <param name="role">新規ロール</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public void Add(Role role, IUnitOfWork unitOfWork)
        {
            if (role.IsSystemRole && role.TenantId != null)
            {
                //Admin向けなのにテナントが設定されていたら制約違反
                throw new UnauthorizedAccessException($"role {role.Name} is for Admin but belongs to specific tenant {role.TenantId}");
            }
            base.Add(role);

            unitOfWork.Commit();

            Refresh();
        }

        /// <summary>
        /// 更新用のロール情報を取得する。
        /// 普段はキャッシュからデータをとるが、それだとEntityFrameworkがキャッシュされたオブジェクトのIDを見失って、編集ではなく新規追加になる恐れがある。
        /// </summary>
        /// <param name="id">ロールID</param>
        public async Task<Role> GetRoleForUpdateAsync(long id)
        {
            return await base.GetByIdAsync(id);
        }

        /// <summary>
        /// ロール情報を更新する。
        /// 引数のRoleはキャッシュからではなく、<see cref="GetRoleForUpdateAsync(long)"/>で直接DBから取得したものを使うこと。
        /// </summary>
        /// <param name="role">更新ロール</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public void Update(Role role, IUnitOfWork unitOfWork)
        {
            //roleを適切に取得していれば、Attachする必要なく、そのまま普通に更新できるはず。

            unitOfWork.Commit();

            Refresh();
        }

        /// <summary>
        /// ロール情報を削除する
        /// </summary>
        /// <param name="id">削除ロールID</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public async Task DeleteAsync(long id, IUnitOfWork unitOfWork)
        {
            var role = await base.GetByIdAsync(id);
            base.Delete(role);

            unitOfWork.Commit();

            Refresh();
        }

        /// <summary>
        /// 指定したユーザが持つロールを、Admin・テナント共に取得する。
        /// ユーザIDの存在チェックは行わない。
        /// </summary>
        /// <param name="userId">ユーザID</param>
        public IEnumerable<Role> GetRoles(long userId)
        {
            var roles = GetModelAll<UserRoleMap>().Include(map => map.Role)
                .Where(map => map.UserId == userId)
                .Select(map => map.Role);
            return roles;
        }

        /// <summary>
        /// 指定したテナントが持つカスタムロールを取得する。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        public async Task<IEnumerable<Role>> GetCustomRolesAsync(long tenantId)
        {
            return (await GetAllRolesAsync()).Where(r => r.TenantId == tenantId);
        }

        /// <summary>
        /// 指定したユーザが持つシステムロールを取得する。
        /// ユーザIDの存在チェックは行わない。
        /// </summary>
        /// <param name="userId">ユーザID</param>
        public IEnumerable<Role> GetSystemRoles(long userId)
        {
            var roles = GetModelAll<UserRoleMap>().Include(map => map.Role)
                // ここでToList()をすることでDBからの取得テーブルを確定させる。
                .ToList()
                .Where(map => map.Role.IsSystemRole == true && map.UserId == userId)
                .Select(map => map.Role);
            return roles;
        }

        /// <summary>
        /// 指定したユーザ持つテナントロールを、(テナントID、ロールのリスト）のディクショナリ形式で 取得する。
        /// ユーザIDの存在チェックは行わない。
        /// </summary>
        /// <param name="userId">ユーザID</param>
        public Dictionary<long, List<Role>> GetTenantRolesDictionary(long userId)
        {
            var maps = GetModelAll<UserRoleMap>().Include(map => map.Role).Include(map => map.TenantMap)
                .Where(map => map.Role.IsSystemRole == false && map.UserId == userId && map.TenantMap != null);

            var tenantDic = new Dictionary<long, List<Role>>();
            foreach (var map in maps)
            {
                if (tenantDic.ContainsKey(map.TenantMap.TenantId))
                {
                    tenantDic[map.TenantMap.TenantId].Add(map.Role);
                }
                else
                {
                    var list = new List<Role>
                    {
                        map.Role
                    };
                    tenantDic.Add(map.TenantMap.TenantId, list);
                }
            }
            return tenantDic;
        }

        /// <summary>
        /// テナント横断で使用可能なテナントロールを取得する。
        /// </summary>
        public async Task<IEnumerable<Role>> GetCommonTenantRolesAsync()
        {
            var roles = (await GetAllRolesAsync()).Where(r => r.TenantId == null);
            return roles;
        }

        /// <summary>
        /// 指定したユーザが特定のテナントで持つテナントロールを取得する。
        /// ユーザID, テナントIDの存在チェックは行わない。
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="tenantId">テナントID</param>
        public IEnumerable<Role> GetTenantRoles(long userId, long tenantId)
        {
            var roles = GetModelAll<UserRoleMap>().Include(map => map.Role).Include(map => map.TenantMap)
                .Where(map => map.Role.IsSystemRole == false && map.UserId == userId && map.TenantMap != null && map.TenantMap.TenantId == tenantId)
                .Select(map => map.Role);
            return roles;
        }

        /// <summary>
        /// 指定したユーザにロールを付与する。
        /// <paramref name="role"/>がシステムロールの場合、<paramref name="userTenantMap"/>はNULLになって、テナントに関係なくそのロールが必ず付与される。
        /// <paramref name="role"/>がテナントロールの場合、<paramref name="userTenantMap"/>は非NULLになる。
        /// その場合、<paramref name="user"/>と<paramref name="userTenantMap"/>のUserIdは一致している必要があり、
        /// <paramref name="role"/>のテナントIDが非NULLなら<paramref name="userTenantMap"/>のテナントIDと一致している必要がある。
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        /// <param name="role">対象ロール</param>
        /// <param name="userTenantMap">テナントマップ</param>
        /// <param name="isCreate">ユーザが新規作成の状態(=ID未割当)ならtrue</param>
        public void AttachRole(User user, Role role, UserTenantMap userTenantMap, bool isCreate)
        {
            if (role.IsSystemRole)
            {
                if (userTenantMap != null)
                {
                    //システムロールの場合、userTenantMapはNULLでないといけない
                    throw new UnauthorizedAccessException($"A system role {role.Id}:{role.Name} is not assigned to specific tenant {userTenantMap.TenantId}.");
                }
            }
            else
            {
                //テナントロールの場合
                if (userTenantMap == null)
                {
                    //userTenantMapはNULLでないといけない
                    throw new UnauthorizedAccessException($"A tenant role {role.Id}:{role.Name} need to be assigned to specific tenant.");
                }
                if (role.TenantId != null && role.TenantId == userTenantMap.TenantId)
                {
                    //ロールがテナント固有のものなら、そのテナントにしか使えない
                    throw new UnauthorizedAccessException($"The tenant role {role.Id}:{role.Name} is only assigned to the tenant {role.TenantId}, not {userTenantMap.TenantId}.");
                }
                if (isCreate && user != userTenantMap.User)
                {
                    //ユーザが一致していないといけない
                    throw new UnauthorizedAccessException($"users are not match. {user.Name} : {userTenantMap.User.Name}.");
                }
                else if (isCreate == false && user.Id != userTenantMap.UserId)
                {
                    //ユーザIDが一致していないといけない
                    throw new UnauthorizedAccessException($"user IDs are not match. {user.Id} : {userTenantMap.UserId}.");
                }
            }
            var model = new UserRoleMap()
            {
                RoleId = role.Id,
                TenantMapId = userTenantMap?.Id
            };
            if (isCreate)
            {
                model.User = user;
            }
            else
            {
                model.UserId = user.Id;
            }
            AddModel<UserRoleMap>(model);
        }

        /// <summary>
        /// 指定したユーザから、すべてのシステムロールを外す。
        /// ユーザIDの存在チェックは行わない。
        /// </summary>
        /// <param name="userId">ユーザID</param>
        public void DetachSystemRole(long userId)
        {
            DeleteModelAll<UserRoleMap>(map => map.UserId == userId && map.Role.IsSystemRole);
        }

        /// <summary>
        /// メニューとロールの対応表を取得する
        /// </summary>
        private async Task<IEnumerable<MenuRoleMap>> GetMenuRolesMapsAsync()
        {
            //メモリキャッシュからロールを取得
            //多重読み込みしてもエラーは起きないので、ロックは取らない
            return await base.GetFromCacheAsync<MenuRoleMap>(memoryCache, cacheSpan, logger);
        }

        /// <summary>
        /// 指定したロールに、指定したメニューへのアクセス権限があるか、確認する
        /// </summary>
        /// <param name="roleId">ロールID</param>
        /// <param name="menuCode">メニューコード</param>
        public async Task<bool> AuthorizeAsync(long roleId, MenuCode menuCode)
        {
            var roleMaps = await GetMenuRolesMapsAsync();
            return roleMaps.FirstOrDefault(m => m.RoleId == roleId && m.MenuCode == menuCode.ToString()) != null;
        }
    }
}

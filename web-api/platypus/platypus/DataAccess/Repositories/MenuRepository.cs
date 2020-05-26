using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories
{
    /// <summary>
    /// メニューテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    /// <seealso cref="Nssol.Platypus.DataAccess.Repositories.Interfaces.IMenuRepository" />
    public class MenuRepository : RepositoryBase<MenuRoleMap>, IMenuRepository
    {
        /// <summary>
        /// ロールリポジトリ
        /// </summary>
        private readonly IRoleRepository roleRepository;

        /// <summary>
        /// メニュー情報をキャッシュするためのインメモリキャッシュ
        /// </summary>
        private IMemoryCache memoryCache;

        /// <summary>
        /// メニュー情報のキャッシュ期間。
        /// 今はメニューが変わることはない（画面がないから）ので、1日キャッシュする
        /// </summary>
        private TimeSpan cacheSpan = new TimeSpan(1, 0, 0, 0);

        private ILogger<MenuRepository> logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MenuRepository(CommonDbContext context,
            IMemoryCache memoryCache,
            IRoleRepository roleRepository,
            ILogger<MenuRepository> logger) : base(context)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.roleRepository = roleRepository;
        }

        /// <summary>
        /// 指定したメニューに共通で割り当てられたロールIDを取得する。
        /// </summary>
        public IEnumerable<Role> GetAttachedRoles(MenuCode menuCode)
        {
            return FindModelAll<MenuRoleMap>(map => map.MenuCode == menuCode.ToString()).Include(map => map.Role)
                .Select(map => map.Role);
        }

        /// <summary>
        /// 指定したメニューに割り当てられたテナント用ロールIDを取得する。
        /// 管理者ロール、および他のテナント固有の設定は含まれない。
        /// </summary>
        public IEnumerable<Role> GetAttachedRoles(MenuCode menuCode, long tenantId)
        {
            return FindModelAll<MenuRoleMap>(map => map.MenuCode == menuCode.ToString()).Include(map => map.Role)
                .Where(map => map.Role.IsSystemRole == false && (map.Role.TenantId == null || map.Role.TenantId == tenantId)).Select(map => map.Role);
        }

        /// <summary>
        /// メニューとロールを紐づける
        /// </summary>
        public void AttachRole(MenuItemInfo menu, Role role)
        {
            var map = new MenuRoleMap()
            {
                RoleId = role.Id,
                MenuCode = menu.Code.ToString()
            };
            AddModel<MenuRoleMap>(map);
        }

        /// <summary>
        /// 指定したメニューに関するロールとのマップ情報をすべて削除する
        /// </summary>
        public void DeleteMenuMap(MenuItemInfo menu)
        {
            DeleteModelAll<MenuRoleMap>(map => map.MenuCode == menu.Code.ToString());
        }

        /// <summary>
        /// 指定したメニューに関する特定テナントのカスタムロールとのマップ情報をすべて削除する
        /// </summary>
        public async Task DeleteMenuMapAsync(MenuItemInfo menu, long tenantId)
        {
            var roleIds = (await roleRepository.GetCustomRolesAsync(tenantId)).Select(r => r.Id);
            DeleteModelAll<MenuRoleMap>(map => map.MenuCode == menu.Code.ToString() && roleIds.Contains(map.RoleId));
        }
    }
}

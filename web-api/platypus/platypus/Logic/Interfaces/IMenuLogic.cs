using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// メニュー関係の操作をまとめたロジック
    /// </summary>
    public interface IMenuLogic
    {
        /// <summary>
        /// ログインユーザが表示可能なKQIのホーム画面用のメニューリストを取得する
        /// </summary>
        /// <returns>トップメニュー用のメニューリスト</returns>
        Task<IEnumerable<MenuItemInfo>> GetTopMenuListAsync();

        /// <summary>
        /// ログインユーザが表示可能なサイドメニューリストを取得する
        /// </summary>
        /// <returns>サイドメニュー用のメニューリスト</returns>
        Task<IEnumerable<MenuItemInfo>> GetSideMenuListAsync();

        /// <summary>
        /// ログインユーザが表示可能なAquariumのホーム画面用のメニューリストを取得する
        /// </summary>
        Task<IEnumerable<MenuItemInfo>> GetAquariumTopMenuListAsync();

        /// <summary>
        /// 指定されたメニューコードのいづれかにアクセス可能か判定する
        /// </summary>
        /// <param name="menuCodes">メニューコードのリスト</param>
        /// <returns>アクセス可能な場合、true</returns>
        Task<bool> IsAccessibleMenuAsync(MenuCode[] menuCodes);

        /// <summary>
        /// 各メニューごとにアクセス許可されている管理者用ロールを取得する
        /// </summary>
        Dictionary<MenuItemInfo, IEnumerable<Role>> GetRoleIdsForAdminDictionary();

        /// <summary>
        /// 各メニューごとにアクセス許可されているテナント用ロールを取得する
        /// </summary>
        Dictionary<MenuItemInfo, IEnumerable<Role>> GetRoleIdsForTenantDictionary(long tenantId);

        /// <summary>
        /// 指定したメニューコードに一致するメニュー情報を取得する
        /// </summary>
        MenuItemInfo GetMenu(MenuCode menuCode);
    }
}

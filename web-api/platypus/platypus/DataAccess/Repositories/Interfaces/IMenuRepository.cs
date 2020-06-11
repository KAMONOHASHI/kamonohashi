using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// メニューテーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IMenuRepository : IRepository<MenuRoleMap>
    {
        /// <summary>
        /// 指定したメニューに共通で割り当てられたロールを取得する。
        /// 特定のテナント固有の設定は含まれない。
        /// </summary>
        /// <param name="menuCode">メニューコード</param>
        IEnumerable<Role> GetAttachedRoles(MenuCode menuCode);

        /// <summary>
        /// 指定したメニューに割り当てられたテナント用ロールを取得する。
        /// 管理者ロール、および他のテナント固有の設定は含まれない。
        /// </summary>
        /// <param name="menuCode">メニューコード</param>
        /// <param name="tenantId">テナントID</param>
        IEnumerable<Role> GetAttachedRoles(MenuCode menuCode, long tenantId);

        /// <summary>
        /// メニューとロールを紐づける
        /// </summary>
        /// <param name="menu">メニュー</param>
        /// <param name="role">ロール</param>
        void AttachRole(MenuItemInfo menu, Role role);

        /// <summary>
        /// 指定したメニューに関するロールとのマップ情報をすべて削除する
        /// </summary>
        /// <param name="menu">メニュー</param>
        void DeleteMenuMap(MenuItemInfo menu);

        /// <summary>
        /// 指定したメニューに関する特定テナントのカスタムロールとのマップ情報をすべて削除する
        /// </summary>
        /// <param name="menu">メニュー</param>
        /// <param name="tenantId">テナントID</param>
        Task DeleteMenuMapAsync(MenuItemInfo menu, long tenantId);
    }
}

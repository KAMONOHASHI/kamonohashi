using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using System.Collections.Generic;

namespace Nssol.Platypus.Infrastructure.Infos
{
    public class MenuItemInfo
    {
        /// <summary>
        /// メニュー名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 英語メニュー名。
        /// </summary>
        /// <remarks>
        /// 今は未使用
        /// </remarks>
        public string NameEn { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 英語説明。
        /// </summary>
        /// <remarks>
        /// 今は未使用
        /// </remarks>
        public string DescriptionEn { get; set; }

        /// <summary>
        /// メニューコード
        /// </summary>
        public MenuCode Code { get; set; }

        /// <summary>
        /// メニューカテゴリ。
        /// 表示側でアイコンや色などを種別ごとに変更する際などに使用される想定。
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 必要なロール種別
        /// None, User, Adminの三種。
        /// Noneであれば、<see cref="MenuRoleMap"/>の結果に関係なく、アクセスできる。
        /// <see cref="MenuType.Tenant"/> であれば、<see cref="MenuRoleMap"/>では<see cref="Role.IsSystemRole"/>=falseのロールとしか紐づけられない
        /// <see cref="MenuType.System"/>であれば、<see cref="MenuRoleMap"/>では<see cref="Role.IsSystemRole"/>=trueのロールとしか紐づけられない
        /// </summary>
        public MenuType MenuType { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// トップメニューに表示するか
        /// </summary>
        public bool ShowTopMenu { get; set; }

        /// <summary>
        /// サイドメニューに表示するか
        /// </summary>
        public bool ShowSideMenu { get; set; }

        /// <summary>
        /// サブメニュー
        /// </summary>
        public IEnumerable<MenuItemInfo> Children { get; set; }
    }
}

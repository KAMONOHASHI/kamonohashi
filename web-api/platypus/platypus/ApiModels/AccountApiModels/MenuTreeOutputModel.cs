using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Infrastructure.Infos;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class MenuTreeOutputModel
    {
        /// <summary>
        /// 表示文字列
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// メニューカテゴリ。
        /// 表示側でアイコンや色などを種別ごとに変更する際などに使用される想定。
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        public class MenuGroup : MenuTreeOutputModel
        {
            /// <summary>
            /// 子
            /// </summary>
            public List<MenuTreeOutputModel> Children { get; set; }
        }

        internal static MenuTreeOutputModel GenerateMenu(MenuItemInfo menu, string lang)
        {
            if (menu.Children == null || menu.Children.Count() == 0)
            {
                //子供がいない＝末端
                return new MenuTreeOutputModel()
                {
                    Label = (lang != "en" || string.IsNullOrEmpty(menu.NameEn)) ? menu.Name : menu.NameEn,
                    Url = menu.Url,
                    Category = menu.Category
                };
            }
            else
            {
                //子供がいる＝グループ
                var group = new MenuGroup()
                {
                    Label = (lang != "en" || string.IsNullOrEmpty(menu.NameEn)) ? menu.Name : menu.NameEn,
                    Children = new List<MenuTreeOutputModel>(),
                    Category = menu.Category
                };
                foreach(MenuItemInfo child in menu.Children)
                {
                    group.Children.Add(GenerateMenu(child, lang));
                }
                return group;
            }
        }
    }
}

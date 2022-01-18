using Nssol.Platypus.Infrastructure.Infos;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class MenuListOutputModel
    {
        public MenuListOutputModel(MenuItemInfo menu, string lang)
        {
            this.Name = (lang != "en" || string.IsNullOrEmpty(menu.NameEn)) ? menu.Name : menu.NameEn;
            this.Description = (lang != "en" || string.IsNullOrEmpty(menu.DescriptionEn)) ? menu.Description : menu.DescriptionEn;
            this.Url = menu.Url;
            this.Category = menu.Category;
        }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// メニューカテゴリ。
        /// 表示側でアイコンや色などを種別ごとに変更する際などに使用される想定。
        /// </summary>
        public string Category { get; set; }
    }
}

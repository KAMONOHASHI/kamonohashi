using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.RoleApiModels
{
    /// <summary>
    /// テナントロール管理用編集入力モデル
    /// </summary>
    public class EditForTenantInputModel
    {
        /// <summary>
        /// ロール名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 表示名
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// 並び順。小さいほど前に表示される。一意性は不要。
        /// </summary>
        [Required]
        public int? SortOrder { get; set; }
    }
}

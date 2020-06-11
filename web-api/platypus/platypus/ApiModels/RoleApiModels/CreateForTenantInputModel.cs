using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.RoleApiModels
{
    /// <summary>
    /// テナントロール管理用作成入力モデル
    /// </summary>
    public class CreateForTenantInputModel
    {
        /// <summary>
        /// 名前
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

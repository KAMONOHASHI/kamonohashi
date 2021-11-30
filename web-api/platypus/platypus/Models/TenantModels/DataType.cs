using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// データ種別
    /// </summary>
    public class DataType : TenantModelBase
    {
        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 並び順。小さいほど前に来る。
        /// </summary>
        public int SortOrder { get; set; }
    }
}
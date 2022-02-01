using System;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// 全モデル共通のモデル
    /// </summary>
    public abstract class ModelBase
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 登録者
        /// </summary>
        [Required]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 登録日
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Required]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 更新日
        /// </summary>
        [Required]
        public DateTime ModifiedAt { get; set; }

    }
}

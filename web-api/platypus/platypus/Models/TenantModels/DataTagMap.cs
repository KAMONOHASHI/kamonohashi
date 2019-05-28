using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// データとタグのマッピングモデル
    /// </summary>
    public class DataTagMap : TenantModelBase
    {
        /// <summary>
        /// データID。
        /// </summary>
        [Required]
        public long DataId { get; set; }
        /// <summary>
        /// タグID（FK）
        /// </summary>
        [Required]
        public long TagId { get; set; }
        /// <summary>
        /// タグの実体
        /// </summary>
        [ForeignKey(nameof(TagId))]
        public virtual Tag Tag { get; set; }
        /// <summary>
        /// データの実体
        /// </summary>
        [ForeignKey(nameof(DataId))]
        public virtual Data Data { get; set; }
    }
}

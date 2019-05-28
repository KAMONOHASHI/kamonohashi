using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 学習履歴とタグのマッピングモデル
    /// </summary>
    public class TrainingHistoryTagMap : TenantModelBase
    {
        /// <summary>
        /// データID。
        /// </summary>
        [Required]
        public long TrainingHistoryId { get; set; }
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
        [ForeignKey(nameof(TrainingHistoryId))]
        public virtual TrainingHistory TrainingHistory { get; set; }
    }
}

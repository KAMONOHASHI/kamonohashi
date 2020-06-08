using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 学習履歴とタグのマッピングモデル
    /// </summary>
    public class TrainingHistoryTagMap : TenantModelBase
    {
        /// <summary>
        /// 学習履歴ID
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
        /// 学習履歴の実体
        /// </summary>
        [ForeignKey(nameof(TrainingHistoryId))]
        public virtual TrainingHistory TrainingHistory { get; set; }
    }
}

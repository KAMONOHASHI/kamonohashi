using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 学習履歴と親学習履歴のマップモデル
    /// </summary>
    public class TrainingHistoryParentMap : TenantModelBase
    {
        /// <summary>
        /// 学習履歴ID
        /// </summary>
        [Required]
        public long TrainingHistoryId { get; set; }

        /// <summary>
        /// 親学習履歴ID
        /// </summary>
        [Required]
        public long ParentId { get; set; }

        /// <summary>
        /// 学習履歴
        /// </summary>
        [ForeignKey(nameof(TrainingHistoryId))]
        public virtual TrainingHistory TrainingHistory { get; set; }

        /// <summary>
        /// 親学習履歴
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public virtual TrainingHistory Parent { get; set; }
    }
}
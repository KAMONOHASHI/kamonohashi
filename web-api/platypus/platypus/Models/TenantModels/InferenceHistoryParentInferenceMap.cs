using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 推論履歴と親学習履歴のマップモデル
    /// </summary>
    public class InferenceHistoryParentInferenceMap : TenantModelBase
    {
        /// <summary>
        /// 推論履歴ID
        /// </summary>
        [Required]
        public long InferenceHistoryId { get; set; }

        /// <summary>
        /// 親学習履歴ID
        /// </summary>
        [Required]
        public long ParentId { get; set; }

        /// <summary>
        /// 推論履歴
        /// </summary>
        [ForeignKey(nameof(InferenceHistoryId))]
        public virtual InferenceHistory InferenceHistory { get; set; }

        /// <summary>
        /// 親推論履歴
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public virtual InferenceHistory Parent { get; set; }
    }
}
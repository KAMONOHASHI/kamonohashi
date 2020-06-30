using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// ノートブック履歴と親学習履歴のマップモデル
    /// </summary>
    public class NotebookHistoryParentInferenceMap : TenantModelBase
    {
        /// <summary>
        /// ノートブック履歴ID
        /// </summary>
        [Required]
        public long NotebookHistoryId { get; set; }

        /// <summary>
        /// 親推論履歴ID
        /// </summary>
        [Required]
        public long ParentId { get; set; }

        /// <summary>
        /// ノートブック履歴
        /// </summary>
        [ForeignKey(nameof(NotebookHistoryId))]
        public virtual NotebookHistory NotebookHistory { get; set; }

        /// <summary>
        /// 親推論履歴
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public virtual InferenceHistory Parent { get; set; }
    }
}
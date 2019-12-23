using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// ノートブック履歴と親学習履歴のマップモデル
    /// </summary>
    public class NotebookHistoryParentTrainingMap : TenantModelBase
    {
        /// <summary>
        /// ノートブック履歴ID
        /// </summary>
        [Required]
        public long NotebookHistoryId { get; set; }

        /// <summary>
        /// 親学習履歴ID
        /// </summary>
        [Required]
        public long ParentId { get; set; }

        /// <summary>
        /// ノートブック履歴
        /// </summary>
        [ForeignKey(nameof(NotebookHistoryId))]
        public virtual NotebookHistory NotebookHistory { get; set; }

        /// <summary>
        /// 親学習履歴
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public virtual TrainingHistory Parent { get; set; }
    }
}
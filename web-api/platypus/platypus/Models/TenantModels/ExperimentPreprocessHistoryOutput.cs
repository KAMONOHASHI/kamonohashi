using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 実験の前処理履歴出力
    /// </summary>
    public class ExperimentPreprocessHistoryOutput : TenantModelBase
    {
        /// <summary>
        /// 実験の前処理履歴ID
        /// </summary>
        [Required]
        public long ExperimentPreprocessedHistoryId { get; set; }

        /// <summary>
        /// 出力データID
        /// </summary>
        [Required]
        public long OutputDataId { get; set; }

        /// <summary>
        /// 実験の前処理履歴
        /// </summary>
        [ForeignKey(nameof(ExperimentPreprocessedHistoryId))]
        public virtual ExperimentPreprocessHistory ExperimentPreprocessHistory { get; set; }

        /// <summary>
        /// 出力データ
        /// </summary>
        [ForeignKey(nameof(OutputDataId))]
        public virtual Data OutputData { get; set; }
    }
}

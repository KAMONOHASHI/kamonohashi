using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 実験の前処理履歴出力
    /// </summary>
    public class ExperimentHistoryOutput : TenantModelBase
    {
        /// <summary>
        /// 実験の前処理履歴ID
        /// </summary>
        [Required]
        public long ExperimentHistoryId { get; set; }

        /// <summary>
        /// 出力データID
        /// </summary>
        [Required]
        public long OutputDataId { get; set; }

        /// <summary>
        /// 実験の前処理履歴
        /// </summary>
        [ForeignKey(nameof(ExperimentHistoryId))]
        public virtual ExperimentHistory ExperimentHistory { get; set; }

        /// <summary>
        /// 出力データ
        /// </summary>
        [ForeignKey(nameof(OutputDataId))]
        public virtual Data OutputData { get; set; }
    }
}

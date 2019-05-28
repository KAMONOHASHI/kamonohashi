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
    /// 前処理履歴出力
    /// </summary>
    public class PreprocessHistoryOutput : TenantModelBase
    {
        /// <summary>
        /// 前処理履歴ID
        /// </summary>
        [Required]
        public long PreprocessHistoryId { get; set; }

        /// <summary>
        /// 出力データID
        /// </summary>
        [Required]
        public long OutputDataId { get; set; }

        /// <summary>
        /// 前処理履歴
        /// </summary>
        [ForeignKey(nameof(PreprocessHistoryId))]
        public virtual PreprocessHistory PreprocessHistory { get; set; }

        /// <summary>
        /// 出力データ
        /// </summary>
        [ForeignKey(nameof(OutputDataId))]
        public virtual Data OutputData { get; set; }
    }
}

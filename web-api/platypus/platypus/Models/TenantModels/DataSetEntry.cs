using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// データセットエントリ
    /// </summary>
    public class DataSetEntry : TenantModelBase
    {
        /// <summary>
        /// データセットID
        /// </summary>
        [Required]
        public long DataSetId { get; set; }
        /// <summary>
        /// データID
        /// </summary>
        [Required]
        public long DataId { get; set; }
        /// <summary>
        /// データ種別ID
        /// </summary>
        [Required]
        public long DataTypeId { get; set; }

        /// <summary>
        /// データセット
        /// </summary>
        [ForeignKey(nameof(DataSetId))]
        public virtual DataSet DataSet { get; set; }
        /// <summary>
        /// データ
        /// </summary>
        [ForeignKey(nameof(DataId))]
        public virtual Data Data { get; set; }
        /// <summary>
        /// データ種別
        /// </summary>
        [ForeignKey(nameof(DataTypeId))]
        public virtual DataType DataType { get; set; }
    }
}

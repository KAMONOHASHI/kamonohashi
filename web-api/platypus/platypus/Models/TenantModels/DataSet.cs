using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// データセット
    /// </summary>
    public class DataSet : TenantModelBase
    {
        /// <summary>
        /// 表示用ID
        /// </summary>
        public long? DisplayId { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// メモ。
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 更新可能か
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 展開時にデータ種別を無視する
        /// </summary>
        public bool IsFlat { get; set; }

        /// <summary>
        /// データセットエントリ
        /// </summary>
        public virtual ICollection<DataSetEntry> DataSetEntries { get; set; }
    }
}

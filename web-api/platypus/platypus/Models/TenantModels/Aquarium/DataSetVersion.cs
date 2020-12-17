using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels.Aquarium
{
    /// <summary>
    /// アクアリウムデータセットバージョン
    /// </summary>
    public class DataSetVersion : TenantModelBase
    {
        /// <summary>
        /// バージョン番号
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// データセットID
        /// </summary>
        public long DataSetId { get; set; }

        /// <summary>
        /// データセット
        /// </summary>
        [ForeignKey(nameof(DataSetId))]
        public virtual DataSet DataSet { get; set; }

        /// <summary>
        /// データセットバージョンエントリ
        /// </summary>
        public virtual IEnumerable<DataSetVersionEntry> DataSetVersionEntries { get; set; }
    }
}

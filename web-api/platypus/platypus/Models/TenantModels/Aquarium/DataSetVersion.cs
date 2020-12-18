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
        /// アクアリウムデータセットID
        /// </summary>
        public long AquariumDataSetId { get; set; }

        /// <summary>
        /// データセットID
        /// </summary>
        public long DataSetId { get; set; }

        /// <summary>
        /// データセット
        /// </summary>
        [ForeignKey(nameof(AquariumDataSetId))]
        public virtual DataSet AquariumDataSet { get; set; }

        /// <summary>
        /// データセット
        /// </summary>
        [ForeignKey(nameof(DataSetId))]
        public virtual Models.TenantModels.DataSet DataSet { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels.Aquarium
{
    /// <summary>
    /// アクアリウムデータセットバージョンエントリ
    /// </summary>
    public class DataSetVersionEntry : TenantModelBase
    {
        /// <summary>
        /// データセットバージョンID
        /// </summary>
        [Required]
        public long DataSetVersionId { get; set; }

        /// <summary>
        /// データID
        /// </summary>
        [Required]
        public long DataId { get; set; }

        /// <summary>
        /// データセットバージョン
        /// </summary>
        [ForeignKey(nameof(DataSetVersionId))]
        public virtual DataSetVersion DataSetVersion { get; set; }

        /// <summary>
        /// データ
        /// </summary>
        [ForeignKey(nameof(DataId))]
        public virtual Data Data { get; set; }
    }
}

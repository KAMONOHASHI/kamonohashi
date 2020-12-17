using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.Models.TenantModels.Aquarium
{
    /// <summary>
    /// アクアリウムデータセット
    /// </summary>
    public class DataSet : TenantModelBase
    {
        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 最新バージョン番号
        /// </summary>
        public long LatestVersion { get; set; }

        /// <summary>
        /// データセットバージョン
        /// </summary>
        public virtual IEnumerable<DataSetVersion> DataSetVersions { get; set; }
    }
}

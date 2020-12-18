using Nssol.Platypus.ApiModels.DataApiModels;
using Nssol.Platypus.Models.TenantModels.Aquarium;
using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.Aquarium.DataSetApiModels
{
    /// <summary>
    /// アクアリウムデータセットバージョンの詳細出力モデル
    /// </summary>
    public class VersionDetailsOutputModel : VersionIndexOutputModel
    {
        public VersionDetailsOutputModel(DataSetVersion dataSetVesion) 
            : base(dataSetVesion) {}

        public class Entry
        {
            /// <summary>
            /// データ
            /// </summary>
            public DataApiModels.IndexOutputModel Data { get; set; }
            /// <summary>
            /// ファイル
            /// </summary>
            public IEnumerable<DataFileOutputModel> Files { get; set; }
        }

        /// <summary>
        /// データエントリ
        /// </summary>
        public IDictionary<string, List<Entry>> Entries { get; set; }

        /// <summary>
        /// IsFlat == trueの場合に参照されるデータエントリ
        /// </summary>
        public IEnumerable<Entry> FlatEntries { get; set; }
    }
}

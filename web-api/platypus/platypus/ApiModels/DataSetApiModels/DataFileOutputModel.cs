using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.ApiModels.DataSetApiModels
{
    public class DataFileOutputModel : IndexOutputModel
    {
        /// <summary>
        /// データセットのエントリ
        /// </summary>
        public IEnumerable<Entry> Entries { get; private set; }

        /// <summary>
        /// IsFlat == trueの場合に参照されるエントリ
        /// </summary>
        public IEnumerable<DataApiModels.DataFileOutputModel> FlatEntries { get; set; }

        public DataFileOutputModel(DataSet dataSet) : base(dataSet)
        {
        }

        public void SetEntries(Dictionary<string, List<ApiModels.DataApiModels.DataFileOutputModel>> entities)
        {
            Entries = entities.Select(pair => new Entry()
            {
                Type = pair.Key,
                Files = pair.Value
            });
        }

        public class Entry
        {
            /// <summary>
            /// データ種別
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// ファイル情報のリスト
            /// </summary>
            public IEnumerable<DataApiModels.DataFileOutputModel> Files { get; set; }
        }
    }
}

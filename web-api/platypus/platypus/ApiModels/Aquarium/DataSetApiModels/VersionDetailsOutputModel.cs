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
            : base(dataSetVesion) { }

        /// <summary>
        /// データエントリ
        /// </summary>
        public IDictionary<string, List<DataApiModels.IndexOutputModel>> Entries { get; set; }

        /// <summary>
        /// IsFlat == trueの場合に参照されるデータエントリ
        /// </summary>
        public IEnumerable<DataApiModels.IndexOutputModel> FlatEntries { get; set; }

        /// <summary>
        /// メモ。中身となるKAMONOHASHIのデータセットのメモが入る
        /// </summary>
        public string Memo { get; set; }
    }
}

using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.DataSetApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(DataSet dataSet) : base(dataSet)
        {
        }

        /// <summary>
        /// データセットのエントリ。キーにデータ種別、値にデータ情報集合を持つ。
        /// </summary>
        public Dictionary<string, List<ApiModels.DataApiModels.IndexOutputModel>> Entries { get; set; }

        /// <summary>
        /// IsFlat == trueの場合に参照されるエントリ
        /// </summary>
        public List<ApiModels.DataApiModels.IndexOutputModel> FlatEntries { get; set; }

        /// <summary>
        /// 実行済みか
        /// </summary>
        public bool IsLocked { get; set; }
    }
}

using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Data data) : base(data)
        {
            FileNames = data.DataProperties?.Select(p => p.Key);
        }

        /// <summary>
        /// 登録者表示名
        /// </summary>
        public string DisplayNameCreatedBy { get; set; }

        /// <summary>
        /// データファイル名リスト
        /// </summary>
        public IEnumerable<string> FileNames { get; set; }

        /// <summary>
        /// 派生元データ
        /// </summary>
        public IndexOutputModel Parent { get; set; }

        /// <summary>
        /// 派生先データ
        /// </summary>
        public IEnumerable<PreprocessHistoryOutputModel> Children { get; set; }
    }
}

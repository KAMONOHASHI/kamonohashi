using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Data data) : base(data)
        {
            FileNames = data.DataProperties?.Select(p => p.Key);
        }

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

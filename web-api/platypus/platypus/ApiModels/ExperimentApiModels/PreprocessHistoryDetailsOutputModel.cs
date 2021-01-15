using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    public class PreprocessHistoryDetailsOutputModel : PreprocessHistoriesOutputModel
    {
        public PreprocessHistoryDetailsOutputModel(ExperimentPreprocessHistory history) : base(history)
        {
        }

        public IEnumerable<long> OutputDataIds { get; set; }
    }
}

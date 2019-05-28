using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.PreprocessingApiModels
{
    public class HistoryDetailsOutputModel : HistoriesOutputModel
    {
        public HistoryDetailsOutputModel(PreprocessHistory history) : base(history)
        {
        }

        public IEnumerable<long> OutputDataIds { get; set; }
    }
}

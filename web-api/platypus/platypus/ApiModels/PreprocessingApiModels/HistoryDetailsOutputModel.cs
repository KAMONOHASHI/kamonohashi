using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;

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

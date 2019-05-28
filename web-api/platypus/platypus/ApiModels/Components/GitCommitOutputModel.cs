using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.Components
{
    public class GitCommitOutputModel : GitCommitInputModel
    {
        /// <summary>
        /// GitサービスのWebUI URL
        /// </summary>
        public string Url { get; set; }
    }
}

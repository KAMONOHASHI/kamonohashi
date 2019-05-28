using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.KubernetesModels
{
    public class CreateJobOutputModel
    {
        public MetadataModel Metadata { get; set; }

        public class MetadataModel
        {
            public string Name { get; set; }
        }
    }
}

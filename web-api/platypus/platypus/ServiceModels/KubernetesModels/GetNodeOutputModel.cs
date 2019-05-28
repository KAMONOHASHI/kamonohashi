using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Infrastructure;

namespace Nssol.Platypus.ServiceModels.KubernetesModels
{
    public class GetNodeOutputModel
    {
        public List<ItemModel> Items { get; set; }

        public class ItemModel
        {
            public MetadataModel Metadata { get; set; }

            public StatusModel Status { get; set; }

            public class MetadataModel
            {
                public string Name { get; set; }

                public string Uid { get; set; }

                public Dictionary<string, string> Labels { get; set; }
            }

            public class StatusModel
            {
                public List<ConditionModel> Conditions { get; set; }
                public NodeResourceModel Capacity { get; set; }

                public class ConditionModel
                {
                    public string Type { get; set; }
                    public string Status { get; set; }
                    public NodeResourceModel Allocatable { get; set; }
                }
            }
        }
    }
}



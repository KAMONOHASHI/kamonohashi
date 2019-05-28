using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.ApiModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.RegistryApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Registry registry) : base(registry)
        {
            Host = registry.Host;
            PortNo = registry.PortNo;
            ApiUrl = registry.ApiUrl;
            RegistryUrl = registry.RegistryUrl;
        }
        
        /// <summary>
        /// Registryホストサーバ名
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Registryホストサーバポート
        /// </summary>
        public int PortNo { get; set; }

        /// <summary>
        /// Registry API URL
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// Registry Url
        /// </summary>
        public string RegistryUrl { get; set; }
    }
}

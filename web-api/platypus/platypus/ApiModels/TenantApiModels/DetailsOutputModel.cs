using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.ApiModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.TenantApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Tenant tenant) : base(tenant)
        {
            StorageId = tenant.StorageId;

            DefaultGitId = tenant.DefaultGitId;
            GitIds = tenant.GitMaps.Select(map => map.GitId);

            DefaultRegistryId = tenant.DefaultRegistryId;
            RegistryIds = tenant.RegistryMaps.Select(map => map.RegistryId);

            AvailableInfiniteTimeNotebook = tenant.AvailableInfiniteTimeNotebook;
        }

        /// <summary>
        /// Default Git ID。
        /// </summary>
        public long? DefaultGitId { get; set; }

        /// <summary>
        /// Git IDs。
        /// </summary>
        public IEnumerable<long> GitIds { get; set; }

        /// <summary>
        /// Default Registry ID。
        /// </summary>
        public long? DefaultRegistryId { get; set; }

        /// <summary>
        /// Registry IDs。
        /// </summary>
        public IEnumerable<long> RegistryIds { get; set; }

        /// <summary>
        /// Storage ID。
        /// </summary>
        public long? StorageId { get; set; }

        /// <summary>
        /// AvailableInfiniteTimeNotebook
        /// </summary>
        public bool AvailableInfiniteTimeNotebook { get; set; }
    }
}

using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.ApiModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.RoleApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Role role) : base(role)
        {
        }

        /// <summary>
        /// 紐づけられているテナント名
        /// </summary>
        /// <remarks>
        /// <see cref="IndexOutputModel.IsSystemRole"/>がTrueの場合は、必ずNULL
        /// </remarks>
        public string TenantName { get; set; }
    }
}

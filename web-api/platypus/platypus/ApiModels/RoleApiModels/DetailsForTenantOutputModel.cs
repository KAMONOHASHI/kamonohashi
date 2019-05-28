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
    public class DetailsForTenantOutputModel : IndexOutputModel
    {
        public DetailsForTenantOutputModel(Role role) : base(role)
        {
        }

        /// <summary>
        /// 全テナント共通のロールか。
        /// Trueの場合、編集できない。
        /// </summary>
        public bool IsShared { get; set; }
    }
}

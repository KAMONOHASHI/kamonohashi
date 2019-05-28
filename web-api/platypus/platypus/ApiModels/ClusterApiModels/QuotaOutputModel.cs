using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.ClusterApiModels
{
    public class QuotaOutputModel : QuotaInputModel
    {
        /// <summary>
        /// テナント表示名
        /// </summary>
        public string TenantName { get; set; }

        public QuotaOutputModel(Tenant tenant)
        {
            this.TenantId = tenant.Id;
            this.TenantName = tenant.DisplayName;
            this.Cpu = Convert(tenant.LimitCpu);
            this.Memory = Convert(tenant.LimitMemory);
            this.Gpu = Convert(tenant.LimitGpu);
        }

        private int Convert(int? d)
        {
            if (d.HasValue)
            {
                return d.Value;
            }
            return 0;
        }
    }
}

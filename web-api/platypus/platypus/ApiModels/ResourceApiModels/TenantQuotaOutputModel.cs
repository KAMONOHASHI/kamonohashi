using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.ResourceApiModels
{
    public class TenantQuotaOutputModel
    {
        public TenantQuotaOutputModel(Tenant info)
        {
            LimitCpu = info.LimitCpu;
            LimitMemory = info.LimitMemory;
            LimitGpu = info.LimitGpu;
        }

        /// <summary>
        /// Cpu制限（クォータ）
        /// </summary>
        public int? LimitCpu { get; set; }

        /// <summary>
        /// メモリ制限（クォータ）
        /// </summary>
        public int? LimitMemory { get; set; }

        /// <summary>
        /// Gpu制限（クォータ）
        /// </summary>
        public int? LimitGpu { get; set; }
    }
}
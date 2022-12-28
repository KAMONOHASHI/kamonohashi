using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories
{
    public class TenantEksMapRepository: RepositoryBase<TenantEksMap>, ITenantEksMapRepository
    {
        private ILogger<TenantEksMapRepository> logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TenantEksMapRepository(CommonDbContext context,
            ILogger<TenantEksMapRepository> logger) : base(context)
        {
            this.logger = logger;
        }

    }
}

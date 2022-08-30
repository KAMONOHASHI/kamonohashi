using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nssol.Platypus.DataAccess.Repositories
{
    public class EksRepository : RepositoryBase<Eks>, IEksRepository
    {
        private ILogger<EksRepository> logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EksRepository(CommonDbContext context, ILogger<EksRepository> logger) : base(context)
        {
            this.logger = logger;
        }

        /// <summary>
        /// EKSのノードへのアサイン状況をリセットする
        /// </summary>
        /// <param name="eksId">対象のEKSのデータのID</param>
        public void ResetEksToTenant(long eksId)
        {
            DeleteModelAll<TenantEksMap>(map => map.EksId == eksId);
        }

        /// <summary>
        /// EKSの情報とテナントの情報をマッピングする
        /// テナントの存在は確認しない
        /// </summary>
        /// <param name="tenants">テナント</param>
        /// <param name="eks">EKS</param>
        /// <param name="isCreate">EKSの情報が新規登録か否か</param>
        /// <returns></returns>        
        public void AttachEksToTenant(IEnumerable<Tenant> tenants, Eks eks, bool isCreate)
        {
            // 登録するテナントごとに確認する
            foreach (Tenant tenant in tenants)
            {
                var map = new TenantEksMap()
                {
                    TenantId = tenant.Id
                };
                if (isCreate)
                {
                    map.Eks = eks;
                }
                else
                {
                    map.EksId = eks.Id;
                }
                AddModel<TenantEksMap>(map);
            }
        }
    }
}

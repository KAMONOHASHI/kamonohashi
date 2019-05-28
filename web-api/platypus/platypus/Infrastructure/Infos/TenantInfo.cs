using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure.Infos
{
    public class TenantInfo
    {
        private TenantInfo(Tenant tenant, long? defaultTenantId)
        {
            Id = tenant?.Id;
            Name = tenant?.Name;
            DisplayName = tenant?.DisplayName;
            Default = tenant?.Id == defaultTenantId;
        }

        public TenantInfo(Tenant tenant, List<Role> roles, long? defaultTenantId) : this(tenant, defaultTenantId)
        {
            Roles = roles?.OrderBy(r => r.SortOrder).Select(x => new RoleInfo(x)).ToList();
        }

        public TenantInfo(Tenant tenant, List<RoleInfo> roles, long? defaultTenantId) : this(tenant, defaultTenantId)
        {
            Roles = roles;
        }

        public TenantInfo(Tenant tenant, Dictionary<Tenant, List<Role>> TenantDic, long? defaultTenantId) : this(tenant, defaultTenantId)
        {
            foreach (var dic in TenantDic)
            {
                if (Id == dic.Key.Id)
                {
                    Roles = dic.Value.Select(x => new RoleInfo(x)).ToList();
                    break;
                }
            }
        }

        /// <summary>
        /// テナントID
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// テナント名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// デフォルト
        /// </summary>
        public Boolean Default { get; set; }
        /// <summary>
        /// テナント表示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// テナントの全ロール名
        /// </summary>
        public List<RoleInfo> Roles { get; set; }
    }
}

using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.Infrastructure.Infos
{
    /// <summary>
    /// テナント情報
    /// </summary>
    public class TenantInfo
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tenant">テナント</param>
        /// <param name="defaultTenantId">デフォルトテナントID</param>
        private TenantInfo(Tenant tenant, long? defaultTenantId)
        {
            Id = tenant?.Id;
            Name = tenant?.Name;
            DisplayName = tenant?.DisplayName;
            Default = tenant?.Id == defaultTenantId;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tenant">テナント</param>
        /// <param name="roles">ロール</param>
        /// <param name="defaultTenantId">デフォルトテナントID</param>
        public TenantInfo(Tenant tenant, List<RoleInfo> roles, long? defaultTenantId) : this(tenant, defaultTenantId)
        {
            Roles = roles?.OrderBy(r => r.SortOrder).ToList();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tenant">テナント</param>
        /// <param name="TenantDic">テナントとロール情報</param>
        /// <param name="defaultTenantId">デフォルトテナントID</param>
        public TenantInfo(Tenant tenant, Dictionary<Tenant, List<RoleInfo>> TenantDic, long? defaultTenantId) : this(tenant, defaultTenantId)
        {
            foreach (var dic in TenantDic)
            {
                if (Id == dic.Key.Id)
                {
                    Roles = dic.Value.ToList();
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
        /// <summary>
        /// 元々KQI上で紐づけあったか。
        /// </summary>
        public Boolean IsOrigin { get; set; }
    }
}

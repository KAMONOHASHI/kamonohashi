using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Infrastructure;

namespace Nssol.Platypus.ApiModels.TenantApiModels
{
    /// <summary>
    /// テナント情報のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(Tenant tenant) : base(tenant)
        {
            Id = tenant.Id;
            Name = tenant.Name;
            DisplayName = tenant.DisplayName;
            StoragePath = tenant.Storage == null  ? "No Object Storage Setting" : $"{tenant.Storage.ServerAddress}/{tenant.StorageBucket}";
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// テナント表示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// ストレージパス。
        /// サーバ名/バケット
        /// </summary>
        public string StoragePath { get; set; }
    }
}

using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.UserApiModels
{
    public class IndexForTenantOutputModel : Components.OutputModelBase
    {
        public IndexForTenantOutputModel(User user) : base(user)
        {
            Id = user.Id;
            Name = user.Name;
            ServiceType = user.ServiceType;
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
        /// 認証サービス種別
        /// </summary>
        public AuthServiceType ServiceType { get; set; }

        /// <summary>
        /// 属しているテナントロール
        /// </summary>
        public IEnumerable<RoleInfo> Roles { get; set; }
    }
}

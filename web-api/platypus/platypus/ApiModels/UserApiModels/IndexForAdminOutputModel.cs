using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.UserApiModels
{
    public class IndexForAdminOutputModel : Components.OutputModelBase
    {
        public IndexForAdminOutputModel(User user) : base(user)
        {
            Id = user.Id;
            Name = user.Name;
            DisplayName = user.DisplayName;
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
        /// 表示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 認証サービス種別
        /// </summary>
        public AuthServiceType ServiceType { get; set; }

        /// <summary>
        /// 属しているシステムロール
        /// </summary>
        public IEnumerable<RoleInfo> SystemRoles { get; set; }

        /// <summary>
        /// 属しているテナント
        /// </summary>
        public List<TenantInfo> Tenants { get; set; }
    }
}

using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.ApiModels.UserGroupApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(UserGroup userGroup):base(userGroup)
        {
            Dn = userGroup.Dn;
            IsDirect = userGroup.IsDirect;
            Roles = userGroup.RoleMaps.Select(map => new RoleInfo(map.Role));
        }

        /// <summary>
        /// 対象ユーザグループのDN情報
        /// </summary>
        public string Dn { get; set; }

        /// <summary>
        /// 対象ユーザグループのDN情報の直接的（直下）が対象か、間接的も許可するか。
        /// </summary>
        public bool IsDirect { get; set; }

        /// <summary>
        /// テナント参加時に付与するロール
        /// </summary>
        public IEnumerable<RoleInfo> Roles { get; set; }
    }
}

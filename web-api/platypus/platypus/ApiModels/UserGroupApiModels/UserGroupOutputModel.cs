using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.ApiModels.UserGroupApiModels
{
    public class UserGroupOutputModel : Components.OutputModelBase
    {
        public UserGroupOutputModel(UserGroup userGroup): base(userGroup)
        {
            Id = userGroup.Id;
            Name = userGroup.Name;
            Memo = userGroup.Memo;
            IsGroup = userGroup.IsGroup;
            Dn = userGroup.Dn;
            IsDirect = userGroup.IsDirect;
            Roles = userGroup.RoleMaps.Select(map => new RoleInfo(map.Role));
        }

        /// <summary>
        /// ユーザグループID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ユーザグループ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 対象ユーザグループがグループか、OUか。
        /// </summary>
        public bool IsGroup { get; set; }

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

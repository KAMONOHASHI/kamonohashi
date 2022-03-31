using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.ApiModels.UserGroupApiModels
{
    /// <summary>
    /// ユーザグループ詳細出力モデル
    /// </summary>
    public class DetailsOutputModel : IndexOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userGroup">ユーザグループ</param>
        public DetailsOutputModel(UserGroup userGroup):base(userGroup)
        {
            IsDirect = userGroup.IsDirect;
            Roles = userGroup.RoleMaps.Select(map => new RoleInfo(map.Role));
        }

        /// <summary>
        /// 対象ユーザグループのDN情報の直接的（直下）が対象か、間接的も許可するか。
        /// </summary>
        /// <remarks>
        /// True：直接、False：間接
        /// </remarks>
        public bool IsDirect { get; set; }

        /// <summary>
        /// テナント参加時に付与するロール情報
        /// </summary>
        public IEnumerable<RoleInfo> Roles { get; set; }
    }
}

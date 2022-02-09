using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.UserGroupApiModels
{
    public class CreateInputModel
    {
        /// <summary>
        /// ユーザグループ名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 対象ユーザグループがグループか、OUか。
        /// </summary>
        [Required]
        public bool IsGroup { get; set; }

        /// <summary>
        /// 対象ユーザグループのDN情報
        /// </summary>
        [Required]
        public string Dn { get; set; }

        /// <summary>
        /// 対象ユーザグループのDN情報の直接的（直下）が対象か、間接的も許可するか。
        /// </summary>
        [Required]
        public bool IsDirect { get; set; }

        /// <summary>
        /// テナント参加時に付与するロールID
        /// </summary>
        [Required]
        public IEnumerable<long> RoleIds { get; set; }
    }
}

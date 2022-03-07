using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.UserGroupApiModels
{
    /// <summary>
    /// ユーザグループ作成入力モデル
    /// </summary>
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
        /// <remarks>
        /// True：グループ、False：OU
        /// </remarks>
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
        /// <remarks>
        /// True：直接、False：間接
        /// </remarks>
        [Required]
        public bool IsDirect { get; set; }

        /// <summary>
        /// テナント参加時に付与するロールIDリスト
        /// </summary>
        [Required]
        public IEnumerable<long> RoleIds { get; set; }
    }
}

using Nssol.Platypus.Infrastructure.Types;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// ユーザー情報
    /// </summary>
    public class User:ModelBase
    {
        /// <summary>
        /// ユーザ名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 別名。<see cref="Name"/>そのままだと、ドット(.)やアットマーク(@)など、コンテナ管理サービス側の制約で使えない文字列が含まれうるので、識別名を別で設ける。
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 既定で選択されるテナントID
        /// </summary>
        [Required]
        public long DefaultTenantId { get; set; }

        /// <summary>
        /// 認証サービス種別
        /// </summary>
        public AuthServiceType ServiceType { get; set; }

        /// <summary>
        /// <see cref="ServiceType"/>が<see cref="AuthServiceType.Local"/>の場合のみ設定可能。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// テナントマッピング情報
        /// </summary>
        public virtual ICollection<UserTenantMap> TenantMaps { get; set; }

        /// <summary>
        /// 既定で選択されるテナント
        /// </summary>
        [ForeignKey(nameof(DefaultTenantId))]
        public virtual Tenant DefaultTenant { get; set; }

        /// <summary>
        /// Slackの送信先URL
        /// </summary>
        public string SlackUrl { get; set; }

        /// <summary>
        /// SlackメッセージのメンションID
        /// </summary>
        public string MentionId { get; set; }
    }
}

using Nssol.Platypus.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// テンプレート
    /// </summary>
    public class Template : ModelBase
    {
        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 最新バージョン番号
        /// </summary>
        public long LatestVersion { get; set; }

        /// <summary>
        /// アクセスレベル
        /// </summary>
        [Required]
        public TemplateAccessLevel AccessLevel { get; set; }

        /// <summary>
        /// テンプレート作成者ユーザID
        /// </summary>
        public long CreaterUserId { get; set; }

        /// <summary>
        /// テンプレート作成者テナントID
        /// </summary>
        public long CreaterTenantId { get; set; }

        /// <summary>
        /// テンプレート作成者ユーザ
        /// </summary>
        [ForeignKey(nameof(CreaterUserId))]
        public virtual User CreaterUser { get; set; }

        /// <summary>
        /// テンプレート作成者テナント
        /// </summary>
        [ForeignKey(nameof(CreaterTenantId))]
        public virtual Tenant CreaterTenant { get; set; }

        /// <summary>
        /// テンプレートバージョン
        /// </summary>
        public virtual IEnumerable<TemplateVersion> TemplateVersions { get; set; }
    }
}

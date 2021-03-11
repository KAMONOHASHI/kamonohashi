using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// テンプレートの出力モデル
    /// </summary>
    public class IndexOutputModel2 : Components.OutputModelBase
    {
        public IndexOutputModel2(Template template) : base(template)
        {
            Id = template.Id;
            Name = template.Name;
            Memo = template.Memo;
            LatestVersion = template.LatestVersion;
            AccessLevel = template.AccessLevel;
            CreaterUserId = template.CreaterUserId;
            CreaterTenantId = template.CreaterTenantId;
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
        public TemplateAccessLevel AccessLevel { get; set; }

        /// <summary>
        /// テンプレート作成者ユーザID
        /// </summary>
        public long CreaterUserId { get; set; }

        /// <summary>
        /// テンプレート作成者テナントID
        /// </summary>
        public long CreaterTenantId { get; set; }
    }
}

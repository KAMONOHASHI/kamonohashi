using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// テンプレート情報のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(ModelTemplate template) : base(template)
        {
            Id = template.Id;
            Name = template.Name;
            Memo = template.Memo;
            Version = template.Version;
            AccessLevel = template.AccessLevel;
            AccessLevelStr = template.AccessLevel.ToString();
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
        /// アクセスレベル
        /// </summary>
        public TemplateAccessLevel AccessLevel { get; set; }

        /// <summary>
        /// アクセスレベルの表示名
        /// </summary>
        public string AccessLevelStr { get; set; }

        /// <summary>
        /// バージョン
        /// </summary>
        public long? Version { get; set; }


    }
}

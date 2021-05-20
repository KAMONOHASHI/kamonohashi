using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// テンプレートバージョンの出力モデル
    /// </summary>
    public class VersionIndexOutputModel : Components.OutputModelBase
    {
        public VersionIndexOutputModel(TemplateVersion templateVersion) : base(templateVersion)
        {
            Id = templateVersion.Id;
            Version = templateVersion.Version;
            TemplateId = templateVersion.TemplateId;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// バージョン番号
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// テンプレートID
        /// </summary>
        public long TemplateId { get; set; }
    }
}

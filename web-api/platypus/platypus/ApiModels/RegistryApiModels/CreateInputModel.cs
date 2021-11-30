using Nssol.Platypus.Infrastructure.Types;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.RegistryApiModels
{
    public class CreateInputModel
    {
        /// <summary>
        /// Registry識別名
        /// </summary>
        [Required]
        [Controllers.Util.CustomValidation(Controllers.Util.CustomValidationType.Alphanumeric)]
        public string Name { get; set; }

        /// <summary>
        /// Registryホストサーバ名
        /// </summary>
        [Required]
        public string Host { get; set; }

        /// <summary>
        /// Registryホストサーバポート
        /// </summary>
        [Required]
        public int? PortNo { get; set; }

        /// <summary>
        /// Registry種別
        /// </summary>
        [Required]
        public RegistryServiceType? ServiceType { get; set; }

        /// <summary>
        /// Registryのプロジェクト名。
        /// <see cref="ServiceType"/>が<see cref="RegistryServiceType.GitLab"/>の場合に必要。
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Registry API URL
        /// </summary>
        [Required]
        public string ApiUrl { get; set; }

        /// <summary>
        /// Registry Url
        /// </summary>
        [Required]
        public string RegistryUrl { get; set; }
    }
}

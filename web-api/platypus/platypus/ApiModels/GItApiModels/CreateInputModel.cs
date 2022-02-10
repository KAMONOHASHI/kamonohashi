using Nssol.Platypus.Infrastructure.Types;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.GitApiModels
{
    public class CreateInputModel
    {
        /// <summary>
        /// 識別名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Git種別
        /// </summary>
        [Required]
        public GitServiceType? ServiceType { get; set; }

        /// <summary>
        /// Git API URL
        /// </summary>
        [Required]
        public string ApiUrl { get; set; }

        /// <summary>
        /// GitリポジトリURL
        /// </summary>
        [Required]
        public string RepositoryUrl { get; set; }
    }
}

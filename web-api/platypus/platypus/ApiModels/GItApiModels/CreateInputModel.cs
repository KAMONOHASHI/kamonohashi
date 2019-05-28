using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.Infrastructure.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

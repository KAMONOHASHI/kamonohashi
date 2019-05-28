using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.Components
{
    public class ContainerImageOutputModel : ContainerImageInputModel
    {
        /// <summary>
        /// レジストリ名。
        /// </summary>
        public string RegistryName { get; set; }

        /// <summary>
        /// {リポジトリ名}/{イメージ名}:{タグ名}
        /// </summary>
        public string Url
        {
            get
            {
                string fullImageName = string.IsNullOrEmpty(Tag) ? Image : $"{Image}:{Tag}";
                return string.IsNullOrEmpty(RegistryName) ? fullImageName : $"{RegistryName}/{fullImageName}";
            }
        }
    }
}

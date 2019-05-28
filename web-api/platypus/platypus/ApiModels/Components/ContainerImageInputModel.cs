using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.Components
{
    public class ContainerImageInputModel
    {
        /// <summary>
        /// レジストリID。
        /// 未指定の場合はテナントのデフォルトが使用される。
        /// </summary>
        public long? RegistryId { get; set; }
        /// <summary>
        /// イメージ名
        /// </summary>
        [Required]
        public string Image { get; set; }
        /// <summary>
        /// タグ（バージョン）名
        /// </summary>
        [Required]
        public string Tag { get; set; }
    }
}

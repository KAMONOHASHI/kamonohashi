using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// タグモデル
    /// </summary>
    public class Tag : TenantModelBase
    {
        /// <summary>
        /// タグ名
        /// </summary>
        [Required]
        public string Name { get; set; }
        
        /// <summary>
        /// データとの対応
        /// </summary>
        public virtual ICollection<DataTagMap> DataMaps { get; set; }
    }
}

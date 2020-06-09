using Nssol.Platypus.Infrastructure.Types;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        /// タグ種別
        /// </summary>
        [Required]
        public TagType Type { get; set; }

        /// <summary>
        /// データとの対応
        /// </summary>
        public virtual ICollection<DataTagMap> DataMaps { get; set; }

        /// <summary>
        /// 学習履歴との対応
        /// </summary>
        public virtual ICollection<TrainingHistoryTagMap> TrainingHistoryMaps { get; set; }
    }
}

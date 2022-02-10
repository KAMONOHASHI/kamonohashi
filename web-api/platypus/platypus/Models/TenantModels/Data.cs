using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// データ
    /// </summary>
    [Table("Data")]
    public class Data : TenantModelBase
    {
        /// <summary>
        /// 表示用ID
        /// </summary>
        public long? DisplayId { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// メモ。
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 親データID
        /// </summary>
        public long? ParentDataId { get; set; }

        /// <summary>
        /// Dockerレジストリ
        /// </summary>
        [ForeignKey(nameof(ParentDataId))]
        public Data Parent { get; set; }

        /// <summary>
        /// データファイル群
        /// </summary>
        public virtual ICollection<DataProperty> DataProperties { get; set; }

        /// <summary>
        /// タグのマッピング
        /// </summary>
        public virtual ICollection<DataTagMap> TagMaps { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        public IEnumerable<string> Tags
        {
            get
            {
                if (TagMaps == null)
                {
                    return null;
                }
                return TagMaps.Select(tm => tm.Tag?.Name);
            }
        }
    }
}
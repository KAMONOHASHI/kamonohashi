using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// リソースモニタノード
    /// </summary>
    public class ResourceNode : ModelBase
    {
        /// <summary>
        /// サンプルID
        /// </summary>
        public long SampleId { get; set; }

        /// <summary>
        /// リソースサンプル
        /// </summary>
        [ForeignKey(nameof(SampleId))]
        public ResourceSample ResourceSample { get; set; }

        /// <summary>
        /// ノード名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public int Cpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        public int Gpu { get; set; }

        /// <summary>
        /// リソースモニタコンテナ
        /// </summary>
        public IEnumerable<ResourceContainer> ResourceContainers { get; set; }
    }
}

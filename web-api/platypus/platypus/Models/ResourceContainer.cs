using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// リソースモニタコンテナ
    /// </summary>
    public class ResourceContainer : ModelBase
    {
        /// <summary>
        /// ノードID
        /// </summary>
        public long NodeId { get; set; }

        /// <summary>
        /// リソースノード
        /// </summary>
        [ForeignKey(nameof(NodeId))]
        public ResourceNode ResourceNode { get; set; }

        /// <summary>
        /// テナントID
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// テナント名
        /// </summary>
        [Required]
        public string TenantName { get; set; }

        /// <summary>
        /// コンテナ名
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
        /// ステータス
        /// </summary>
        [Required]
        public string Status { get; set; }
    }
}

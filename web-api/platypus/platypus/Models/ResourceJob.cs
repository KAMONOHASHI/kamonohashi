using System;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// リソースモニタジョブ
    /// </summary>
    public class ResourceJob : ModelBase
    {
        /// <summary>
        /// ノード名
        /// </summary>
        [Required]
        public string NodeName { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public int NodeCpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）
        /// </summary>
        public int NodeMemory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        public int NodeGpu { get; set; }

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
        /// 作成日時
        /// </summary>
        public DateTime JobCreatedAt { get; set; }

        /// <summary>
        /// 開始日時
        /// </summary>
        public DateTime? JobStartedAt { get; set; }

        /// <summary>
        /// 完了日時
        /// </summary>
        public DateTime JobCompletedAt { get; set; }

        /// <summary>
        /// コンテナ名
        /// </summary>
        [Required]
        public string ContainerName { get; set; }

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

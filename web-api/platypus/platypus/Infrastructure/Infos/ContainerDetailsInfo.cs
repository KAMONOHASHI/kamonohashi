using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure.Infos
{
    public class ContainerDetailsInfo
    {
        /// <summary>
        /// コンテナ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// テナント名
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// コンテナのステータス
        /// </summary>
        public ContainerStatus Status { get; set; }

        /// <summary>
        /// コンテナへのアクセス先ホスト
        /// </summary>
        public string NodeIpAddress { get; set; }
        /// <summary>
        /// ノード名
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 実行者
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 実行日時
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// コンテナの状況を表すメッセージ。異常系の場合に参照される。
        /// </summary>
        public string ConditionNote { get; set; }

        /// <summary>
        /// イメージ名
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public float Cpu { get; set; }
        /// <summary>
        /// メモリ容量（GiB）
        /// </summary>
        public float Memory { get; set; }
        /// <summary>
        /// GPU数
        /// </summary>
        public int Gpu { get; set; }
    }
}

using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;

namespace Nssol.Platypus.ApiModels.ResourceApiModels
{
    /// <summary>
    /// テナントごとのコンテナ詳細の出力モデル
    /// </summary>
    public class ContainerDetailsForTenantOutputModel
    {
        public ContainerDetailsForTenantOutputModel(ContainerDetailsInfo info)
        {
            Name = info.Name;
            CreatedBy = info.CreatedBy;
            Cpu = info.Cpu;
            Memory = info.Memory;
            Gpu = info.Gpu;
            Status = info.Status.Name;
            StatusType = info.Status.StatusType;
            ConditionNote = info.ConditionNote;
            NodeName = info.NodeName;
        }

        /// <summary>
        /// コンテナ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 実行者
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// コンテナ種別
        /// </summary>
        public ContainerType ContainerType { get; set; }

        /// <summary>
        /// コンテナの状態に対する注釈。何か異常が発生している際は注釈が表示される。
        /// </summary>
        public string ConditionNote { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public float Cpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）
        /// </summary>
        public float Memory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        public int Gpu { get; set; }

        /// <summary>
        /// ステータス種別
        /// </summary>
        public string StatusType { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ノード名
        /// </summary>
        public string NodeName { get; set; }
    }
}

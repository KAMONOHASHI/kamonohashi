using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.ClusterApiModels
{
    /// <summary>
    /// ノードのリソース情報出力モデル
    /// </summary>
    public class NodeResourceOutputModel
    {
        /// <summary>
        /// k8sのノード情報とDBのノード情報から、ノードのリソース情報を作成する
        /// </summary>
        public NodeResourceOutputModel(Node node, NodeInfo nodeInfo)
        {
            Name = node.Name;
            Memo = node.Memo;
            Partition = node.Partition;
            AccessLevel = node.AccessLevel;
            AllocatableCpu = nodeInfo.Cpu;
            AllocatableMemory = nodeInfo.Memory;
            AllocatableGpu = nodeInfo.Gpu;
        }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// パーティション
        /// </summary>
        public string Partition { get; set; }

        /// <summary>
        /// ノードのアクセスレベル。
        /// アクセス権がないテナントはこのノードに新規にコンテナを立てることができなくなる。
        /// アクセルレベルが下がっても、実行中のコンテナを殺すことはしない。
        /// </summary>
        public NodeAccessLevel AccessLevel { get; set; }

        /// <summary>
        /// 割り当て可能なCPU
        /// </summary>
        public float AllocatableCpu { get; set; }

        /// <summary>
        /// 割り当て可能なメモリ(単位：GB)
        /// </summary>
        public float AllocatableMemory { get; set; }

        /// <summary>
        /// 割り当て可能なGPU
        /// </summary>
        public float AllocatableGpu { get; set; }
    }
}

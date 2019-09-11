using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.ResourceApiModels
{
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
            TensorBoardEnabled = node.TensorBoardEnabled;
            AllocatableCpu = nodeInfo.Cpu;
            AllocatableMemory = nodeInfo.Memory;
            AllocatableGpu = nodeInfo.Gpu;

            ContainerResourceList = new List<ContainerDetailsOutputModel>();
        }

        /// <summary>
        /// DBのノード情報のみから、ノードのリソース情報を作成する（k8sで見つからない）
        /// </summary>
        public NodeResourceOutputModel(Node node)
        {
            Name = node.Name + " !Disconnected!";
            Memo = node.Memo;
            Partition = node.Partition;
            AccessLevel = node.AccessLevel;
            TensorBoardEnabled = node.TensorBoardEnabled;

            ContainerResourceList = new List<ContainerDetailsOutputModel>();
        }

        /// <summary>
        /// k8sのノード情報のみから、ノードのリソース情報を作成する（DBで見つからない）
        /// </summary>
        public NodeResourceOutputModel(NodeInfo nodeInfo)
        {
            Name = "Unknown:" + nodeInfo.Name;
            AccessLevel = NodeAccessLevel.Disabled;
            AllocatableCpu = nodeInfo.Cpu;
            AllocatableMemory = nodeInfo.Memory;
            AllocatableGpu = nodeInfo.Gpu;

            ContainerResourceList = new List<ContainerDetailsOutputModel>();
        }

        /// <summary>
        /// 未アサインのコンテナを格納するためのモデル
        /// </summary>
        public NodeResourceOutputModel()
        {
            Name = "*Unassigned*";
            AccessLevel = NodeAccessLevel.Disabled;

            ContainerResourceList = new List<ContainerDetailsOutputModel>();
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
        /// TensorBoardの実行可否設定
        /// </summary>
        public bool TensorBoardEnabled { get; set; }

        /// <summary>
        /// 割り当て可能なCpu
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

        /// <summary>
        /// 割り当て済みCpu
        /// </summary>
        public float AssignedCpu { get; set; }

        /// <summary>
        /// 割り当て済みメモリ(単位：GB)
        /// </summary>
        public float AssignedMemory { get; set; }

        /// <summary>
        /// 割り当て済みGPU
        /// </summary>
        public float AssignedGpu { get; set; }

        /// <summary>
        /// Cpu情報取得
        /// </summary>
        public string CpuInfo
        {
            get
            {
                return $"{AssignedCpu} / {AllocatableCpu:0.0}";
            }
        }

        /// <summary>
        /// メモリ情報取得
        /// </summary>
        public string MemoryInfo
        {
            get
            {
                return $"{AssignedMemory} GB / {AllocatableMemory:0.0} GB";
            }
        }

        /// <summary>
        /// Gpu情報取得
        /// </summary>
        public string GpuInfo
        {
            get
            {
                return $"{AssignedGpu} / {AllocatableGpu:0.0}";
            }
        }

        /// <summary>
        /// コンテナリソースのリスト
        /// </summary>
        public List<ContainerDetailsOutputModel> ContainerResourceList { get; set; }

        public void Add(ContainerDetailsOutputModel model)
        {
            AssignedCpu = Util.SumOfFloat(AssignedCpu, model.Cpu);
            AssignedMemory = Util.SumOfFloat(AssignedMemory, model.Memory);
            AssignedGpu += model.Gpu;
            ContainerResourceList.Add(model);
        }

        /// <summary>
        /// モデルのデータ(CPU, MEMORY, GPU)を加算するのみで、モデルのリストへの追加は行わない
        /// </summary>
        public void IncrementData(ContainerDetailsOutputModel model)
        {
            AssignedCpu = Util.SumOfFloat(AssignedCpu, model.Cpu);
            AssignedMemory = Util.SumOfFloat(AssignedMemory, model.Memory);
            AssignedGpu += model.Gpu;
        }
    }
}

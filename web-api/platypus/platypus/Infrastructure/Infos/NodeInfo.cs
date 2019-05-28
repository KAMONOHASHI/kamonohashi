using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure.Infos
{
    public class NodeInfo
    {
        /// <summary>
        /// ノード名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 割り当て可能なメモリ(単位：Gi)
        /// </summary>
        public float Memory { get; set; }

        /// <summary>
        /// 割り当て可能なGPU数
        /// </summary>
        public int Gpu { get; set; }

        /// <summary>
        /// 割り当て可能なCpuコア数
        /// </summary>
        public float Cpu { get; set; }

        /// <summary>
        /// ノードに設定されたラベル
        /// </summary>
        public Dictionary<string, string> Labels { get; set; }
    }
}

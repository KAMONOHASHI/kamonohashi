using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.KubernetesModels
{
    /// <summary>
    /// ノードのリソース状況を表す情報をJsonデシリアライズするためのクラス。
    /// 各REST APIの結果で頻繁に出てくる上に、単位付きの文字列表現で返してくる仕様のため、共通化する。
    /// 特定API特有な値を取得するための、非共通的なプロパティが含まれるが、気にしない。
    /// </summary>
    public class NodeResourceModel
    {
        public string Cpu { get; set; }
        public float CpuNum
        {
            get
            {
                return ConvertCpuStrToFloat(Cpu);
            }
        }
        
        [JsonProperty("nvidia.com/gpu")]
        //[JsonProperty("alpha.kubernetes.io/nvidia-gpu")] // バージョンアップでラベル名が変わったらしい
        public int Gpu { get; set; }

        public string Memory { get; set; }

        /// <summary>
        /// メモリ量（GB）
        /// </summary>
        public float MemoryGi
        {
            get
            {
                return ConvertMemoryStrToFloatGi(Memory);
            }
        }

        /// <summary>
        /// 一部APIでのみ使用される
        /// </summary>
        public int Pods { get; set; }

        /// <summary>
        /// CPU数の文字列表現を、Float型に変換する。
        /// 数値変換に失敗した場合、そのまま例外が上がる。
        /// </summary>
        private static float ConvertCpuStrToFloat(string cpuStr)
        {
            if (string.IsNullOrWhiteSpace(cpuStr))
            {
                return 0;
            }
            if (cpuStr.EndsWith("m")) //mが付いていたら、1/1000倍にする
            {
                var cpuGi = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(cpuGi) / 1000;
            }
            return float.Parse(cpuStr);
        }

        /// <summary>
        /// メモリ容量の文字列表現(e.g. 5Ki, 12Mi, 234Gi)を、Gi単位のFloat型に変換する。
        /// 末尾に単位がない場合、Byte単位と見なす。
        /// 数値変換に失敗した場合、そのまま例外が上がる。
        /// </summary>
        private static float ConvertMemoryStrToFloatGi(string memoryStr)
        {
            if (string.IsNullOrWhiteSpace(memoryStr))
            {
                return 0;
            }
            int divisor;
            string numStr;
            if (memoryStr.EndsWith("Gi")) //元がGi単位なら、そのまま
            {
                divisor = 1;
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
            }
            else if (memoryStr.EndsWith("Mi")) //元がMi単位なら、1024で割る
            {
                divisor = 1024;
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
            }
            else if (memoryStr.EndsWith("m")) //元がm単位なら、1000で割る
            {
                divisor = 1000;
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
            }
            else if (memoryStr.EndsWith("Ki")) //元がKi単位なら、1024*1024で割る
            {
                divisor = 1024 * 1024;
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
            }
            else //その他の場合、Byte単位と見なす
            {
                divisor = 1024 * 1024 * 1024;
                numStr = memoryStr;
            }
            return float.Parse(numStr) / divisor;
        }
    }
}

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
                return ConvertCpuStrToFloatGB(Cpu);
            }
        }
        
        [JsonProperty("nvidia.com/gpu")]
        //[JsonProperty("alpha.kubernetes.io/nvidia-gpu")] // バージョンアップでラベル名が変わったらしい
        public int Gpu { get; set; }

        public string Memory { get; set; }

        /// <summary>
        /// メモリ量（GB）
        /// </summary>
        public float MemoryGB
        {
            get
            {
                return ConvertMemoryStrToFloatGB(Memory);
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
        private static float ConvertCpuStrToFloatGB(string cpuStr)
        {
            if (string.IsNullOrWhiteSpace(cpuStr))
            {
                return 0;
            }
            string numStr;
            if (cpuStr.EndsWith("Ei")) //元がEi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("e") || cpuStr.EndsWith("E")) //元がeまたはE単位なら、1000*1000*1000を掛ける
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr) * 1000 * 1000 * 1000;
            }
            else if (cpuStr.EndsWith("Pi")) //元がPi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("p") || cpuStr.EndsWith("P")) //元がpまたはP単位なら、1000*1000を掛ける
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr) * 1000 * 1000;
            }
            else if (cpuStr.EndsWith("Ti")) //元がTi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("t") || cpuStr.EndsWith("T")) //元がtまたはT単位なら、1000を掛ける
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr) * 1000;
            }
            else if (cpuStr.EndsWith("Gi")) //元がGi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("g") || cpuStr.EndsWith("G")) //元がgまたはG単位なら、そのまま
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr);
            }
            else if (cpuStr.EndsWith("Mi")) //元がMi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("m") || cpuStr.EndsWith("M")) //元がmまたはM単位なら、1000で割る
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr) / 1000;
            }
            else if (cpuStr.EndsWith("Ki")) //元がKi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("k") || cpuStr.EndsWith("K")) //元がkまたはK単位なら、1000*1000で割る
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr) / 1000 / 1000; ;
            }
            // その他の場合、GB単位と見なす
            return float.Parse(cpuStr);
        }

        /// <summary>
        /// メモリ容量の文字列表現(e.g. 5Ki, 12Mi, 234Gi)を、GB単位のFloat型に変換する。
        /// 末尾に単位がない場合、Byte単位と見なす。
        /// 数値変換に失敗した場合、そのまま例外が上がる。
        /// </summary>
        private static float ConvertMemoryStrToFloatGB(string memoryStr)
        {
            if (string.IsNullOrWhiteSpace(memoryStr))
            {
                return 0;
            }
            string numStr;
            if (memoryStr.EndsWith("Ei")) //元がEi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("e") || memoryStr.EndsWith("E")) //元がeまたはE単位なら、1000*1000*1000を掛ける
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr) * 1000 * 1000 * 1000;
            }
            else if (memoryStr.EndsWith("Pi")) //元がPi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("p") || memoryStr.EndsWith("P")) //元がpまたはP単位なら、1000*1000を掛ける
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr) * 1000 * 1000;
            }
            else if (memoryStr.EndsWith("Ti")) //元がTi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("t") || memoryStr.EndsWith("T")) //元がtまたはT単位なら、1000を掛ける
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr) * 1000;
            }
            else if (memoryStr.EndsWith("Gi")) //元がGi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("g") || memoryStr.EndsWith("G")) //元がgまたはG単位なら、そのまま
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr);
            }
            else if (memoryStr.EndsWith("Mi")) //元がMi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("m") || memoryStr.EndsWith("M")) //元がmまたはM単位なら、1000で割る
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr) / 1000;
            }
            else if (memoryStr.EndsWith("Ki")) //元がKi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("k") || memoryStr.EndsWith("K")) //元がkまたはK単位なら、1000*1000で割る
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr) / 1000 / 1000; ;
            }
            else //その他の場合、Byte単位と見なす
            {
                numStr = memoryStr;
                return float.Parse(numStr) / 1000 / 1000 / 1000;
            }
        }
    }
}

using Newtonsoft.Json;
using System;

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
            if (cpuStr.EndsWith("Ei", StringComparison.CurrentCulture)) //元がEi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("e", StringComparison.CurrentCulture) || cpuStr.EndsWith("E", StringComparison.CurrentCulture)) //元がeまたはE単位なら、1000*1000*1000を掛ける
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr) * 1000 * 1000 * 1000;
            }
            else if (cpuStr.EndsWith("Pi", StringComparison.CurrentCulture)) //元がPi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("p", StringComparison.CurrentCulture) || cpuStr.EndsWith("P", StringComparison.CurrentCulture)) //元がpまたはP単位なら、1000*1000を掛ける
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr) * 1000 * 1000;
            }
            else if (cpuStr.EndsWith("Ti", StringComparison.CurrentCulture)) //元がTi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("t", StringComparison.CurrentCulture) || cpuStr.EndsWith("T", StringComparison.CurrentCulture)) //元がtまたはT単位なら、1000を掛ける
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr) * 1000;
            }
            else if (cpuStr.EndsWith("Gi", StringComparison.CurrentCulture)) //元がGi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("g", StringComparison.CurrentCulture) || cpuStr.EndsWith("G", StringComparison.CurrentCulture)) //元がgまたはG単位なら、そのまま
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr);
            }
            else if (cpuStr.EndsWith("Mi", StringComparison.CurrentCulture)) //元がMi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("m", StringComparison.CurrentCulture) || cpuStr.EndsWith("M", StringComparison.CurrentCulture)) //元がmまたはM単位なら、1000で割る
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 1);
                return float.Parse(numStr) / 1000;
            }
            else if (cpuStr.EndsWith("Ki", StringComparison.CurrentCulture)) //元がKi単位なら、Byte単位にして単位換算する
            {
                numStr = cpuStr.Substring(0, cpuStr.Length - 2);
                return float.Parse(numStr) * 1024 / 1000 / 1000 / 1000;
            }
            else if (cpuStr.EndsWith("k", StringComparison.CurrentCulture) || cpuStr.EndsWith("K", StringComparison.CurrentCulture)) //元がkまたはK単位なら、1000*1000で割る
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
            if (memoryStr.EndsWith("Ei", StringComparison.CurrentCulture)) //元がEi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("e", StringComparison.CurrentCulture) || memoryStr.EndsWith("E", StringComparison.CurrentCulture)) //元がeまたはE単位なら、1000*1000*1000を掛ける
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr) * 1000 * 1000 * 1000;
            }
            else if (memoryStr.EndsWith("Pi", StringComparison.CurrentCulture)) //元がPi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("p", StringComparison.CurrentCulture) || memoryStr.EndsWith("P", StringComparison.CurrentCulture)) //元がpまたはP単位なら、1000*1000を掛ける
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr) * 1000 * 1000;
            }
            else if (memoryStr.EndsWith("Ti", StringComparison.CurrentCulture)) //元がTi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("t", StringComparison.CurrentCulture) || memoryStr.EndsWith("T", StringComparison.CurrentCulture)) //元がtまたはT単位なら、1000を掛ける
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr) * 1000;
            }
            else if (memoryStr.EndsWith("Gi", StringComparison.CurrentCulture)) //元がGi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("g", StringComparison.CurrentCulture) || memoryStr.EndsWith("G", StringComparison.CurrentCulture)) //元がgまたはG単位なら、そのまま
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr);
            }
            else if (memoryStr.EndsWith("Mi", StringComparison.CurrentCulture)) //元がMi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("m", StringComparison.CurrentCulture) || memoryStr.EndsWith("M", StringComparison.CurrentCulture)) //元がmまたはM単位なら、1000で割る
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 1);
                return float.Parse(numStr) / 1000;
            }
            else if (memoryStr.EndsWith("Ki", StringComparison.CurrentCulture)) //元がKi単位なら、Byte単位にして単位換算する
            {
                numStr = memoryStr.Substring(0, memoryStr.Length - 2);
                return float.Parse(numStr) * 1024 / 1000 / 1000 / 1000;
            }
            else if (memoryStr.EndsWith("k", StringComparison.CurrentCulture) || memoryStr.EndsWith("K", StringComparison.CurrentCulture)) //元がkまたはK単位なら、1000*1000で割る
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

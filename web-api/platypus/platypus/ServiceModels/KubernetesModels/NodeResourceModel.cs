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
                return ConvertResourceStrToFloatGB(Cpu);
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
                return ConvertResourceStrToFloatGB(Memory);
            }
        }

        /// <summary>
        /// 一部APIでのみ使用される
        /// </summary>
        public int Pods { get; set; }

        /// <summary>
        /// CPU数、メモリ容量の文字列表現(e.g. 5Ki, 12Mi, 234Gi)を、GB単位のFloat型に変換する。
        /// 末尾に単位がない場合、Byte単位と見なす。
        /// 数値変換に失敗した場合、そのまま例外が上がる。
        /// </summary>
        private static float ConvertResourceStrToFloatGB(string resourceStr)
        {
            if (string.IsNullOrWhiteSpace(resourceStr))
            {
                return 0;
            }
            string numStr;
            if (resourceStr.EndsWith("Ei")) //元がEi単位なら、Byte単位にして単位換算する
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (resourceStr.EndsWith("e") || resourceStr.EndsWith("E")) //元がeまたはE単位なら、1000*1000*1000を掛ける
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 1);
                return float.Parse(numStr) * 1000 * 1000 * 1000;
            }
            else if (resourceStr.EndsWith("Pi")) //元がPi単位なら、Byte単位にして単位換算する
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (resourceStr.EndsWith("p") || resourceStr.EndsWith("P")) //元がpまたはP単位なら、1000*1000を掛ける
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 1);
                return float.Parse(numStr) * 1000 * 1000;
            }
            else if (resourceStr.EndsWith("Ti")) //元がTi単位なら、Byte単位にして単位換算する
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (resourceStr.EndsWith("t") || resourceStr.EndsWith("T")) //元がtまたはT単位なら、1000を掛ける
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 1);
                return float.Parse(numStr) * 1000;
            }
            else if (resourceStr.EndsWith("Gi")) //元がGi単位なら、Byte単位にして単位換算する
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (resourceStr.EndsWith("g") || resourceStr.EndsWith("G")) //元がgまたはG単位なら、そのまま
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 1);
                return float.Parse(numStr);
            }
            else if (resourceStr.EndsWith("Mi")) //元がMi単位なら、Byte単位にして単位換算する
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 2);
                return float.Parse(numStr) * 1024 * 1024 / 1000 / 1000 / 1000;
            }
            else if (resourceStr.EndsWith("m") || resourceStr.EndsWith("M")) //元がmまたはM単位なら、1000で割る
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 1);
                return float.Parse(numStr) / 1000;
            }
            else if (resourceStr.EndsWith("Ki")) //元がKi単位なら、Byte単位にして単位換算する
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 2);
                return float.Parse(numStr) * 1024 / 1000 / 1000 / 1000;
            }
            else if (resourceStr.EndsWith("k") || resourceStr.EndsWith("K")) //元がkまたはK単位なら、1000*1000で割る
            {
                numStr = resourceStr.Substring(0, resourceStr.Length - 1);
                return float.Parse(numStr) / 1000 / 1000; ;
            }
            else //その他の場合、Byte単位と見なす
            {
                numStr = resourceStr;
                return float.Parse(numStr) / 1000 / 1000 / 1000;
            }
        }
    }
}

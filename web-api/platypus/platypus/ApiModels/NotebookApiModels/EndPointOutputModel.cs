using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.NotebookApiModels
{
    /// <summary>
    /// ノートブックアクセス用URLの出力モデル
    /// </summary>
    public class EndPointOutputModel
    {
        public EndPointOutputModel(string url)
        {
            Url = url;
        }

        /// <summary>
        /// NotebookのエンドポイントURL
        /// </summary>
        public string Url { get; set; }
    }
}

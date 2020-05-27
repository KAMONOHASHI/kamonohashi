using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.DataSetApiModels
{
    // データセットのデータのパスと、NFSのファイルのパスとのペア情報を保持するモデル
    public class PathPairOutputModel
    {
        public PathPairOutputModel(string dataPath, string storedPath)
        {
            this.DataPath = dataPath;
            this.StoredPath = storedPath;
        }
        /// <summary>
        /// データセットのデータのパス 例: /training/123/test.bin
        /// </summary>
        public string DataPath { get; set; }


        /// <summary>
        /// NFS上のファイルのパス 例: 32f8b149-8bf2-42bf-9656-d512c4092449.bin
        /// </summary>
        public string StoredPath { get; set; }
    }
}

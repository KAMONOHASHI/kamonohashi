using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure.Infos
{
    public class EndPointInfo
    {
        /// <summary>
        /// エンドポイントの識別名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// ホスト
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// ポート番号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// エンドポイントURL
        /// </summary>
        public string Url { get
            {
                return $"{Host}:{Port}";
            }
        }
    }
}

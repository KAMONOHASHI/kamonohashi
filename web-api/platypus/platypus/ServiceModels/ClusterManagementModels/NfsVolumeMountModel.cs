using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.ClusterManagementModels
{
    public class NfsVolumeMountModel
    {
        /// <summary>
        /// 識別名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// コンテナ側のパス。
        /// このディレクトリ直下にk8sのsubpath機能で<see cref="SubPath"/>ディレクトリがつくられる。
        /// </summary>
        public string MountPath { get; set; }
        /// <summary>
        /// コンテナ側のサブパス
        /// </summary>
        public string SubPath { get; set; }

        /// <summary>
        /// サーバ名
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// サーバ側のパス
        /// </summary>
        public string ServerPath { get; set; }

        /// <summary>
        /// 読み取り専用フラグ
        /// </summary>
        public bool ReadOnly { get; set; }

    }
}

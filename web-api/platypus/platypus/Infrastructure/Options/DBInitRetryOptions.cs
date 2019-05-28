using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure.Options
{
    public class DBInitRetryOptions
    {
        /// <summary>
        /// DB 初期化失敗時の retry 最大数
        /// </summary>
        public int InitDBRetryMaxCount { get; set; }

        /// <summary>
        /// DB 初期化失敗時の retry 間の sleep 秒
        /// </summary>
        public int InitDBRetrySleepSec { get; set; }

        /// <summary>
        /// ObjectStore 同期化失敗時の retry 最大数
        /// </summary>
        public int SyncObjectStoreRetryMaxCount { get; set; }

        /// <summary>
        /// ObjectStore 同期化失敗時の retry 間の sleep 秒
        /// </summary>
        public int SyncObjectStoreRetrySleepSec { get; set; }

        /// <summary>
        /// Cluster 同期化失敗時の retry 最大数
        /// </summary>
        public int SyncClusterRetryMaxCount { get; set; }

        /// <summary>
        /// Cluster 同期化失敗時の retry 間の sleep 秒
        /// </summary>
        public int SyncClusterRetrySleepSec { get; set; }
    }
}

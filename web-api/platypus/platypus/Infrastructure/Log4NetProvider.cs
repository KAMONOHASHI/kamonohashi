using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// lo4netのILoggerを提供するロガープロバイダークラス
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILoggerProvider" />
    public class Log4NetProvider : ILoggerProvider
    {
        private ConcurrentDictionary<string, ILogger> loggers = new ConcurrentDictionary<string, ILogger>();

        /// <summary>
        /// 指定された名前でロガーを生成します。
        /// ロガーはリクエスト内でキャッシュされます
        /// </summary>
        /// <param name="categoryName">ロガー名</param>
        /// <returns>ロガー</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, new Log4NetAdapter(categoryName));
        }

        /// <summary>
        /// アンマネージ リソースの解放またはリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose()
        {
            if (loggers != null)
            {
                loggers.Clear();
                loggers = null;

                GC.SuppressFinalize(this);
            }
        }
    }
}

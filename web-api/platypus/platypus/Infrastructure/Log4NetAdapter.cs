using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// ASP.NET Core のロガーの仕組みで log4Net を使用するためのアダプタークラス
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger" />
    public class Log4NetAdapter : ILogger
    {
        private ILog logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">ロガー名</param>
        public Log4NetAdapter(string name)
        {
            var repository = typeof(LogManager).GetTypeInfo().Assembly;
            logger = LogManager.GetLogger(repository, name);
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        /// An IDisposable that ends the logical operation scope on dispose.
        /// </returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// 指定されたログレベルが有効か否かをチェックします。
        /// </summary>
        /// <param name="logLevel">対象のログレベル</param>
        /// <returns>
        /// True:有効
        /// </returns>
        /// <exception cref="ArgumentException">指定されたログレベル[{logLevel}]は適用できません。</exception>
        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return logger.IsDebugEnabled;
                case LogLevel.Information:
                    return logger.IsInfoEnabled;
                case LogLevel.Warning:
                    return logger.IsWarnEnabled;
                case LogLevel.Error:
                    return logger.IsErrorEnabled;
                case LogLevel.Critical:
                    return logger.IsFatalEnabled;
                default:
                    throw new ArgumentException("指定されたログレベル[{logLevel}]は適用できません。");
            }
        }

        /// <summary>
        /// 指定された条件でログ出力します。
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel">ログレベル</param>
        /// <param name="eventId">イベントID</param>
        /// <param name="state">ログメッセージオブジェクト</param>
        /// <param name="exception">例外オブジェクト</param>
        /// <param name="formatter">ログメッセージ文字列に変換するフォーマッタ関数</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string message = null;
            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            else
            {
                //message 
            }
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    logger.Debug(message, exception);
                    break;
                case LogLevel.Information:
                    logger.Info(message, exception);
                    break;
                case LogLevel.Warning:
                    logger.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    logger.Error(message, exception);
                    break;
                case LogLevel.Critical:
                    logger.Fatal(message, exception);
                    break;
                default:
                    logger.Warn("指定されたログレベル[{logLevel}]は識別できません。Infoとしてログメッセージを出力します。");
                    logger.Info(message, exception);
                    break;
            }
        }
    }
}

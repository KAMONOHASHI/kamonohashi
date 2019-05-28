using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// Controller, Logic, DataAccessの各レイヤ内で閉じない共通処理をまとめたユーティリティクラス。
    /// <see cref="Startup"/> や Filterなどにまたがるstaticメソッドも含む。
    /// </summary>
    public static class LogUtil
    {
        /// <summary>
        /// イベントログに出力するイベントID
        /// </summary>
        public static readonly EventId EventId = new EventId(1, "platypus");

        /// <summary>
        /// プレゼンテーション層でのログ文字列を生成する。
        /// </summary>
        /// <param name="remoteIpAddress">接続元IPアドレス</param>
        /// <param name="user">ユーザ名。Nullか空なら"-"になる</param>
        /// <param name="requestId">リクエストID</param>
        /// <param name="method">HTTPメソッド</param>
        /// <param name="path">"/"から始まるリクエストURLパス。Nullか空なら"-"になる</param>
        /// <param name="message">ログメッセージ</param>
        private static string CreatePLog(IPAddress remoteIpAddress, string user, string requestId, string method, PathString path, string message)
        {
            string userStr = string.IsNullOrEmpty(user) ? "-" : user;
            string pathStr = string.IsNullOrEmpty(path) ? "-" : path.Value;
            string log = $"{remoteIpAddress},{userStr},{requestId},{method},{pathStr},{message}";
            return log;
        }

        /// <summary>
        /// プレゼンテーション層でのHTTPコンテキストに関連したログ文字列を生成する。
        /// </summary>
        /// <param name="context">HTTPコンテキスト</param>
        /// <param name="message">ログメッセージ</param>
        private static string CreatePLog(HttpContext context, string message)
        {
            string log = CreatePLog(context.Connection.RemoteIpAddress, context.GetClaims()?.Name, context.TraceIdentifier,
                context.Request.Method, context.Request.Path, message
            );
            return log;
        }
        /// <summary>
        /// システムの稼働に関するログを出力する。
        /// </summary>
        public static void WriteSystemLog(Action<string, object[]> logMethod, string message)
        {
            string log = CreatePLog(IPAddress.Loopback, "platypus", "platypus", "-", "/system", message);
            logMethod(log, Array.Empty<object>());
        }
        /// <summary>
        /// システムの稼働に関するエラーログを出力する。
        /// </summary>
        public static void WriteSystemErrorLog(ILogger logger, string message, Exception e)
        {
            string log = CreatePLog(IPAddress.Loopback, "platypus", "platypus", "-", "/system", message);
            logger.LogError(EventId, e, message);
        }

        /// <summary>
        /// プレゼンテーション層でのログを出力する。
        /// </summary>
        /// <param name="logMethod">ILoggerインスタンスのログ出力メソッドを指定</param>
        /// <param name="context">HTTPコンテキスト</param>
        /// <param name="message">ログメッセージ</param>
        public static void WritePLog(Action<string, object[]> logMethod, HttpContext context, string message)
        {
            string log = CreatePLog(context.Connection.RemoteIpAddress, context.GetClaims()?.Name, context.TraceIdentifier,
                context.Request.Method, context.Request.Path, message
            );
            logMethod(log, Array.Empty<object>());
        }

        /// <summary>
        /// プレゼンテーション層での例外ログを出力する。
        /// </summary>
        /// <param name="logger">ILoggerインスタンス</param>
        /// <param name="context">HTTPコンテキスト</param>
        /// <param name="message">ログメッセージ</param>
        /// <param name="e">例外</param>
        public static void WriteErrorPLog(ILogger logger, HttpContext context, string message, Exception e)
        {
            string log = CreatePLog(context, message);
            logger.LogError(EventId, e, log);
        }

        /// <summary>
        /// ロジック層でのログを出力する。
        /// </summary>
        /// <param name="logMethod">ILoggerインスタンスのログ出力メソッドを指定</param>
        /// <param name="user">ログイン中のユーザ名</param>
        /// <param name="requestId">リクエストID</param>
        /// <param name="message">ログメッセージ</param>
        public static void WriteLLog(Action<string, object[]> logMethod, string user, string requestId, string message)
        {
            string log = $"{user},{requestId},{message}";
            logMethod(log, Array.Empty<object>());
        }

        /// <summary>
        /// ロジック層での例外ログを出力する。
        /// </summary>
        /// <param name="logger">ILoggerインスタンス</param>
        /// <param name="user">ログイン中のユーザ名</param>
        /// <param name="requestId">リクエストID</param>
        /// <param name="message">ログメッセージ</param>
        /// <param name="e">例外</param>
        public static void WriteErrorLLog(ILogger logger, string user, string requestId, string message, Exception e)
        {
            string log = $"{user},{requestId},{message}";
            logger.LogError(EventId, e, log);
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.ApiModels;
using Nssol.Platypus.Controllers.Util;
using System;

namespace Nssol.Platypus.Filters
{
    /// <summary>
    /// WEBAPIにおいてハンドルされない例外を処理する属性クラス。
    /// <see cref="PlatypusApiControllerBase"/> に付与すればサブクラス全てに付与したことになる。
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute" />
    public sealed class JsonExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public ILogger<JsonExceptionHandlerAttribute> Logger { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        public JsonExceptionHandlerAttribute(ILogger<JsonExceptionHandlerAttribute> logger)
        {
            this.Logger = logger;
        }


        /// <summary>
        /// 例外をハンドルします。
        /// ログを出力し、500エラーをあらわすJSONオブジェクトをかえします。
        /// </summary>
        /// <param name="context">例外コンテキスト</param>
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            Infrastructure.LogUtil.WriteErrorPLog(Logger, context.HttpContext, "Raise Unhandled Exception in API Request.", context.Exception);

            int statusCode = 500;
            if (context.Exception is UnauthorizedAccessException)
            {
                //権限のない操作を実行した。トークン不正の可能性があるので、ログオフさせるために403ではなく401を返す。
                statusCode = 401;
            }

            var response = new JsonErrorResponse()
            {
                Type = this.GetType().FullName,
                Title = context.Exception.Message,
                Instance = context.HttpContext?.Request?.Path.Value,
                Detail = context.Exception.StackTrace,
            };
            context.Result = new CustomJsonResult(statusCode, response);
            base.OnException(context);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Filters
{
    /// <summary>
    /// グローバル集約エラーハンドラ属性クラス
    /// </summary>
    /// <remarks>
    /// ハンドルされなかったすべての例外（WebAPI以外）のログを出力します。
    /// </remarks>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute" />
    public sealed class GlobalExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public ILogger<GlobalExceptionHandlerAttribute> Logger { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        public GlobalExceptionHandlerAttribute(ILogger<GlobalExceptionHandlerAttribute> logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// 例外をハンドルし、ログ出力します。
        /// JsonExceptionHandlerAttributeが付与されている場合は本処理は実行しません。
        /// </summary>
        /// <param name="context">例外コンテキスト</param>
        /// <returns></returns>
        /// <inheritdoc />
        public async override Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ActionDescriptor.FilterDescriptors.Where(x=>x.Filter.GetType() == typeof(ServiceFilterAttribute))
                .Count(x=>(x.Filter as ServiceFilterAttribute).ServiceType == typeof(JsonExceptionHandlerAttribute)) > 0)
            {
                await base.OnExceptionAsync(context);
                return;
            }

            Infrastructure.LogUtil.WriteErrorPLog(Logger, context.HttpContext, "Raise Unhandled Exception in WebUI Request.", context.Exception);

            context.Result = new ViewResult()
            {
                StatusCode = 500,
                ViewName = "500"
            };

            await base.OnExceptionAsync(context);
            return;
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.ApiModels;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using System.Linq;
using System.Net;

namespace Nssol.Platypus.Controllers.Util
{
    /// <summary>
    /// API用のコントローラに汎用的な処理を追加するための共通親クラス。
    /// メンバは基本的にはprotected。
    /// </summary>
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ServiceFilter(typeof(JsonExceptionHandlerAttribute))]
    public class PlatypusApiControllerBase : Controller
    {
        /// <summary>
        /// 変更なしを意味する文字列。
        /// ASP.NET Coreだとnullと空文字の区別ができず、そのままだと「空文字への変更」と「変更なし」を区別できない。
        /// なので、空文字を表す文字列を定義して、区別をつける。
        /// </summary>
        /// <remarks>
        /// TODO: Python3系やLinux/Macで空文字列を指定した際のASP.NET側の文字列がどうなるか検証する。
        /// </remarks>
        public const string ValueOfEmptyString = "''";
        
        /// <summary>
        /// ログインユーザ情報。
        /// </summary>
        protected UserInfo CurrentUserInfo { get; private set; }

        /// <summary>
        /// ログ出力を行うためのロガーインスタンス。
        /// </summary>
        private ILogger logger { get; set; }

        /// <summary>
        /// ログイン中のユーザ名
        /// </summary>
        protected string UserName
        {
            get
            {
                var user = User?.Identity?.Name;
                return string.IsNullOrEmpty(user) ? "-" : user;
            }
        }
        /// <summary>
        /// アクセス元IPアドレス
        /// </summary>
        protected string RemoteIpAddress
        {
            get
            {
                return HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }
        /// <summary>
        /// リクエストURL
        /// </summary>
        protected string RequestUrl
        {
            get
            {
                var url = HttpContext.Request.Path.Value;
                return string.IsNullOrEmpty(url) ? "-" : url;
            }
        }
        /// <summary>
        /// リクエストごとにユニークに採番されるID
        /// </summary>
        protected string RequestId
        {
            get
            {
                return HttpContext.TraceIdentifier;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlatypusApiControllerBase(IHttpContextAccessor accessor) : base()
        {
            var provider = accessor.HttpContext.RequestServices;
            //ロガーのDI
            ILoggerFactory loggerFactory = (ILoggerFactory)provider.GetService(typeof(ILoggerFactory));
            logger = loggerFactory.CreateLogger(this.GetType());

            IMultiTenancyLogic multiTenancyLogic = (IMultiTenancyLogic)provider.GetService(typeof(IMultiTenancyLogic));
            CurrentUserInfo = multiTenancyLogic.CurrentUserInfo;
        }

        /// <summary>
        /// ヘッダーに総数を追加する
        /// </summary>
        protected void SetTotalCountToHeader(int totalCount)
        {
            Request.HttpContext.Response.Headers.Add("X-Total-Count", totalCount.ToString());
        }

        #region 項目編集用メソッド
        /// <summary>
        /// 入力値が、空なのか、初期化なのか、新規文字列なのかによって、返す値を変える
        /// </summary>
        protected string EditColumn(string input, string current)
        {
            if (input == null)
            {
                //入力が何もない＝未指定
                return current;
            }
            if (input == ValueOfEmptyString)
            {
                //入力が空を表す文字列＝空で初期化
                return "";
            }
            return input;
        }
        /// <summary>
        /// 入力値が、空なのか、新規文字列なのかによって、返す値を変える（空文字更新はしない）
        /// </summary>
        protected string EditColumnNotEmpty(string input, string current)
        {
            if (string.IsNullOrEmpty(input))
            {
                //入力が何もないまたは、空を表す文字列＝未指定
                return current;
            }
            return input;
        }
        /// <summary>
        /// 入力値が、空なのか、新規値なのかによって、返す値を変える
        /// </summary>
        protected int EditColumn(int? input, int current)
        {
            if (input == null)
            {
                //入力が何もない＝未指定
                return current;
            }
            return input.Value;
        }
        /// <summary>
        /// 入力値が、空なのか、新規値なのかによって、返す値を変える
        /// </summary>
        protected bool EditColumn(bool? input, bool current)
        {
            if (input == null)
            {
                //入力が何もない＝未指定
                return current;
            }
            return input.Value;
        }
        #endregion

        #region JsonResultを返すメソッド群

        /// <summary>
        /// 既定されたJSON形式の BadRequest を返す
        /// </summary>
        /// <param name="message">送信するエラーメッセージ</param>
        protected JsonResult JsonBadRequest(string message = null)
        {
            //return new CustomJsonResult(400, new JsonErrorResponse() { Errors = new List<JsonErrorMessage>() { new JsonErrorMessage() { Message = message, Code = 1 } } });
            return JsonError(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// ModelStateから入力値検査エラーを取得し、既定されたJSON形式の BadRequest として返す
        /// </summary>
        /// <param name="message">送信するエラーメッセージ</param>
        protected JsonResult JsonValidationError(string message = null)
        {
            foreach(var model in ModelState.Values)
            {

            }
            return JsonError(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// 既定されたJSON形式の NotFound を返す
        /// </summary>
        /// <param name="message">送信するエラーメッセージ</param>
        protected JsonResult JsonNotFound(string message = null)
        {
            return JsonError(HttpStatusCode.NotFound, message);
        }

        /// <summary>
        /// 既定されたJSON形式の Conflict を返す
        /// </summary>
        /// <param name="message">送信するエラーメッセージ</param>
        protected JsonResult JsonConflict(string message = null)
        {
            return JsonError(HttpStatusCode.Conflict, message);
        }

        /// <summary>
        /// 既定されたJSON形式のエラーメッセージを返す
        /// </summary>
        /// <param name="status">レスポンスのHTTPステータスコード</param>
        /// <param name="message">送信するエラーメッセージ</param>
        protected JsonResult JsonError(HttpStatusCode status, string message)
        {
            var response = new JsonErrorResponse()
            {
                Type = this.GetType().FullName,
                Title = message,
                Instance = RequestUrl
            };

            var errors = ModelState?.Values.SelectMany(x => x.Errors);
            if (errors != null && errors.Count() > 0)
            {
                //エラーメッセージがあればそれを出力。なければExceptionのエラーメッセージを出力。
                response.Errors = errors.Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage);
            }

            return new CustomJsonResult((int)status, response);
        }

        /// <summary>
        /// 既定されたJSON形式の OK を返す
        /// </summary>
        /// <param name="data">送信するJSONデータ</param>
        protected JsonResult JsonOK(object data)
        {
            return new CustomJsonResult((int)HttpStatusCode.OK, data);
        }

        /// <summary>
        /// 既定されたJSON形式の NotFound を返す
        /// </summary>
        /// <param name="data">送信するJSONデータ</param>
        protected JsonResult JsonCreated(object data)
        {
            return new CustomJsonResult((int)HttpStatusCode.Created, data);
        }

        /// <summary>
        /// 既定されたJSON形式の NoContent を返す
        /// </summary>
        protected JsonResult JsonNoContent()
        {
            return new CustomJsonResult((int)HttpStatusCode.NoContent, null);
        }

        #endregion

        #region ログメソッド
        /// <summary>
        /// Errorログを出力する
        /// </summary>
        protected void LogError(string message)
        {
            LogUtil.WritePLog(logger.LogError, this.HttpContext, message);
        }
        /// <summary>
        /// Warningログを出力する
        /// </summary>
        protected void LogWarning(string message)
        {
            LogUtil.WritePLog(logger.LogWarning, this.HttpContext, message);
        }
        /// <summary>
        /// Infomationログを出力する
        /// </summary>
        protected void LogInformation(string message)
        {
            LogUtil.WritePLog(logger.LogInformation, this.HttpContext, message);
        }
        /// <summary>
        /// Debugログを出力する
        /// </summary>
        protected void LogDebug(string message)
        {
            LogUtil.WritePLog(logger.LogDebug, this.HttpContext, message);
        }
        #endregion
    }
}

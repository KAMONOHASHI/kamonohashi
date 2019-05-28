using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.Util
{
    /// <summary>
    /// Json形式のレスポンスを返すためのActionResultクラス
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.JsonResult" />
    public class CustomJsonResult : JsonResult
    {
        /// <summary>
        /// 日付フォーマット。
        /// yyyy-MM-dd'T'HH:mm:ss.ffK(UTCだとZになる)
        /// </summary>
        private const string DateFormat = "yyyy-MM-dd'T'HH:mm:ss.ffK";
        private int statusCode = 200;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="code">ステータスコード</param>
        /// <param name="data">データ</param>
        public CustomJsonResult(int code, object data)
            : base(data)
        {
            this.statusCode = code;
        }

        /// <summary>
        /// MVCのアクションメソッドの結果を処理します。
        /// </summary>
        /// <param name="context">実行コンテキスト</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">context</exception>
        /// <inheritdoc />
        public async override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new UnauthorizedAccessException("The context of this HTTP request is not defined.");
            }

            HttpResponse response = context.HttpContext.Response;
            await SerializeJsonAsync(response);
        }

        /// <summary>
        /// 指定されたデータをJSONにシリアライズしレスポンスに格納します。
        /// </summary>
        /// <param name="response">格納するレスポンス</param>
        /// <returns></returns>
        public async Task SerializeJsonAsync(HttpResponse response)
        {
            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json; charset=utf-8";
            }

            response.Headers.Add("X-Content-Type-Options", "nosniff");
            response.Headers.Add("Pragma", "no-cache");
            response.Headers.Add("Cache-Control", "no-store, no-cache");
            response.Headers.Add("X-XSS-Protection", "1; mode=block");

            if(response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                response.Headers["Access-Control-Allow-Origin"] = "*";
            }
            else
            {
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            }
            response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
            //response.Headers.Add("Access-Control-Allow-Credentials", "true");
            //response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            //response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");

            // HTTPS対応
            response.Headers.Add("Strict-Transport-Security", "max-age=15768000");

            response.StatusCode = statusCode;
            if (Value != null)
            {
                // Using Json.NET serializer
                var isoConvert = new IsoDateTimeConverter();
                isoConvert.DateTimeStyles = System.Globalization.DateTimeStyles.AdjustToUniversal;
                isoConvert.DateTimeFormat = DateFormat;
                //isoConvert,
                await response.WriteAsync(JsonConvert.SerializeObject(
                    Value, new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Converters = new List<JsonConverter>() { isoConvert },
                        Formatting = Formatting.Indented,
                        StringEscapeHandling = StringEscapeHandling.Default,
                    }),
                    Encoding.UTF8 //指定しなくてもUTF8になるが、念のため明記
                );
                return;
            }
            if (response.StatusCode != 204) //NoContentは結果を書けないので、それ以外の場合だけ対応
            {
                await response.WriteAsync("", Encoding.UTF8);
            }
            return;
        }

    }
}

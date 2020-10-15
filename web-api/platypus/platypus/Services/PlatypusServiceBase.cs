
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nssol.Platypus.Infrastructure;
using RazorLight;
using RazorLight.Razor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services
{
    /// <summary>
    /// サービスクラスに汎用的な処理を追加するための共通親クラス。
    /// 現時点では特にロジック層との違いはないため、処理はすべて委譲。
    /// 何か不都合が起きたらちゃんと分離する。
    /// </summary>
    public class PlatypusServiceBase : Logic.PlatypusLogicBase
    {
        /// <summary>
        /// テンプレートファイルから文字列を生成するためのエンジン。
        /// パラメタ名を静的に解決しているようで、汎化(e.g. object)にしてしまうと、具象型のプロパティにアクセスできない。
        /// そのため、文字列生成処理自体はBaseでメソッド化せず、実クラス側で行う。
        /// </summary>
        /// <remarks>
        /// 文字列化に失敗すると<see cref="RazorLight.Generation.TemplateGenerationException"/>が発生する。
        /// 基本的にユーザ入力で失敗することはないため、try/catchは用意しない。
        /// </remarks>
        protected IRazorLightEngine RenderEngine { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlatypusServiceBase(Logic.Interfaces.ICommonDiLogic commonDiLogic, string templateDir = null) : base(commonDiLogic)
        {
            if(string.IsNullOrEmpty(templateDir) == false)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), templateDir);
                var project = new FileSystemRazorProject(path);
                //自動で追加される拡張子を消す https://github.com/toddams/RazorLight/issues/94
                project.Extension = "";
                IRazorLightEngine engine = new RazorLightEngineBuilder()
                  .UseProject(project)
                  .UseMemoryCachingProvider()
                  .Build();
                RenderEngine = engine;
            }
        }

        /// <summary>
        /// 外部サービスのREST APIにPOSTリクエストを送信する。
        /// </summary>
        /// <returns>送信結果</returns>
        protected async Task<ResponseResult> SendPostRequestAsync(RequestParam param)
        {
            return await SendRequestAndGetStringAsync(HttpMethod.Post, param);
        }

        /// <summary>
        /// 外部サービスのREST APIにPUTリクエストを送信する。
        /// </summary>
        /// <returns>送信結果</returns>
        protected async Task<ResponseResult> SendPutRequestAsync(RequestParam param)
        {
            return await SendRequestAndGetStringAsync(HttpMethod.Put, param);
        }

        /// <summary>
        /// 外部サービスのREST APIにGETリクエストを送信する。
        /// </summary>
        /// <param name="param">リクエストパラメータ</param>
        /// <param name="errLogMode">ログモードで true ならエラー出力</param>
        /// <returns>送信結果</returns>
        protected async Task<ResponseResult> SendGetRequestAsync(RequestParam param, bool errLogMode=true)
        {
            return await SendRequestAndGetStringAsync(HttpMethod.Get, param, errLogMode);
        }

        /// <summary>
        /// 外部サービスのREST APIにGETリクエストを送信する。
        /// 結果をStream形式で取得する
        /// </summary>
        /// <param name="param">リクエストパラメータ</param>
        /// <returns>送信結果</returns>
        protected async Task<Result<Stream, string>> SendGetRequestAndReturnStreamAsync(RequestParam param)
        {
            var response = await SendRequestAsync(HttpMethod.Get, param);
            if (response.IsSuccessStatusCode)
            {
                Stream content = await response.Content.ReadAsStreamAsync();
                return Result<Stream, string>.CreateResult(content);
            }
            else
            {
                string errorMessage = await GetResultFromHttpResponseAsync(response, true);
                // 2回呼び出しているが、何か意味があるのか？
                return Result<Stream, string>.CreateErrorResult(await GetResultFromHttpResponseAsync(response, true));
            }
        }

        /// <summary>
        /// 外部サービスのREST APIにDELETEリクエストを送信する。
        /// </summary>
        /// <param name="param">リクエストパラメータ</param>
        /// <returns>送信結果</returns>
        protected async Task<ResponseResult> SendDeleteRequestAsync(RequestParam param)
        {
            return await SendRequestAndGetStringAsync(HttpMethod.Delete, param);
        }

        /// <summary>
        /// 外部サービスのREST APIにPATCHリクエストを送信する。
        /// <see cref="RequestParam.MediaType"/>は application/json-patch+json で上書きされる。
        /// </summary>
        /// <param name="param">リクエストパラメータ</param>
        /// <returns>送信結果</returns>
        protected async Task<ResponseResult> SendPatchRequestAsync(RequestParam param)
        {
            param.MediaType = RequestParam.MediaTypePatchJson;
            return await SendRequestAndGetStringAsync(new HttpMethod("PATCH"), param);
        }

        /// <summary>
        /// 外部サービスのREST APIにリクエストを送信する。
        /// </summary>
        /// <param name="method">HTTPメソッド種類</param>
        /// <param name="param">リクエストパラメータ</param>
        /// <param name="errLogMode">ログモードで true ならエラー出力</param>
        /// <returns>送信結果</returns>
        private async Task<ResponseResult> SendRequestAndGetStringAsync(HttpMethod method, RequestParam param, bool errLogMode=true)
        {
            using (HttpResponseMessage response = await SendRequestAsync(method, param))
            {
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return ResponseResult.CreateResult(content);
                }
                else
                {
                    string errorMessage = await GetResultFromHttpResponseAsync(response, errLogMode);
                    return ResponseResult.CreateErrorResult(errorMessage);
                }
            }
        }

        /// <summary>
        /// 外部サービスのREST APIにリクエストを送信する。
        /// </summary>
        /// <param name="method">HTTPメソッド種類</param>
        /// <param name="param">リクエストパラメータ</param>
        /// <returns>送信結果</returns>
        protected async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, RequestParam param)
        {
            var httpClientHandler = new HttpClientHandler();
            if (string.IsNullOrEmpty(param.Proxy) == false)
            {
                httpClientHandler.UseProxy = true;
                httpClientHandler.Proxy = new ServiceHttpProxy(param.Proxy);
            }
            // HTTPS証明書チェック無効化
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                //Bodyがなければ、ここでMIMEを追加
                if (string.IsNullOrEmpty(param.Body))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(param.MediaType));
                }
                //認証ヘッダ追加
                client.DefaultRequestHeaders.Authorization = param.AuthorizationHeader;
                //ユーザエージェントが指定されていれば指定
                if (string.IsNullOrEmpty(param.UserAgent) == false)
                {
                    client.DefaultRequestHeaders.UserAgent.Clear();
                    client.DefaultRequestHeaders.Add("User-Agent", param.UserAgent);
                }
                //その他ヘッダ追加
                if(param.Headers != null && param.Headers.Count > 0)
                {
                    foreach(var header in param.Headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                
                string url = param.BaseUrl.EndsWith("/") ? $"{param.BaseUrl}{param.RequestUri.TrimStart('/')}" : $"{param.BaseUrl}/{param.RequestUri.TrimStart('/')}";

                LogDebug($"API呼び出し {method.Method} {url}");

                using (HttpRequestMessage request = new HttpRequestMessage(method, url))
                {
                    if (string.IsNullOrEmpty(param.Body) == false)
                    {
                        request.Content = new StringContent(param.Body, Encoding.UTF8, param.MediaType);
                    }

                    // PostAsync/GetAsyncとかで送る方法もあるけど、ソースを読む限りこれと同じやり方をしているので、ここでもSendAsyncにまとめる
                    // https://github.com/dotnet/corefx/blob/master/src/System.Net.Http/src/System/Net/Http/HttpClient.cs
                    return await client.SendAsync(request);
                }
            }
        }

        /// <summary>
        /// 問い合わせ結果からエラーメッセージを抽出する
        /// </summary>
        /// <returns>成功時は文字列、失敗時は<see cref="ContainerStatus"/>を持つ<see cref="Result{T, U}"/></returns>
        protected async Task<string> GetResultFromHttpResponseAsync(HttpResponseMessage response, bool errLogMode)
        {
            var statusCode = (int)response.StatusCode;
            string content = await response.Content.ReadAsStringAsync();
            string errorMessage = $"{statusCode} : {GetErrorMessageFromResponse(content)}";
            if (errLogMode)
            {
                LogError($"API呼び出しERROR {response.RequestMessage.Method} {response.RequestMessage.RequestUri} {errorMessage}");
            }
            else
            {
                LogInformation($"API呼び出し失敗 {response.RequestMessage.Method} {response.RequestMessage.RequestUri} {errorMessage}");
            }
            return errorMessage;
        }

        /// <summary>
        /// レスポンスからエラーメッセージを抽出するためのメソッド。
        /// サービスによってメッセージのフォーマットが異なるため、オーバーライドして上書き可能にしている。
        /// </summary>
        protected virtual string GetErrorMessageFromResponse(string content)
        {
            return content;
        }

        /// <summary>
        /// 型変換して結果を取得
        /// </summary>
        protected T ConvertResult<T>(ResponseResult result)
        {
            try
            {
                if (result.IsSuccess)
                {
                    return JsonConvert.DeserializeObject<T>(result.Value);
                }
                else
                {
                    throw new ArgumentException(result.Error);
                }
            }
            catch (JsonSerializationException e)
            {
                //Jsonのパースに失敗
                LogError("フォーマットエラー:", e);
                throw;
            }
        }

        /// <summary>
        /// REST APIのパラメータモデル
        /// </summary>
        protected class RequestParam
        {
            public const string MediaTypeJson = "application/json";
            public const string MediaTypeYaml = "application/yaml";
            public const string MediaTypePatchJson = "application/json-patch+json";

            /// <summary>URLのドメイン部分まで（e.g. http://sample.ne.jp ）</summary>
            public string BaseUrl { get; set; }
            /// <summary>URLのドメイン部分以降（e.g. /api/v1/sample ）</summary>
            public string ApiPath { get; set; }
            /// <summary>送信内容</summary>
            public string Body { get; set; }
            /// <summary>ユーザ名</summary>
            public string UserName { get; set; }
            /// <summary>パスワード</summary>
            public string Password { get; set; }
            /// <summary>認証トークン</summary>
            public string Token { get; set; }
            /// <summary>認証トークンタイプ。Autorization headerに記載。Bearer、token等を想定</summary>
            public string TokenType { get; set; }
            /// <summary>
            /// ヘッダー
            /// </summary>
            public Dictionary<string, string> Headers { get; set; }
            /// <summary>プロキシ</summary>
            public string Proxy { get; set; }
            /// <summary>
            /// MIME。
            /// デフォルトは application/json になる。
            /// </summary>
            public string MediaType { get; set; } = "application/json";
            /// <summary>クエリパラメータ</summary>
            public Dictionary<string, string> QueryParams { get; set; }
            /// <summary>ユーザエージェント</summary>
            public string UserAgent { get; set; }

            /// <summary>
            /// リクエストURL
            /// </summary>
            public string RequestUri
            {
                get
                {
                    if(QueryParams == null)
                    {
                        return ApiPath;
                    }
                    string uri = QueryHelpers.AddQueryString(ApiPath, QueryParams);
                    //上記のURLエンコードのバグ、(); が正しくエスケープされない
                    uri = uri.Replace("(", "%28").Replace(")", "%29").Replace(";", "%3B");

                    return uri;
                }
            }

            /// <summary>
            /// 認証ヘッダ
            /// </summary>
            public AuthenticationHeaderValue AuthorizationHeader
            {
                get
                {
                    if(string.IsNullOrEmpty(UserName) == false && string.IsNullOrEmpty(Password) == false)
                    {
                        var byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", UserName, Password));
                        return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }
                    else if(string.IsNullOrEmpty(Token) == false)
                    {
                        var tokenType = TokenType ?? "Bearer";
                        return new AuthenticationHeaderValue(tokenType, Token);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// REST APIの実行結果モデル
        /// </summary>
        protected class ResponseResult : Result<string, string>
        {
            public new static ResponseResult CreateResult(string value)
            {
                ResponseResult response = new ResponseResult()
                {
                    Value = value
                };
                return response;
            }

            /// <summary>
            /// 異常終了結果を作成する。
            /// </summary>
            public new static ResponseResult CreateErrorResult(string error)
            {
                ResponseResult response = new ResponseResult()
                {
                    Error = error
                };
                return response;
            }
        }
    }
}

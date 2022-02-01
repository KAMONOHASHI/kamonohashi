namespace Nssol.Platypus.Infrastructure.Options
{
    /// <summary>
    /// Webセキュリティ設定情報をappsettings.jsonからデシリアライズするためのクラス。
    /// JsonWebTokenで必要なキーの生成を行う機能も含む。
    /// </summary>
    public class WebSecurityOptions
    {
        /// <summary>
        /// APIでトークンのAudience (aud) クレームに指定する文字列。
        /// トークンの受け取り手（のリスト）を表す。
        /// 必要であれば、受け手側で検証を行う。
        /// </summary>
        public string ApiJwtAudience { get; set; }
        /// <summary>
        /// APIでトークンのIssuer (iss) クレームに指定する文字列。
        /// 発行者を表す。
        /// 必要であれば、受け手側で検証を行う。
        /// </summary>
        public string ApiJwtIssuer { get; set; }
        /// <summary>
        /// APIでトークンのExpiration (exp) クレームに指定する数値。
        /// トークンの有効期限（秒）。
        /// </summary>
        public int ApiJwtExpirationSec { get; set; }

        /// <summary>
        /// リクエストパイプラインのデバッグ用ログを出力するか否か
        /// </summary>
        public bool EnableRequestPiplineDebugLog { get; set; }

        /// <summary>
        /// Swaggerを有効化するか否か
        /// </summary>
        public bool EnableSwagger { get; set; }
    }
}

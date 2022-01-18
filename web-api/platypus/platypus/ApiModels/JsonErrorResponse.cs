using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels
{
    /// <summary>
    /// Jsonエラーレスポンスクラス。
    /// TODO: RFC7807に準拠させる。
    /// </summary>
    public class JsonErrorResponse
    {
        /// <summary>
        /// 問題の種類
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 問題の理由
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 問題の詳細
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 問題発生個所
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// エラーリスト。
        /// 複数の問題が発生した場合に利用する拡張情報。
        /// </summary>
        public IEnumerable<object> Errors { get; set; }
    }
}

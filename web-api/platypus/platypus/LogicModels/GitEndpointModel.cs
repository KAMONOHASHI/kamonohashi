namespace Nssol.Platypus.LogicModels
{
    public class GitEndpointModel
    {
        /// <summary>
        /// 認証情報なしURL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 認証情報付きURL
        /// </summary>
        public string FullUrl { get; set; }
        /// <summary>
        /// トークン
        /// </summary>
        public string Token { get; set; }
    }
}

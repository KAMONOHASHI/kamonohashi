namespace Nssol.Platypus.ApiModels.NotebookApiModels
{
    /// <summary>
    /// ノートブックアクセス用の出力モデル
    /// </summary>
    public class EndPointOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nodePort">ノードポート番号</param>
        /// <param name="token">token</param>
        public EndPointOutputModel(string nodePort, string token)
        {
            NodePort = nodePort;
            Token = token;
        }

        /// <summary>
        /// Notebookのノードポート番号
        /// </summary>
        public string NodePort { get; set; }

        /// <summary>
        /// Notebookのアクセストークン
        /// </summary>
        public string Token { get; set; }
    }
}

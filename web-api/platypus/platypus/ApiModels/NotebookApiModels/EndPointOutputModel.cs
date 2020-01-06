namespace Nssol.Platypus.ApiModels.NotebookApiModels
{
    /// <summary>
    /// ノートブックアクセス用URLの出力モデル
    /// </summary>
    public class EndPointOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="url">エンドポイントURL</param>
        public EndPointOutputModel(string url)
        {
            Url = url;
        }

        /// <summary>
        /// NotebookのエンドポイントURL
        /// </summary>
        public string Url { get; set; }
    }
}

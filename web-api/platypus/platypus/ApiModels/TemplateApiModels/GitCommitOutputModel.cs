namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// Git情報の出力モデル
    /// </summary>
    public class GitCommitOutputModel : Components.GitCommitOutputModel
    {
        /// <summary>
        /// Gitトークン
        /// </summary>
        public string Token { get; set; }
    }
}

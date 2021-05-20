namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// Git情報の入力モデル
    /// </summary>
    public class GitCommitNullableInputModel : Components.GitCommitNullableInputModel
    {
        /// <summary>
        /// Gitトークン
        /// </summary>
        public string Token { get; set; }
    }
}

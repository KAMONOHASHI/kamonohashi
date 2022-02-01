namespace Nssol.Platypus.ApiModels.Components
{
    public class GitCommitOutputModel : GitCommitInputModel
    {
        /// <summary>
        /// GitサービスのWebUI URL
        /// </summary>
        public string Url { get; set; }
    }
}

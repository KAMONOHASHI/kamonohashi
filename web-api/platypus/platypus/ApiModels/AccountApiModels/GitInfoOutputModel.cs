using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class GitInfoOutputModel
    {
        /// <summary>
        /// デフォルトのGit
        /// </summary>
        public long? DefaultGitId { get; set; }
        /// <summary>
        /// Git認証情報一覧
        /// </summary>
        public IEnumerable<GitCredentialOutputModel> Gits { get; set; }
    }
}

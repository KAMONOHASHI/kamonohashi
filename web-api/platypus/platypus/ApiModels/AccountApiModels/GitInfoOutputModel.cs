using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

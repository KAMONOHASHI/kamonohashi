using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class GitCredentialOutputModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// トークン
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// サービス名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gitサービス種別
        /// </summary>
        public GitServiceType ServiceType { get; set; }

        public GitCredentialOutputModel(UserTenantGitMap map)
        {
            Id = map.Git.Id;
            Name = map.Git.Name;
            ServiceType = map.Git.ServiceType;
            Token = map.GitToken;
        }

        public GitCredentialOutputModel(Git git)
        {
            Id = git.Id;
            Name = git.Name;
        }
    }
}

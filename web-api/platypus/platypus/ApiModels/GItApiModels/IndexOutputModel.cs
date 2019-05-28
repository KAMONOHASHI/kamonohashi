using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;

namespace Nssol.Platypus.ApiModels.GitApiModels
{
    /// <summary>
    /// テナント情報のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(Git git) : base(git)
        {
            Id = git.Id;
            Name = git.Name;
            ServiceType = git.ServiceType;
            ApiUrl = git.ApiUrl;
            RepositoryUrl = git.RepositoryUrl;
        }
        /// <summary>
        /// Git ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 識別名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gitサーバー種別
        /// </summary>
        public GitServiceType ServiceType { get; set; }

        /// <summary>
        /// Gitコマンドでアクセスする際のURL。
        /// </summary>
        public string RepositoryUrl { get; set; }

        /// <summary>
        /// API URL of the git repository.
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// Gitサービス種別名
        /// </summary>
        public string ServiceTypeName
        {
            get
            {
                return ServiceType.ToString();
            }
        }
    }
}

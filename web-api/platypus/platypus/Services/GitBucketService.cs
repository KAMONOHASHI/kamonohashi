using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels.Git;
using Nssol.Platypus.ServiceModels.Git.GitHubModels;
using Nssol.Platypus.ServiceModels.GitHubModels;
using Nssol.Platypus.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services
{
    /// <summary>
    /// GitBucketのAPI（原則GitHubと互換）を呼び出すサービス
    /// </summary>
    public class GitBucketService : GitHubService
    {
        // for DI
        private readonly WebSecurityOptions options;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GitBucketService(
            IOptions<WebSecurityOptions> options,
            Logic.Interfaces.ICommonDiLogic commonDiLogic) : base(options, commonDiLogic)
        {
            this.options = options.Value;
        }
    }
}

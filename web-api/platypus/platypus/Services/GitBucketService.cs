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

        /// <summary>
        /// リポジトリ一覧を取得する。
        /// 特に範囲は限定せず、<see cref="Git.Token"/>の権限で参照可能なすべてのリポジトリが対象となる。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <returns>リポジトリ一覧</returns>
        public async override Task<Result<IEnumerable<RepositoryModel>, string>> GetAllRepositoriesAsync(UserTenantGitMap gitMap)
        {
            // API呼び出しパラメータ作成
            RequestParam param = CreateRequestParam(gitMap);

            if (string.IsNullOrEmpty(gitMap.GitToken))
            {
                param.ApiPath = $"/repositories";

                // API 呼び出し
                Result<string, string> response = await this.SendGetRequestAsync(param);

                if (response.IsSuccess)
                {
                    var result = JsonConvert.DeserializeObject<IEnumerable<GetRepositoryModel>>(response.Value);
                    return Result<IEnumerable<RepositoryModel>, string>.CreateResult(
                            result.Select(e => new RepositoryModel()
                            {
                                Owner = e.owner.login, //GitHubではAPI実行にそのリポジトリのオーナー名が必要なので、それをKeyに入れる
                                Name = e.name,
                                FullName = e.full_name,
                            }).OrderBy(e => e.FullName));
                }
                else
                {
                    return Result<IEnumerable<RepositoryModel>, string>.CreateErrorResult(response.Error);
                }
            }
            else
            {
                param.ApiPath = $"/user/repos";

                // API 呼び出し
                Result<string, string> response = await this.SendGetRequestAsync(param);

                if (response.IsSuccess)
                {
                    var result = JsonConvert.DeserializeObject<IEnumerable<GetRepositoryModel>>(response.Value);
                    return Result<IEnumerable<RepositoryModel>, string>.CreateResult(
                            result.Select(e => new RepositoryModel()
                            {
                                Owner = e.owner.login, //GitHubではAPI実行にそのリポジトリのオーナー名が必要なので、それをKeyに入れる
                            Name = e.name,
                                FullName = e.full_name,
                            }).OrderBy(e => e.FullName));
                }
                else
                {
                    return Result<IEnumerable<RepositoryModel>, string>.CreateErrorResult(response.Error);
                }
            }
        }
    }
}

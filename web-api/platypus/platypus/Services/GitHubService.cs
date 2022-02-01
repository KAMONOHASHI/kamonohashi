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
    /// GitHubのAPIを呼び出すサービス
    /// </summary>
    public class GitHubService : PlatypusServiceBase, IGitService
    {
        // for DI
        private readonly WebSecurityOptions options;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GitHubService(
            IOptions<WebSecurityOptions> options,
            Logic.Interfaces.ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.options = options.Value;
        }

        /// <summary>
        /// リポジトリ一覧を取得する。
        /// 特に範囲は限定せず、<see cref="Git.Token"/>の権限で参照可能なすべてのリポジトリが対象となる。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <returns>リポジトリ一覧</returns>
        public async virtual Task<Result<IEnumerable<RepositoryModel>, string>> GetAllRepositoriesAsync(UserTenantGitMap gitMap)
        {
            if (string.IsNullOrEmpty(gitMap.GitToken))
            {
                //トークンが設定されていない場合、GitHubのリポジトリ一覧は取得できない
                //なので空の結果を返す

                return Result<IEnumerable<RepositoryModel>, string>.CreateResult(new List<RepositoryModel>());
            }

            // API呼び出しパラメータ作成
            RequestParam param = CreateRequestParam(gitMap);
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

        /// <summary>
        /// ブランチ一覧を取得する。
        /// 対象リポジトリが存在しない場合はnullが返る。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <returns>ブランチ一覧</returns>
        public async Task<Result<IEnumerable<BranchModel>, string>> GetAllBranchesAsync(UserTenantGitMap gitMap, string repositoryName, string owner)
        {
            // API呼び出しパラメータ作成
            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = $"/repos/{owner}/{repositoryName}/branches";

            // API 呼び出し
            Result<string, string> response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<IEnumerable<GetBranchModel>>(response.Value);
                return Result<IEnumerable<BranchModel>, string>.CreateResult(
                        result.Select(e => new BranchModel()
                        {
                            BranchName = e.name,
                            CommitId = e.commit?.sha
                        }));
            }
            else
            {
                return Result<IEnumerable<BranchModel>, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// コミット一覧を取得する。
        /// 対象リポジトリが存在しない場合はnullが返る。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="branchName">ブランチ名</param>
        /// <param name="page">ページ番号</param>
        /// <returns>コミット一覧</returns>
        public async Task<Result<IEnumerable<CommitModel>, string>> GetAllCommitsAsync(UserTenantGitMap gitMap, string repositoryName, string owner, string branchName,string page)
        {
            // API呼び出しパラメータ作成
            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = $"/repos/{owner}/{repositoryName}/commits";
            if (string.IsNullOrEmpty(branchName) == false)
            {
                param.QueryParams = new Dictionary<string, string>
                {
                    { "sha", branchName},
                    { "per_page", "100"},
                    { "page", page}
                };
            }

            // API 呼び出し
            Result<string, string> response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<IEnumerable<GetCommitModel>>(response.Value);
                return Result<IEnumerable<CommitModel>, string>.CreateResult(
                        result.Select(e => new CommitModel()
                        {
                            CommitId = e.sha,
                            Comment = e.commit?.message,
                            CommitAt = e.commit?.author?.date.ToLocalFormatedString(),
                            CommitterName = e.commit?.author?.name
                        }));
            }
            else
            {
                return Result<IEnumerable<CommitModel>, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// 指定したブランチのHEADコミットを取得する
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="branchName">ブランチ名</param>
        /// <returns>コミット</returns>
        public async Task<Result<CommitModel, string>> GetCommitAsync(UserTenantGitMap gitMap, string repositoryName, string owner, string branchName)
        {
            // API呼び出しパラメータ作成
            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = $"/repos/{owner}/{repositoryName}/commits/{branchName}";

            // API 呼び出し
            Result<string, string> response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<GetCommitModel>(response.Value);
                var commit = new CommitModel()
                {
                    CommitId = result.sha,
                    Comment = result.commit?.message,
                    CommitAt = result.commit?.author?.date.ToLocalFormatedString(),
                    CommitterName = result.commit?.author?.name
                };
                return Result<CommitModel, string>.CreateResult(commit);
            }
            else
            {
                return Result<CommitModel, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// 指定したコミットIDのコミット詳細を取得する。
        /// 対象リポジトリが存在しない場合はnullが返る。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="commitId">コミットID</param>
        /// <returns>コミット詳細</returns>
        public async Task<Result<CommitModel, string>> GetCommitByIdAsync(UserTenantGitMap gitMap, string repositoryName, string owner, string commitId)
        {
            // API呼び出しパラメータ作成
            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = $"/repos/{owner}/{repositoryName}/commits/{commitId}";

            // API 呼び出し
            Result<string, string> response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<GetCommitModel>(response.Value);
                var commit = new CommitModel()
                {
                    CommitId = result.sha,
                    Comment = result.commit?.message,
                    CommitAt = result.commit?.author?.date.ToLocalFormatedString(),
                    CommitterName = result.commit?.author?.name
                };
                return Result<CommitModel, string>.CreateResult(commit);
            }
            else
            {
                return Result<CommitModel, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// 指定したtokenのgitサービス側のユーザー名を取得する
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <returns>コミット詳細</returns>
        public async Task<Result<string, string>> GetUserNameByTokenAsync(UserTenantGitMap gitMap)
        {
            // API呼び出しパラメータ作成
            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = $"/user";

            // API 呼び出し
            Result<string, string> response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<GetUserModel>(response.Value);

                return Result<string, string>.CreateResult(result.login);
            }
            else
            {
                return Result<string, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// 共通で使うパラメータを生成
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <returns>リクエストパラメータ</returns>
        protected virtual RequestParam CreateRequestParam(UserTenantGitMap gitMap)
        {
            RequestParam param = new RequestParam()
            {
                BaseUrl = gitMap.Git.ApiUrl,
                //Proxy = options.Proxy, // API Server からのアクセスは特にコード内では制御せず、OS設定に任せる
                Token = gitMap.GitToken,
                UserAgent = "C#App",
                TokenType = "token",
            };
            return param;
        }
    }
}

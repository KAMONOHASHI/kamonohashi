using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels.Git;
using Nssol.Platypus.ServiceModels.Git.GitLabModels;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services
{
    public class GitLabService : PlatypusServiceBase, IGitService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GitLabService(
            Logic.Interfaces.ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
        }

        /// <summary>
        /// リポジトリ一覧を取得する。
        /// 特に範囲は限定せず、<see cref="Git.Token"/>の権限で参照可能なすべてのリポジトリが対象となる。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <returns>リポジトリ一覧</returns>
        public async virtual Task<Result<IEnumerable<RepositoryModel>, string>> GetAllRepositoriesAsync(UserTenantGitMap gitMap)
        {
            var response = await SendGetFullPageRequestsAsync<GetRepositoryModel>("/api/v4/projects", gitMap, new Dictionary<string, string>());

            if (response.IsSuccess)
            {
                //ロジック層に返すための汎用モデルに変換
                var outModel = response.Value.Select(e => new RepositoryModel()
                {
                    Name = e.path, // e.nameは表示そのまま（スペースが入りうる）なので、pathからとる
                    FullName = e.path_with_namespace,
                    Owner = e.RepositoryOwner,
                }).OrderBy(e => e.FullName);
                return Result<IEnumerable<RepositoryModel>, string>.CreateResult(outModel);
            }
            else
            {
                LogError(response.Error);
                return Result<IEnumerable<RepositoryModel>, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// オーナー名＆リポジトリ名から、プロジェクトIDを取得する。
        /// </summary>
        /// <remarks>
        /// GitLab APIではGitで一般的な{オーナー名}/{リポジトリ名}ではなく、プロジェクトIDという独自識別子でリポジトリを特定する。
        /// なので、この変換を行うためのAPI呼び出しが必要。
        /// ちなみに、逆にWebUIを参照する際はオーナー名が必要という仕様。
        /// </remarks>
        /// <param name="gitMap">Git情報</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <returns>プロジェクトID</returns>
        protected async virtual Task<Result<string, string>> GetProjectIdAsync(UserTenantGitMap gitMap, string repositoryName, string owner)
        {
            //検索上限が100件だが、リポジトリ名でフィルタ（部分一致）をかけて検索するので、そこまでの件数にはならない想定
            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = $"/api/v4/projects";
            param.QueryParams = new Dictionary<string, string>()
            {
                { "search", repositoryName },
                { "per_page", "100" }
            };
            var response = await SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                //一度GitLabの専用出力モデルに吐き出す
                var result = JsonConvert.DeserializeObject<IEnumerable<GetRepositoryModel>>(response.Value);
                //ロジック層に返すための版用モデルに変換
                var outModel = result.FirstOrDefault(r => r.path_with_namespace == $"{owner}/{repositoryName}");
                if (outModel == null)
                {
                    string message = $"Repository {owner}/{repositoryName} is not found.";
                    LogWarning(message);
                    return Result<string, string>.CreateErrorResult(message);
                }
                return Result<string, string>.CreateResult(outModel.id.ToString());
            }
            else
            {
                LogError(response.Error);
                return Result<string, string>.CreateErrorResult(response.Error);
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
            var projectId = await GetProjectIdAsync(gitMap, repositoryName, owner);
            if (projectId.IsSuccess == false)
            {
                //プロジェクトIDがない＝ブランチが一つもない＝という事なので、nullを返却
                return Result<IEnumerable<BranchModel>, string>.CreateResult(null);
            }
            RequestParam param = CreateRequestParam(gitMap);
            string apiPath = $"/api/v4/projects/{projectId.Value}/repository/branches";

            var response = await SendGetFullPageRequestsAsync<GetBranchModel>(apiPath, gitMap, new Dictionary<string, string>());

            if (response.IsSuccess)
            {
                return Result<IEnumerable<BranchModel>, string>.CreateResult(
                        response.Value.Select(e => new BranchModel()
                        {
                            BranchName = e.name,
                            CommitId = e.commit?.id
                        }));
            }
            else
            {
                LogError(response.Error);
                return Result<IEnumerable<BranchModel>, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// コミット一覧を取得する。
        /// 対象リポジトリが存在しない場合はnullが返る。
        /// すべて取ってくると件数が多すぎると想定されるため、最大100件固定。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="branchName">ブランチ名</param>
        /// <param name="page">ページ番号</param>
        /// <returns>コミット一覧</returns>
        public async Task<Result<IEnumerable<CommitModel>, string>> GetAllCommitsAsync(UserTenantGitMap gitMap, string repositoryName, string owner, string branchName, string page)
        {
            var projectId = await GetProjectIdAsync(gitMap, repositoryName, owner);
            if (projectId.IsSuccess == false)
            {
                //プロジェクトIDがない＝ブランチが一つもない＝という事なので、nullを返却
                return Result<IEnumerable<CommitModel>, string>.CreateResult(null);
            }

            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = $"/api/v4/projects/{projectId.Value}/repository/commits";
            if (string.IsNullOrEmpty(branchName) == false)
            {
                param.QueryParams = new Dictionary<string, string>
                {
                    { "ref_name", branchName },
                    { "per_page", "100" }, //指定しない場合は20件になるので、100件まで拡張
                    {"page",page }
                };
            }

            var response = await SendGetRequestAsync(param);
            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<IEnumerable<GetCommitModel>>(response.Value);
                return Result<IEnumerable<CommitModel>, string>.CreateResult(
                        result.Select(e => new CommitModel()
                        {
                            CommitId = e.id,
                            Comment = e.message,
                            CommitAt = e.committed_date.ToLocalFormatedString(),
                            CommitterName = e.committer_name
                        }));
            }
            else
            {
                LogError(response.Error);
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
            var projectId = await GetProjectIdAsync(gitMap, repositoryName, owner);
            if (projectId.IsSuccess == false)
            {
                return Result<CommitModel, string>.CreateErrorResult(projectId.Error);
            }

            //ブランチ名には"/"が入る可能性があるので、URLエンコードする
            string branch = Uri.EscapeDataString(branchName);

            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = $"/api/v4/projects/{projectId.Value}/repository/commits/{branch}";

            // API 呼び出し
            Result<string, string> response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<GetCommitModel>(response.Value);
                var commit = new CommitModel()
                {
                    CommitId = result.id,
                    Comment = result.message,
                    CommitAt = result.committed_date.ToLocalFormatedString(),
                    CommitterName = result.committer_name
                };
                return Result<CommitModel, string>.CreateResult(commit);
            }
            else
            {
                LogError(response.Error);
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
            var projectId = await GetProjectIdAsync(gitMap, repositoryName, owner);
            if (projectId.IsSuccess == false)
            {
                return Result<CommitModel, string>.CreateErrorResult(projectId.Error);
            }

            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = $"/api/v4/projects/{projectId.Value}/repository/commits/{commitId}";

            // API 呼び出し
            Result<string, string> response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<GetCommitModel>(response.Value);
                var commit = new CommitModel()
                {
                    CommitId = result.id,
                    Comment = result.message,
                    CommitAt = result.committed_date.ToLocalFormatedString(),
                    CommitterName = result.committer_name
                };
                return Result<CommitModel, string>.CreateResult(commit);
            }
            else
            {
                LogError(response.Error);
                return Result<CommitModel, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// GitLabは一度に100件しか結果を返してくれない。
        /// なので必要な数だけページングしながら結果を書き集めてくる。
        /// 引数の<paramref name="queryParams"/>は中で都度書き換えられるので注意。
        /// </summary>
        /// <param name="apiPath">APIパス</param>
        /// <param name="gitMap">Git情報</param>
        /// <param name="queryParams">クエリパラメータ</param>
        /// <returns>取得一覧</returns>
        private async Task<Result<List<T>, string>> SendGetFullPageRequestsAsync<T>(string apiPath, UserTenantGitMap gitMap, Dictionary<string, string> queryParams)
        {
            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = apiPath;
            param.QueryParams = queryParams;
            queryParams.Add("per_page", "10");
            queryParams.Add("page", "0"); //ダミーの数字を入れておく

            //ページ番号は1から始まるので、その手前で初期化
            int page = 0;

            List<T> outModel = new List<T>();
            do
            {
                page++;
                queryParams["page"] = page.ToString();

                var response = await SendRequestAsync(HttpMethod.Get, param);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    //一度GitLabの専用出力モデルに吐き出す
                    var result = JsonConvert.DeserializeObject<List<T>>(content);
                    //結果をマージ
                    outModel.AddRange(result);

                    int totalPages = int.Parse(response.Headers.First(h => h.Key == "X-Total-Pages").Value.First());
                    if (totalPages <= page)
                    {
                        return Result<List<T>, string>.CreateResult(outModel);
                    }
                }
                else
                {
                    //一度でも失敗したらエラー
                    string errorMessage = await GetResultFromHttpResponseAsync(response, true);
                    LogError(errorMessage);
                    return Result<List<T>, string>.CreateErrorResult(errorMessage);
                }
            }
            while (true);
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
            param.ApiPath = $"/api/v4/user";

            // API 呼び出し
            Result<string, string> response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<GetUserModel>(response.Value);

                return Result<string, string>.CreateResult(result.username);
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
        protected RequestParam CreateRequestParam(UserTenantGitMap gitMap)
        {
            return new RequestParam()
            {
                BaseUrl = gitMap.Git.ApiUrl,
                UserAgent = "C#App",
                Headers = new Dictionary<string, string>()
                {
                    {"Private-Token", gitMap.GitToken } //GitLabはトークンの形式がBearerではないので、ヘッダに独自で追加
                }
            };
        }
    }
}

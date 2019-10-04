using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels.Git;
using Nssol.Platypus.ServiceModels.Git.GitLabModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services
{
    /// <summary>
    /// GitLab.com用のAPIを呼び出すサービス
    /// </summary>
    public class GitLabComService : GitLabService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GitLabComService(
            Logic.Interfaces.ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
        }
        
        /// <summary>
        /// リポジトリ一覧を取得する。
        /// </summary>
        /// <returns>リポジトリ一覧</returns>
        public async override Task<Result<IEnumerable<RepositoryModel>, string>> GetAllRepositoriesAsync(UserTenantGitMap gitMap)
        {
            await Task.CompletedTask;

            // GitLab.comはトークンに関係なく、Publicリポジトリ(10,000件を超える)も取得するので一覧は取得しない
            // ユーザに手入力で設定させるため空のリストを返す
            return Result<IEnumerable<RepositoryModel>, string>.CreateResult(new List<RepositoryModel>());
        }

        /// <summary>
        /// オーナー名＆リポジトリ名から、プロジェクトIDを取得する。
        /// </summary>
        /// <remarks>
        /// GitLab APIではGitで一般的な{オーナー名}/{リポジトリ名}ではなく、プロジェクトIDという独自識別子でリポジトリを特定する。
        /// なので、この変換を行うためのAPI呼び出しが必要。
        /// ちなみに、逆にWebUIを参照する際はオーナー名が必要という仕様。
        /// </remarks>
        protected async override Task<Result<string, string>> GetProjectIdAsync(UserTenantGitMap gitMap, string repositoryName, string owner)
        {
            // オーナー名＆リポジトリ名を指定して取得する
            RequestParam param = CreateRequestParam(gitMap);
            param.ApiPath = $"/api/v4/projects/{owner}%2F{repositoryName}";

            var response = await SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                //一度GitLabの専用出力モデルに吐き出す
                var result = JsonConvert.DeserializeObject<IEnumerable<GetRepositoryModel>>("[" + response.Value + "]");
                //ロジック層に返すための版用モデルに変換
                var outModel = result.First();
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
    }
}

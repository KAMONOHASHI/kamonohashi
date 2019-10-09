using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.LogicModels;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels.Git;
using Nssol.Platypus.Services;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// Git管理を行うロジック
    /// </summary>
    public class GitLogic : PlatypusLogicBase, IGitLogic
    {
        private readonly IGitRepository gitRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GitLogic(ICommonDiLogic commonDiLogic,
            IGitRepository gitRepository) : base(commonDiLogic)
        {
            this.gitRepository = gitRepository;
        }

        /// <summary>
        /// リポジトリ一覧を取得する。
        /// </summary>
        /// <returns>リポジトリ一覧</returns>
        public async Task<Result<IEnumerable<RepositoryModel>, string>> GetAllRepositoriesAsync(long gitId)
        {
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            IGitService gitService = GetGitService(map?.Git);
            if (gitService != null)
            {
                return await gitService.GetAllRepositoriesAsync(map);
            }
            else
            {
                return Result<IEnumerable<RepositoryModel>, string>.CreateErrorResult("The selected tenant isn't related to proper git service.");
            }
        }

        /// <summary>
        /// ブランチ一覧を取得する。
        /// 対象リポジトリが存在しない場合はnullが返る。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <returns>ブランチ一覧</returns>
        public async Task<Result<IEnumerable<BranchModel>, string>> GetAllBranchesAsync(long gitId, string repositoryName, string owner)
        {
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            IGitService gitService = GetGitService(map?.Git);
            if (gitService != null)
            {
                return await gitService.GetAllBranchesAsync(map, repositoryName, owner);
            }
            else
            {
                return Result<IEnumerable<BranchModel>, string>.CreateErrorResult("The selected tenant isn't related to proper git service.");
            }
        }

        /// <summary>
        /// コミット一覧を取得する
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="branchName">ブランチ名</param>
        /// <returns>コミット一覧</returns>
        public async Task<Result<IEnumerable<CommitModel>, string>> GetAllCommitsAsync(long gitId, string repositoryName, string owner, string branchName)
        {
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            IGitService gitService = GetGitService(map?.Git);
            if (gitService != null)
            {
                return await gitService.GetAllCommitsAsync(map, repositoryName, owner, branchName);
            }
            else
            {
                return Result<IEnumerable<CommitModel>, string>.CreateErrorResult("The selected tenant isn't related to proper git service.");
            }
        }

        /// <summary>
        /// 指定したブランチ名のHEADリビジョンに一致するCommitIdを取得する。
        /// 失敗した場合はnullを返す。
        /// </summary>
        public async Task<string> GetCommitIdAsync(long gitId, string repositoryName, string owner, string branchName)
        {
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            IGitService gitService = GetGitService(map?.Git);
            if (gitService != null)
            {
                var commit = await gitService.GetCommitAsync(map, repositoryName, owner, branchName);
                if (commit.IsSuccess)
                {
                    return commit.Value.CommitId;
                }
            }
            return null;
        }

        /// <summary>
        /// git pullするためのURLを取得する。
        /// </summary>
        public GitEndpointModel GetPullUrl(long gitId, string repositoryName, string owner)
        {
            if (string.IsNullOrEmpty(repositoryName) || string.IsNullOrEmpty(owner))
            {
                return null;
            }
            UserTenantGitMap map = GetCurrentGitMap(gitId);

            string url = $"{map.Git.RepositoryUrl}/{owner}/{repositoryName}.git";
            var result = new GitEndpointModel()
            {
                Url = url,
                Token = map.GitToken
            };
            if (string.IsNullOrEmpty(map.GitToken))
            {
                result.FullUrl = url;
            }
            else
            {
                // http の場合、以下のようなフォーマットで git clone できる
                //  http://kqi:${token}@${host}/${owner}/${repositoryName}.git
                // ユーザ名がkqiになっているのは、空文字以外の任意文字列を入れないと認証失敗になる問題が発見されたため。
                UriBuilder builder = new UriBuilder(url);
                builder.UserName = "kqi";
                builder.Password = map.GitToken;
                result.FullUrl = builder.Uri.ToString();
            }

            return result;
        }

        /// <summary>
        /// コミット内容の差分表示を参照できるWebUI URLを取得する。
        /// ページがない場合nullが返る。
        /// </summary>
        public string GetCommitUiUrl(long gitId, string repositoryName, string owner, string commitId)
        {
            if (string.IsNullOrEmpty(repositoryName) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(commitId))
            {
                return null;
            }
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            IGitService gitService = GetGitService(map?.Git);
            if (gitService != null)
            {
                //今のところ全GitサービスでURLが共通なので、サービス層ではなくロジック層で作って返す
                return $"{map.Git.RepositoryUrl}/{owner}/{repositoryName}/commit/{commitId}";
            }
            return null;
        }

        /// <summary>
        /// コミット内容のTree表示を参照できるWebUI URLを取得する。
        /// ページがない場合nullが返る。
        /// </summary>
        public string GetTreeUiUrl(long gitId, string repositoryName, string owner, string commitId)
        {
            if(string.IsNullOrEmpty(repositoryName) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(commitId))
            {
                return null;
            }
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            IGitService gitService = GetGitService(map?.Git);
            if (gitService != null)
            {
                //今のところ全GitサービスでURLが共通なので、サービス層ではなくロジック層で作って返す
                return $"{map.Git.RepositoryUrl}/{owner}/{repositoryName}/tree/{commitId}";
            }
            return null;
        }

        /// <summary>
        /// ログインユーザ＆テナントに紐付くGitMap情報を取得する
        /// </summary>
        /// <returns></returns>
        private UserTenantGitMap GetCurrentGitMap(long gitId)
        {
            var tenant = CurrentUserInfo?.SelectedTenant;
            if (tenant == null)
            {
                return null;
            }
            return gitRepository.GetUserTenantGitMap(CurrentUserInfo.Id, tenant.Id, gitId);
        }

        /// <summary>
        /// 現在のテナントに紐付くGitサービスインスタンスを取得する
        /// </summary>
        private IGitService GetGitService(Git git)
        {
            if (git != null)
            {
                if (git.ServiceType == GitServiceType.GitLab)
                {
                    return CommonDiLogic.DynamicDi<GitLabService>();
                }
                else if (git.ServiceType == GitServiceType.GitHub)
                {
                    return CommonDiLogic.DynamicDi<GitHubService>();
                }
                else if (git.ServiceType == GitServiceType.GitLabCom)
                {
                    return CommonDiLogic.DynamicDi<GitLabComService>();
                }
            }
            return null;
        }
    }
}

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
        /// <param name="gitId">Git ID</param>
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
        /// コミット一覧を取得する。
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
        /// 指定したブランチ名のHEADリビジョンに一致するコミットIDを取得する。
        /// 失敗した場合はnullを返す。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="branchName">ブランチ名</param>
        /// <returns>コミットID</returns>
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
        /// 指定したコミットIDのコミット詳細を取得する。
        /// 失敗した場合はnullを返す。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="commitId">コミットID</param>
        /// <returns>コミット詳細</returns>
        public async Task<Result<CommitModel, string>> GetCommitAsync(long gitId, string repositoryName, string owner, string commitId)
        {
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            IGitService gitService = GetGitService(map?.Git);
            if (gitService != null)
            {
                return await gitService.GetCommitByIdAsync(map, repositoryName, owner, commitId);

            }
            return Result<CommitModel, string>.CreateErrorResult("The selected tenant isn't related to proper git service."); ;
        }

        /// <summary>
        /// git pullするためのURLを取得する。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <returns>git pullするためのURL</returns>
        public async Task<Result<GitEndpointModel, string>> GetPullUrlAsync(long gitId, string repositoryName, string owner)
        {
            if (string.IsNullOrEmpty(repositoryName) || string.IsNullOrEmpty(owner))
            {
                return Result<GitEndpointModel, string>.CreateResult(null);
            }
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            string gitUrl = map.Git.RepositoryUrl.TrimEnd('/');
            string url = $"{gitUrl}/{owner}/{repositoryName}.git";
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
                //  http://${user}:${token}@${host}/${owner}/${repositoryName}.git
                IGitService gitService = GetGitService(map?.Git);
                var userNameResult = await gitService.GetUserNameByTokenAsync(map);

                if (!userNameResult.IsSuccess)
                {
                    return Result<GitEndpointModel, string>.CreateErrorResult(userNameResult.Error);
                }
                if (userNameResult.Value == null)
                {
                    return Result<GitEndpointModel, string>.CreateErrorResult("invalid git service response");
                }

                UriBuilder builder = new UriBuilder(url);
                builder.UserName = userNameResult.Value;
                builder.Password = map.GitToken;
                result.FullUrl = builder.Uri.ToString();
            }

            return Result<GitEndpointModel, string>.CreateResult(result);
        }

        /// <summary>
        /// git pullするためのURLを取得する。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="extraToken">Gitの認証トークン</param>
        /// <returns>git pullするためのURL</returns>
        public async Task<Result<GitEndpointModel, string>> GetPullUrlAsync(long gitId, string repositoryName, string owner, string extraToken)
        {
            if (string.IsNullOrEmpty(repositoryName) || string.IsNullOrEmpty(owner))
            {
                return Result<GitEndpointModel, string>.CreateResult(null);
            }
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            string gitUrl = map.Git.RepositoryUrl.TrimEnd('/');
            string url = $"{gitUrl}/{owner}/{repositoryName}.git";
            var orgToken = map.GitToken;
            try
            {
                if (extraToken != null)
                {
                    map.GitToken = extraToken;
                }
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
                    //  http://${user}:${token}@${host}/${owner}/${repositoryName}.git
                    IGitService gitService = GetGitService(map?.Git);
                    var userNameResult = await gitService.GetUserNameByTokenAsync(map);

                    if (!userNameResult.IsSuccess)
                    {
                        return Result<GitEndpointModel, string>.CreateErrorResult(userNameResult.Error);
                    }
                    if (userNameResult.Value == null)
                    {
                        return Result<GitEndpointModel, string>.CreateErrorResult("invalid git service response");
                    }

                    UriBuilder builder = new UriBuilder(url);
                    builder.UserName = userNameResult.Value;
                    builder.Password = map.GitToken;
                    result.FullUrl = builder.Uri.ToString();
                }
                return Result<GitEndpointModel, string>.CreateResult(result);
            }
            finally
            {
                map.GitToken = orgToken;
            }
        }

        /// <summary>
        /// コミット内容の差分表示を参照できるWebUI URLを取得する。
        /// ページがない場合nullが返る。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="commitId">コミットID</param>
        /// <returns>WebUI URL</returns>
        public string GetCommitUiUrl(long gitId, string repositoryName, string owner, string commitId)
        {
            if (string.IsNullOrEmpty(repositoryName) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(commitId))
            {
                return null;
            }
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            IGitService gitService = GetGitService(map?.Git);
            string gitUrl = map.Git.RepositoryUrl.TrimEnd('/');
            if (gitService != null)
            {
                //今のところ全GitサービスでURLが共通なので、サービス層ではなくロジック層で作って返す
                return $"{gitUrl}/{owner}/{repositoryName}/commit/{commitId}";
            }
            return null;
        }

        /// <summary>
        /// コミット内容のTree表示を参照できるWebUI URLを取得する。
        /// ページがない場合nullが返る。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="commitId">コミットID</param>
        /// <returns>WebUI URL</returns>
        public string GetTreeUiUrl(long gitId, string repositoryName, string owner, string commitId)
        {
            if (string.IsNullOrEmpty(repositoryName) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(commitId))
            {
                return null;
            }
            UserTenantGitMap map = GetCurrentGitMap(gitId);
            IGitService gitService = GetGitService(map?.Git);
            string gitUrl = map.Git.RepositoryUrl.TrimEnd('/');

            if (gitService != null)
            {
                //今のところ全GitサービスでURLが共通なので、サービス層ではなくロジック層で作って返す
                return $"{gitUrl}/{owner}/{repositoryName}/tree/{commitId}";
            }
            return null;
        }

        /// <summary>
        /// ログインユーザ＆テナントに紐付くGitMap情報を取得する
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <returns>Gitマップ情報</returns>
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
        /// <param name="git">Git情報</param>
        /// <returns>Gitサービスインスタンス</returns>
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
                else if (git.ServiceType == GitServiceType.GitBucket)
                {
                    return CommonDiLogic.DynamicDi<GitBucketService>();
                }
            }
            return null;
        }
    }
}

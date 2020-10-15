using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.LogicModels;
using Nssol.Platypus.ServiceModels.Git;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// Git管理を行うロジックインターフェース
    /// </summary>
    public interface IGitLogic
    {
        /// <summary>
        /// レポジトリ一覧を取得する。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <returns>リポジトリ一覧</returns>
        Task<Result<IEnumerable<RepositoryModel>, string>> GetAllRepositoriesAsync(long gitId);

        /// <summary>
        /// ブランチ一覧を取得する。
        /// 対象リポジトリが存在しない場合はnullが返る。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <returns>ブランチ一覧</returns>
        Task<Result<IEnumerable<BranchModel>, string>> GetAllBranchesAsync(long gitId, string repositoryName, string owner);

        /// <summary>
        /// コミット一覧を取得する。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="branchName">ブランチ名</param>
        /// <returns>コミット一覧</returns>
        Task<Result<IEnumerable<CommitModel>, string>> GetAllCommitsAsync(long gitId, string repositoryName, string owner, string branchName);

        /// <summary>
        /// 指定したブランチ名のHEADリビジョンに一致するコミットIDを取得する。
        /// 失敗した場合はnullを返す。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="branchName">ブランチ名</param>
        /// <returns>コミットID</returns>
        Task<string> GetCommitIdAsync(long gitId, string repositoryName, string owner, string branchName);

        /// <summary>
        /// 指定したコミットIDのコミット詳細を取得する。
        /// 失敗した場合はnullを返す。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="commitId">コミットID</param>
        /// <returns>コミット詳細</returns>
        Task<Result<CommitModel, string>> GetCommitAsync(long gitId, string repositoryName, string owner, string commitId);

        /// <summary>
        /// git pullするためのURLを取得する。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <returns>git pullするためのURL</returns>
        Task<Result<GitEndpointModel, string>> GetPullUrlAsync(long gitId, string repositoryName, string owner);

        /// <summary>
        /// コミット内容の差分表示を参照できるWebUI URLを取得する。
        /// ページがない場合nullが返る。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="commitId">コミットID</param>
        /// <returns>WebUI URL</returns>
        string GetCommitUiUrl(long gitId, string repositoryName, string owner, string commitId);

        /// <summary>
        /// コミット内容のTree表示を参照できるWebUI URLを取得する。
        /// ページがない場合nullが返る。
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="commitId">コミットID</param>
        /// <returns>WebUI URL</returns>
        string GetTreeUiUrl(long gitId, string repositoryName, string owner, string commitId);
    }
}

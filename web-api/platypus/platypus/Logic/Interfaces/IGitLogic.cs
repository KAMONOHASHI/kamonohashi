using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.LogicModels;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels.Git;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// Git管理を行うロジック
    /// </summary>
    public interface IGitLogic
    {
        /// <summary>
        /// レポジトリ一覧を取得する
        /// </summary>
        /// <returns>Git一覧</returns>
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
        /// コミット一覧を取得する
        /// </summary>
        /// <param name="gitId">Git ID</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="branchName">ブランチ名</param>
        /// <returns>コミット一覧</returns>
        Task<Result<IEnumerable<CommitModel>, string>> GetAllCommitsAsync(long gitId, string repositoryName, string owner, string branchName);

        /// <summary>
        /// 指定したブランチ名のHEADリビジョンに一致するCommitIdを取得する。
        /// 失敗した場合はnullを返す。
        /// </summary>
        Task<string> GetCommitIdAsync(long gitId, string repositoryName, string owner, string branchName);

        /// <summary>
        /// git pullするためのURLを取得する。
        /// </summary>
        GitEndpointModel GetPullUrl(long gitId, string repositoryName, string owner);

        /// <summary>
        /// コミット内容の差分表示を参照できるWebUI URLを取得する。
        /// ページがない場合nullが返る。
        /// </summary>
        string GetCommitUiUrl(long gitId, string repositoryName, string owner, string commitId);

        /// <summary>
        /// コミット内容のTree表示を参照できるWebUI URLを取得する。
        /// ページがない場合nullが返る。
        /// </summary>
        string GetTreeUiUrl(long gitId, string repositoryName, string owner, string commitId);
    }
}

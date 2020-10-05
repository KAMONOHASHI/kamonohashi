using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels.Git;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services.Interfaces
{
    /// <summary>
    /// Gitサービスインターフェース
    /// </summary>
    public interface IGitService
    {
        /// <summary>
        /// リポジトリ一覧を取得する。
        /// 特に範囲は限定せず、<see cref="Git.Token"/>の権限で参照可能なすべてのリポジトリが対象となる。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <returns>リポジトリ一覧</returns>
        Task<Result<IEnumerable<RepositoryModel>, string>> GetAllRepositoriesAsync(UserTenantGitMap gitMap);

        /// <summary>
        /// ブランチ一覧を取得する。
        /// 対象リポジトリが存在しない場合はnullが返る。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">リポジトリの識別子。サービスごとにフォーマットが異なる。</param>
        /// <returns>ブランチ一覧</returns>
        Task<Result<IEnumerable<BranchModel>, string>> GetAllBranchesAsync(UserTenantGitMap gitMap, string repositoryName, string owner);

        /// <summary>
        /// コミット一覧を取得する。
        /// 対象リポジトリが存在しない場合はnullが返る。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">リポジトリの識別子。サービスごとにフォーマットが異なる。</param>
        /// <param name="branchName">ブランチ名</param>
        /// <returns>コミット一覧</returns>
        Task<Result<IEnumerable<CommitModel>, string>> GetAllCommitsAsync(UserTenantGitMap gitMap, string repositoryName, string owner, string branchName);

        /// <summary>
        /// 指定したブランチのHEADコミットを取得する
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">リポジトリの識別情報</param>
        /// <param name="branchName">ブランチ名</param>
        /// <returns>コミット</returns>
        Task<Result<CommitModel, string>> GetCommitAsync(UserTenantGitMap gitMap, string repositoryName, string owner, string branchName);

        /// <summary>
        /// 指定したコミットIDのコミット詳細を取得する。
        /// 対象リポジトリが存在しない場合はnullが返る。
        /// </summary>
        /// <param name="gitMap">Git情報</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="commitId">コミットID</param>
        /// <returns>コミット詳細</returns>
        Task<Result<CommitModel, string>> GetCommitByIdAsync(UserTenantGitMap gitMap, string repositoryName, string owner, string commitId);
        /// <summary>
        /// 指定したtokenのgitサービス側のユーザー名を取得する
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>コミット詳細</returns>
        Task<Result<string, string>> GetUserNameByTokenAsync(UserTenantGitMap gitMap);

    }
}

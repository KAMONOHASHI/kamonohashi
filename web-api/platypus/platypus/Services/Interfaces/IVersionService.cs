using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.ServiceModels;
using Nssol.Platypus.ServiceModels.GitHubModels;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services.Interfaces
{
    /// <summary>
    /// KAMONOHASHIのバージョン情報を管理するサービスにアクセスするためのサービスクラスのインターフェース
    /// </summary>
    public interface IVersionService
    {
        /// <summary>
        /// 指定したバージョン番号のKAMONOHASHIのバージョン情報を取得する
        /// </summary>
        /// <param name="version">バージョン番号</param>
        /// <returns>バージョン情報</returns>
        Task<Result<VersionModel, string>> GetKQIVersionAsync(string version);

        /// <summary>
        /// KAMONOHASHIの最新リリース情報を取得する
        /// </summary>
        /// <returns>最新リリース情報</returns>
        Task<Result<ReleaseModel, string>> GetLatestReleaseAsync();
    }
}

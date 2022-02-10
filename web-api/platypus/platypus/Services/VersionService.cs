using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.ServiceModels;
using Nssol.Platypus.ServiceModels.GitHubModels;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services
{
    /// <summary>
    /// KAMONOHASHIのバージョン情報に関するサービスにアクセスするためのAPIを呼び出すクラス
    /// </summary>
    public class VersionService : PlatypusServiceBase, IVersionService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VersionService(ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
        }

        /// <summary>
        /// 指定したバージョン番号のKAMONOHASHIのバージョン情報を取得する
        /// </summary>
        /// <param name="version">バージョン番号</param>
        /// <returns>バージョン情報</returns>
        public async Task<Result<VersionModel, string>> GetKQIVersionAsync(string version)
        {
            // API呼び出しパラメータ作成
            RequestParam param = new RequestParam()
            {
                // バージョン情報が記載されたJSONを取得
                BaseUrl = $"https://kamonohashi.ai",
                ApiPath = $"/version{version.Replace(".", "", StringComparison.CurrentCulture)}/index.html",
                UserAgent = "C#App"
            };

            // API 呼び出し
            Result<string, string> response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<VersionModel>(response.Value);
                return Result<VersionModel, string>.CreateResult(result);
            }
            else
            {
                return Result<VersionModel, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// KAMONOHASHIの最新リリース情報を取得する
        /// </summary>
        /// <returns>最新リリース情報</returns>
        public async Task<Result<ReleaseModel, string>> GetLatestReleaseAsync()
        {
            // API呼び出しパラメータ作成
            RequestParam param = new RequestParam()
            {
                // GitHubから最新リリース情報を取得する
                BaseUrl = "https://api.github.com",
                ApiPath = $"/repos/KAMONOHASHI/kamonohashi/releases/latest",
                UserAgent = "C#App"
            };

            // API 呼び出し
            Result<string, string> response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<GetReleasesModel>(response.Value);
                return Result<ReleaseModel, string>.CreateResult(
                    new ReleaseModel()
                    {
                        TagName = result.tag_name,
                        Name = result.name, // GitHubのリリース一覧取得APIでは、リリース日の取得ができないので、必要とあらばリリース名から取得する
                        Draft = result.draft,
                        Prerelease = result.prerelease
                    });
            }
            else
            {
                return Result<ReleaseModel, string>.CreateErrorResult(response.Error);
            }
        }
    }
}

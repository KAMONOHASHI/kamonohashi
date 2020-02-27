using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Nssol.Platypus.ApiModels.VersionApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.ServiceModels;
using System;
using System.Collections.Generic;
using System.Net;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// バージョン情報を扱うためのAPI集
    /// </summary>
    [Route("api/v1/version")]
    public class VersionController : PlatypusApiControllerBase
    {
        /// <summary>
        /// バージョン情報に関するロジック
        /// </summary>
        private readonly IVersionLogic versionLogic;

        /// <summary>
        /// バージョン情報をキャッシュするためのインメモリキャッシュ
        /// </summary>
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VersionController(
            IVersionLogic versionLogic,
            IMemoryCache memoryCache,
            IHttpContextAccessor accessor
            ) : base(accessor)
        {
            this.versionLogic = versionLogic;

            // インメモリキャッシュ
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// バージョン情報を取得
        /// </summary>
        /// <returns>バージョン情報</returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(VersionOutputModel), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            // インメモリキャッシュに格納されているバージョン情報を取得する
            VersionModel versionModel = memoryCache.Get<VersionModel>("version");

            VersionOutputModel outputModel = new VersionOutputModel();

            if (versionModel != null)
            {
                outputModel.Version = versionModel.Version;
                outputModel.Messages = GetMessages(versionModel.ReleaseDate, versionModel.Version, versionModel.LatestVersion);
            }
            else
            {
                outputModel.Version = versionLogic.GetVersion(); // 現在のバージョンを取得する
                outputModel.Messages = new List<string> { "バージョン情報の取得に失敗しました。" };
            }

            return JsonOK(outputModel);
        }

        /// <summary>
        /// バージョンに関してのメッセージを作成・取得する
        /// </summary>
        /// <param name="releaseDate">リリース日</param>
        /// <param name="version">バージョン番号</param>
        /// <param name="latestVersion">最新バージョン番号</param>
        /// <returns>メッセージ一覧</returns>
        private List<string> GetMessages(string releaseDate, string version, string latestVersion)
        {
            List<string> messages = new List<string>();

            // サポートに関してのメッセージを作成
            if (releaseDate != null)
            {
                // サポート対象期間はリリース日から1年間
                DateTime supportEndDate = DateTime.Parse(releaseDate);
                supportEndDate = supportEndDate.AddYears(1);
                supportEndDate = supportEndDate.AddDays(-1);

                // サポート対象期間を過ぎているか
                if (supportEndDate < DateTime.Now)
                {
                    messages.Add($"バージョン { version } は { supportEndDate.ToString("yyyy/MM/dd") } にサポートが終了しました。");
                }
                else
                {
                    messages.Add($"バージョン { version } は { supportEndDate.ToString("yyyy/MM/dd") } にサポートが終了します。");
                }
            }
            else
            {
                messages.Add($"バージョン { version } のサポート状況の取得に失敗しました。");
            }

            // 最新バージョンに関してのメッセージを作成
            if (latestVersion != null)
            {
                // 最新バージョンを使用しているか
                if (version != latestVersion)
                {
                    messages.Add($"最新バージョン { latestVersion } が公開されています。バージョンアップすることを推奨します。");
                }
                else
                {
                    messages.Add($"最新バージョン { version } を使用しています。");
                }
            }
            else
            {
                messages.Add("最新バージョン情報の取得に失敗しました。");
            }

            return messages;
        }
    }
}

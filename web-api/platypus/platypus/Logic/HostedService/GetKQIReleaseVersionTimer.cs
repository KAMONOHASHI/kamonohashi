using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.ServiceModels;
using Nssol.Platypus.Services.Interfaces;
using System;

namespace Nssol.Platypus.Logic.HostedService
{
    /// <summary>
    ///KAMONOHASHIのバージョン情報を定期的に取得するタイマークラス
    /// </summary>
    public class GetKQIReleaseVersionTimer : HostedServiceTimerBase
    {
        /// <summary>
        /// バージョン情報に関するロジック
        /// </summary>
        private readonly IVersionLogic versionLogic;

        /// <summary>
        /// 外部のバージョン情報に関するサービス
        /// </summary>
        private readonly IVersionService versionService;

        /// <summary>
        /// バージョン情報をキャッシュするためのインメモリキャッシュ
        /// </summary>
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// バージョン確認をしないかするか。 (appsettings.json、または launchSettings.json で設定)
        /// しない場合ture、する場合false。
        /// </summary>
        private readonly bool noCheckVersion;

        /// <summary>
        /// コンストラクタで各 DI オブジェクトを引数で受け取ります。
        /// </summary>
        public GetKQIReleaseVersionTimer(
            IVersionLogic versionLogic,
            IVersionService versionService,
            IMemoryCache memoryCache,
            IOptions<GetKQIReleaseVersionTimerOptions> getReleaseVersionTimerOptions,
            ILogger<GetKQIReleaseVersionTimer> logger
            ) : base(logger, getReleaseVersionTimerOptions.Value)
        {
            this.versionLogic = versionLogic;
            this.versionService = versionService;

            // メモリキャッシュ
            this.memoryCache = memoryCache;

            this.noCheckVersion = getReleaseVersionTimerOptions.Value.NoCheckVersion;
        }

        /// <summary>
        /// タイマーとして各種データが設定されているかをチェックします。
        /// もし、false を返却したならタイマーが生成されません。
        /// </summary>
        protected override bool IsValid()
        {
            bool ret = true;

            return ret;
        }

        /// <summary>
        /// KAMONOHASHIのバージョン情報を取得するメソッド
        /// </summary>
        protected override void DoWork(object state, int doWorkCount)
        {
            LogInfo($"KAMONOHASHIのバージョン情報を取得します。(第 {doWorkCount} 回目)");
            try
            {
                // 現在のバージョンを取得する
                string version = versionLogic.GetVersion();

                // テストのため、バージョン番号を指定。
                version = "1.1.5";
                //version = "1.1.6";

                VersionModel versionModel = new VersionModel(version);

                // バージョン確認するか否か確認
                if (!noCheckVersion)
                {
                    // 開発時は問い合わせない
                    if (version != "develop")
                    {
                        // KAMONOHASHIのバージョン管理サービスに問い合わせる
                        var currentVersion = versionService.GetKQIVersionAsync(version).Result;
                        if (currentVersion.IsSuccess && currentVersion.Value != null)
                        {
                            versionModel.ReleaseDate = ToFormatedString(currentVersion.Value.ReleaseDate);
                            versionModel.Support = currentVersion.Value.Support;
                        }
                    }

                    // KAMONOHASHIの最新リリース番号を取得する
                    var latestVersion = versionService.GetLatestReleaseAsync().Result;
                    if (latestVersion.IsSuccess && latestVersion.Value != null)
                    {
                        versionModel.LatestVersion = latestVersion.Value.TagName;
                    }
                }

                // インメモリキャッシュにバージョン情報を格納する
                memoryCache.Set("version", versionModel);
            }
            catch (Exception e)
            {
                // 通信エラーなどで例外をキャッチしたが ERROR ログを出力して処理を継続
                LogError($"KAMONOHASHIのバージョン情報取得中に例外をキャッチしましたが処理を継続します。 例外メッセージ=\"{e.Message}\"");
            }
        }

        /// <summary>
        /// 指定した日付を "yyyy/MM/dd" のフォーマットに変換して返却する。
        /// 変換に失敗した場合はnullを返却する。
        /// </summary>
        /// <param name="date">フォーマット変換対象の日付文字列</param>
        private string ToFormatedString(string date)
        {
            try
            {
                return DateTime.Parse(date).ToString("yyyy/MM/dd");
            }
            catch(Exception e)
            {
                LogWarn($"日付変換で例外が発生しました。msg=\"{e.Message}\"");
                return null;
            }
        }
    }
}

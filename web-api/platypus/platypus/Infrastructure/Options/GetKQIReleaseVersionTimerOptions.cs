using Nssol.Platypus.Logic.HostedService;

namespace Nssol.Platypus.Infrastructure.Options
{
    /// <summary>
    /// <see cref="GetKQIReleaseVersionTimer"/> の実行スケジュールを設定するオプション・クラスです。
    /// <see cref="TimerScheduleOptionsBase"/> を継承し、新たに独自の属性を追加しています。
    /// </summary>
    public class GetKQIReleaseVersionTimerOptions : TimerScheduleOptionsBase
    {
        /// <summary>
        /// バージョン確認をしないかするか。
        /// しない場合ture、する場合false。
        /// </summary>
        public bool NoCheckVersion { get; set; }
    }
}

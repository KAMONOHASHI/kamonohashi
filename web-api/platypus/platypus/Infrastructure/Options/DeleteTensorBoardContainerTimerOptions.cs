using Nssol.Platypus.Logic.HostedService;

namespace Nssol.Platypus.Infrastructure.Options
{
    /// <summary>
    /// <see cref="DeleteTensorBoardContainerTimer"/> の実行スケジュールを設定するオプション・クラスです。
    /// <see cref="TimerScheduleOptionsBase"/> を継承しますが、新たに追加する独自の属性はありません。
    /// </summary>
    public class DeleteTensorBoardContainerTimerOptions : TimerScheduleOptionsBase
    {
        // 個別で設定する属性は無い
    }
}

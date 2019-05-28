using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.HostedService
{
    /// <summary>
    /// HostedService の　IF を持つタイマーの基本クラスです。
    /// Start/Stop/Dispose のメソッドを共通化し、タイマー制御を行います。
    /// 導出クラスではタイマーで実行されるメソッドや、タイマーの実行開始時間を制御する dueTime を返却するメソッドを実装してください。
    /// </summary>
    abstract public class HostedServiceTimerBase : IHostedService, IDisposable
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// タイマーのスケージュールを設定したオブジェクト
        /// </summary>
        private TimerScheduleOptionsBase timerScheduleOptions;

        /// <summary>
        /// タイマー
        /// </summary>
        private Timer timer;

        /// <summary>
        /// 起動回数
        /// </summary>
        private int doWorkCount;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        protected HostedServiceTimerBase(ILogger logger, TimerScheduleOptionsBase timerScheduleOptions)
        {
            this.logger = logger;
            this.timerScheduleOptions = timerScheduleOptions;
            doWorkCount = 0;
            
            // timer オブジェクトは StartAsync() 初回実行時に生成
            this.timer = null;

            // タイマーのスケジュール設定を解析
            timerScheduleOptions.SetLogger(logger);
            timerScheduleOptions.ParseScheduleData();
        }

        /// <summary>
        /// タイマーより定期的に起動されるメソッドで、導出クラスで実装します。
        /// このメソッドの起動回数(初回は1)は doWorkCount で渡されます。
        /// </summary>
        protected abstract void DoWork(object state, int doWorkCount);

        /// <summary>
        /// 状態チェック用のメソッドで導出クラスで個別に実装します。
        /// StartAsync() 時に呼び出され false ならタイマーの生成を行わずエラーをログ出力すます。
        /// </summary>
        protected abstract bool isValid();

        /// <summary>
        /// タイマーが定期的に実行するメソッドです。
        /// 実際の処理は、導出クラスで メソッド DoWork() を実装してください。
        /// </summary>
        private void DoWorkBase(object state)
        {
            // 実行回数の increment
            doWorkCount++;
            // タイマーの実行内容は導出クラスで個別実装すること
            DoWork(state, doWorkCount);
            // Timer のリスケジュール
            TimeSpan? dueTime = timerScheduleOptions.GetDueTime();
            if (dueTime == null)
            {
                LogWarn("次回の待機時間が null なのでタイマーを停止します。");
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                return;
            }
            bool ret = timer.Change(dueTime.Value, TimeSpan.FromDays(1)); // 必ず再スケジュールするので period の値は適当で良い
            if (!ret)
            {
                LogError("タイマーの再スケジュールに失敗しました。今後、このタイマーは起動されませんがサーバ WEB-API は継続します。");
            }
        }

        /// <summary>
        /// HostedService サービス停止時に実行される公開メソッドです。
        /// 初期実行時はタイマーを生成し、２回目以降なら再スケジューリングを行います。
        /// タイマーの生成や再スケジュールに失敗しても Warn ログを出力したうえで結果は Task.CompletedTask を返却します。
        /// </summary>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //
            // 導出クラスの状態やタイマー・スケジュールよりタイマー生成が可能かどうかのチェック
            //
            if (!this.isValid())
            {
                LogError("導出クラスのデータ設定状態が不正なのでタイマーは稼働しません。");
                return Task.CompletedTask;
            }
            if (!timerScheduleOptions.IsValid())
            {
                LogError("タイマーのスケジュール設定が不正なのでタイマーは稼働しません。");
                return Task.CompletedTask;
            }
            TimeSpan? dueTime = timerScheduleOptions.GetStartingtDueTime();
            if (dueTime == null)
            {
                LogWarn("初回の待機時間が null なのでタイマーは稼働しません。");
                return Task.CompletedTask;
            }

            //
            // タイマーの生成
            //
            if (timer == null)
            {
                // 最初の１回目は Timer を生成
                try
                {
                    timer = new Timer(DoWorkBase, null, dueTime.Value, TimeSpan.FromDays(1)); // 必ず再スケジュールするので period の値は適当で良い
                    LogInfo($"StartAsync() が呼ばれたので Timer を生成してスケジューリングしました。dueTime=\"{dueTime.ToString()}\"");
                }
                catch (Exception e)
                {
                    LogError("タイマーの生成に失敗しました。このサービスは起動されませんがサーバ WEB-API は継続します。" +
                        $"dueTime=\"{dueTime.ToString()}\", 例外 msg=\"{e.Message}\"");
                }
            }
            else
            {
                // ２回目以降は再スケジュール
                bool ret = timer.Change(dueTime.Value, TimeSpan.FromDays(1)); // 必ず再スケジュールするので period の値は適当で良い
                if (ret)
                {
                    LogInfo($"StartAsync() が呼ばれたので Timer を再スケジューリングしました。dueTime=\"{dueTime.ToString()}\"");
                }
                else
                {
                    LogError("タイマーの再スケジュールに失敗しました。今後、このサービスは起動されませんがサーバ WEB-API は継続します。" +
                        $"dueTime=\"{dueTime.ToString()}\"");
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// HostedService サービス停止時に実行される公開メソッドです。
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            LogInfo("StopAsync() が呼ばれたので Timer を停止します。");
            timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }

        /// <summary>
        /// HostedService サービス dispose 時に実行される公開メソッドです。
        /// </summary>
        public void Dispose()
        {
            LogInfo("Dispose() が呼ばれたので Timer#Dispose() を実行します。");
            timer?.Dispose();
        }

        /// <summary>
        /// ERROR ログ出力でユーザ名は "-" とします。
        /// </summary>
        public void LogError(string msg)
        {
            LogUtil.WriteLLog(logger.LogError, "-", "-", msg);
        }

        /// <summary>
        /// WARN ログ出力でユーザ名は "-" とします。
        /// </summary>
        public void LogWarn(string msg)
        {
            LogUtil.WriteLLog(logger.LogWarning, "-", "-", msg);
        }

        /// <summary>
        /// INFO ログ出力でユーザ名は "-" とします。
        /// </summary>
        public void LogInfo(string msg)
        {
            LogUtil.WriteLLog(logger.LogInformation, "-", "-", msg);
        }

        /// <summary>
        /// DEBUG ログ出力でユーザ名は "-" とします。
        /// </summary>
        public void LogDebug(string msg)
        {
            LogUtil.WriteLLog(logger.LogDebug, "-", "-", msg);
        }
    }
}

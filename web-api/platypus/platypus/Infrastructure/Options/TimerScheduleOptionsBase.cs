using Microsoft.Extensions.Logging;
using System;

namespace Nssol.Platypus.Infrastructure.Options
{
    /// <summary>
    /// HostedService(Timer類)のスケジュール設定情報を管理する基本クラス。
    /// </summary>
    public class TimerScheduleOptionsBase
    {
        /// <summary>
        /// <seealso cref="TimeZoneInfo.FindSystemTimeZoneById(string)"/>   で利用するタイムゾーン ID 名を設定する。
        /// 空白ならローカル(マシーンのインストール時に設定した地域)のタイムゾーンを代用する。
        /// </summary>
        /// <example>
        /// 日本のタイムゾーンなら "Tokyo Standard Time" と設定する。
        /// 米国東海岸のタイムゾーンなら "US Eastern Standard Time" と設定する。
        /// 標準時間なら "UTC" と設定する。
        /// </example>
        public String TimeZoneId { get; set; }

        /// <summary>
        /// 曜日と時刻で実行時間を指定する。
        /// 曜日は英語名(省略形も可)、時刻は hh:mm:ss とし '=' で連結する。
        /// 複数曜日指定の場合は ';' で連結する。
        /// 全ての曜日で実行する場合は "Everyday" を指定する。
        /// なお、空文字を指定したならエラーログを出力し、タイマーは稼働しない。
        /// </summary>
        /// <example>
        /// 毎週水曜日の 03:00:00 に実行するなら "Wed=03:00:00" と設定する。
        /// 毎週月曜日から金曜日の 22:00:00 に実行するなら "Mon=22:00:00;Tue=22:00:00;Wed=22:00:00;Thu=22:00:00;Fri=22:00:00" と設定する。
        /// 毎日 02:00:00 に実行するなら "Everyday=02:00:00" と設定する。
        /// 月曜日の 23:00:00 と土曜日の 06:00:00 なら "Mon=23:00:00;Sat=06:00:00" と設定する。
        /// 毎日 04:00:00 だが日曜日だけ 02:00:00 に実行するなら "Everyday=04:00:00;Sun=02:00:00" と設定する。
        /// </example>
        public string WeeklyTimeSchedule { get; set; }

        /// <summary>
        /// 初回起動までの待機時間(秒)を指定する。
        /// 初回のみ web-api 起動直後に実行したい場合に秒で時間指定する。
        /// 0 以下なら WeeklyTimeSchedule に従った待機時間を採用する。
        /// </summary>
        public int StartingDueTimeSpanSecond { get; set; }

        /// <summary>
        /// 設定データを解析して実行時間の情報を格納します。
        /// 解析の成否はメソッド isValid() で判別します。
        /// </summary>
        public void ParseScheduleData()
        {
            string classMethodName = "TimerScheduleOptionsBase#parseScheduleData()";
            valid = true;

            // タイムゾーンの取得
            if (string.IsNullOrEmpty(TimeZoneId))
            {
                // タイムゾーン名が空白なら local を代用
                timeZoneInfo = TimeZoneInfo.Local;
            }
            else
            {
                try
                {
                    // 指定地域のタイムゾーン
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
                }
                catch (Exception e)
                {
                    // タイムゾーンの取得が失敗したので valid=false とする
                    LogError($"{classMethodName}: タイムゾーンの取得に失敗しました。timeZone名=\"{TimeZoneId}\", msg=\"{e.Message}\"");
                    valid = false;
                    return;
                }
            }
            LogDebug($"{classMethodName}: タイムゾーン \"{timeZoneInfo.Id}\" に従ってタイマーを起動します。なお、ローカルのタイムゾーンは \"{TimeZoneInfo.Local.Id}\" です。");

            // 週次設定データの設定有無の判別
            if (IsNoSchedule())
            {
                // 週次設定データが空ならタイマーは稼働しない
                LogDebug($"{classMethodName}: スケジュールが設定されていません。");
                return;
            }

            // 曜日毎の DateTime を格納するサイズ７の配列を生成
            weekTimeArray = new DateTime?[7];
            for (int weekIndex = 0; weekIndex < 7; weekIndex++)
            {
                // まずは null で初期化するが null ならその曜日の起動時間は未設定という扱いにする
                weekTimeArray[weekIndex] = null;
            }
            DateTime sunday_base = new DateTime(2019, 3, 24);  // 日曜日の起点 (日付は利用しないので、あくまでサンプル設定)
            foreach (string keyvals in WeeklyTimeSchedule.Split(';'))
            {
                string[] key_val = keyvals.Split('=');
                if (key_val.Length != 2 || string.IsNullOrEmpty(key_val[0]) || string.IsNullOrEmpty(key_val[1]))
                {
                    LogError($"{classMethodName}: 不正な曜日と時間指定です。曜日と時間は '=' で連結してください。不正文字列=\"{keyvals}\"");
                    valid = false;
                    return;
                }
                DateTime time;
                try
                {
                    // 時分秒の取得
                    time = DateTime.Parse(key_val[1]);
                }
                catch (Exception e)
                {
                    LogError($"{classMethodName}: 時間指定が不正です。時間=\"{key_val[1]}\", 例外=\"{e.Message}\"");
                    valid = false;
                    return;
                }
                if (key_val[0].ToLower().StartsWith("every"))
                {
                    // １週間全ての曜日に時間設定
                    for (int weekIndex = 0; weekIndex < 7; weekIndex++)
                    {
                        weekTimeArray[weekIndex] = new DateTime(sunday_base.Year, sunday_base.Month, sunday_base.Day + weekIndex, time.Hour, time.Minute, time.Second);
                    }
                }
                else if (key_val[0].ToLower().StartsWith("dynamic"))
                {
                    // 初回サーバ起動の曜日、時間によって動的に時間が設定される
                    DateTime now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, timeZoneInfo);
                    weekTimeArray[(int)now.DayOfWeek] = new DateTime(sunday_base.Year, sunday_base.Month, sunday_base.Day + (int)now.DayOfWeek, now.Hour, now.Minute, now.Second);
                }
                else
                {
                    // 個別曜日で時間設定
                    int weekIndex = GetWeekIndex(key_val[0]);
                    if (weekIndex < 0 || weekIndex > 6)
                    {
                        LogError($"{classMethodName}: 不正な曜日指定です。曜日の英字略称名=\"{key_val[0]}\"");
                        valid = false;
                        return;
                    }
                    weekTimeArray[weekIndex] = new DateTime(sunday_base.Year, sunday_base.Month, sunday_base.Day + weekIndex, time.Hour, time.Minute, time.Second);
                }
            }

            // 曜日毎の時間設定をデバッグ出力
            for (int weekIndex = 0; weekIndex < 7; weekIndex++)
            {
                if (weekTimeArray[weekIndex] != null)
                {
                    DateTime dt = weekTimeArray[weekIndex].Value;
                    LogDebug($"{classMethodName}: 毎週 {dt.ToString("dddd")} {dt.ToString("T")} (タイムゾーン {timeZoneInfo.Id}) のスケジュール設定を認識。");
                }
            }
        }

        /// <summary>
        /// ロガーを設定する
        /// </summary>
        /// <param name="logger">ロガー</param>
        public void SetLogger(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// スケジュール WeeklyTimeSchedule が設定されてなくタイマーを起動しないなら true を返却する。
        /// </summary>
        private bool IsNoSchedule()
        {
            return string.IsNullOrEmpty(WeeklyTimeSchedule);
        }

        /// <summary>
        /// データの設定状態の成否を返却する。正しく設定されているなら true, 不正なら false を返却する。
        /// </summary>
        public bool IsValid()
        {
            return valid;
        }

        /// <summary>
        /// 初回起動までの時間を取得する
        /// </summary>
        public TimeSpan? GetStartingtDueTime()
        {
            if (StartingDueTimeSpanSecond > 0)
            {
                return TimeSpan.FromSeconds(StartingDueTimeSpanSecond);
            }
            return GetDueTime();
        }

        /// <summary>
        /// タイムゾーンにおける現在時刻より次のスケジュールで指定される曜日・時刻までの待機時間を返却する。
        /// </summary>
        public TimeSpan? GetDueTime()
        {
            string classMethodName = "TimerScheduleOptionsBase#getDueTime()";
            if (IsNoSchedule())
            {
                // スケジュールが存在しない場合は null を返却
                LogDebug($"{classMethodName}: スケジュールが設定されていませんので null を返却します。");
                return null;
            }
            // タイムゾーンにおける現在時刻
            DateTime zoneNowTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, timeZoneInfo);
            LogDebug($"{classMethodName}: タイムゾーン \"{timeZoneInfo.Id}\" での現在日付: 曜日 {zoneNowTime.ToString("dddd")}, 日付時刻 {zoneNowTime.ToString("G")}");
            TimeSpan zoneNowTimeSpan = new TimeSpan(zoneNowTime.Hour, zoneNowTime.Minute, zoneNowTime.Second);
            int nowWeekIndex = (int) zoneNowTime.DayOfWeek;
            if (weekTimeArray[nowWeekIndex] != null)
            {
                // 当該曜日で現時刻より後のスケジュールが存在するなら、その時刻までの TimeSpan を返却
                DateTime weekTime = weekTimeArray[nowWeekIndex].Value;
                TimeSpan weekTimeSpan = new TimeSpan(weekTime.Hour, weekTime.Minute, weekTime.Second);
                if (weekTimeSpan > zoneNowTimeSpan)
                {
                    TimeSpan dueTime = weekTimeSpan - zoneNowTimeSpan;
                    LogDebug($"{classMethodName}: 次回実行のスケジュール: 曜日 {weekTime.ToString("dddd")}, 時間 {weekTime.ToString("T")}, それまでの待機時間 {dueTime}");
                    return dueTime;
                }
            }
            int dayCount = 0;
            nowWeekIndex++;
            for (int i = 0; i < 7; i++)
            {
                if (nowWeekIndex == 7)
                {
                    // nowWeekIndex が 7 なら 0 に戻す
                    nowWeekIndex = 0;
                }
                if (weekTimeArray[nowWeekIndex] != null)
                {
                    // nowWeekIndex の曜日にスケジュールが存在
                    DateTime weekTime = weekTimeArray[nowWeekIndex].Value;
                    TimeSpan weekTimeSpan = new TimeSpan(weekTime.Hour, weekTime.Minute, weekTime.Second);
                    TimeSpan dueTime = weekTimeSpan + TimeSpan.FromDays(dayCount) + (TimeSpan.FromDays(1) - zoneNowTimeSpan);
                    LogDebug($"{classMethodName}: 次回実行のスケジュール: 曜日 {weekTime.ToString("dddd")}, 時間 {weekTime.ToString("T")}, それまでの待機時間 {dueTime}");
                    return dueTime;
                }
                // 次の曜日
                nowWeekIndex++;
                dayCount++;
            }
            // 必ずスケジュールデータが存在するので、ここにきたらエラー
            LogError($"{classMethodName}: 待機時間を計算できませんでした。よって、タイマーは稼働されません。");
            return null;
        }

        /// <summary>
        /// ロガー
        /// </summary>
        private ILogger logger = null;

        /// <summary>
        /// データの設定状態の成否を格納する。設定状態が正しいなら true, 不正なら false を格納している。
        /// </summary>
        private bool valid;

        /// <summary>
        /// TimeZoneId に対応するタイムゾーンを格納している。空文字なら TimeZoneInfo.Local を採用する。
        /// </summary>
        private TimeZoneInfo timeZoneInfo;

        /// <summary>
        /// 各曜日(Sun=0,Mon=1,...,Sat=6)の時間を格納する配列である。
        /// もし null が設定されているなら、その曜日のスケジュール時間は無い。
        /// </summary>
        private DateTime?[] weekTimeArray;

        /// <summary>
        /// 文字列より曜日番号(Sun=0,Mon=1,...,Sat=6)を返却
        /// </summary>
        /// <param name="weekName">曜日文字列</param>
        /// <returns>曜日番号</returns>
        private int GetWeekIndex(string weekName)
        {
            if (weekName.ToLower().StartsWith("sun"))
            {
                return (int) DayOfWeek.Sunday;
            }
            else if (weekName.ToLower().StartsWith("mon"))
            {
                return (int) DayOfWeek.Monday;
            }
            else if (weekName.ToLower().StartsWith("tue"))
            {
                return (int) DayOfWeek.Tuesday;
            }
            else if (weekName.ToLower().StartsWith("wed"))
            {
                return (int) DayOfWeek.Wednesday;
            }
            else if (weekName.ToLower().StartsWith("thu"))
            {
                return (int) DayOfWeek.Thursday;
            }
            else if (weekName.ToLower().StartsWith("fri"))
            {
                return (int) DayOfWeek.Friday;
            }
            else if (weekName.ToLower().StartsWith("sat"))
            {
                return (int) DayOfWeek.Saturday;
            }
            return -1;
        }

        /// <summary>
        /// ERROR ログ出力でユーザ名は "-" とします。
        /// </summary>
        /// <param name="msg">メッセージ</param>
        private void LogError(string msg)
        {
            if (logger != null)
            {
                LogUtil.WriteLLog(logger.LogError, "-", "-", msg);
            }
        }

        /// <summary>
        /// WARN ログ出力でユーザ名は "-" とします。
        /// </summary>
        /// <param name="msg">メッセージ</param>
        private void LogWarn(string msg)
        {
            if (logger != null)
            {
                LogUtil.WriteLLog(logger.LogWarning, "-", "-", msg);
            }
        }

        /// <summary>
        /// INFO ログ出力でユーザ名は "-" とします。
        /// </summary>
        /// <param name="msg">メッセージ</param>
        private void LogInfo(string msg)
        {
            if (logger != null)
            {
                LogUtil.WriteLLog(logger.LogInformation, "-", "-", msg);
            }
        }

        /// <summary>
        /// DEBUG ログ出力でユーザ名は "-" とします。
        /// </summary>
        /// <param name="msg">メッセージ</param>
        private void LogDebug(string msg)
        {
            if (logger != null)
            {
                LogUtil.WriteLLog(logger.LogDebug, "-", "-", msg);
            }
        }
    }
}

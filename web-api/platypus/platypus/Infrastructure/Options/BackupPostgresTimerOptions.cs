using Nssol.Platypus.Logic.HostedService;

namespace Nssol.Platypus.Infrastructure.Options
{
    /// <summary>
    /// <see cref="BackupPostgresTimer"/> の実行スケジュールを設定するオプション・クラスです。
    /// <see cref="TimerScheduleOptionsBase"/> を継承し、新たに SystemNamespace などの属性を追加しています。
    /// </summary>
    public class BackupPostgresTimerOptions : TimerScheduleOptionsBase
    {
        /// <summary>
        /// BackupPostgresTimer のコマンドを実行する Namespace
        /// </summary>
        public string SystemNamespace { get; set; }

        /// <summary>
        /// バックアップ・ファイル格納ディレクトリのパス
        /// </summary>
        public string FileSavedPath { get; set; }

        /// <summary>
        /// バックアップ・ファイルの名前部分
        /// </summary>
        public string FileBodyName { get; set; }

        /// <summary>
        /// バックアップ・ファイルの最大保存ファイル数
        /// </summary>
        public int MaxNumberOfBackupFiles { get; set; }
    }
}

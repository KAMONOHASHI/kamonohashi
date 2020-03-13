using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Web;

namespace Nssol.Platypus.Logic.HostedService
{
    /// <summary>
    /// Postgres のデータバックを定期的に実行するタイマーです。
    /// </summary>
    public class BackupPostgresTimer : HostedServiceTimerBase
    {
        /// <summary>
        ///  DI で注入される kubernetes による cluster サービス
        /// </summary>
        private readonly IClusterManagementService clusterManagementService;

        /// <summary>
        /// kubernetes の token (環境変数、または launchSettings.json で設定)
        /// </summary>
        private readonly string kubernetesToken;

        /// <summary>
        /// バックアップコマンド実行 namespace 名 (appsettings.json で設定)
        /// </summary>
        private readonly string systemNamespace;

        /// <summary>
        /// バックアップ・ファイル格納ディレクトリのパス
        /// </summary>
        private readonly string backupSavedPath;

        /// <summary>
        /// バックアップ・ファイルの本体名
        /// </summary>
        private readonly string backupFileBodyName;

        /// <summary>
        /// Postgres バックアップ実行時の DB ユーザ名
        /// </summary>
        private readonly string dbUser;

        /// <summary>
        /// Postgres バックアップ対象の DB 名
        /// </summary>
        private readonly string dbName;

        /// <summary>
        /// バックアップ・ファイルの最大保存ファイル数
        /// </summary>
        private readonly int maxNumberOfBackupFiles;

        /// <summary>
        /// Pod 名取得時の appName (定数)
        /// </summary>
        private const string APP_NAME = "postgres";

        /// <summary>
        /// Pod 名取得時の limit 値 (定数)
        /// </summary>
        private const int LIMIT = 500;

        /// <summary>
        /// バックアップコマンド実行時における終了ステータス確認待ち時間(ミリ秒)
        /// </summary>
        private const int SLEEP_MILLISEC = 1000;

        /// <summary>
        /// バックアップコマンド実行時における終了ステータス確認回数の最大値
        /// </summary>
        private const int MAX_LOOP_COUNT = 20;

        /// <summary>
        /// コンストラクタで各 DI オブジェクトを引数で受け取ります。
        /// </summary>
        public BackupPostgresTimer(
            IClusterManagementService clusterManagementService,
            IOptions<ContainerManageOptions> containerManageOptions,
            IOptions<BackupPostgresTimerOptions> backupPostgresTimerOptions,
            ILogger<BackupPostgresTimer> logger
            ) : base(logger, backupPostgresTimerOptions.Value)
        {
            // kubernetes による cluster 管理サービス
            this.clusterManagementService = clusterManagementService;
            // kubernetes の token
            this.kubernetesToken = containerManageOptions.Value.ResourceManageKey;

            // backup コマンド実行環境の各情報
            this.systemNamespace = backupPostgresTimerOptions.Value.SystemNamespace;
            this.backupSavedPath = backupPostgresTimerOptions.Value.FileSavedPath;
            this.backupFileBodyName = backupPostgresTimerOptions.Value.FileBodyName;
            this.maxNumberOfBackupFiles = backupPostgresTimerOptions.Value.MaxNumberOfBackupFiles;

            // DB 接続情報
            string dbInfo = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            IDictionary<string, string> dbInfoDic = string2KeyValDic(dbInfo, ';', '=');
            dbUser = dbInfoDic["User Id"];
            dbName = dbInfoDic["Database"];
        }

        /// <summary>
        /// タイマーとして各種データが設定されているかをチェックします。
        /// もし、false を返却したならタイマーが生成されません。
        /// </summary>
        protected override bool IsValid()
        {
            bool ret = true;
            if (string.IsNullOrEmpty(kubernetesToken))
            {
                LogError("kubernetes のトークンが設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(systemNamespace))
            {
                LogError("DB バックアップを行う kubernetes の namesapace が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(backupSavedPath))
            {
                LogError("バックアップ・ファイル格納先のディレクトリ名が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(backupFileBodyName))
            {
                LogError("バックアップ・ファイル名が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(dbUser))
            {
                LogError("DB ユーザ名が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(dbName))
            {
                LogError("データベース名が設定されていません。");
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// Postgres のデータバックを実施するメソッドです。
        /// </summary>
        protected override void DoWork(object state, int doWorkCount)
        {
            LogInfo($"Postgres のデータバックを実施します。(第 {doWorkCount} 回目)");
            try
            {
                // Pod 名の取得
                var resultPod = clusterManagementService.GetPodNameAsync(systemNamespace, APP_NAME, LIMIT, kubernetesToken).Result;
                if (!resultPod.IsSuccess)
                {
                    LogError($"Pod 名を取得できませんでしたので Postgres バックアップ処理は中断します。 namespace=\"{systemNamespace}\", appName=\"{APP_NAME}\"");
                    return;
                }

                // バックアップ・コマンド
                string yyymmdd = getTodayString("yyyyMMdd");
                string fileName = $"{backupFileBodyName}-{yyymmdd}.sql";
                string command = $"pg_dumpall -U {dbUser} -l {dbName} > {backupSavedPath}/{fileName}";
                var resultExec = clusterManagementService.ExecBashCommandAsync(systemNamespace, resultPod.Value, HttpUtility.UrlEncode(command), APP_NAME, kubernetesToken, SLEEP_MILLISEC, MAX_LOOP_COUNT).Result;
                if (!resultExec)
                {
                    LogError($"Postgres のバックアップに失敗しました。command=\"{command}\"");
                    return;
                }
                // バックアップは正常に終了
                LogInfo($"Postgres のバックアップを完了しました。command=\"{command}\"");

                // バックアップ最大ファイル数が 0 以下なら、ファイルの削除は行わない
                if (maxNumberOfBackupFiles <= 0)
                {
                    LogInfo($"Postgres バックアップファイルの削除は実施しません。");
                    return;
                }

                // 最大個数以上のバックアップが存在しているなら、日付サフィックスの若い順に削除
                // ref: https://hogashi.hatenablog.com/entry/2016/03/25/050613
                command = $"rm `ls -1v {backupSavedPath}/{backupFileBodyName}-*.sql | head -n-{maxNumberOfBackupFiles}`";
                resultExec = clusterManagementService.ExecBashCommandAsync(systemNamespace, resultPod.Value, HttpUtility.UrlEncode(command), APP_NAME, kubernetesToken, SLEEP_MILLISEC, MAX_LOOP_COUNT).Result;
                if (resultExec)
                {
                    LogInfo($"Postgres バックアップファイルの削除に問題はありませんでした。command=\"{command}\"");
                }
                else
                {
                    LogError($"Postgres バックアップファイルの削除に問題が発生しました。command=\"{command}\"");
                }
            }
            catch (Exception e)
            {
                // DB 系での削除操作などで例外をキャッチしたが ERROR ログを出力して処理を継続
                LogError($"Postgres のバックアップ中に例外をキャッチしましたが処理を継続します。 例外メッセージ=\"{e.Message}\"");
            }
        }

        /// <summary>
        /// 本日の日付を format で変換して返却
        /// </summary>
        private string getTodayString(string format)
        {
            try
            {
                return DateTime.Now.ToString(format);
            }
            catch(Exception e)
            {
                LogWarn($"日付変換で例外が発生しました。msg=\"{e.Message}\"");
                return DateTime.Now.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// 文字列を key, value の Dictionary に変換して返却
        /// </summary>
        private IDictionary<string, string> string2KeyValDic(string data, char split_1st, char split_2nd)
        {
            var dic = new Dictionary<string, string>();
            foreach (string attr in data.Split(split_1st))
            {
                string[] key_val = attr.Split(split_2nd);
                try
                {
                    dic.Add(key_val[0], key_val[1]);
                }
                catch (Exception e)
                {
                    LogWarn($"Dictionary に対するデータ登録時に例外が発生しました。key_val=\"{key_val}\", msg=\"{e.Message}\"");
                }
            }
            return dic;
        }
    }
}

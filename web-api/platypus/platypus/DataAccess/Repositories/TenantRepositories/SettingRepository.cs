using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    public class SettingRepository : RepositoryBase<Setting>, ISettingRepository
    {
        /// <summary>
        /// APIでトークンの生成に使う共通鍵。
        /// 常に不定なのでstatic。
        /// </summary>
        private static SymmetricSecurityKey signingKey;

        private Seed seed;
        private IUnitOfWork unitOfWork;
        private DBInitRetryOptions DBInitRetryOptions;
        private ILogger<SettingRepository> logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingRepository(
            CommonDbContext context,
            Seed seed,
            IOptions<DBInitRetryOptions> DBInitRetryOptions,
            IUnitOfWork unitOfWork,
            ILogger<SettingRepository> logger) : base(context)
        {
            this.seed = seed;
            this.DBInitRetryOptions = DBInitRetryOptions.Value;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        /// <summary>
        /// APIでトークンの生成に使う共通鍵を取得する
        /// </summary>
        public SymmetricSecurityKey GetApiJwtSigningKey()
        {
            // DB の初期化
            initializeDB();

            if (signingKey == null)
            {
                string apiSecurityTokenPass;

                var settings = GetAll();
                if(settings?.Count() == 0)
                {
                    //初回起動なので、乱数でパスフレーズを作る
                    apiSecurityTokenPass = Util.GenerateRandamString(32);
                    Add(new Setting()
                    {
                        ApiSecurityTokenPass = apiSecurityTokenPass
                    });
                    unitOfWork.Commit(); //DBに保存
                }
                else
                {
                    apiSecurityTokenPass = settings.First().ApiSecurityTokenPass;
                }

                byte[] key = Encoding.UTF8.GetBytes(apiSecurityTokenPass.ToCharArray(), 0, 32);
                signingKey = new SymmetricSecurityKey(key);
            }
            return signingKey;
        }

        /// <summary>
        /// DB を初期化します。同時に ObjectStore や Cluster(k8s) と同期させます。
        /// Postgress が未起動状態で失敗するようなら retry します。
        /// </summary>
        private void initializeDB()
        {
            int retryCount = 1;
            for ( ; retryCount <= DBInitRetryOptions.InitDBRetryMaxCount; retryCount++)
            {
                if (retryCount > 1)
                {
                    LogDebug($"DB の初期化の retry まで {DBInitRetryOptions.InitDBRetrySleepSec} 秒間 sleep します。");
                    Thread.Sleep(DBInitRetryOptions.InitDBRetrySleepSec * 1000);
                }
                LogInfo($"DB を初期化します。(第 {retryCount} 回目)");
                try
                {
                    // DB のマイグレーション
                    seed.Migrate();

                    // DB が初期化済みかどうかをチェック
                    if (seed.IsInitializedDB())
                    {
                        LogInfo($"DB は既に初期化されています。");
                        break;
                    }

                    // DB 初期化の設定値のチェック
                    if (!seed.isValidDeployOptions())
                    {
                        LogError($"DB 初期化用の設定値が不正ですので全ての処理を終了します。");
                        // 全ての処理を終了
                        return;
                    }

                    // DB の初期化
                    seed.InitilizeDB();
                    LogInfo($"DB を初期化しました。");
                    break;
                }
                catch (Exception e)
                {
                    //例外をキャッチしたが ERROR ログを出力して処理を継続(retry)
                    LogError($"DB の初期化中に例外をキャッチしました。 例外メッセージ=\"{e.Message}\"");
                }
            }
            if (retryCount > DBInitRetryOptions.InitDBRetryMaxCount)
            {
                LogError($"DB の初期化を {DBInitRetryOptions.InitDBRetryMaxCount} 回実行しましたが最終的に失敗で終わりました。");
                // DB の初期化が失敗したので処理を終了
                return;
            }

            //
            //　初期生成テナントと ObjectStore を同期
            //
            retryCount = 1;
            for (; retryCount <= DBInitRetryOptions.SyncObjectStoreRetryMaxCount; retryCount++)
            {
                if (retryCount > 1)
                {
                    LogDebug($"初期生成テナントと ObjectStore の同期処理の retry まで {DBInitRetryOptions.SyncObjectStoreRetrySleepSec} 秒間 sleep します。");
                    Thread.Sleep(DBInitRetryOptions.SyncObjectStoreRetrySleepSec * 1000);
                }
                try
                {
                    LogInfo($"初期生成テナントと ObjectStore を同期します。(第 {retryCount} 回目)");
                    seed.SyncInitialObjectStore();
                    LogInfo($"初期生成テナントと ObjectStore を同期しました。");
                    break;
                }
                catch (Exception e)
                {
                    //例外をキャッチしたが ERROR ログを出力して処理を継続(retry)
                    LogError($"初期生成テナントと ObjectStore の同期処理中に例外をキャッチしました。 例外メッセージ=\"{e.Message}\"");
                }
            }
            if (retryCount > DBInitRetryOptions.SyncObjectStoreRetryMaxCount)
            {
                LogError($"初期生成テナントと ObjectStore を同期処理を {DBInitRetryOptions.SyncObjectStoreRetryMaxCount} 回実行しましたが最終的に失敗で終わりました。");
                // 処理失敗でも次に続く
            }

            //
            //　初期生成テナントと ClusterManager(k8s) を同期
            //
            retryCount = 1;
            for (; retryCount <= DBInitRetryOptions.SyncClusterRetryMaxCount; retryCount++)
            {
                if (retryCount > 1)
                {
                    LogDebug($"初期生成テナントと ClusterManager(k8s) の同期処理の retry まで {DBInitRetryOptions.SyncClusterRetrySleepSec} 秒間 sleep します。");
                    Thread.Sleep(DBInitRetryOptions.SyncClusterRetrySleepSec * 1000);
                }
                try
                {
                    LogInfo($"初期生成テナントと ClusterManager(k8s) を同期します。(第 {retryCount} 回目)");
                    if (seed.SyncInitialClusterManager())
                    {
                        LogInfo($"初期生成テナントと ClusterManager(k8s) を同期しました。");
                        break;
                    }
                    else
                    {
                        LogError($"初期生成テナントと ClusterManager(k8s) の同期に失敗しました。");
                    }
                }
                catch (Exception e)
                {
                    //例外をキャッチしたが ERROR ログを出力して処理を継続(retry)
                    LogError($"初期生成テナントと ClusterManager(k8s) の同期処理中に例外をキャッチしました。 例外メッセージ=\"{e.Message}\"");
                }
            }
            if (retryCount > DBInitRetryOptions.SyncClusterRetryMaxCount)
            {
                LogError($"初期生成テナントと ClusterManager(k8s) を同期処理を {DBInitRetryOptions.SyncClusterRetryMaxCount} 回実行しましたが最終的に失敗で終わりました。");
                // 処理失敗でも次に続く
            }
        }

        /// <summary>
        /// ERROR ログ出力でユーザ名は "-" とします。
        /// </summary>
        private void LogError(string msg)
        {
            LogUtil.WriteLLog(logger.LogError, "-", "-", msg);
        }

        /// <summary>
        /// WARN ログ出力でユーザ名は "-" とします。
        /// </summary>
        private void LogWarn(string msg)
        {
            LogUtil.WriteLLog(logger.LogWarning, "-", "-", msg);
        }

        /// <summary>
        /// INFO ログ出力でユーザ名は "-" とします。
        /// </summary>
        private void LogInfo(string msg)
        {
            LogUtil.WriteLLog(logger.LogInformation, "-", "-", msg);
        }

        /// <summary>
        /// DEBUG ログ出力でユーザ名は "-" とします。
        /// </summary>
        private void LogDebug(string msg)
        {
            LogUtil.WriteLLog(logger.LogDebug, "-", "-", msg);
        }
    }
}

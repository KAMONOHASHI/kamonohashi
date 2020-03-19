using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Linq;

namespace Nssol.Platypus.Logic.HostedService
{
    /// <summary>
    /// テーブル NotebookHistories の生存期間を超えている実行中レコードと、それに対応する kubernetes 上の実コンテナを削除するタイマーです。
    /// </summary>
    public class DeleteNotebookContainerTimer : HostedServiceTimerBase
    {
        // DI で注入されるオブジェクト類
        private readonly INotebookHistoryRepository notebookHistoryRepository;
        private readonly IClusterManagementService clusterManagementService;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// kubernetes の token (環境変数、または launchSettings.json で設定)
        /// </summary>
        private readonly string kubernetesToken;

        /// <summary>
        /// コンストラクタで各 DI オブジェクトを引数で受け取ります。
        /// </summary>
        public DeleteNotebookContainerTimer(
            INotebookHistoryRepository notebookHistoryRepository,
            IClusterManagementService clusterManagementService,
            IUnitOfWork unitOfWork,
            IOptions<ContainerManageOptions> containerManageOptions,
            IOptions<DeleteNotebookContainerTimerOptions> deleteNotebookContainerTimerOptions,
            ILogger<DeleteNotebookContainerTimer> logger
            ) : base(logger, deleteNotebookContainerTimerOptions.Value)
        {
            // 各 DI オブジェクトの設定
            this.notebookHistoryRepository = notebookHistoryRepository;
            this.clusterManagementService = clusterManagementService;
            this.unitOfWork = unitOfWork;

            // kubernetes の token
            this.kubernetesToken = containerManageOptions.Value.ResourceManageKey;
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
            return ret;
        }

        /// <summary>
        /// テーブル NotebookHistories の生存期間を超えている実行中レコードと、対応する kubernetes 上の実コンテナを削除するメソッドです。
        /// </summary>
        protected override void DoWork(object state, int doWorkCount)
        {
            LogInfo($"テーブル NotebookHistories の生存期間を超えている実行中レコードと、対応する kubernetes 上の実コンテナを削除します。(第 {doWorkCount} 回目)");
            try
            {
                // テーブル NotebookHistories の'Killed'以外の全レコードを取得
                var notebookHistories = notebookHistoryRepository.GetAllIncludeTenantAsNoTrackingAsync().Result.Where(h => h.GetStatus().ToString() != ContainerStatus.Killed.Name);
                if (notebookHistories.Count() == 0)
                {
                    // レコードが1件も存在しなければ終了
                    LogInfo("テーブル NotebookHistories にレコードは1つも存在しませんでした。");
                    return;
                }
                // テーブル NotebookHistories の更新したレコード数をカウントする変数
                var dbUpdateCount = 0;
                // 削除した kubernetes 上の実コンテナ数をカウントする変数
                var deleteContainerCount = 0;
                foreach (NotebookHistory notebookHistory in notebookHistories)
                {
                    // 実コンテナのステータス情報を取得する
                    var newStatus = clusterManagementService.GetContainerStatusAsync(notebookHistory.Key, notebookHistory.Tenant.Name, kubernetesToken).Result;
                    if (notebookHistory.GetStatus().Key != newStatus.Key)
                    {
                        // ステータス更新があったので、変更処理
                        notebookHistoryRepository.UpdateStatusAsync(notebookHistory.Id, newStatus, true);
                    }

                    // 実行中コンテナか、生存期間が設定されているかチェック（生存期間が0の場合はコンテナ削除の対象にしない）
                    if (notebookHistory.GetStatus().Exist() && notebookHistory.ExpiresIn != 0)
                    {
                        // 経過時間を取得
                        long elapsedTicks = DateTime.Now.Ticks - (notebookHistory.StartedAt.HasValue ? notebookHistory.StartedAt.Value.Ticks : 0);
                        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                        // 生存期間を超えているか確認
                        if (elapsedSpan.TotalSeconds > notebookHistory.ExpiresIn)
                        {
                            // 生存期間を超えている場合、テーブル NotebookHistories のレコードのステータスをKilledに更新
                            notebookHistoryRepository.UpdateStatusAsync(notebookHistory.Id, ContainerStatus.Killed, DateTime.Now, true);
                            dbUpdateCount++;
                            try
                            {
                                // kubernetes 上の実コンテナを削除する
                                var destroyResult = clusterManagementService.DeleteContainerAsync(ContainerType.Notebook, notebookHistory.Key, notebookHistory.Tenant.Name, kubernetesToken).Result;
                                if (destroyResult)
                                {
                                    // 実際に削除対応したならカウントアップ
                                    deleteContainerCount++;
                                }
                            }
                            catch (Exception e)
                            {
                                // 何らかの例外をキャッチしたが ERROR ログを出力して処理を継続
                                LogError($"kubernetes 上の実コンテナ削除で例外をキャッチしましたが処理を継続します。 例外メッセージ=\"{e.Message}\"");
                            }
                        }
                    }
                }
                // テーブル NotebookHistories のステータス更新レコードをコミット
                unitOfWork.Commit();
                if (dbUpdateCount == deleteContainerCount)
                {
                    // テーブル NotebookHistories の更新レコード数と、対応する kubernetes 上の実コンテナ削除数が同じ(データ数が正常に一致)
                    LogInfo($"レコードを {dbUpdateCount} 件更新, 実コンテナを {deleteContainerCount} 個削除しました。");
                }
                else
                {
                    // 何らかの理由で更新数と削除数が一致していなかった場合 (致命的な問題ではないので WARN ログとする)
                    LogWarn($"レコードを {dbUpdateCount} 件更新, 実コンテナを {deleteContainerCount} 個削除しました。更新レコード数と削除個数に矛盾がありました。");
                }
            }
            catch (Exception e)
            {
                // DB 系での削除操作などで例外をキャッチしたが ERROR ログを出力して処理を継続
                LogError($"例外をキャッチしましたが処理を継続します。 例外メッセージ=\"{e.Message}\"");
            }
        }
    }
}

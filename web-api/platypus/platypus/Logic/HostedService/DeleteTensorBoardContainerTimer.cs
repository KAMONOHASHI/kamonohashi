using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Linq;

namespace Nssol.Platypus.Logic.HostedService
{
    /// <summary>
    /// テーブル TensorBoardContainers の全レコードと、それに対応する kubernetes 上の実コンテナを削除するタイマーです。
    /// </summary>
    public class DeleteTensorBoardContainerTimer : HostedServiceTimerBase
    {
        // DI で注入されるオブジェクト類
        private readonly ITensorBoardContainerRepository tensorBoardContainerRepository;
        private readonly IClusterManagementService clusterManagementService;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// kubernetes の token (環境変数、または launchSettings.json で設定)
        /// </summary>
        private readonly string kubernetesToken;

        /// <summary>
        /// コンストラクタで各 DI オブジェクトを引数で受け取ります。
        /// </summary>
        public DeleteTensorBoardContainerTimer(
            ITensorBoardContainerRepository tensorBoardContainerRepository,
            IClusterManagementService clusterManagementService,
            IUnitOfWork unitOfWork,
            IOptions<ContainerManageOptions> containerManageOptions,
            IOptions<DeleteTensorBoardContainerTimerOptions> deleteTensorBoardContainerTimerOptions,
            ILogger<DeleteTensorBoardContainerTimer> logger
            ) : base(logger, deleteTensorBoardContainerTimerOptions.Value)
        {
            // 各 DI オブジェクトの設定
            this.tensorBoardContainerRepository = tensorBoardContainerRepository;
            this.clusterManagementService = clusterManagementService;
            this.unitOfWork = unitOfWork;

            // kubernetes の token
            this.kubernetesToken = containerManageOptions.Value.ResourceManageKey;
        }

        /// <summary>
        /// タイマーとして各種データが設定されているかをチェックします。
        /// もし、false を返却したならタイマーが生成されません。
        /// </summary>
        protected override bool isValid()
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
        /// テーブル TensorBoardContainers の全レコードと、対応する kubernetes 上の実コンテナを削除するメソッドです。
        /// </summary>
        protected override void DoWork(object state, int doWorkCount)
        {
            LogInfo($"テーブル TensorBoardContainers の全レコードと、対応する kubernetes 上の実コンテナを削除します。(第 {doWorkCount} 回目)");
            try
            {
                // 削除対象のテーブル TensorBoardContainers の全レコードを取得
                var containers = tensorBoardContainerRepository.GetAllIncludePortAndTenantAsync().Result;
                var dbEntryCount = containers.Count();
                if (dbEntryCount == 0)
                {
                    // 削除対象が存在しなければ終了
                    LogInfo("削除対象のレコードは1つも存在しませんでした。");
                    return;
                }
                //  削除した kubernetes 上の実コンテナ数をカウントする変数
                var deleteContainerCount = 0;
                foreach (TensorBoardContainer container in containers)
                {
                    try
                    {
                        // kubernetes 上の実コンテナを削除
                        var destroyResult = clusterManagementService.DeleteContainerAsync(ContainerType.TensorBoard, container.Name, container.Tenant.Name, kubernetesToken).Result;
                        if (destroyResult)
                        {
                            // 実際に対応する削除したならカウントアップ
                            deleteContainerCount++;
                        }
                    }
                    catch (Exception e)
                    {
                        // 何らかの例外をキャッチしたが ERROR ログを出力して処理を継続
                        LogError($"kubernetes 上の実コンテナ削除で例外をキャッチしましたが処理を継続します。 例外メッセージ=\"{e.Message}\"");
                    }
                    // テーブル TensorBoardContainers のレコード削除
                    tensorBoardContainerRepository.Delete(container, true);
                }
                // テーブル TensorBoardContainers の全レコード削除をコミット
                unitOfWork.Commit();
                if (dbEntryCount == deleteContainerCount)
                {
                    // テーブル TensorBoardContainers の削除レコード数と、対応する kubernetes 上の実コンテナ削除数が同じ(データ数が正常に一致)
                    LogInfo($"レコードと実コンテナを {dbEntryCount} 削除しました。");
                }
                else
                {
                    // 何らかの理由で削除数が一致していなかった場合 (致命的な問題ではないので WARN ログとする)
                    LogWarn($"レコードを {dbEntryCount}, 実コンテナを {deleteContainerCount} 削除しました。削除個数に矛盾がありました。");
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

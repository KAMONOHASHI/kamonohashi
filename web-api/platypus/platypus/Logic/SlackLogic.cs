using Microsoft.Extensions.Options;
using Nssol.Platypus.ApiModels.AccountApiModels;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.ServiceModels.Webhook.SlackModels;
using Nssol.Platypus.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// Slack通知用ロジック
    /// </summary>
    public class SlackLogic : PlatypusLogicBase, ISlackLogic
    {
        private readonly ISlackService slackService;
        private readonly ContainerManageOptions containerOptions;

        // ステータスと色の対応づけ
        private readonly Dictionary<ContainerStatus, string> ColorDictionary
            = new Dictionary<ContainerStatus, string>
        {
            { ContainerStatus.Completed,    "#67C23A" },
            { ContainerStatus.Killed,       "#D00000" },
            { ContainerStatus.UserCanceled, "#E6A23C" }
        };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlackLogic(
            ICommonDiLogic commonDiLogic,
            ISlackService slackService,
            IOptions<ContainerManageOptions> containerOptions) : base(commonDiLogic)
        {
            this.slackService = slackService;
            this.containerOptions = containerOptions.Value;
        }

        /// <summary>
        /// 学習結果を通知する
        /// </summary>
        /// <param name="history">学習履歴モデル</param>
        public void InformJobResult(TrainingHistory history)
        {
            // SlackURLが設定されていないときは通知しない
            if (string.IsNullOrEmpty(CurrentUserInfo.SlackUrl))
            {
                return;
            }
            // KAMONOHASHIのホスト情報が登録されていないときは通知しない
            if (string.IsNullOrEmpty(containerOptions.WebEndPoint))
            {
                LogWarning("ホスト情報が未登録のため通知処理をスキップしました");
                return;
            }

            // 終了ステータスでないときは履歴削除として判定して処理する
            if (ColorDictionary.ContainsKey(history.GetStatus()))
            {
                // 終了ステータスのとき
                slackService.SendMessageAsync(new SendMessageInputModel()
                {
                    Id = history.Id,
                    Name = history.Name,
                    CreatedBy = history.CreatedBy,
                    MentionId = CurrentUserInfo.MentionId,
                    Status = history.GetStatus().Name,
                    Tenant = CurrentUserInfo.SelectedTenant,
                    Url = $"http://{containerOptions.WebEndPoint}/kamonohashi/#/training/{history.Id}?tenantId={CurrentUserInfo.SelectedTenant.Id}",
                    Title = "KAMONOHASHI 学習結果通知",
                    Color = ColorDictionary[history.GetStatus()],
                    WebhookUrl = CurrentUserInfo.SlackUrl
                });
            }
            else
            {
                // 終了ステータスでないとき(Pending, Running等)
                slackService.SendMessageAsync(new SendMessageInputModel()
                {
                    Id = history.Id,
                    Name = history.Name,
                    CreatedBy = history.CreatedBy,
                    MentionId = CurrentUserInfo.MentionId,
                    Tenant = CurrentUserInfo.SelectedTenant,
                    Url = $"http://{containerOptions.WebEndPoint}/kamonohashi/#/training?tenantId={CurrentUserInfo.SelectedTenant.Id}",
                    Title = "KAMONOHASHI 学習履歴削除通知",
                    Color = ColorDictionary[ContainerStatus.Killed],
                    WebhookUrl = CurrentUserInfo.SlackUrl,
                    Message = "ジョブ実行中に履歴情報が削除されました"
                });
            }
        }

        /// <summary>
        /// 推論結果を通知する
        /// </summary>
        /// <param name="history">推論履歴モデル</param>
        public void InformJobResult(InferenceHistory history)
        {
            // SlackURLが設定されていないときは通知しない
            if (string.IsNullOrEmpty(CurrentUserInfo.SlackUrl))
            {
                return;
            }
            // KAMONOHASHIのホスト情報が登録されていないときは通知しない
            if (string.IsNullOrEmpty(containerOptions.WebEndPoint))
            {
                LogWarning("ホスト情報が未登録のため通知処理をスキップしました");
                return;
            }

            // 終了ステータスでないときは履歴削除として判定して処理する
            if (ColorDictionary.ContainsKey(history.GetStatus()))
            {
                // 終了ステータスのとき
                slackService.SendMessageAsync(new SendMessageInputModel()
                {
                    Id = history.Id,
                    Name = history.Name,
                    CreatedBy = history.CreatedBy,
                    MentionId = CurrentUserInfo.MentionId,
                    Status = history.GetStatus().Name,
                    Tenant = CurrentUserInfo.SelectedTenant,
                    Url = $"http://{containerOptions.WebEndPoint}/kamonohashi/#/inference/{history.Id}?tenantId={CurrentUserInfo.SelectedTenant.Id}",
                    Title = "KAMONOHASHI 推論結果通知",
                    Color = ColorDictionary[history.GetStatus()],
                    WebhookUrl = CurrentUserInfo.SlackUrl
                });
            }
            else
            {
                // 終了ステータスでないとき(Pending, Running等)
                slackService.SendMessageAsync(new SendMessageInputModel()
                {
                    Id = history.Id,
                    Name = history.Name,
                    CreatedBy = history.CreatedBy,
                    MentionId = CurrentUserInfo.MentionId,
                    Tenant = CurrentUserInfo.SelectedTenant,
                    Url = $"http://{containerOptions.WebEndPoint}/kamonohashi/#/inference?tenantId={CurrentUserInfo.SelectedTenant.Id}",
                    Title = "KAMONOHASHI 推論履歴削除通知",
                    Color = ColorDictionary[ContainerStatus.Killed],
                    WebhookUrl = CurrentUserInfo.SlackUrl,
                    Message = "ジョブ実行中に履歴情報が削除されました"
                });
            }
        }

        /// <summary>
        /// テスト通知する
        /// </summary>
        /// <param name="model">Webhook情報モデル</param>
        public async Task<Result<string, string>> InformTest(WebhookModel model)
        {
            // SlackURLが設定されていないときは通知しない
            if (string.IsNullOrEmpty(model.SlackUrl))
            {
                return Result<string, string>.CreateErrorResult("URLが設定されていません");
            }

            // KAMONOHASHIのホスト情報が登録されていないときは通知しない
            if (string.IsNullOrEmpty(containerOptions.WebEndPoint))
            {
                return Result<string, string>.CreateErrorResult("KAMONOHASHIのホスト情報が登録されていないため、通知できませんでした");
            }

            // テスト通知を送信
            var result = await slackService.SendTestMessageAsync(new SendMessageInputModel()
            {
                CreatedBy = CurrentUserInfo.Name,
                Url = $"http://{containerOptions.WebEndPoint}/kamonohashi/#/",
                WebhookUrl = model.SlackUrl,
                MentionId = model.MentionId
            });

            if (result)
            {
                return Result<string, string>.CreateResult("メッセージの送信に成功しました");
            }
            else
            {
                return Result<string, string>.CreateErrorResult("メッセージの送信に失敗しました。入力されたURLに誤りがあります");
            }
        }
    }
}

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
    public class SlackLogic : PlatypusLogicBase, ISlackLogic
    {

        private readonly ISlackService slackService;
        private ContainerManageOptions containerOptions;

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

            // TODO containerOptionsWebEndPointが未設定のときは通知しない

            // SlackURLが設定されていないときは通知しない
            if (!string.IsNullOrEmpty(CurrentUserInfo.SlackUrl))
            {
                var mentionId = "";
                if (!string.IsNullOrEmpty(CurrentUserInfo.MentionId))
                {
                    mentionId = $"@{CurrentUserInfo.MentionId}";
                }
                slackService.SendMessageAsync(new SendMessageInputModel()
                {
                    Id = history.Id,
                    Name = history.Name,
                    CreatedBy = history.CreatedBy,
                    MentionId = mentionId,
                    Status = history.GetStatus().Name,
                    Tenant = CurrentUserInfo.SelectedTenant,
                    Url = $"http://{containerOptions.WebEndPoint}/kamonohashi/#/training/{history.Id}?tenantId={CurrentUserInfo.SelectedTenant.Id}",
                    Title = "KAMONOHASHI 学習結果通知",
                    Color = ColorDictionary[history.GetStatus()],
                    WebhookUrl = CurrentUserInfo.SlackUrl
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
            if (!string.IsNullOrEmpty(CurrentUserInfo.SlackUrl))
            {

                var mentionId = "";
                if (!string.IsNullOrEmpty(CurrentUserInfo.MentionId))
                {
                    mentionId = $"@{CurrentUserInfo.MentionId}";
                }
                slackService.SendMessageAsync(new SendMessageInputModel()
                {
                    Id = history.Id,
                    Name = history.Name,
                    CreatedBy = history.CreatedBy,
                    MentionId = mentionId,
                    Status = history.GetStatus().Name,
                    Tenant = CurrentUserInfo.SelectedTenant,
                    Url = $"http://{containerOptions.WebEndPoint}/kamonohashi/#/inference/{history.Id}?tenantId={CurrentUserInfo.SelectedTenant.Id}",
                    Title = "KAMONOHASHI 推論結果通知",
                    Color = ColorDictionary[history.GetStatus()],
                    WebhookUrl = CurrentUserInfo.SlackUrl
                });
            }
        }

        /// <summary>
        /// テスト通知する
        /// </summary>
        /// <param name="model">Webhook情報モデル</param>
        public async Task<bool> InformTest(WebhookModel model)
        {
            // SlackURLが設定されていないときは通知しない
            if (!string.IsNullOrEmpty(model.SlackUrl))
            {
                // KAMONOHASHIのホスト情報が登録されていないときも通知しない
                if (string.IsNullOrEmpty(containerOptions.WebEndPoint))
                {
                    return false;
                }

                return await slackService.sendTestMessageAsync(new SendMessageInputModel()
                {
                    CreatedBy = CurrentUserInfo.Name,
                    Url = $"http://{containerOptions.WebEndPoint}/kamonohashi/#",
                    WebhookUrl = model.SlackUrl,
                    MentionId = model.MentionId
                });
            }
            return false;
        }

        // ステータスと色の対応づけ
        private static readonly Dictionary<ContainerStatus, string> ColorDictionary = new Dictionary<ContainerStatus, string>
        {
            { ContainerStatus.Completed, "#67C23A" },
            { ContainerStatus.Killed, "#D00000"},
            { ContainerStatus.UserCanceled, "#E6A23C" }
        };
    }
}

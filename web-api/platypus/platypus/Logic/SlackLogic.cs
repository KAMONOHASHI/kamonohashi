using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.ServiceModels.Webhook.SlackModels;
using Nssol.Platypus.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.HostedService
{
    public class SlackLogic : PlatypusLogicBase, ISlackLogic
    {

        private readonly ISlackService slackService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlackLogic(ICommonDiLogic commonDiLogic, ISlackService slackService) : base(commonDiLogic)
        {
            this.slackService = slackService;
        }
        
        /// <summary>
        /// 学習結果を通知する
        /// </summary>
        public void InformJobResult(TrainingHistory history)
        {
            // SlackURLが設定されていないときは通知しない
            if (!string.IsNullOrEmpty(CurrentUserInfo.SelectedTenant.SlackUrl))
            {
                slackService.SendMessageAsync(new SendMessageInputModel()
                {
                    Id = history.Id,
                    Name = history.Name,
                    CreatedBy = history.CreatedBy,
                    Status = history.GetStatus().Name,
                    Tenant = CurrentUserInfo.SelectedTenant,
                    // TODO 環境によって変わるURLをどう処理するのか
                    Url = $"http://kamonohashi/#/training/{history.Id}?tenantId={CurrentUserInfo.SelectedTenant.Id}",
                    Title = "KAMONOHASHI 学習結果通知",
                    Color = ColorDictionary[history.GetStatus()],
                    WebhookUrl = CurrentUserInfo.SelectedTenant.SlackUrl
                });
            }
        }

        /// <summary>
        /// 推論結果を通知する
        /// </summary>
        public void InformJobResult(InferenceHistory history)
        {
            // SlackURLが設定されていないときは通知しない
            if (!string.IsNullOrEmpty(CurrentUserInfo.SelectedTenant.SlackUrl))
            {
                slackService.SendMessageAsync(new SendMessageInputModel()
                {
                    Id = history.Id,
                    Name = history.Name,
                    CreatedBy = history.CreatedBy,
                    Status = history.GetStatus().Name,
                    Tenant = CurrentUserInfo.SelectedTenant,
                    // TODO 環境によって変わるURLをどう処理するのか
                    Url = $"http://kamonohashi/#/inference/{history.Id}?tenantId={CurrentUserInfo.SelectedTenant.Id}",
                    Title = "KAMONOHASHI 推論結果通知",
                    Color = ColorDictionary[history.GetStatus()],
                    WebhookUrl = CurrentUserInfo.SelectedTenant.SlackUrl
                });
            }
        }

        /// <summary>
        /// テスト通知する
        /// </summary>
        public async Task<bool> InformTest(string url)
        {
            // SlackURLが設定されていないときは通知しない
            if (!string.IsNullOrEmpty(url))
            {
                return await slackService.sendTestMessageAsync(new SendMessageInputModel()
                {
                    CreatedBy = CurrentUserInfo.Name,
                    // TODO 環境によって変わるURLをどう処理するのか
                    Url = null,
                    WebhookUrl = url
                });
            }
            return false;
        }

        // ステータスと色の対応づけ
        private static Dictionary<ContainerStatus, string> ColorDictionary = new Dictionary<ContainerStatus, string>
        {
            { ContainerStatus.Completed, "#67C23A" },
            { ContainerStatus.Killed, "#D00000"},
            { ContainerStatus.UserCanceled, "#E6A23C" }
        };
    }
}

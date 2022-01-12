using System.Threading.Tasks;
using Nssol.Platypus.Services.Interfaces;
using Nssol.Platypus.ServiceModels.Webhook.SlackModels;
using System.Net.Http;

namespace Nssol.Platypus.Services
{
    public class SlackService : PlatypusServiceBase, ISlackService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlackService(
            Logic.Interfaces.ICommonDiLogic commonDiLogic) : base(commonDiLogic, @"ServiceModels/Webhook/SlackModels/Templates")
        {
        }

        /// <summary>
        /// Slackにメッセージを送信する
        /// </summary>
        /// <param name="model">メッセージ送信モデル</param>
        public async void SendMessageAsync(SendMessageInputModel model)
        {
            string body = await RenderEngine.CompileRenderAsync("send_message.json", new
            {
                Color = model.Color,
                Title = model.Title,
                Mention = model.MentionId,
                Id = model.Id,
                Name = model.Name,
                TenantName = model.Tenant.Name,
                UserName = model.CreatedBy,
                Status = model.Status,
                URL = model.Url
            });
            try
            {
                var response = await this.SendPostRequestAsync(new RequestParam()
                {
                    BaseUrl = model.BaseUrl,
                    ApiPath = model.ApiPath,
                    Body = body
                });
                if (!response.IsSuccess)
                {
                    LogWarning("メッセージ送信に失敗: " + response.Error);
                }
            }
            catch (HttpRequestException)
            {
                LogWarning("メッセージ送信に失敗: URLに誤りがあります");
            }
        }

        /// <summary>
        /// Slackにテスト通知を送信する
        /// </summary>
        /// <param name="model">メッセージ送信モデル</param>
        /// <returns>メッセージ送信可否</returns>
        public async Task<bool> sendTestMessageAsync(SendMessageInputModel model)
        {
            string body = await RenderEngine.CompileRenderAsync("test_message.json", new
            {
                UserName = model.CreatedBy,
                URL = model.Url
            });
            try
            {
                var response = await this.SendPostRequestAsync(new RequestParam()
                {
                    BaseUrl = model.BaseUrl,
                    ApiPath = model.ApiPath,
                    Body = body
                });
                if (response.IsSuccess)
                {
                    return true;
                }
                else
                {
                    LogWarning("メッセージ送信に失敗: " + response.Error);
                    return false;
                }
            }
            catch (HttpRequestException)
            {
                LogWarning("メッセージ送信に失敗: URLに誤りがあります");
                return false;
            }
        }
    }
}




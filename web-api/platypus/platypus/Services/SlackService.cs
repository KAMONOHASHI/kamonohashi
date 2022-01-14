using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.ServiceModels.Webhook.SlackModels;
using Nssol.Platypus.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services
{
    /// <summary>
    /// Slack通知用サービス
    /// </summary>
    public class SlackService : PlatypusServiceBase, ISlackService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlackService(
            ICommonDiLogic commonDiLogic)
            : base(commonDiLogic, @"ServiceModels/Webhook/SlackModels/Templates") { }

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
                URL = model.Url,
                Message = model.Message
            });

            try
            {
                var response = await SendPostRequestAsync(new RequestParam()
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
        public async Task<bool> SendTestMessageAsync(SendMessageInputModel model)
        {
            string body = await RenderEngine.CompileRenderAsync("test_message.json", new
            {
                UserName = model.CreatedBy,
                URL = model.Url,
                Mention = model.MentionId
            });

            try
            {
                var response = await SendPostRequestAsync(new RequestParam()
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




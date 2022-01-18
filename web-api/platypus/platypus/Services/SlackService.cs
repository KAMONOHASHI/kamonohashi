using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.ServiceModels.Webhook.SlackModels;
using Nssol.Platypus.Services.Interfaces;
using System;
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
                Mention = model.Mention,
                Id = model.Id,
                Name = model.Name,
                TenantName = model.Tenant.DisplayName,
                UserName = model.CreatedBy,
                Status = model.Status,
                Url = model.Url,
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
                // Slackへのメッセージ送信が成功したときは"ok"が返される
                if (!"ok".Equals(response.Value, StringComparison.CurrentCulture))
                {
                    LogWarning("メッセージ送信に失敗");
                }
            }
            catch (InvalidOperationException)
            {
                LogWarning("メッセージ送信に失敗: URLに誤りがあります");
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
                Url = model.Url,
                Mention = model.Mention
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
                    return false;
                }
                // Slackへのメッセージ送信が成功したときは"ok"が返される
                if (!"ok".Equals(response.Value, StringComparison.CurrentCulture))
                {
                    LogWarning("メッセージ送信に失敗");
                    return false;
                }

                return true;
            }
            catch (InvalidOperationException)
            {
                LogWarning("メッセージ送信に失敗: URLに誤りがあります");
                return false;
            }
            catch (HttpRequestException)
            {
                LogWarning("メッセージ送信に失敗: URLに誤りがあります");
                return false;
            }
        }
    }
}




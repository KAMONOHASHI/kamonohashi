using System.Threading.Tasks;
using Nssol.Platypus.ServiceModels.Webhook.SlackModels;

namespace Nssol.Platypus.Services.Interfaces
{
    public interface ISlackService
    {
        /// <summary>
        /// Slackにメッセージを送信する
        /// </summary>
        /// <param name="model">メッセージ送信モデル</param>
        void SendMessageAsync(SendMessageInputModel model);

        /// <summary>
        /// Slackにテスト通知を送信する
        /// </summary>
        /// <param name="model">メッセージ送信モデル</param>
        /// <returns>メッセージ送信可否</returns>
        Task<bool> sendTestMessageAsync(SendMessageInputModel model);
    }
}

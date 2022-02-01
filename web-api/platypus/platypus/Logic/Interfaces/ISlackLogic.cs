using Nssol.Platypus.ApiModels.AccountApiModels;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// Slack通知用ロジックインターフェース
    /// </summary>
    public interface ISlackLogic
    {
        /// <summary>
        /// 学習結果を通知する
        /// </summary>
        /// <param name="history">学習履歴モデル</param>
        void InformJobResult(TrainingHistory history);

        /// <summary>
        /// 推論結果を通知する
        /// </summary>
        /// <param name="history">推論履歴モデル</param>
        void InformJobResult(InferenceHistory history);

        /// <summary>
        /// テスト通知する
        /// </summary>
        /// <param name="model">Webhook情報モデル</param>
        Task<Result<string, string>> InformTest(WebhookModel model);
    }
}

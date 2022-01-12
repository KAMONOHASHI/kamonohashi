using Nssol.Platypus.ApiModels.AccountApiModels;
using Nssol.Platypus.Models.TenantModels;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
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
        Task<bool> InformTest(WebhookModel model);
    }
}

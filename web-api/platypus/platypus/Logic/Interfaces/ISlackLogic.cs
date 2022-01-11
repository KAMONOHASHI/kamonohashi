using Nssol.Platypus.ServiceModels.Webhook.SlackModels;
using Nssol.Platypus.Models.TenantModels;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    public interface ISlackLogic
    {
        /// <summary>
        /// 学習結果を通知する
        /// </summary>
        void InformJobResult(TrainingHistory history);

        /// <summary>
        /// 推論結果を通知する
        /// </summary>
        void InformJobResult(InferenceHistory history);

        /// <summary>
        /// テスト通知する
        /// </summary>
        Task<bool> InformTest(string url);
    }
}

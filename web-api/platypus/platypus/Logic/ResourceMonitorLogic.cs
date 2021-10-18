using Microsoft.Extensions.Options;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// リソースモニタロジッククラス
    /// </summary>
    public class ResourceMonitorLogic : PlatypusLogicBase, IResourceMonitorLogic
    {
        // DI で注入されるオブジェクト類
        private readonly IRepository<ResourceJob> resourceJobRepository;
        private readonly ResourceMonitorOptions resourceMonitorOptions;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ResourceMonitorLogic(
            ICommonDiLogic commonDiLogic,
            IRepository<ResourceJob> resourceJobRepository,
            IOptions<ResourceMonitorOptions> resourceMonitorOptions)
            : base(commonDiLogic)
        {
            this.resourceJobRepository = resourceJobRepository;
            this.resourceMonitorOptions = resourceMonitorOptions.Value;
        }

        /// <summary>
        /// ジョブ実行履歴を追加する
        /// </summary>
        /// <remarks>コミットは呼び出し側で行う</remarks>
        /// <param name="resourceJob">リソースモニタジョブ履歴</param>
        public void AddJobHistory(ResourceJob resourceJob)
        {
            if (resourceMonitorOptions.EnableJobHistory)
            {
                resourceJobRepository.Add(resourceJob);
            }
        }
    }
}

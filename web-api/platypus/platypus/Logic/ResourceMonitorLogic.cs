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
        private readonly IRepository<ResourceJob> resourceJobRepository;
        private readonly ResourceMonitorOptions resourceMonitorOptions;

        public ResourceMonitorLogic(
            ICommonDiLogic commonDiLogic,
            IRepository<ResourceJob> resourceJobRepository,
            IOptions<ResourceMonitorOptions> resourceMonitorOptions)
            : base(commonDiLogic)
        {
            this.resourceJobRepository = resourceJobRepository;
            this.resourceMonitorOptions = resourceMonitorOptions.Value;
        }

        public void AddJobHistory(ResourceJob resourceJob)
        {
            if (resourceMonitorOptions.EnableJobHistory)
            {
                resourceJobRepository.Add(resourceJob);
            }
        }
    }
}

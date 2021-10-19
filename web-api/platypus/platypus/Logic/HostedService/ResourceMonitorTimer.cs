using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.Logic.HostedService
{
    /// <summary>
    /// リソースモニタタイマー
    /// </summary>
    public class ResourceMonitorTimer : HostedServiceTimerBase
    {
        // DI で注入されるオブジェクト類
        private readonly IRepository<ResourceSample> resourceSampleRepository;
        private readonly IRepository<ResourceNode> resourceNodeRepository;
        private readonly IRepository<ResourceContainer> resourceContainerRepository;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly ITenantRepository tenantRepository;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// kubernetes の token (環境変数、または launchSettings.json で設定)
        /// </summary>
        private readonly string kubernetesToken;

        /// <summary>
        /// コンストラクタで各 DI オブジェクトを引数で受け取ります。
        /// </summary>
        public ResourceMonitorTimer(
            IRepository<ResourceSample> resourceSampleRepository,
            IRepository<ResourceNode> resourceNodeRepository,
            IRepository<ResourceContainer> resourceContainerRepository,
            IClusterManagementLogic clusterManagementLogic,
            ITenantRepository tenantRepository,
            IUnitOfWork unitOfWork,
            IOptions<ContainerManageOptions> containerManageOptions,
            IOptions<ResourceMonitorTimerOptions> resourceMonitorTimerOptions,
            ILogger<ResourceMonitorTimer> logger
            ) : base(logger, resourceMonitorTimerOptions.Value)
        {
            // 各 DI オブジェクトの設定
            this.resourceSampleRepository = resourceSampleRepository;
            this.resourceNodeRepository = resourceNodeRepository;
            this.resourceContainerRepository = resourceContainerRepository;
            this.clusterManagementLogic = clusterManagementLogic;
            this.tenantRepository = tenantRepository;
            this.unitOfWork = unitOfWork;

            // kubernetes の token
            this.kubernetesToken = containerManageOptions.Value.ResourceManageKey;
        }

        /// <summary>
        /// タイマーとして各種データが設定されているかをチェックします。
        /// もし、false を返却したならタイマーが生成されません。
        /// </summary>
        protected override bool IsValid()
        {
            bool ret = true;
            if (string.IsNullOrEmpty(kubernetesToken))
            {
                LogError("kubernetes のトークンが設定されていません。");
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// クラスタ上に登録されているノードおよびコンテナのリソース情報をDBに登録する。
        /// </summary>
        /// <param name="doWorkCount">実行回数</param>
        protected override async void DoWork(object state, int doWorkCount)
        {
            LogInfo($"Resource monitor {doWorkCount}");
            try
            {
                var resourceSample = new ResourceSample
                {
                    SampledAt = DateTime.Now,
                };
                resourceSampleRepository.Add(resourceSample);

                // nodeName:ResourceNode
                var nodeDictionary = new Dictionary<string, ResourceNode>();
                foreach (var x in await clusterManagementLogic.GetAllNodesAsync())
                {
                    nodeDictionary[x.Name] = new ResourceNode
                    {
                        Name = x.Name,
                        Cpu = (int)x.Cpu,
                        Memory = (int)x.Memory,
                        Gpu = x.Gpu,
                        ResourceSample = resourceSample,
                    }; 
                }
                nodeDictionary[""] = new ResourceNode
                {
                    Name = "",
                    Cpu = 0,
                    Memory = 0,
                    Gpu = 0,
                    ResourceSample = resourceSample,
                }; 
                resourceNodeRepository.AddRange(nodeDictionary.Values.AsQueryable());

                var containers = await clusterManagementLogic.GetAllContainerDetailsInfosAsync();
                if (containers.IsSuccess)
                {
                    foreach (var x in containers.Value)
                    {
                        if (nodeDictionary.TryGetValue(x.NodeName ?? "", out ResourceNode resourceNode))
                        {
                            var tenant = tenantRepository.GetFromTenantName(x.TenantName);
                            if (tenant != null)
                            {
                                resourceContainerRepository.Add(new ResourceContainer
                                {
                                    TenantId = tenant.Id,
                                    TenantName = tenant.Name,
                                    Name = x.Name,
                                    Cpu = (int)x.Cpu,
                                    Memory = (int)x.Memory,
                                    Gpu = x.Gpu,
                                    Status = x.Status.Key,
                                    ResourceNode = resourceNode,
                                });
                            }
                        }
                    }
                }

                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                // DB 系で例外をキャッチしたが ERROR ログを出力して処理を継続
                LogError($"例外をキャッチしましたが処理を継続します。 例外メッセージ=\"{e.Message}\"");
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels.ClusterManagementModels;
using Nssol.Platypus.Services;
using Nssol.Platypus.Services.Interfaces;
using System;

namespace Nssol.Platypus.Logic.HostedService
{
    /// <summary>
    /// DB のテナント・レジストリ・ノード情報を Cluster(k8s) へ同期させるタイマーです。
    /// </summary>
    public class SyncClusterFromDBTimer : HostedServiceTimerBase
    {
        // DI で注入されるオブジェクト類
        private readonly ITenantRepository tenantRepository;
        private readonly IRegistryRepository registryRepository;
        private readonly INodeRepository nodeRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IClusterManagementService clusterManagementService;
        private readonly GitLabRegistryService gitLabRegistryService;
        private readonly DockerHubRegistryService dockerHubRegistryService;
        private readonly PrivateDockerRegistryService privateDockerRegistryService;
        private readonly ContainerManageOptions containerManageOptions;

        /// <summary>
        /// コンストラクタで各 DI オブジェクトを引数で受け取ります。
        /// </summary>
        public SyncClusterFromDBTimer(
            ITenantRepository tenantRepository,
            IRegistryRepository registryRepository,
            INodeRepository nodeRepository,
            IUnitOfWork unitOfWork,
            IClusterManagementService clusterManagementService,
            GitLabRegistryService gitLabRegistryService,
            DockerHubRegistryService dockerHubRegistryService,
            PrivateDockerRegistryService privateDockerRegistryService,
            IOptions<ContainerManageOptions> containerManageOptions,
            IOptions<SyncClusterFromDBOptions> SyncClusterFromDBOptions,
            ILogger<SyncClusterFromDBTimer> logger
            ) : base(logger, SyncClusterFromDBOptions.Value)
        {
            // 各 DI オブジェクトの設定
            this.tenantRepository = tenantRepository;
            this.registryRepository = registryRepository;
            this.nodeRepository = nodeRepository;
            this.unitOfWork = unitOfWork;
            this.clusterManagementService = clusterManagementService;
            this.gitLabRegistryService = gitLabRegistryService;
            this.dockerHubRegistryService = dockerHubRegistryService;
            this.privateDockerRegistryService = privateDockerRegistryService;
            this.containerManageOptions = containerManageOptions.Value;
        }

        /// <summary>
        /// タイマーとして各種データが設定されているかをチェックします。
        /// もし、false を返却したならタイマーが生成されません。
        /// </summary>
        protected override bool isValid()
        {
            bool ret = true;
            if (string.IsNullOrEmpty(containerManageOptions.ContainerLabelPartition))
            {
                LogError("ContainerManageOptions の ContainerLabelPartition が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(containerManageOptions.ContainerLabelTensorBoardEnabled))
            {
                LogError("ContainerManageOptions の ContainerLabelTensorBoardEnabled が設定されていません。");
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// DB のテナント・レジストリ・ノード情報を Cluster(k8s) へ同期させるメソッドです。
        /// </summary>
        protected override void DoWork(object state, int doWorkCount)
        {
            LogInfo($"DB のテナント・レジストリ・ノード情報を Cluster(k8s) へ同期させます。");
            try
            {
                // DB のテナント情報に対応する名前空間、ロール、クォータを Cluster(k8s) へ同期
                var tenants = tenantRepository.GetAllTenants();
                foreach (Tenant tenant in tenants)
                {
                    // 名前空間とロールを同期
                    bool ret = clusterManagementService.RegistTenantAsync(tenant.Name).Result;
                    if (ret)
                    {
                        LogDebug($"DB のテナント \"{tenant.Name}\" に対応する名前空間とロールを Cluster(k8s) へ同期させました。");
                    }
                    else
                    {
                        LogError($"DB のテナント \"{tenant.Name}\" に対応する名前空間とロールを Cluster(k8s) へ同期させる処理に失敗しました。");
                    }
                    // クォータを同期
                    int cpu = tenant.LimitCpu == null ? 0 : tenant.LimitCpu.Value;
                    int memory = tenant.LimitMemory == null ? 0 : tenant.LimitMemory.Value;
                    int gpu = tenant.LimitGpu == null ? 0 : tenant.LimitGpu.Value;
                    ret = clusterManagementService.SetQuotaAsync(tenant.Name, cpu, memory, gpu).Result;
                    string quotaInfo = $"cpu={cpu} memory={memory} gpu={gpu}";
                    if (ret)
                    {
                        LogDebug($"DB のテナント \"{tenant.Name}\" に対応するクォータ [{quotaInfo}] を Cluster(k8s) へ同期させました。");
                    }
                    if (!ret)
                    {
                        LogError($"DB のテナント \"{tenant.Name}\" に対応するクォータ [{quotaInfo}] を Cluster(k8s) へ同期させる処理に失敗しました。");
                    }
                    // テナントの古い ClusterToken を削除
                    tenantRepository.DeleteClusterToken(tenant.Id);
                }
                // テナントの古い ClusterToken 削除を確定する
                unitOfWork.Commit();

                // DB のレジストリ情報(UserTenantRegistryMap)に対応するシークレットを Cluster(k8s) へ同期
                var userTenantRegistryMaps = registryRepository.GetUserTenantRegistryMapAll();
                foreach (UserTenantRegistryMap userTenantRegistryMap in userTenantRegistryMaps)
                {
                    // ログ情報
                    string registryPasswd = string.IsNullOrEmpty(userTenantRegistryMap.RegistryPassword) ? "無し" : "有り";
                    string mapInfo = $"UserTenantRegistryMapId={userTenantRegistryMap.Id}, UserId={userTenantRegistryMap.UserId}, " +
                        $"RegistryPassword={registryPasswd}, TenantRegistryMapId={userTenantRegistryMap.TenantRegistryMapId}, " +
                        $"TernantId={userTenantRegistryMap.TenantRegistryMap.TenantId}, " +
                        $"RegistryId={userTenantRegistryMap.TenantRegistryMap.RegistryId}";
                    Registry registry = userTenantRegistryMap.Registry;
                    if (registry == null)
                    {
                        // Registry が null というのはあり得ないが、取り敢えずはチェック
                        LogDebug($"DB のレジストリ情報 [{mapInfo}] に対応する Registry が存在しません。");
                        continue;
                    }
                    mapInfo += $", RegistryServiceType={registry.ServiceType}";

                    // テナントの取得
                    Tenant tenant = tenantRepository.Get(userTenantRegistryMap.TenantRegistryMap.TenantId);
                    if (tenant == null)
                    {
                        // テナントが null というのはあり得ないが、取り敢えずはチェック
                        LogError($"DB のレジストリ情報 [{ mapInfo}] に対応するテナントが存在しません。");
                        continue;
                    }
                    // RegistryService の取得
                    IRegistryService registryService = getRegistryService(registry);
                    if (registryService == null)
                    {
                        LogError($"DB のレジストリ情報 [{mapInfo}] に対応する RegistryService を取得できませんでした。");
                        continue;
                    }

                    // パスワードが空なら同期させない
                    if (string.IsNullOrEmpty(userTenantRegistryMap.RegistryPassword))
                    {
                        LogDebug($"DB のレジストリ情報 [{mapInfo}] のレジストリ・パスワードが空なので Cluster(k8s) への同期は行いません。");
                        continue;
                    }

                    // Docker コンフィグの取得
                    string dockerCfg = registryService.GetDockerCfgAuthString(userTenantRegistryMap);
                    if (dockerCfg == null)
                    {
                        LogError($"DB のレジストリ情報 [{ mapInfo}] に同期させる Docker コンフィグを取得できませんでした。");
                        continue;
                    }
                    // シークレット情報の生成
                    var inModel = new RegistRegistryTokenInputModel()
                    {
                        TenantName = tenant.Name,
                        RegistryTokenKey = userTenantRegistryMap.RegistryTokenKey,
                        DockerCfgAuthString = dockerCfg,
                        Url = userTenantRegistryMap.Registry.RegistryUrl
                    };

                    // Cluster(k8s) にシークレットを同期
                    bool ret = clusterManagementService.RegistRegistryTokenyAsync(inModel).Result;
                    if (ret)
                    {
                        LogDebug($"DB のレジストリ情報 [{mapInfo}] に対応するシークレットを Cluster(k8s) へ同期させました。");
                    }
                    else
                    {
                        LogError($"DB のレジストリ情報 [{mapInfo}] に対応するシークレットを Cluster(k8s) へ同期させる処理に失敗しました。");
                    }
                }

                // DB のノード情報に対応するパーティションを Cluster(k8s) へ同期
                var nodes = nodeRepository.GetAll();
                foreach (Node node in nodes)
                {
                    bool ret = clusterManagementService.SetNodeLabelAsync(node.Name, containerManageOptions.ContainerLabelPartition, node.Partition).Result;
                    if (!ret)
                    {
                        LogError($"DB のノード情報 [{node.Name}] に対応するパーティションを Cluster(k8s) へ同期させる処理に失敗しました。");
                        continue;
                    }
                    string tensorBoardEnabledStr = node.TensorBoardEnabled ? "true" : "";
                    ret = clusterManagementService.SetNodeLabelAsync(node.Name, containerManageOptions.ContainerLabelTensorBoardEnabled, tensorBoardEnabledStr).Result;
                    if (ret)
                    {
                        LogDebug($"DB のノード情報 [{node.Name}] に対応するパーティションと TensorBoard 可否設定を Cluster(k8s) へ同期させました。");
                    }
                    else
                    {
                        LogError($"DB のノード情報 [{node.Name}] に対応する TensorBoard 可否設定を Cluster(k8s) へ同期させる処理に失敗しました。");
                    }
                    
                }
                LogInfo("DB のテナント・レジストリ・ノード情報を Cluster(k8s) へ同期させる処理は終了しました。");
            }
            catch (Exception e)
            {
                //例外をキャッチしたが ERROR ログを出力して処理を継続
                LogError($"DB のテナント・レジストリ・ノード情報を Cluster(k8s) へ同期させる時に例外をキャッチしましたが web-api は継続処理します。 例外メッセージ=\"{e.Message}\"");
            }
        }

        /// <summary>
        /// Registry の ServiceType に応じた IRegistryService の実オブジェクトを返却します。
        /// 本来は <see cref="CommonDiLogic.DynamicDi{T}"/> を通して返却する予定だったが、
        /// Controller 経由で DI していないので null を返却してしまうので代用のメソッドを作成した。
        /// </summary>
        private IRegistryService getRegistryService(Registry registry)
        {
            if (registry.ServiceType == RegistryServiceType.GitLab)
            {
                return gitLabRegistryService;
            }
            else if (registry.ServiceType == RegistryServiceType.DockerHub)
            {
                return dockerHubRegistryService;
            }
            else if (registry.ServiceType == RegistryServiceType.PrivateDockerRegistry)
            {
                return privateDockerRegistryService;
            }
            // 将来的に RegistryServiceType が増えたら追加実装すること。
            return null;
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nssol.Platypus.ApiModels.ResourceApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// リソース管理を扱うためのAPI集
    /// </summary>
    [ApiController]
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/admin/resource")]
    public class ResourceController : PlatypusApiControllerBase
    {
        // for DI
        private readonly ITenantRepository tenantRepository;
        private readonly IUserRepository userRepository;
        private readonly IRepository<ResourceSample> resourceSampleRepository;
        private readonly IRepository<ResourceContainer> resourceContainerRepository;
        private readonly IRepository<ResourceJob> resourceJobRepository;
        private readonly ICommonDiLogic commonDiLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IUnitOfWork unitOfWork;
        private readonly ContainerManageOptions containerManageOptions;

        public ResourceController(
            ITenantRepository tenantRepository,
            IUserRepository userRepository,
            IRepository<ResourceSample> resourceSampleRepository,
            IRepository<ResourceContainer> resourceContainerRepository,
            IRepository<ResourceJob> resourceJobRepository,
            ICommonDiLogic commonDiLogic,
            IClusterManagementLogic clusterManagementLogic,
            IUnitOfWork unitOfWork,
            IOptions<ContainerManageOptions> containerManageOptions,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.tenantRepository = tenantRepository;
            this.userRepository = userRepository;
            this.resourceSampleRepository = resourceSampleRepository;
            this.resourceContainerRepository = resourceContainerRepository;
            this.resourceJobRepository = resourceJobRepository;
            this.commonDiLogic = commonDiLogic;
            this.clusterManagementLogic = clusterManagementLogic;
            this.unitOfWork = unitOfWork;
            this.containerManageOptions = containerManageOptions.Value;
        }

        /// <summary>
        /// ノード単位のリソースデータを取得する
        /// </summary>
        /// <param name="nodeRepository">DI用</param>
        /// <returns>リソースデータ</returns>
        [HttpGet("nodes")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<NodeResourceOutputModel>))]
        public async Task<IActionResult> GetResourceByNode([FromServices] INodeRepository nodeRepository)
        {
            var nodes = nodeRepository.GetAll().OrderBy(n => n.Name);
            var nodeInfos = (await clusterManagementLogic.GetAllNodesAsync())?.ToList(); //Removeできるように、Listにしておく
            if(nodeInfos == null)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, string.Join("\n", "Fetching nodes is failed."));
            }
            var result = new Dictionary<string, NodeResourceOutputModel>();

            //DBから探索
            foreach(var node in nodes)
            {
                var info = nodeInfos.FirstOrDefault(i => i.Name == node.Name);
                if(info == null)
                {
                    result.Add(node.Name, new NodeResourceOutputModel(node));
                }
                else
                {
                    result.Add(node.Name, new NodeResourceOutputModel(node, info));
                    nodeInfos.Remove(info);
                }
            }
            //k8sにしかないノードを追加
            foreach(var info in nodeInfos)
            {
                result.Add(info.Name, new NodeResourceOutputModel(info));
            }

            var response = await clusterManagementLogic.GetAllContainerDetailsInfosAsync();
            if (response.IsSuccess)
            {
                foreach(var container in response.Value)
                {
                    if(string.IsNullOrEmpty(container.NodeName))
                    {
                        //ノード名がNULL＝まだ未割当
                        if (result.ContainsKey("*Unassigned*") == false)
                        {
                            result.Add("*Unassigned*", new NodeResourceOutputModel());
                        }
                        result["*Unassigned*"].Add(CreateContainerDetailsOutputModel(container));
                    }
                    else if(result.ContainsKey(container.NodeName))
                    {
                        result[container.NodeName].Add(CreateContainerDetailsOutputModel(container));
                    }
                    else
                    {
                        //nodeInfoから必要な情報を取って、結果に含める
                        var nodeInfo = nodeInfos.Find(n => n.Name == container.NodeName);
                        if(nodeInfo == null)
                        {
                            //登録ノードに所属していないコンテナは無視
                            continue;
                        }
                        var model = new NodeResourceOutputModel(nodeInfo);
                        model.Add(CreateContainerDetailsOutputModel(container));
                        result.Add(container.NodeName, model);
                    }
                }

                foreach (var node in result.Values)
                {
                    // テナント名（表示名）を昇順で並び替える
                    node.ContainerResourceList = node.ContainerResourceList.OrderBy(c => c.TenantName).ToList();
                }


                return JsonOK(result.Values);
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, string.Join("\n", "Fetching container resource is failed.", response.Error));
            }
        }

        /// <summary>
        /// テナント単位のリソースデータを取得する
        /// </summary>
        /// <param name="nodeRepository">DI用</param>
        /// <returns>リソースデータ</returns>
        [HttpGet("tenants")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<TenantResourceOutputModel>))]
        public async Task<IActionResult> GetResourceByTenant([FromServices] INodeRepository nodeRepository)
        {
            var nodes = nodeRepository.GetAll().OrderBy(n => n.Name);
            var result = tenantRepository.GetAllTenants().OrderBy(t => t.DisplayName).ToDictionary(t => t.Name, t => new TenantResourceOutputModel(t));
            
            var response = await clusterManagementLogic.GetAllContainerDetailsInfosAsync();
            if (response.IsSuccess)
            {
                foreach (var container in response.Value)
                {
                    //nodeInfoから必要な情報を取って、結果に含める
                    var nodeInfo = nodes.FirstOrDefault(n => n.Name == container.NodeName);
                    if (nodeInfo == null)
                    {
                        //登録ノードに所属していないコンテナは無視
                        continue;
                    }

                    if (result.ContainsKey(container.TenantName))
                    {
                        //テナント単位で集計する場合、テナントIDは自明なので、CreateContainerDetailsOutputModel()は使わない
                        result[container.TenantName].Add(new ContainerDetailsOutputModel(container)
                        {
                            CreatedBy = userRepository.GetUserName(container.CreatedBy)
                        });
                    }
                    else
                    {
                        //正体不明のテナントに紐づいたコンテナ

                        var unknownModel = new TenantResourceOutputModel(container.TenantName, containerManageOptions);
                        unknownModel.Add(new ContainerDetailsOutputModel(container)
                        {
                            CreatedBy = userRepository.GetUserName(container.CreatedBy)
                        });
                        result.Add(container.TenantName, unknownModel);
                    }
                }
                
                foreach (var tenant in result.Values)
                {
                    // ノード名の昇順に並び替える
                    tenant.ContainerResourceList = tenant.ContainerResourceList.OrderBy(c => c.NodeName).ToList();
                }
                return JsonOK(result.Values);
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, string.Join("\n", "Fetching container resource is failed.", response.Error));
            }
        }

        /// <summary>
        /// 起動中のコンテナ一覧を取得する
        /// </summary>
        /// <param name="nodeRepository">DI用</param>
        /// <returns>リソースデータ</returns>
        [HttpGet("containers")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ContainerDetailsOutputModel>))]
        public async Task<IActionResult> GetResourceByContainer([FromServices] INodeRepository nodeRepository)
        {
            var nodes = nodeRepository.GetAll().OrderBy(n => n.Name);
            var result = await clusterManagementLogic.GetAllContainerDetailsInfosAsync();
            if (result.IsSuccess)
            {
                // 登録ノードに所属していないコンテナは無視し、ノード名（昇順）、テナント名（昇順）で並び替える
                return JsonOK(result.Value
                    .Select(info => CreateContainerDetailsOutputModel(info)).OrderBy(c => c.NodeName).ThenBy(c => c.TenantName)
                    .Where(info => nodes.FirstOrDefault(n => n.Name == info.NodeName) != null));
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, string.Join("\n", "Fetching container resource is failed.", result.Error));
            }
        }

        /// <summary>
        /// <see cref="ContainerDetailsOutputModel.TenantId"/>をセットする
        /// </summary>
        private ContainerDetailsOutputModel CreateContainerDetailsOutputModel(ContainerDetailsInfo info)
        {
            var model = new ContainerDetailsOutputModel(info)
            {
                CreatedBy = userRepository.GetUserName(info.CreatedBy)
            };
            var tenant = tenantRepository.GetFromTenantName(info.TenantName);
            if(tenant == null)
            {
                if (info.TenantName == containerManageOptions.KqiAdminNamespace)
                {
                    // KqiAdminNamespace の場合、KQI管理者用とする。
                    model.TenantName = containerManageOptions.KqiAdminNamespace;
                    model.TenantId = 0;
                }else if (info.TenantName.StartsWith(containerManageOptions.KqiNamespacePrefix))
                {
                    // KqiNamespacePrefix で始まるテナント名の場合、kqi-systemとする
                    model.TenantName = "kqi-system";
                    model.TenantId = -1;
                }
                else
                {
                    //知らないテナントのコンテナが起動している
                    LogError($"There is a container for the unknown tenant {model.TenantName}");
                    model.TenantName = "Unknown:" + model.TenantName;
                    model.TenantId = -1;
                }
            }
            else
            {
                model.TenantId = tenant.Id;
                model.TenantName = tenant.DisplayName;
            }
            return model;
        }

        /// <summary>
        /// コンテナ種別一覧を取得
        /// </summary>
        /// <returns>コンテナ種別一覧</returns>
        [HttpGet("container-types")]
        [Filters.PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType(typeof(IEnumerable<EnumInfo>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllTypes()
        {
            var containerTypes = Enum.GetValues(typeof(ContainerType)) as ContainerType[];

            return JsonOK(containerTypes.Select(c => new EnumInfo() { Id = (int)c, Name = c.ToString() }));
        }

        /// <summary>
        /// 指定コンテナのリソースデータを取得する
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        /// <param name="name">コンテナ名</param>
        /// <returns>リソースデータ</returns>
        [HttpGet("containers/{tenantId}/{name}")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ContainerDetailsOutputModel))]
        public async Task<IActionResult> GetResourceByContainerName([FromRoute] long tenantId, [FromRoute] string name)
        {
            //入力チェック
            if (string.IsNullOrWhiteSpace(name))
            {
                return JsonBadRequest("Name is required.");
            }
            //データの存在チェック
            var tenant = GetTenant(tenantId);
            if (tenant == null)
            {
                return JsonNotFound($"Tenant ID {tenantId} is not found.");
            }

            var info = await clusterManagementLogic.GetContainerDetailsInfoAsync(name, tenant.Name, true);
            if(info.Status == ContainerStatus.None)
            {
                return JsonNotFound($"Container named {name} is not found.");
            }
            var result = new ContainerDetailsOutputModel(info)
            {
                TenantId = tenant.Id,
                TenantName = tenant.Name,
                DisplayName = tenant.DisplayName,
                ContainerType = CheckContainerType(name, true).Item1, //コンテナの種別を確認
                CreatedBy = userRepository.GetUserName(info.CreatedBy)
            };

            return JsonOK(result);
        }

        /// <summary>
        /// 指定コンテナのログを取得する
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        /// <param name="name">コンテナ名</param>
        /// <returns>リソースデータ</returns>
        [HttpGet("containers/{tenantId}/{name}/log")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType(typeof(System.IO.Stream), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLogByContainerName([FromRoute] long tenantId, [FromRoute] string name)
        {
            //入力チェック
            if (string.IsNullOrWhiteSpace(name))
            {
                return JsonBadRequest("Name is required.");
            }
            //データの存在チェック
            var tenant = GetTenant(tenantId);
            if (tenant == null)
            {
                return JsonNotFound($"Tenant ID {tenantId} is not found.");
            }

            var fileContents = await clusterManagementLogic.DownloadLogAsync(name, tenant.Name, true);

            if (fileContents.IsSuccess == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to get container log: {fileContents.Error}");
            }
            else
            {
                return File(fileContents.Value, "text/plain", $"{tenant.Name}_{name}_log");
            }
        }

        /// <summary>
        /// 指定コンテナのイベントを取得する
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        /// <param name="name">コンテナ名</param>
        /// <returns>リソースデータ</returns>
        [HttpGet("containers/{tenantId}/{name}/events")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType(typeof(System.IO.Stream), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetEventsByContainerName([FromRoute] long tenantId, [FromRoute] string name)
        {
            //入力チェック
            if (string.IsNullOrWhiteSpace(name))
            {
                return JsonBadRequest("Name is required.");
            }
            //データの存在チェック
            var tenant = GetTenant(tenantId);
            if (tenant == null)
            {
                return JsonNotFound($"Tenant ID {tenantId} is not found.");
            }

            var events = await clusterManagementLogic.GetEventsAsync(tenant, name, true, true);

            if (events.IsSuccess == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to get container events: {events.Error}");
            }
            else
            {
                return JsonOK(events.Value);
            }
        }

        /// <summary>
        /// 指定コンテナを削除する
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        /// <param name="name">コンテナ名</param>
        [HttpDelete("containers/{tenantId}/{name}")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteResourceByContainerId([FromRoute] long tenantId, [FromRoute] string name)
        {
            // データの存在チェック
            var tenant = GetTenant(tenantId);
            if (tenant == null)
            {
                return JsonNotFound($"Tenant ID {tenantId} is not found.");
            }

            return await DeleteContainerAsync(tenant, name, true);
        }

        /// <summary>
        /// 指定されたIDからテナントを取得する。
        /// ただし、IDが "0" の場合は、KQI管理者用テナントを返す。
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        /// <returns>テナント情報</returns>
        private Tenant GetTenant(long tenantId)
        {
            if (tenantId == 0)
            {
                // IDが "0" の場合、KQI管理者用とする。(テナント実体は存在しないが、k8sの名前空間として使用している)
                return new Tenant()
                {
                    Id = tenantId,
                    Name = containerManageOptions.KqiAdminNamespace,
                    DisplayName = containerManageOptions.KqiAdminNamespace
                };
            }
            else
            {
                return tenantRepository.Get(tenantId);
            }
        }

        /// <summary>
        /// コンテナ名からコンテナ種別と対応するインスタンスを取得する
        /// </summary>
        /// <param name="containerName">コンテナ名</param>
        /// <param name="force">Admin権限で実行するか</param>
        /// <returns>コンテナ種別と対応するインスタンス</returns>
        private Tuple<ContainerType, TenantModelBase> CheckContainerType(string containerName, bool force)
        {
            //今はprefixだけで判断する
            if (containerName.StartsWith("tensorboard"))
            {
                //TensorBoardコンテナ
                var container = commonDiLogic.DynamicDi<ITensorBoardContainerRepository>().Find(t => t.Name == containerName, force);
                if (container == null)
                {
                    // 存在しないハズのTensorBoardコンテナが生き残っている
                    LogWarning($"Find unknown TensorBoard container: {containerName}");
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Unknown, null);
                }
                else
                {
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.TensorBoard, container);
                }
            }
            else if (containerName.StartsWith("preproc"))
            {
                //前処理は preproc-{ID} というルールになってるので、そこからIDを取得する
                long id = int.Parse(containerName.Substring(containerName.IndexOf('-') + 1));

                //前処理コンテナ
                var container = commonDiLogic.DynamicDi<IPreprocessHistoryRepository>().GetPreprocessHistoryIncludeDataAndPreprocess(id, force);

                if(container == null)
                {
                    //前処理が存在しない＝DB上では前処理削除に成功したんだけど、コンテナの削除には何かの原因で失敗した状態
                    // 存在しないハズのコンテナが生き残っている
                    LogWarning($"Find unknown preprocessing container: {containerName}");
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Unknown, null);
                }

                return new Tuple<ContainerType, TenantModelBase>(ContainerType.Preprocessing, container);
            }
            else if (containerName.StartsWith("notebook"))
            {
                //ノートブックコンテナ
                var container = commonDiLogic.DynamicDi<INotebookHistoryRepository>().Find(t => t.Key == containerName, force);
                if (container == null)
                {
                    // 存在しないハズのコンテナが生き残っている
                    LogWarning($"Find unknown container: {containerName}");
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Unknown, null);
                }
                else if (container.GetStatus().Exist() == false)
                {
                    // 既に終了しているハズのジョブのコンテナ＝何かの理由でコンテナだけ消せなかった
                    // ジョブ側には何の影響も与えたくないので、未知のコンテナとして削除する
                    LogWarning($"Find exited container: {containerName}");
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Unknown, null);
                }
                else
                {
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Notebook, container);
                }
            }
            else if (containerName.StartsWith("inference"))
            {
                //推論コンテナ
                var container = commonDiLogic.DynamicDi<IInferenceHistoryRepository>().Find(t => t.Key == containerName, force);
                if (container == null)
                {
                    // 存在しないハズのコンテナが生き残っている
                    LogWarning($"Find unknown container: {containerName}");
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Unknown, null);
                }
                else if (container.GetStatus().Exist() == false)
                {
                    // 既に終了しているハズのジョブのコンテナ＝何かの理由でコンテナだけ消せなかった
                    // ジョブ側には何の影響も与えたくないので、未知のコンテナとして削除する
                    LogWarning($"Find exited container: {containerName}");
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Unknown, null);
                }
                else
                {
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Inferencing, container);
                }
            }
            else if (containerName.StartsWith("training"))
            {
                //学習コンテナ
                var container = commonDiLogic.DynamicDi<ITrainingHistoryRepository>().Find(t => t.Key == containerName, force);
                if (container == null)
                {
                    // 存在しないハズのコンテナが生き残っている
                    LogWarning($"Find unknown container: {containerName}");
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Unknown, null);
                }
                else if(container.GetStatus().Exist() == false)
                {
                    // 既に終了しているハズのジョブのコンテナ＝何かの理由でコンテナだけ消せなかった
                    // ジョブ側には何の影響も与えたくないので、未知のコンテナとして削除する
                    LogWarning($"Find exited container: {containerName}");
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Unknown, null);
                }
                else
                {
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Training, container);
                }
            }
            else
            {
                // DBに登録していないテナントデータ削除用（管理者用）コンテナ
                if (containerName.StartsWith("delete-tenant"))
                {
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.DeleteTenant, null);
                }
                else
                {
                    // 存在しないハズのコンテナが残っている
                    LogWarning($"Find unknown container: {containerName}");
                    return new Tuple<ContainerType, TenantModelBase>(ContainerType.Unknown, null);
                }
            }
        }

        /// <summary>
        /// 指定コンテナを削除する
        /// </summary>
        /// <param name="tenant">対象テナント</param>
        /// <param name="name">コンテナ名</param>
        /// <param name="force">Admin権限で実行するか</param>
        private async Task<IActionResult> DeleteContainerAsync(Models.Tenant tenant, string name, bool force)
        {
            //入力チェック
            if (string.IsNullOrWhiteSpace(name))
            {
                return JsonBadRequest("Name is required.");
            }

            var container = CheckContainerType(name, force);

            switch (container.Item1)
            {
                case ContainerType.TensorBoard:
                    //C#は各caseがスコープ共通のため、同一変数名を使えない。止む無く変数名を分ける。
                    var trainingLogicForTensorBoard = commonDiLogic.DynamicDi<ITrainingLogic>();
                    //TensorBoardコンテナを削除する
                    await trainingLogicForTensorBoard.DeleteTensorBoardAsync(container.Item2 as TensorBoardContainer, force);
                    break;
                case ContainerType.Training:
                    //学習コンテナを強制終了させる
                    var trainingLogicForTraining = commonDiLogic.DynamicDi<ITrainingLogic>();
                    await trainingLogicForTraining.ExitAsync(container.Item2 as TrainingHistory, ContainerStatus.Killed, force);
                    break;
                case ContainerType.Inferencing:
                    //推論コンテナを強制終了させる
                    var inferenceLogic = commonDiLogic.DynamicDi<IInferenceLogic>();
                    await inferenceLogic.ExitAsync(container.Item2 as InferenceHistory, ContainerStatus.Killed, force);
                    break;
                case ContainerType.Notebook:
                    //ノートブックコンテナを強制終了させる
                    var notebookLogic = commonDiLogic.DynamicDi<INotebookLogic>();
                    await notebookLogic.ExitAsync(container.Item2 as NotebookHistory, ContainerStatus.Killed, force);
                    break;
                case ContainerType.Preprocessing:
                    //前処理コンテナを強制終了させる

                    //コンテナがいる＝前処理中なので、前処理済みデータは無条件で削除できると判断する
                    var preprocessLogic = commonDiLogic.DynamicDi<IPreprocessLogic>();
                    await preprocessLogic.DeleteAsync(container.Item2 as PreprocessHistory, force);
                    break;
                case ContainerType.DeleteTenant:
                    //テナントデータ削除用コンテナを強制終了させる
                    await clusterManagementLogic.DeleteContainerAsync(ContainerType.DeleteTenant, name, tenant.Name, force);
                    break;

                default:
                    //正体不明コンテナを削除する
                    var result = await clusterManagementLogic.DeleteContainerAsync(ContainerType.Unknown, name, tenant.Name, force);
                    if (result == false)
                    {
                        return JsonNotFound($"Container named {name} is not found.");
                    }
                    break;
            }

            return JsonNoContent();
        }

        /// <summary>
        /// 特定テナント向けに起動中のコンテナ一覧を取得する
        /// </summary>
        /// <returns>リソースデータ</returns>
        [HttpGet("/api/v{api-version:apiVersion}/tenant/resource/containers")]
        [PermissionFilter(MenuCode.TenantResource)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ContainerDetailsForTenantOutputModel>))]
        public async Task<IActionResult> GetResourceForTenant()
        {
            var result = await clusterManagementLogic.GetAllContainerDetailsInfosAsync(CurrentUserInfo.SelectedTenant.Name);
            if (result.IsSuccess)
            {
                return JsonOK(result.Value.Select(info => new ContainerDetailsForTenantOutputModel(info)
                {
                    CreatedBy = userRepository.GetUserName(info.CreatedBy)
                }));
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, string.Join("\n", "Fetching container resource is failed.", result.Error));
            }
        }

        /// <summary>
        /// 特定テナントに紐づくノード単位のリソースデータを取得する
        /// </summary>
        /// <returns>特定テナントに紐づくノード単位のリソースデータ</returns>
        [HttpGet("/api/v{api-version:apiVersion}/tenant/resource/nodes")]
        [PermissionFilter(MenuCode.TenantResource)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<NodeResourceOutputModel>))]
        public async Task<IActionResult> GetResourceByNodeForTenant([FromServices] INodeRepository nodeRepository)
        {
            // 返却データの Dictionary 生成
            var result = new Dictionary<string, NodeResourceOutputModel>();

            // 特定テナントが所属するノードの取得
            var nodes = nodeRepository.GetAccessibleNodes(CurrentUserInfo.SelectedTenant.Id);
            if (nodes.Count() == 0)
            {
                // 所属ノードが１つもない場合は空情報を返却
                return JsonOK(result.Values);
            }

            // 現在稼働のノード情報を k8s 経由で取得
            var nodeInfos = await clusterManagementLogic.GetAllNodesAsync();
            if (nodeInfos == null)
            {
                // 返却値が null ならエラーと見做す
                return JsonError(HttpStatusCode.ServiceUnavailable, string.Join("\n", "Fetching nodes is failed."));
            }

            // 返却データを特定テナントが所属するノードで作成・初期化
            foreach (var node in nodes)
            {
                // 現在稼働中のノード情報と突き合わせ
                var info = nodeInfos.FirstOrDefault(i => i.Name == node.Name);
                if (info == null)
                {
                    // 稼働していない場合はノード名に " !Disconnected!" を付加
                    result.Add(node.Name, new NodeResourceOutputModel(node));
                }
                else
                {
                    // 稼働している場合はノード名をそのまま利用
                    result.Add(node.Name, new NodeResourceOutputModel(node, info));
                }
            }

            // 全てのコンテナ情報を取得
            var response = await clusterManagementLogic.GetAllContainerDetailsInfosAsync();
            if (!response.IsSuccess)
            {
                // 取得に失敗したならエラーと見做す
                return JsonError(HttpStatusCode.ServiceUnavailable, string.Join("\n", "Fetching container resource is failed.", response.Error));
            }

            // ノード毎にリソース情報を加算する
            foreach (var container in response.Value)
            {
                if (string.IsNullOrEmpty(container.NodeName))
                {
                    // ノード名が空なら計上対象外
                    continue;
                }
                else if (!result.ContainsKey(container.NodeName))
                {
                    // 関連の無いノードなら計上対象外
                    continue;
                }

                // テナント名の照合 (container に ID が無いので名前で照合)
                if (container.TenantName.Equals(CurrentUserInfo.SelectedTenant.Name))
                {
                    // 現テナントと同じならリソース情報詳細を追加
                    result[container.NodeName].Add(CreateContainerDetailsOutputModel(container));
                }
                else
                {
                    // CPU, Memory, GPU のデータを加算
                    result[container.NodeName].IncrementData(CreateContainerDetailsOutputModel(container));
                }
            }
            return JsonOK(result.Values);
        }

        /// <summary>
        /// 指定コンテナのリソースデータを取得する
        /// </summary>
        /// <param name="name">コンテナ名</param>
        /// <returns>リソースデータ</returns>
        [HttpGet("/api/v{api-version:apiVersion}/tenant/resource/containers/{name}")]
        [PermissionFilter(MenuCode.TenantResource)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ContainerDetailsForTenantOutputModel))]
        public async Task<IActionResult> GetResourceByContainerNameForTenant([FromRoute] string name)
        {
            //入力チェック
            if (string.IsNullOrWhiteSpace(name))
            {
                return JsonBadRequest("Name is required.");
            }

            var tenant = CurrentUserInfo.SelectedTenant;

            var info = await clusterManagementLogic.GetContainerDetailsInfoAsync(name, tenant.Name, false);
            if (info.Status == ContainerStatus.None)
            {
                return JsonNotFound($"Container named {name} is not found.");
            }
            var result = new ContainerDetailsForTenantOutputModel(info)
            {
                CreatedBy = userRepository.GetUserName(info.CreatedBy),
                ContainerType = CheckContainerType(name, false).Item1 //コンテナの種別を確認
            };

            return JsonOK(result);
        }

        /// <summary>
        /// 指定コンテナのログを取得する
        /// </summary>
        /// <param name="name">コンテナ名</param>
        /// <returns>リソースデータ</returns>
        [HttpGet("/api/v{api-version:apiVersion}/tenant/resource/containers/{name}/log")]
        [PermissionFilter(MenuCode.TenantResource)]
        [ProducesResponseType(typeof(System.IO.Stream), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLogByContainerIdForTenant([FromRoute] string name)
        {
            //入力チェック
            if (string.IsNullOrWhiteSpace(name))
            {
                return JsonBadRequest("Name is required.");
            }

            var tenant = CurrentUserInfo.SelectedTenant;

            var fileContents = await clusterManagementLogic.DownloadLogAsync(name, tenant.Name, false);

            if (fileContents.IsSuccess == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to get container log: {fileContents.Error}");
            }
            else
            {
                return File(fileContents.Value, "text/plain", $"{tenant.Name}_{name}_log");
            }
        }

        /// <summary>
        /// 指定コンテナを削除する
        /// </summary>
        [HttpDelete("/api/v{api-version:apiVersion}/tenant/resource/containers/{name}")]
        [PermissionFilter(MenuCode.TenantResource)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteResourceByContainerIdForTenant([FromRoute] string name)
        {
            var tenant = CurrentUserInfo.SelectedTenant;

            return await DeleteContainerAsync(tenant, name, false);
        }

        #region history
        /// <summary>
        /// コンテナリソース履歴のメタデータを取得する
        /// </summary>
        [HttpGet("histories/containers/metadata")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType(typeof(HistoryMetadataOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHistoriesContainersMetadata()
        {
            var result = new HistoryMetadataOutputModel
            {
                Count = await resourceContainerRepository.Count(),
            };
            if (0 < result.Count)
            {
                var resourceSamples = resourceSampleRepository.GetAll();
                result.StartDate = resourceSamples.Min(x => x.SampledAt).ToFormattedDateString();
                result.EndDate = resourceSamples.Max(x => x.SampledAt).ToFormattedDateString();
            }
            return JsonOK(result);
        }

        /// <summary>
        /// コンテナリソース履歴のデータを取得する
        /// </summary>
        /// <param name="startDate">開始日</param>
        /// <param name="endDate">終了日</param>
        /// <param name="withHeader">ヘッダ情報を付与するか</param>
        [HttpGet("histories/containers/data")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHistoriesContainersData(string startDate, string endDate, bool withHeader)
        {
            // excelで直接オープン可能なcsvを作成するため、encodingはasciiとする
            // bufferSizeはbufferSizeを省略できるコンストラクタのデフォルト値と同じにする
            // streamはasp.net側でdisposeするのでleaveOpenはtrueにする
            using (var sw = new StreamWriter(new MemoryStream(), Encoding.ASCII, 1024, true))
            {
                var resourceSamples = resourceSampleRepository.GetAll();
                if (!string.IsNullOrEmpty(startDate))
                {
                    if (!DateTime.TryParse(startDate, out DateTime lower))
                    {
                        return JsonBadRequest("Invalid start date.");
                    }
                    resourceSamples = resourceSamples.Where(x => lower.Date <= x.SampledAt);
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    if (!DateTime.TryParse(endDate, out DateTime upper))
                    {
                        return JsonBadRequest("Invalid end date.");
                    }
                    resourceSamples = resourceSamples.Where(x => x.SampledAt < upper.Date + new TimeSpan(1, 0, 0, 0));
                }

                if (withHeader)
                {
                    await sw.WriteLineAsync("SampledAt,NodeName,NodeCpu,NodeMemory,NodeGpu,TenantId,TenantName"
                        + ",ContainerName,Cpu,Memory,Gpu,Status");
                }
                resourceSamples = resourceSamples.Include(x => x.ResourceNodes).ThenInclude(y => y.ResourceContainers);
                foreach (var x in resourceSamples.OrderBy(x => x.SampledAt))
                {
                    foreach (var y in x.ResourceNodes.OrderBy(y => y.Name))
                    {
                        foreach (var z in y.ResourceContainers.OrderBy(z => z.TenantId).ThenBy(z => z.Name))
                        {
                            await sw.WriteLineAsync($"{x.SampledAt},{y.Name},{y.Cpu},{y.Memory},{y.Gpu}"
                                + $",{z.TenantId},{z.TenantName},{z.Name},{z.Cpu},{z.Memory},{z.Gpu},{z.Status}");
                        }
                    }
                }

                await sw.FlushAsync();
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                return File(sw.BaseStream, "text/csv", $"{DateTime.Now:yyyyMMddHHmmss}_container.csv", false);
            }
        }

        /// <summary>
        /// コンテナリソース履歴を削除する
        /// </summary>
        /// <param name="model">削除対象の入力モデル</param>
        [HttpPatch("histories/containers")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult PatchHistoriesContainers([FromBody] HistoryDeleteInputModel model)
        {
            if (string.IsNullOrEmpty(model.EndDate))
            {
                resourceSampleRepository.DeleteAll(x => true);
            }
            else
            {
                if (!DateTime.TryParse(model.EndDate, out DateTime upper))
                {
                    return JsonBadRequest("Invalid end date.");
                }
                resourceSampleRepository.DeleteAll(x => x.SampledAt < upper.Date + new TimeSpan(1, 0, 0, 0));
            }
            unitOfWork.Commit();
            return JsonOK(null);
        }

        /// <summary>
        /// ジョブ実行履歴のメタデータを取得する
        /// </summary>
        [HttpGet("histories/jobs/metadata")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType(typeof(HistoryMetadataOutputModel), (int)HttpStatusCode.OK)]
        public IActionResult GetHistoriesJobsMetadata()
        {
            var resourceJobs = resourceJobRepository.GetAll();
            var result = new HistoryMetadataOutputModel
            {
                Count = resourceJobs.Count(),
            };
            if (0 < result.Count)
            {
                result.StartDate = resourceJobs.Min(x => x.JobCreatedAt).ToFormattedDateString();
                result.EndDate = resourceJobs.Max(x => x.JobCreatedAt).ToFormattedDateString();
            }
            return JsonOK(result);
        }

        /// <summary>
        /// ジョブ実行履歴のデータを取得する
        /// </summary>
        /// <param name="startDate">開始日</param>
        /// <param name="endDate">終了日</param>
        /// <param name="withHeader">ヘッダ情報を付与するか</param>
        [HttpGet("histories/jobs/data")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHistoriesJobsData(string startDate, string endDate, bool withHeader)
        {
            // excelで直接オープン可能なcsvを作成するため、encodingはasciiとする
            // bufferSizeはbufferSizeを省略できるコンストラクタのデフォルト値と同じにする
            // streamはasp.net側でdisposeするのでleaveOpenはtrueにする
            using (var sw = new StreamWriter(new MemoryStream(), Encoding.ASCII, 1024, true))
            {
                var resourceJobs = resourceJobRepository.GetAll();
                if (!string.IsNullOrEmpty(startDate))
                {
                    if (!DateTime.TryParse(startDate, out DateTime lower))
                    {
                        return JsonBadRequest("Invalid start date.");
                    }
                    resourceJobs = resourceJobs.Where(x => lower.Date <= x.JobCreatedAt);
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    if (!DateTime.TryParse(endDate, out DateTime upper))
                    {
                        return JsonBadRequest("Invalid end date.");
                    }
                    resourceJobs = resourceJobs.Where(x => x.JobCreatedAt < upper.Date + new TimeSpan(1, 0, 0, 0));
                }

                if (withHeader)
                {
                    await sw.WriteLineAsync("NodeName,NodeCpu,NodeMemory,NodeGpu,TenantId,TenantName"
                        + ",JobCreatedAt,JobStartedAt,JobCompletedAt,ContainerName,Cpu,Memory,Gpu,Status");
                }
                foreach (var x in resourceJobs.OrderBy(x => x.JobCreatedAt))
                {
                    await sw.WriteLineAsync($"{x.NodeName},{x.NodeCpu},{x.NodeMemory},{x.NodeGpu},{x.TenantId},{x.TenantName}"
                        + $",{x.JobCreatedAt.ToFormatedString()},{x.JobStartedAt?.ToFormatedString() ?? ""},{x.JobCompletedAt.ToFormatedString()}"
                        + $",{x.ContainerName},{x.Cpu},{x.Memory},{x.Gpu},{x.Status}");
                }

                await sw.FlushAsync();
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                return File(sw.BaseStream, "text/csv", $"{DateTime.Now:yyyyMMddHHmmss}_job.csv", false);
            }
        }

        /// <summary>
        /// ジョブ実行履歴を削除する
        /// </summary>
        /// <param name="model">削除対象の入力モデル</param>
        [HttpPatch("histories/jobs")]
        [PermissionFilter(MenuCode.Resource)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult PatchHistoriesJobs([FromBody] HistoryDeleteInputModel model)
        {
            if (string.IsNullOrEmpty(model.EndDate))
            {
                resourceJobRepository.DeleteAll(x => true);
            }
            else
            {
                if (!DateTime.TryParse(model.EndDate, out DateTime upper))
                {
                    return JsonBadRequest("Invalid end date.");
                }
                resourceJobRepository.DeleteAll(x => x.JobCreatedAt < upper.Date + new TimeSpan(1, 0, 0, 0));
            }
            unitOfWork.Commit();
            return JsonOK(null);
        }
        #endregion
    }
}

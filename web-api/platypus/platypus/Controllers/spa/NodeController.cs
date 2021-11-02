using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nssol.Platypus.ApiModels.NodeApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// ノード管理を扱うためのAPI集
    /// </summary>
    [ApiController]
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/admin/nodes")]
    public class NodeController : PlatypusApiControllerBase
    {
        private readonly INodeRepository nodeRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly ContainerManageOptions containerManageOptions;

        public NodeController(
            INodeRepository nodeRepository,
            IUnitOfWork unitOfWork,
            IClusterManagementLogic clusterManagementLogic,
            IOptions<ContainerManageOptions> containerManageOptions,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.nodeRepository = nodeRepository;
            this.unitOfWork = unitOfWork;
            this.clusterManagementLogic = clusterManagementLogic;
            this.containerManageOptions = containerManageOptions.Value;
        }

        /// <summary>
        /// 全ノード一覧を取得
        /// </summary>
        [HttpGet]
        [PermissionFilter(MenuCode.Node)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll([FromQuery] string name, [FromQuery]int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            var nodes = nodeRepository.GetAll();

            if (string.IsNullOrEmpty(name) == false)
            {
                nodes = nodes.Where(n => n.Name.Contains(name)).OrderBy(n => n.Name);
            }
            else
            {
                nodes = nodes.OrderBy(n => n.Name);
            }

            if (withTotal)
            {
                //ノードの場合は件数が少ない想定なので、別のSQLを投げずにカウントしてしまう
                SetTotalCountToHeader(nodes.Count());
            }

            if (perPage.HasValue)
            {
                nodes = nodes.Paging(page, perPage.Value);
            }

            return JsonOK(nodes.Select(n => new IndexOutputModel(n)));
        }

        /// <summary>
        /// ノードアクセスレベルの一覧を取得する
        /// </summary>
        [HttpGet("/api/v{api-version:apiVersion}/admin/node-access-levels")]
        [PermissionFilter(MenuCode.Node)]
        [ProducesResponseType(typeof(IEnumerable<EnumInfo>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllTypes()
        {
            var accessLevels = Enum.GetValues(typeof(NodeAccessLevel)) as NodeAccessLevel[];

            return JsonOK(accessLevels.Select(n => new EnumInfo() { Id = (int)n, Name = n.ToString() }));
        }

        /// <summary>
        /// 指定されたIDのノード情報を取得。
        /// </summary>
        /// <param name="id">ノードID</param>
        /// <param name="nodeTenantMapRepository">DI用</param>
        [HttpGet("{id}")]
        [PermissionFilter(MenuCode.Node)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long? id, [FromServices] INodeTenantMapRepository nodeTenantMapRepository)
        {
            if (id == null)
            {
                return JsonBadRequest("Node ID is required.");
            }
            var node = await nodeRepository.GetByIdAsync(id.Value);
            if (node == null)
            {
                return JsonNotFound($"Node Id {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(node);
            if(model.AccessLevel == NodeAccessLevel.Private)
            {
                //プライベートモードの時に限り、アクセス可能なテナントを探索する
                model.AssignedTenants = nodeRepository.GetAssignedTenants(node.Id).Select(t => new DetailsOutputModel.AssignedTenant()
                {
                    Id = t.Id,
                    Name = t.Name,
                    DisplayName = t.DisplayName
                });
            }

            return JsonOK(model);
        }

        /// <summary>
        /// 新規にノードを登録する
        /// </summary>
        [HttpPost]
        [PermissionFilter(MenuCode.Node)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody]CreateInputModel model,
            [FromServices] ITenantRepository tenantRepository)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            //同じ名前のノードは登録できないので、確認する
            Node node = await nodeRepository.GetByNameAsync(model.Name);
            if (node != null)
            {
                return JsonConflict($"Node {model.Name} already exists: ID = {node.Id}");
            }

            node = new Node()
            {
                Name = model.Name,
                Memo = model.Memo,
                Partition = model.Partition,
                AccessLevel = model.AccessLevel == null ? NodeAccessLevel.Disabled : model.AccessLevel.Value,
                TensorBoardEnabled = model.TensorBoardEnabled,
                NotebookEnabled = model.NotebookEnabled
            };

            if(node.AccessLevel != NodeAccessLevel.Disabled)
            {
                //アクセスレベルがDisable以外であれば、k8sとの同期を行う
                //ノードが存在しない場合を考慮し、もし失敗しても気にせず更新処理を続ける
                await clusterManagementLogic.UpdatePartitionLabelAsync(node.Name, node.Partition);

                if (node.TensorBoardEnabled)
                {
                    await clusterManagementLogic.UpdateTensorBoardEnabledLabelAsync(node.Name, true);
                }

                if (node.NotebookEnabled)
                {
                    await clusterManagementLogic.UpdateNotebookEnabledLabelAsync(node.Name, true);
                }

                // KQI管理者用名前空間の実行を許可する
                await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, containerManageOptions.KqiAdminNamespace, true);

                if (node.AccessLevel == NodeAccessLevel.Private)
                {
                    //テナントをアサイン
                    if (model.AssignedTenantIds != null)
                    {
                        foreach (long tenantId in model.AssignedTenantIds)
                        {
                            Tenant tenant = tenantRepository.Get(tenantId);
                            if (tenant == null)
                            {
                                return JsonNotFound($"Tenant ID {tenantId} is not found.");
                            }
                            await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, tenant.Name, true);
                        }
                        nodeRepository.AssignTenants(node, model.AssignedTenantIds, true);
                    }
                }
                else
                {
                    // アクセスレベルが "Public" の場合、全てのテナントをアサイン
                    var tenants = tenantRepository.GetAllTenants();
                    foreach (Tenant tenant in tenants)
                    {
                        await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, tenant.Name, true);
                    }
                }
            }

            nodeRepository.Add(node);
            unitOfWork.Commit();

            return JsonCreated(new IndexOutputModel(node));
        }

        /// <summary>
        /// ノード情報の編集
        /// </summary>
        [HttpPut("{id}")]
        [PermissionFilter(MenuCode.Node)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody]CreateInputModel model,
            [FromServices] ITenantRepository tenantRepository) //EditとCreateで項目が同じなので、入力モデルを使いまわし
        {
            //データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var node = await nodeRepository.GetByIdAsync(id.Value);
            if (node == null)
            {
                return JsonNotFound($"Node ID {id.Value} is not found.");
            }

            //同じ名前のノードは登録できないので、確認する
            if (await nodeRepository.ExistsAsync(n => n.Id != node.Id && n.Name == model.Name))
            {
                return JsonConflict($"Node {model.Name} already exists: ID = {node.Id}");
            }
            
            //NodeはCLIではなく画面から変更されるので、常にすべての値を入れ替える
            node.Name = model.Name;
            node.Memo = model.Memo;
            node.Partition = model.Partition;
            node.TensorBoardEnabled = model.TensorBoardEnabled;
            node.NotebookEnabled = model.NotebookEnabled;
            node.AccessLevel = model.AccessLevel.Value;

            // まずは全てのアサイン情報を削除する
            nodeRepository.ResetAssinedTenants(node.Id);
            var tenants = tenantRepository.GetAllTenants();
            foreach (Tenant tenant in tenants)
            {
                await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, tenant.Name, false);
            }

            if (node.AccessLevel != NodeAccessLevel.Disabled)
            {
                //アクセスレベルがDisable以外であれば、k8sとの同期を行う
                //ノードが存在しない場合を考慮し、もし失敗しても気にせず更新処理を続ける
                //代わりに、前回成功している確証がないので、値が変更前と同じでも更新処理は行う
                await clusterManagementLogic.UpdatePartitionLabelAsync(node.Name, node.Partition);

                await clusterManagementLogic.UpdateTensorBoardEnabledLabelAsync(node.Name, node.TensorBoardEnabled);

                await clusterManagementLogic.UpdateNotebookEnabledLabelAsync(node.Name, node.NotebookEnabled);

                // KQI管理者用名前空間の実行を許可する
                await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, containerManageOptions.KqiAdminNamespace, true);

                if (node.AccessLevel == NodeAccessLevel.Private)
                {
                    // テナントをアサイン
                    if (model.AssignedTenantIds != null)
                    {
                        foreach (long tenantId in model.AssignedTenantIds)
                        {
                            Tenant tenant = tenantRepository.Get(tenantId);
                            if (tenant == null)
                            {
                                return JsonNotFound($"Tenant ID {tenantId} is not found.");
                            }
                            await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, tenant.Name, true);
                        }
                        nodeRepository.AssignTenants(node, model.AssignedTenantIds, false);
                    }
                }
                else
                {
                    // アクセスレベルが "Public" の場合、全てのテナントをアサイン
                    foreach (Tenant tenant in tenants)
                    {
                        await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, tenant.Name, true);
                    }
                }
            }
            else
            {
                // アクセスレベルが "Disable" の場合、KQI管理者用名前空間の実行を拒否する
                await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, containerManageOptions.KqiAdminNamespace, false);
            }

            unitOfWork.Commit();

            return JsonOK(new IndexOutputModel(node));
        }

        /// <summary>
        /// ノードを削除する。
        /// </summary>
        [HttpDelete("{id}")]
        [PermissionFilter(MenuCode.Node)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id,
            [FromServices] ITenantRepository tenantRepository)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var node = await nodeRepository.GetByIdAsync(id.Value);
            if (node == null)
            {
                return JsonNotFound($"Node ID {id.Value} is not found.");
            }

            // 削除対象のノードでコンテナ稼働中の場合、削除しない
            var response = await clusterManagementLogic.GetAllContainerDetailsInfosAsync();
            if (response.IsSuccess)
            {
                int runningCount = 0;
                foreach (var container in response.Value)
                {
                    // ステータスによらず、全て稼働中と見做す
                    if (node.Name.Equals(container.NodeName))
                    {
                        runningCount++;
                    }
                }
                if (runningCount > 0)
                {
                    return JsonConflict($"Running containers exists deleting node. node name=[{node.Name}], running container count=[{runningCount}]");
                }
            }

            if (string.IsNullOrEmpty(node.Partition) != false)
            {
                //パーティションが設定されていたら、消す
                //既にノードが外されている場合を考慮し、もし失敗しても気にせず削除処理を続ける
                await clusterManagementLogic.UpdatePartitionLabelAsync(node.Name, "");
            }

            // 全てのアサイン情報を削除する
            var tenants = tenantRepository.GetAllTenants();
            foreach (Tenant tenant in tenants)
            {
                await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, tenant.Name, false);
            }

            // KQI管理者用名前空間のアサイン情報を削除する
            await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, containerManageOptions.KqiAdminNamespace, false);

            nodeRepository.ResetAssinedTenants(node.Id);
            nodeRepository.Delete(node);
            unitOfWork.Commit();

            return JsonNoContent();
        }

        /// <summary>
        /// ノード情報をDBからClusterへ同期させる
        /// </summary>
        [HttpPost("sync-cluster-from-db")]
        [PermissionFilter(MenuCode.Node)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SyncPartitionToCluster([FromServices] INodeRepository nodeRepository,
            [FromServices] ITenantRepository tenantRepository)
        {
            //DB側の情報を取得
            var nodes = nodeRepository.GetAll();
            foreach (Node node in nodes)
            {
                bool result = await clusterManagementLogic.UpdatePartitionLabelAsync(node.Name, node.Partition);
                if (result == false)
                {
                    //DBからノードを取っているため、名前ミスで失敗する可能性はないハズ。ダメだったら例外扱い。
                    return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to update partitions. Please contact your server administrator.");
                }
                await clusterManagementLogic.UpdateTensorBoardEnabledLabelAsync(node.Name, node.TensorBoardEnabled);
                await clusterManagementLogic.UpdateNotebookEnabledLabelAsync(node.Name, node.NotebookEnabled);

                // まずは全てのアサイン情報を削除する
                var tenants = tenantRepository.GetAllTenants();
                foreach (Tenant tenant in tenants)
                {
                    await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, tenant.Name, false);
                }

                // アクセスレベルが "Disable" 以外であれば、アクセス可能なテナント情報の同期を行う
                if (node.AccessLevel != NodeAccessLevel.Disabled)
                {
                    // KQI管理者用名前空間の実行を許可する
                    await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, containerManageOptions.KqiAdminNamespace, true);

                    // アクセスレベルが "Private" の場合、可能なテナント一覧を取得する
                    if (node.AccessLevel == NodeAccessLevel.Private)
                    {
                        tenants = nodeRepository.GetAssignedTenants(node.Id);
                    }

                    // テナント情報をアサインする
                    if (tenants != null)
                    {
                        foreach (Tenant tenant in tenants)
                        {
                            bool ret = clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, tenant.Name, true).Result;
                            if (!ret)
                            {
                                LogError($"ノード [{node.Name}] にテナント [{tenant.Name}] のアクセス許可を Cluster(k8s) へ同期させる処理に失敗しました。");
                            }
                        }
                    }
                }
                else
                {
                    // アクセスレベルが "Disable" の場合、KQI管理者用名前空間の実行を拒否する
                    await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, containerManageOptions.KqiAdminNamespace, false);
                }
            }

            return JsonOK(nodes.Select(n => new IndexOutputModel(n)));
        }
    }
}

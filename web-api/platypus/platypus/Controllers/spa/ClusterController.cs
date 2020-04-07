using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Models;
using Nssol.Platypus.ApiModels.ClusterApiModels;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;

namespace Nssol.Platypus.Controllers.spa
{
    [Route("api/v1")]
    public class ClusterController : PlatypusApiControllerBase
    {
        private readonly ITensorBoardContainerRepository tensorBoardContainerRepository;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IUnitOfWork unitOfWork;

        public ClusterController(
          ITensorBoardContainerRepository tensorBoardContainerRepository,
          IClusterManagementLogic clusterManagementLogic,
          IUnitOfWork unitOfWork,
          IHttpContextAccessor accessor) : base(accessor)
        {
            this.tensorBoardContainerRepository = tensorBoardContainerRepository;
            this.clusterManagementLogic = clusterManagementLogic;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 接続中のテナントに有効なパーティションの一覧を取得する。
        /// </summary>
        [HttpGet("tenant/partitions")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public IActionResult GetPartitions([FromServices] INodeRepository nodeRepository)
        {
            //パーティション情報はDBから取得する。クラスタ側との同期は行わないので、必要なら別のAPIを呼ぶ仕様。
            var nodes = nodeRepository.GetAccessibleNodes(CurrentUserInfo.SelectedTenant.Id);
            var partitions = nodes.Select(n => n.Partition).Distinct().Where(p => string.IsNullOrEmpty(p) == false).OrderBy(e => e); //並び替えして返却
            return JsonOK(partitions);
        }

        /// <summary>
        /// パーティションの一覧を取得する。
        /// </summary>
        [HttpGet("admin/partitions")]
        [Filters.PermissionFilter(MenuCode.Node)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public IActionResult GetPartitionsForAdmin([FromServices] INodeRepository nodeRepository)
        {
            //パーティション情報はDBから取得する。クラスタ側との同期は行わないので、必要なら別のAPIを呼ぶ仕様。
            var nodes = nodeRepository.GetAll();
            var partitions = nodes.Select(n => n.Partition).Distinct().Where(p => string.IsNullOrEmpty(p) == false).OrderBy(e => e); //並び替えして返却
            return JsonOK(partitions);
        }

        /// <summary>
        /// クォータ設定を取得する。
        /// </summary>
        [HttpGet("admin/quotas")]
        [Filters.PermissionFilter(MenuCode.Quota)]
        [ProducesResponseType(typeof(IEnumerable<QuotaOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetQuotas([FromServices] ITenantRepository tenantRepository)
        {
            var result = tenantRepository.GetAllTenants().OrderBy(t => t.DisplayName).Select(t => new QuotaOutputModel(t));
            return JsonOK(result);
        }

        /// <summary>
        /// クォータ設定を更新する。
        /// </summary>
        /// <remarks>
        /// 0が指定された場合、上限なしを示す。また、指定のなかったテナントは更新しない。
        /// </remarks>
        [HttpPost("admin/quotas")]
        [Filters.PermissionFilter(MenuCode.Quota)]
        [ProducesResponseType(typeof(IEnumerable<QuotaOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditQuotas([FromBody] IEnumerable<QuotaInputModel> models, [FromServices] ITenantRepository tenantRepository)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            var result = new List<QuotaOutputModel>();

            foreach (var inputModel in models)
            {
                //更新用に、キャッシュではなくDBから直接取得
                Tenant tenant = await tenantRepository.GetTenantForUpdateAsync(inputModel.TenantId.Value);
                if (tenant == null)
                {
                    return JsonNotFound($"Tenant ID {inputModel.TenantId.Value} is not found.");
                }
                tenant.LimitCpu = inputModel.Cpu == 0 ? (int?)null : inputModel.Cpu;
                tenant.LimitMemory = inputModel.Memory == 0 ? (int?)null : inputModel.Memory;
                tenant.LimitGpu = inputModel.Gpu == 0 ? (int?)null : inputModel.Gpu;

                //結果に格納
                result.Add(new QuotaOutputModel(tenant));
                
                await clusterManagementLogic.SetQuotaAsync(tenant);
            }

            unitOfWork.Commit();
            tenantRepository.Refresh();

            return JsonOK(result);
        }

        /// <summary>
        /// DB上の全てのTensorBoardコンテナ情報を対応する実コンテナごと削除する。
        /// </summary>
        /// <remarks>
        /// REST APIとして定時バッチから実行される想定。
        /// </remarks>
        [HttpDelete("admin/tensorboards")]
        [Filters.PermissionFilter(MenuCode.Node)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAll()
        {
            var containers = await tensorBoardContainerRepository.GetAllIncludePortAndTenantAsync();

            int count = 0; //削除したコンテナの数
            bool failure = false; //1件でも失敗したか

            LogInformation("コンテナのDBレコード削除、コンテナ強制終了を開始。");

            foreach (TensorBoardContainer container in containers)
            {
                var destroyResult = await clusterManagementLogic.DeleteContainerAsync(ContainerType.TensorBoard, container.Name, container.Tenant.Name, true);

                //コンテナ削除に成功した場合
                if (destroyResult)
                {
                    count++;
                }
                else
                {
                    LogError($"テナント:{container.Tenant.Name}のコンテナ:{container.Name}のコンテナ強制終了時に予期しないエラーが発生しました。");

                    //TensorBoard削除に限り、ミスって削除しても問題が大きくないため、途中でエラーが発生しても最後まで処理し続ける

                    failure = true;
                }

                //DB側も削除する
                //コンテナの削除に失敗したとしても、ログは出しているし、DBに残していてもできる事がないので、無視して削除する
                tensorBoardContainerRepository.Delete(container, true);
            }
            unitOfWork.Commit();
            if(failure)
            {
                //1件以上失敗しているので、エラー扱い
                return JsonError(HttpStatusCode.ServiceUnavailable, $"failed to delete some tensorboard containers. deleted: {count}");
            }
            else
            {
                return JsonOK(count);
            }
        }

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="id">テナントID</param>
        /// <param name="name">コンテナ名</param>
        /// <param name="tenantRepository">DI用</param>
        [HttpGet("admin/events/{id}")]
        [Filters.PermissionFilter(MenuCode.Tenant)]
        [ProducesResponseType(typeof(IEnumerable<ContainerEventInfo>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetEvents([FromRoute] long? id, [FromQuery] string name, [FromServices] ITenantRepository tenantRepository)
        {
            if (id == null)
            {
                return JsonBadRequest("Tenant ID is required.");
            }

            Tenant tenant = tenantRepository.Get(id.Value);
            if (tenant == null)
            {
                return JsonNotFound($"Tenant Id {id.Value} is not found.");
            }

            var result = string.IsNullOrEmpty(name) ?
                await clusterManagementLogic.GetEventsAsync(tenant, true) :
                await clusterManagementLogic.GetEventsAsync(tenant, name, true, false);
            if (result.IsSuccess)
            {
                return JsonOK(result.Value);
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"failed to access to container: {result.Error.Name}");
            }
        }
    }
}
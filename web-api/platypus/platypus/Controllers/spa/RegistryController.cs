using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.RegistryApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    [Route("api/v1/admin/registry")]
    public class RegistryController : PlatypusApiControllerBase
    {
        private readonly IRegistryLogic registryLogic;
        private readonly ITenantRepository tenantRepository;
        private readonly IRegistryRepository registryRepository;
        private readonly IUnitOfWork unitOfWork;

        public RegistryController(
            IRegistryLogic registryLogic,
            ITenantRepository tenantRepository,
            IRegistryRepository registryRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.registryLogic = registryLogic;
            this.tenantRepository = tenantRepository;
            this.registryRepository = registryRepository;
            this.unitOfWork = unitOfWork;
        }


        #region レジストリ登録
        /// <summary>
        /// 登録済みのDockerレジストリ エンドポイント一覧を取得
        /// </summary>
        [HttpGet("endpoints")]
        [Filters.PermissionFilter(MenuCode.Registry, MenuCode.Tenant)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var registryEndpoints = registryRepository.GetRegistryAll();

            return JsonOK(registryEndpoints.Select(g => new IndexOutputModel(g)));
        }

        /// <summary>
        /// Dockerレジストリ種別一覧を取得
        /// </summary>
        [HttpGet("types")]
        [Filters.PermissionFilter(MenuCode.Registry)]
        [ProducesResponseType(typeof(IEnumerable<EnumInfo>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllTypes()
        {
            var registryTypes = Enum.GetValues(typeof(RegistryServiceType)) as RegistryServiceType[];
            //Noneは除外して返却
            return JsonOK(registryTypes.Where(r => r != RegistryServiceType.None).Select(r => new EnumInfo() { Id = (int)r, Name = r.ToString() }));
        }

        /// <summary>
        /// 指定されたIDのDockerレジストリ エンドポイント情報を取得
        /// </summary>
        /// <param name="id">GitエンドポイントID</param>
        [HttpGet("endpoints/{id}")]
        [Filters.PermissionFilter(MenuCode.Registry)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetails(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Registry ID is required.");
            }

            Registry registry = await registryRepository.GetByIdAsync(id.Value);
            if (registry == null)
            {
                return JsonNotFound($"Registry Id {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(registry);

            return JsonOK(model);
        }

        /// <summary>
        /// 新規にDockerレジストリ エンドポイントを登録する
        /// </summary>
        [HttpPost("endpoints")]
        [Filters.PermissionFilter(MenuCode.Registry)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public IActionResult Create([FromBody]CreateInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            //同じ名前のレジストリは登録できないので、確認する
            var registry = registryRepository.GetRegistryAll().FirstOrDefault(r => r.Name == model.Name);
            if (registry != null)
            {
                return JsonConflict($"Registry {model.Name} already exists.");
            }

            registry = new Registry()
            {
                Name = model.Name,
                Host = model.Host,
                PortNo = model.PortNo.Value,
                ServiceType = model.ServiceType.Value,
                ProjectName = model.ProjectName,
                ApiUrl = model.ApiUrl,
                RegistryUrl = model.RegistryUrl
            };

            registryRepository.Add(registry);
            unitOfWork.Commit();

            var result = new IndexOutputModel(registry);

            return JsonOK(result);
        }

        /// <summary>
        /// Dockerレジストリ エンドポイント情報の編集
        /// </summary>
        [HttpPut("endpoints/{id}")]
        [Filters.PermissionFilter(MenuCode.Registry)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody]CreateInputModel model, //EditとCreateで項目が同じなので、入力モデルを使いまわし
            [FromServices] IClusterManagementLogic clusterManagementLogic)
        {
            //データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var registry = await registryRepository.GetByIdAsync(id.Value);
            if (registry == null)
            {
                return JsonNotFound($"Registry ID {id.Value} is not found.");
            }
            //データの編集可否チェック
            if (registry.IsNotEditable)
            {
                return JsonBadRequest($"Registry ID {id.Value} is not allowed to edit.");
            }

            //同じ名前のレジストリは登録できないので、自分の他に同名の物がいないか、確認する
            var otherRegistry = registryRepository.GetRegistryAll().FirstOrDefault(r => r.Name == model.Name && r.Id != id);
            if (otherRegistry != null)
            {
                return JsonConflict($"Registry {model.Name} already exists.");
            }

            registry.Name = model.Name;
            registry.Host = model.Host;
            registry.PortNo = model.PortNo.Value;
            registry.ServiceType = model.ServiceType.Value;
            registry.ProjectName = model.ProjectName;
            registry.ApiUrl = model.ApiUrl;
            registry.RegistryUrl = model.RegistryUrl;

            registryRepository.Update(registry);
            unitOfWork.Commit();

            return JsonOK(new IndexOutputModel(registry));
        }

        /// <summary>
        /// Dockerレジストリ エンドポイント情報の削除
        /// </summary>
        [HttpDelete("endpoints/{id}")]
        [Filters.PermissionFilter(MenuCode.Registry)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id,
            [FromServices] IClusterManagementLogic clusterManagementLogic,
            [FromServices] DataAccess.Repositories.Interfaces.TenantRepositories.ITrainingHistoryRepository trainingHistoryRepository)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var registry = await registryRepository.GetByIdAsync(id.Value);
            if (registry == null)
            {
                return JsonNotFound($"Registry ID {id.Value} is not found.");
            }
            //データの編集可否チェック
            if (registry.IsNotEditable)
            {
                return JsonBadRequest($"Registry ID {id.Value} is not allowed to delete.");
            }

            //このレジストリを登録しているテナントがいた場合、削除はできない
            var tenant = registryRepository.GetTenant(registry.Id);
            if (tenant != null)
            {
                return JsonConflict($"Registry {registry.Id}:{registry.Name} is used at Tenant {tenant.Id}:{tenant.Name}.");
            }
            //このレジストリを使った履歴がある場合、削除はできない
            var training = trainingHistoryRepository.Find(t => t.ContainerRegistryId == registry.Id);
            if (training != null)
            {
                return JsonConflict($"Registry {registry.Id}:{registry.Name} is used at training {training.Id} in Tenant {training.TenantId}.");
            }

            //クラスタ管理サービス側の登録情報は特に削除しない（残っていても問題ない）
            registryRepository.Delete(registry);
            unitOfWork.Commit();

            return JsonNoContent();
        }
        #endregion

        #region 接続テナント設定
        /// <summary>
        /// テナント管理者が選択可能な登録済みのDockerレジストリ エンドポイント一覧を取得
        /// </summary>
        /// <param name="id">テナントID</param>
        [HttpGet("/api/v1/tenant/{id}/registry/endpoints")]
        [Filters.PermissionFilter(MenuCode.TenantSetting)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllForTenant(long? id)
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

            // 編集不可の全レジストリ情報を取得
            var registryNotEditableEndpoints = registryRepository.GetRegistryAll().Where(r => r.IsNotEditable == true);

            // 指定したテナントに紐づく全レジストリ情報を取得
            var registryEndpointsForTenant = registryRepository.GetRegistryAll(id.Value);

            // 重複を除き結合する
            var registryEndpoints = registryNotEditableEndpoints.Union(registryEndpointsForTenant).OrderBy(r => r.Id);

            return JsonOK(registryEndpoints.Select(r => new IndexOutputModel(r)));
        }
        #endregion

        #region レジストリアクセス
        /// <summary>
        /// レジストリに存在する全イメージの取得
        /// </summary>
        /// <param name="registryId">レジストリID</param>
        /// <returns>イメージ名のリスト</returns>
        [HttpGet("/api/v1/registries/{registryId}/images")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetImages([FromRoute] long? registryId)
        {
            long? selectedRegistryId = registryId == null ?
                CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id :
                registryId;

            if(selectedRegistryId == null)
            {
                return JsonNotFound($"There is no registry you can use. Please contact a user administrator.");
            }

            var result = await registryLogic.GetAllImageListAsync(selectedRegistryId.Value);
            if (result != null)
            {
                return JsonOK(result);
            }
            else
            {
                return JsonBadRequest($"Can not Access Registry Id {selectedRegistryId.Value}: Invalid Registry Server or Token");
            }
        }

        /// <summary>
        /// イメージ名に対応するタグ一覧を取得
        /// </summary>
        /// <param name="registryId">レジストリID</param>
        /// <param name="image">イメージ名</param>
        /// <returns>タグ名のリスト</returns>
        [HttpGet("/api/v1/registries/{registryId}/images/{image}/tags")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTagsAsync([FromRoute] long? registryId, [FromRoute] string image)
        {
            long? selectedRegistryId = registryId == null ?
                CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id :
                registryId;

            if (selectedRegistryId == null)
            {
                return JsonNotFound($"There is no registry you can use. Please contact a user administrator.");
            }

            var result = await registryLogic.GetAllTagListAsync(selectedRegistryId.Value, image);
            if (result.IsSuccess)
            {
                return JsonOK(result.Value);
            }
            else
            {
                return JsonBadRequest($"Can not Access Registry Id {selectedRegistryId.Value}: {result.Error}");
            }
        }

        /// <summary>
        /// 階層化されたURLを吸収するためのダミーAPI。
        /// 製品版のSwaggerからは削除する。
        /// </summary>
        [HttpGet("/api/v1/registries/{registryId}/images/{*segments}")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        public async Task<IActionResult> AllocatieRoute([FromRoute] long? registryId, [FromRoute] string segments)
        {
            string[] segmentsArray = segments.Split('/');

            if (segmentsArray.Length < 2)
            {
                return JsonNotFound();
            }

            //最後がtagsでなければならない
            if(segmentsArray[segmentsArray.Length - 1] != "tags")
            {
                return JsonNotFound();
            }

            //それ以外がイメージ名
            string image = string.Join("/", segmentsArray.Take(segmentsArray.Length - 1));

            return await GetTagsAsync(registryId, image);
        }

        #endregion
    }
}
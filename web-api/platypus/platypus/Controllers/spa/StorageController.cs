using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.StorageApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.LogicModels.StorageLogicModels;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// ObjectStorageへのアクセス回りの処理をまとめたコントローラ。
    /// URLはアクションメソッド単位で割り当てる。
    /// 多数のメニューで使用される想定のため、権限制御は行わない。
    /// </summary>
    public class StorageController : PlatypusApiControllerBase
    {
        private readonly IStorageLogic storageLogic;
        private readonly ITenantRepository tenantRepository;
        private readonly IUnitOfWork unitOfWork;

        public StorageController(
            IStorageLogic storageLogic,
            ITenantRepository tenantRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.storageLogic = storageLogic;
            this.tenantRepository = tenantRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Storageエンドポイント登録
        /// <summary>
        /// 登録済みのStorageエンドポイント一覧を取得
        /// </summary>
        [HttpGet("/api/v1/admin/storage/endpoints")]
        [Filters.PermissionFilter(MenuCode.Storage, MenuCode.Tenant)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var storageEndpoints = tenantRepository.GetStorageAll();

            return JsonOK(storageEndpoints.Select(g => new IndexOutputModel(g)));
        }

        /// <summary>
        /// 指定されたIDのStorageエンドポイント情報を取得。
        /// </summary>
        /// <param name="id">StorageエンドポイントID</param>
        [HttpGet("/api/v1/admin/storage/endpoints/{id}")]
        [Filters.PermissionFilter(MenuCode.Storage)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public IActionResult GetDetails(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Storage ID is required.");
            }

            Storage storage = tenantRepository.GetStorage(id.Value);
            if (storage == null)
            {
                return JsonNotFound($"Storage Id {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(storage);

            return JsonOK(model);
        }

        /// <summary>
        /// 新規にStorageエンドポイントを登録する
        /// </summary>
        [HttpPost("/api/v1/admin/storage/endpoints")]
        [Filters.PermissionFilter(MenuCode.Storage)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public IActionResult Create([FromBody]CreateInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            Storage storage = new Storage()
            {
                Name = model.Name,
                ServerAddress = model.ServerUrl,
                AccessKey = model.AccessKey,
                SecretKey = model.SecretKey,
                NfsServer = model.NfsServer,
                NfsRoot = model.NfsRoot
            };

            tenantRepository.AddStorage(storage);
            unitOfWork.Commit();
            tenantRepository.Refresh();

            var result = new IndexOutputModel(storage);

            return JsonOK(result);
        }

        /// <summary>
        /// Storageエンドポイント情報の編集
        /// </summary>
        [HttpPut("/api/v1/admin/storage/endpoints/{id}")]
        [Filters.PermissionFilter(MenuCode.Storage)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody]CreateInputModel model) //EditとCreateで項目が同じなので、入力モデルを使いまわし
        {
            //データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var storage = await tenantRepository.GetStorageForUpdateAsync(id.Value);
            if (storage == null)
            {
                return JsonNotFound($"Storage ID {id.Value} is not found.");
            }

            storage.Name = model.Name;
            storage.ServerAddress = model.ServerUrl;
            storage.AccessKey = model.AccessKey;
            storage.SecretKey = model.SecretKey;
            storage.NfsServer = model.NfsServer;
            storage.NfsRoot = model.NfsRoot;

            tenantRepository.UpdateStorage(storage);

            // このStorageを登録しているテナントがいた場合、バケットを作成する
            var tenants = tenantRepository.GetAllTenants().Where(t => t.StorageId == id.Value);
            foreach (Tenant tenant in tenants)
            {
                //バケットを作成する
                await storageLogic.CreateBucketAsync(tenant, storage);
            }

            unitOfWork.Commit();
            tenantRepository.Refresh();

            return JsonOK(new IndexOutputModel(storage));
        }

        /// <summary>
        /// Storageエンドポイント情報の削除
        /// </summary>
        [HttpDelete("/api/v1/admin/storage/endpoints/{id}")]
        [Filters.PermissionFilter(MenuCode.Storage)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var storage = await tenantRepository.GetStorageForUpdateAsync(id.Value);
            if (storage == null)
            {
                return JsonNotFound($"Storage ID {id.Value} is not found.");
            }

            //このStorageを登録しているテナントがいた場合、削除はできない
            var tenant = tenantRepository.GetAllTenants().FirstOrDefault(t => t.StorageId == storage.Id);
            if (tenant != null)
            {
                return JsonConflict($"Storage {storage.Id}:{storage.Name} is used at Tenant {tenant.Id}:{tenant.Name}.");
            }

            tenantRepository.DeleteStorage(storage);
            unitOfWork.Commit();
            tenantRepository.Refresh();

            return JsonNoContent();
        }
        #endregion

        #region ストレージアクセス
        /// <summary>
        /// 分割アップロードを行うためのパラメータを取得する
        /// </summary>
        [HttpGet("/api/v1/upload/parameter")]
        [ProducesResponseType(typeof(MultiPartUploadModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMultipleUploadUrlv2([FromQuery]MultiPartUploadInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            if (model.PartSum == 0)
            {
                return JsonBadRequest("The uploading file can not be parted. The file may be empty (0 byte).");
            }
            ResourceType type;
            if(Enum.TryParse(model.Type, true, out type) == false)
            {
                LogDebug("有効なリソースタイプが指定されていません。");
                return JsonBadRequest($"Unexpected resource type { model.Type}");
            }
            return JsonOK(await storageLogic.GetPartUploadPreSignedUrlAsync(type, model.FileName, model.PartSum));
        }

        /// <summary>
        /// 分割アップロードの処理を完了する
        /// </summary>
        [HttpPost("/api/v1/upload/complete")]
        [ProducesResponseType(typeof(CompleteMultiplePartUploadInputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CompleteMultiUploadv2([FromBody]CompleteMultiplePartUploadInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            if (model.PartETags.Count() == 0)
            {
                return JsonBadRequest("The uploaded file was not able to be parted. The file may be empty (0 byte).");
            }
            await storageLogic.CompletPartUploadAsync(model);
            return JsonOK(model);
        }

        /// <summary>
        /// ダウンロード用の一時署名URLを取得する
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <param name="storedPath">保存先パス</param>
        /// <param name="fileName">ダウンロード時に書き換えるファイル名</param>
        /// <param name="secure">HTTPS化するか</param>
        /// <returns>署名付きダウンロードURL</returns>
        [HttpGet("/api/v1/download/url")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public IActionResult GetStorageUrl(string type, string storedPath, string fileName, bool secure)
        {
            ResourceType resourceType;
            if (string.IsNullOrEmpty(storedPath) || Enum.TryParse<ResourceType>(type, true, out resourceType) == false)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            string url = storageLogic.GetPreSignedUriForGet(resourceType, storedPath, fileName, secure == false).ToString();

            return JsonOK(new { Url = url });
        }
        #endregion
    }
}

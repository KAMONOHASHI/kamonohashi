using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.ApiModels.EksApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Models.TenantModels.Aquarium;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// EKS登録管理API
    /// </summary>
    [ApiController]
    [ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/eks")]
    public class EksController : PlatypusApiControllerBase
    {
        // for DI
        private readonly IEksRepository eksRepository;
        private readonly ITenantRepository tenantRepository;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EksController(
            IEksRepository eksRepository,
            ITenantRepository tenantRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.eksRepository = eksRepository;
            this.tenantRepository = tenantRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 指定されたEKS条件を登録する
        /// </summary>
        [HttpPost]
        [Filters.PermissionFilter(MenuCode.Quota)]
        [ProducesResponseType(typeof(EksOutputModel), (int)HttpStatusCode.Created)]
        public IActionResult Create([FromBody] EksInputModel eksInputModel)
        {
            // 入力チェック
            // ポート番号が空欄 (null) の場合は 80 を設定する
            if (!eksInputModel.PortNumber.HasValue)
            {
                eksInputModel.PortNumber = 80;
            }

            // 同一ホスト名のEksがすでに存在する場合はエラーを返す
            var existSameEks =
                eksRepository.GetAll()
                .Where(e => eksInputModel.HostName.Equals(e.HostName))
                .ToList();

            if (existSameEks.Count > 0)
            {
                return JsonBadRequest($"Same hostname EKS data is exist. Hostname: {eksInputModel.HostName}");
            }

            // 指定されたテナントが存在するか確認し、テナントの一覧を取得する
            List<Tenant> tenants = new List<Tenant>();
            foreach (long tenantId in eksInputModel.AnableTenantsId)
            {
                var tenant = tenantRepository.Get(tenantId);
                if (tenant == null)
                {
                    return JsonBadRequest($"Tenant ID {tenantId} is not found.");
                }
                tenants.Add(tenant);
            }

            // DBにセーブする
            // EKS情報の格納
            var eks = new Eks()
            {
                Name = eksInputModel.Name,
                Memo = eksInputModel.Memo,
                HostName = eksInputModel.HostName,
                PortNumber = eksInputModel.PortNumber.Value.ToString(),
                Token = eksInputModel.Token
            };
            eksRepository.Add(eks);

            // EKSとmapping情報の格納
            eksRepository.AttachEksToTenant(tenants, eks, true);

            // DBの変更をコミット
            unitOfWork.Commit();

            // ToDo クラスタ設定 (ネームスペース作成等) を実施する

            return JsonOK(new EksOutputModel(eks, tenants));
        }

        /// <summary>
        /// 指定されたEKS条件を登録する
        /// </summary>
        [HttpPost("{id}")]
        [Filters.PermissionFilter(MenuCode.Quota)]
        [ProducesResponseType(typeof(EksOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Update([FromBody] EksInputModel eksInputModel, long id)
        {
            // 入力チェック
            // ポート番号が空欄 (null) の場合は 80 を設定する
            if (!eksInputModel.PortNumber.HasValue)
            {
                eksInputModel.PortNumber = 80;
            }

            // データの存在チェック
            var eks = await eksRepository.GetByIdAsync(id);
            if (eks == null)
            {
                return JsonBadRequest($"EKS id {id} is not Found");
            }

            // 同一ホスト名のEksがすでに存在する場合はエラーを返す
            var existSameEks =
                eksRepository.GetAll()
                .Where(e => eksInputModel.HostName.Equals(e.HostName))
                .ToList();
            // 元データと同一ホスト名の場合は、同一のホスト名を持つレコードが2つ以上存在するかで判定
            // 異なる場合は、同一ホスト名を持つレコードが1つ以上存在するかで判定
            if (
                (eks.HostName.Equals(eksInputModel.HostName) && existSameEks.Count > 1)
                || (!eks.HostName.Equals(eksInputModel.HostName) && existSameEks.Count > 0)
                )
            {
                return JsonBadRequest($"Same hostname EKS data is exist. Hostname: {eksInputModel.HostName}");
            }

            // 指定されたテナントが存在するか確認し、テナントの一覧を取得する
            List<Tenant> tenants = new List<Tenant>();
            foreach (long tenantId in eksInputModel.AnableTenantsId)
            {
                var tenant = tenantRepository.Get(tenantId);
                if (tenant == null)
                {
                    return JsonBadRequest($"Tenant ID {tenantId} is not found.");
                }
                tenants.Add(tenant);
            }

            // DBにセーブする
            // EKS情報のアップデート
            eks.Id = id;
            eks.Name = eksInputModel.Name;
            eks.Memo = eksInputModel.Memo;
            eks.HostName = eksInputModel.HostName;
            eks.PortNumber = eksInputModel.PortNumber.Value.ToString();
            eks.Token = eksInputModel.Token;
            eksRepository.Update(eks);

            // EKSとmapping情報を一度リセットしてからつけなおす
            eksRepository.ResetEksToTenant(id);
            eksRepository.AttachEksToTenant(tenants, eks, false);

            // DBの変更をコミット
            unitOfWork.Commit();

            // ToDo クラスタ設定 (ネームスペース作成等) を実施する

            return JsonOK(new EksOutputModel(eks, tenants));
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.RoleApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// ロール管理を扱うためのAPI集
    /// </summary>
    [ApiController]
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/tenant/roles")]
    public class RoleController : PlatypusApiControllerBase
    {
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;

        public RoleController(
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Adminロール管理

        /// <summary>
        /// 全ロール一覧を取得
        /// </summary>
        [HttpGet("/api/v{api-version:apiVersion}/admin/roles")]
        [PermissionFilter(MenuCode.Role, MenuCode.User, MenuCode.Menu)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllForAdmin()
        {
            var roles = await roleRepository.GetAllRolesAsync();

            return JsonOK(roles.Select(r => new IndexOutputModel(r)));
        }

        /// <summary>
        /// 指定されたIDのロール情報を取得。
        /// </summary>
        /// <param name="id">ロールID</param>
        /// <param name="tenantRepository">DI用</param>
        [HttpGet("/api/v{api-version:apiVersion}/admin/roles/{id}")]
        [PermissionFilter(MenuCode.Role)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetailForAdmin(long? id, [FromServices] ITenantRepository tenantRepository)
        {
            if (id == null)
            {
                return JsonBadRequest("Role ID is required.");
            }
            var role = await roleRepository.GetRoleAsync(id.Value);
            if (role == null)
            {
                return JsonNotFound($"Node Id {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(role);
            if (model.TenantId != null)
            {
                model.TenantName = tenantRepository.Get(model.TenantId.Value).Name;
            }

            return JsonOK(model);
        }

        /// <summary>
        /// 新規にロールを登録する
        /// </summary>
        [HttpPost("/api/v{api-version:apiVersion}/admin/roles")]
        [PermissionFilter(MenuCode.Role)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateForAdmin([FromBody]CreateInputModel model, [FromServices] ITenantRepository tenantRepository)
        {
            // データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            if (model.IsSystemRole && model.TenantId != null)
            {
                // Admin向けなのにテナント固有にしてあったら入力ミス
                return JsonBadRequest($"Invalid inputs. the role is for admin, but set to specific tenant { model.TenantId }");
            }

            // データの存在チェック
            if (model.TenantId != null && tenantRepository.Get(model.TenantId.Value) == null)
            {
                return JsonNotFound($"Tenant ID {model.TenantId.Value} is not found.");
            }

            return await CreateAsync(model);
        }

        /// <summary>
        /// ロール情報の編集
        /// </summary>
        [HttpPut("/api/v{api-version:apiVersion}/admin/roles/{id}")]
        [PermissionFilter(MenuCode.Role)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditForAdmin(long? id, [FromBody]EditInputModel model, [FromServices] ITenantRepository tenantRepository)
        {
            // データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            if (model.IsSystemRole && model.TenantId != null)
            {
                // Admin向けなのにテナント固有にしてあったら入力ミス
                return JsonBadRequest($"Invalid inputs. The role is for admin, but set to specific tenant { model.TenantId }");
            }

            // データの存在チェック
            Role role = await roleRepository.GetRoleForUpdateAsync(id.Value);
            if (role == null)
            {
                return JsonNotFound($"Role ID {id.Value} is not found.");
            }

            if (model.TenantId != null && tenantRepository.Get(model.TenantId.Value) == null)
            {
                return JsonNotFound($"Tenant ID {model.TenantId.Value} is not found.");
            }

            if (role.IsSystemRole != model.IsSystemRole)
            {
                // ロールの種類は変更できない
                return JsonBadRequest("Invalid inputs. The role type is not allowed to change.");
            }

            // データの編集可否チェック
            if (role.IsNotEditable)
            {
                // 名称と表示名の変更は許可しない。ソート順の変更のみ許可する。
                if (role.Name != model.Name || role.DisplayName != model.DisplayName)
                {
                    return JsonBadRequest($"Role ID {id.Value} is not editable. Only the sort order is allowed to edit.");
                }
            }

            // 同じ名前のロールは登録できないので、確認する
            Role registeredRole = (await roleRepository.GetAllRolesAsync()).FirstOrDefault(r => r.Name == model.Name && r.Id != id.Value);
            if (registeredRole != null)
            {
                return JsonConflict($"Role {model.Name} already exists: ID = {registeredRole.Id}");
            }

            role.Name = model.Name;
            role.DisplayName = model.DisplayName;
            role.IsSystemRole = model.IsSystemRole;
            if (role.IsSystemRole)
            {
                role.TenantId = null;
            }
            else
            {
                role.TenantId = model.TenantId;
            }
            role.SortOrder = model.SortOrder.Value;

            roleRepository.Update(role, unitOfWork);

            unitOfWork.Commit();

            return JsonOK(new IndexOutputModel(role));
        }

        /// <summary>
        /// ロールを削除する。
        /// </summary>
        [HttpDelete("/api/v{api-version:apiVersion}/admin/roles/{id}")]
        [PermissionFilter(MenuCode.Role)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteForAdmin(long? id, [FromServices] ITenantRepository tenantRepository)
        {
            // データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            // データの存在チェック
            var role = await roleRepository.GetRoleAsync(id.Value);
            if (role == null)
            {
                return JsonNotFound($"Role ID {id.Value} is not found.");
            }

            // データの編集可否チェック
            if (role.IsNotEditable)
            {
                return JsonBadRequest($"Role ID {id.Value} is not allowed to delete.");
            }

            await roleRepository.DeleteAsync(id.Value, unitOfWork);

            unitOfWork.Commit();

            return JsonNoContent();
        }

        #endregion

        #region テナントロール管理

        /// <summary>
        /// 現在選択中のテナントで利用可能なロール一覧を取得。
        /// </summary>
        /// <remarks>
        /// システムロール以外の共通ロールと、テナント用カスタムロールが対象。
        /// </remarks>
        [HttpGet]
        [PermissionFilter(MenuCode.TenantRole, MenuCode.TenantUser)] // テナントユーザ画面からも参照する
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllForTenant()
        {
            var roles = await roleRepository.GetAllRolesAsync();

            // テナントで使用可能なロールの条件
            //  - Adminロールでない
            //  - TenantIdがNULLまたは現在選択中のテナントと一致
            var rolesForCurrentTenant = roles.Where(
                r => r.IsSystemRole == false &&
                (r.TenantId == null || r.TenantId == CurrentUserInfo.SelectedTenant.Id));

            return JsonOK(rolesForCurrentTenant.Select(r => new IndexOutputModel(r)));
        }

        /// <summary>
        /// 指定されたIDのテナント用カスタムロール情報を取得。
        /// </summary>
        /// <param name="id">ロールID</param>
        [HttpGet("{id}")]
        [PermissionFilter(MenuCode.TenantRole)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetailForTenant(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Role ID is required.");
            }

            var role = await roleRepository.GetRoleAsync(id.Value);
            if (role == null)
            {
                return JsonNotFound($"Role Id {id.Value} is not found.");
            }

            if (role.IsSystemRole == true || (role.TenantId != null && role.TenantId != CurrentUserInfo.SelectedTenant.Id))
            {
                // 参照不可のロールにアクセスしようとしている
                LogWarning($"Role {role.Name} is not allowed to read by the current user.");
                return JsonNotFound($"Role Id {id.Value} is not found."); // エラーメッセージは404と変えない
            }

            var model = new DetailsOutputModel(role);
            if (model.TenantId != null)
            {
                model.TenantName = CurrentUserInfo.SelectedTenant.Name;
            }

            return JsonOK(model);
        }

        /// <summary>
        /// 新規にロールを登録する
        /// </summary>
        [HttpPost]
        [PermissionFilter(MenuCode.TenantRole)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateForTenant([FromBody]CreateForTenantInputModel model)
        {
            // データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            return await CreateAsync(new CreateInputModel()
            {
                Name = model.Name,
                DisplayName = model.DisplayName,
                SortOrder = model.SortOrder,
                IsSystemRole = false,
                TenantId = CurrentUserInfo.SelectedTenant.Id
            });
        }

        /// <summary>
        /// テナント用カスタムロール情報の編集
        /// </summary>
        [HttpPut("{id}")]
        [PermissionFilter(MenuCode.TenantRole)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditForTenant(long? id, [FromBody]EditForTenantInputModel model)
        {
            // データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            // データの存在チェック
            Role role = await roleRepository.GetRoleForUpdateAsync(id.Value);
            if (role == null)
            {
                return JsonNotFound($"Role ID {id.Value} is not found.");
            }

            // データの編集可否チェック
            if (role.IsNotEditable)
            {
                // 名称と表示名の変更は許可しない。ソート順の変更のみ許可する。
                if (role.Name != model.Name || role.DisplayName != model.DisplayName)
                {
                    return JsonBadRequest($"Role ID {id.Value} is not editable. Only the sort order is allowed to edit.");
                }
            }

            // システムロール、共通テナントロール、他のテナントのテナントロールは編集不可
            if (role.IsSystemRole == true || role.TenantId == null || role.TenantId != CurrentUserInfo.SelectedTenant.Id)
            {
                // 参照不可のロールにアクセスしようとしている
                LogWarning($"Role {role.Name} is not allowed to edit by the current user.");
                return JsonNotFound($"Role Id {id.Value} is not found."); // エラーメッセージは404と変えない
            }

            // 同じ名前のロールは登録できないので、確認する
            Role registeredRole = (await roleRepository.GetAllRolesAsync()).FirstOrDefault(r => r.Name == model.Name && r.Id != id.Value);
            if (registeredRole != null)
            {
                return JsonConflict($"Role {model.Name} already exists: ID = {registeredRole.Id}");
            }

            role.Name = model.Name;
            role.DisplayName = model.DisplayName;
            role.IsSystemRole = false;
            role.SortOrder = model.SortOrder.Value;

            roleRepository.Update(role, unitOfWork);

            return JsonOK(new IndexOutputModel(role));
        }

        /// <summary>
        /// テナント用カスタムロールを削除する。
        /// </summary>
        [HttpDelete("{id}")]
        [PermissionFilter(MenuCode.TenantRole)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteForTenant(long? id)
        {
            // データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            // データの存在チェック
            var role = await roleRepository.GetRoleAsync(id.Value);
            if (role == null)
            {
                return JsonNotFound($"Role ID {id.Value} is not found.");
            }

            // データの編集可否チェック
            if (role.IsNotEditable)
            {
                return JsonBadRequest($"Role ID {id.Value} is not allowed to delete.");
            }

            // システムロール、共通テナントロール、他のテナントのテナントロールは削除不可
            if (role.IsSystemRole == true || role.TenantId == null || role.TenantId != CurrentUserInfo.SelectedTenant.Id)
            {
                // 参照不可のロールにアクセスしようとしている
                LogWarning($"Role {role.Name} is not allowed to delete by the current user.");
                return JsonNotFound($"Role Id {id.Value} is not found."); // エラーメッセージは404と変えない
            }

            await roleRepository.DeleteAsync(id.Value, unitOfWork);

            unitOfWork.Commit();

            return JsonNoContent();
        }

        #endregion

        /// <summary>
        /// 新規にロールを登録する。
        /// 単体での入力値チェックは事前にやっておくこと。
        /// </summary>
        private async Task<IActionResult> CreateAsync(CreateInputModel model)
        {
            // 同じ名前のロールは登録できないので、確認する
            Role role = (await roleRepository.GetAllRolesAsync()).FirstOrDefault(r => r.Name == model.Name);
            if (role != null)
            {
                return JsonConflict($"Role {model.Name} already exists: ID = {role.Id}");
            }

            role = new Role()
            {
                Name = model.Name,
                DisplayName = model.DisplayName,
                SortOrder = model.SortOrder.Value,
                IsSystemRole = model.IsSystemRole,
                TenantId = model.TenantId
            };

            roleRepository.Add(role, unitOfWork);

            return JsonCreated(new IndexOutputModel(role));
        }
    }
}

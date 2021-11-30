using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.MenuApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// メニューアクセス管理を扱うためのAPI集
    /// </summary>
    [ApiController]
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/menu")]
    public class MenuController : PlatypusApiControllerBase
    {
        private readonly IMenuLogic menuLogic;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;

        public MenuController(
            IMenuLogic menuLogic,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.menuLogic = menuLogic;
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        }


        /// <summary>
        /// テナント向けに、メニューとロールのマッピング情報一覧を取得
        /// </summary>
        [HttpGet("/api/v{api-version:apiVersion}/tenant/menus")]
        [PermissionFilter(MenuCode.TenantMenu)]
        [ProducesResponseType(typeof(IEnumerable<MenuForTenantOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetMenuRoleMapForTenant()
        {
            var tenantId = CurrentUserInfo.SelectedTenant.Id;

            var results = new List<MenuForTenantOutputModel>();

            //表示情報をロジック層から取得。ここではAPIモデルへの詰替えだけ。
            var dict = menuLogic.GetRoleIdsForTenantDictionary(tenantId);
            foreach (var pair in dict)
            {
                var result = new MenuForTenantOutputModel()
                {
                    Id = pair.Key.Code,
                    Name = pair.Key.Name,
                    Description = pair.Key.Description,
                    MenuType = pair.Key.MenuType,
                    Roles = pair.Value.Select(r => new MenuForTenantOutputModel.RoleModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Editable = r.TenantId == tenantId
                    })
                };
                results.Add(result);
            }

            return JsonOK(results);
        }

        /// <summary>
        /// テナント向けの、メニューとロールのマッピング情報を更新
        /// </summary>
        [HttpPut("/api/v{api-version:apiVersion}/tenant/menus/{id}")]
        [PermissionFilter(MenuCode.TenantMenu)]
        [ProducesResponseType(typeof(MenuForTenantOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditMenuRoleMapForTenant([FromRoute] MenuCode? id, [FromBody] IEnumerable<long> roleIds, [FromServices] IMenuRepository menuRepository)
        {
            if (id == null)
            {
                return JsonBadRequest("Menu Id is required.");
            }

            var menu = menuLogic.GetMenu(id.Value);
            if (menu == null)
            {
                return JsonNotFound($"Menu Id {id.Value} is not found.");
            }

            //まずは関係するロールマップをすべて削除
            await menuRepository.DeleteMenuMapAsync(menu, CurrentUserInfo.SelectedTenant.Id);

            foreach (var roleId in roleIds)
            {
                var role = await roleRepository.GetRoleAsync(roleId);
                if (role == null)
                {
                    return JsonNotFound($"Role Id {roleId} is not found.");
                }

                if (role.IsSystemRole)
                {
                    //システムメニューはテナント側で紐づけできない。警告出して404扱い。
                    LogWarning($"Role {role.Name} is not allowed to edit by the current user.");
                    return JsonNotFound($"Role Id {roleId} is not found.");
                }
                else
                {
                    if (role.TenantId != CurrentUserInfo.SelectedTenant.Id)
                    {
                        //別のテナントのカスタムロールを編集しようとしている。警告出して404扱い。
                        LogWarning($"Role {role.Name} is not allowed to edit by the current user.");
                        return JsonNotFound($"Role Id {roleId} is not found.");
                    }
                    if (menu.MenuType != MenuType.Tenant)
                    {
                        //テナントメニュー以外は紐づけできない（Public/Internal/Unknownは紐づける必要がないから）
                        JsonConflict($"A tenant menu is only attached to a tenant role.");
                    }
                    menuRepository.AttachRole(menu, role);
                }
            }

            unitOfWork.Commit();
            roleRepository.Refresh(); //キャッシュを破棄

            //表示情報を取得
            var result = new MenuForTenantOutputModel()
            {
                Id = menu.Code,
                Name = menu.Name,
                Description = menu.Description,
                Roles = menuRepository.GetAttachedRoles(menu.Code).Select(r => new MenuForTenantOutputModel.RoleModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                })
            };

            return JsonOK(result);
        }

        /// <summary>
        /// テナント向けのメニュー種別一覧を取得
        /// </summary>
        [HttpGet("/api/v{api-version:apiVersion}/tenant/menu-types")]
        [PermissionFilter(MenuCode.TenantMenu)]
        [ProducesResponseType(typeof(IEnumerable<EnumInfo>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllTypesForTenant()
        {
            var menus = new List<MenuType>()
            {
                MenuType.Internal,
                MenuType.Tenant,
                MenuType.Public
            };

            return JsonOK(menus.Select(m => new EnumInfo() { Id = (int)m, Name = m.ToString() }));
        }

        /// <summary>
        /// 管理者向けに、メニューとロールのマッピング情報一覧を取得
        /// </summary>
        [HttpGet("/api/v{api-version:apiVersion}/admin/menus")]
        [PermissionFilter(MenuCode.Menu)]
        [ProducesResponseType(typeof(IEnumerable<MenuForAdminOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetMenuRoleMapForAdmin()
        {
            var results = new List<MenuForAdminOutputModel>();

            //表示情報をロジック層から取得。ここではAPIモデルへの詰替えだけ。
            var dict = menuLogic.GetRoleIdsForAdminDictionary();
            foreach (var pair in dict)
            {
                var result = new MenuForAdminOutputModel()
                {
                    Id = pair.Key.Code,
                    Name = pair.Key.Name,
                    Description = pair.Key.Description,
                    MenuType = pair.Key.MenuType,
                    Roles = pair.Value.Select(r => new MenuForAdminOutputModel.RoleModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        IsSystemRole = r.IsSystemRole
                    })
                };
                results.Add(result);
            }

            return JsonOK(results);
        }

        /// <summary>
        /// 管理者向けの、メニューとロールのマッピング情報を更新
        /// </summary>
        [HttpPut("/api/v{api-version:apiVersion}/admin/menus/{id}")]
        [PermissionFilter(MenuCode.Menu)]
        [ProducesResponseType(typeof(MenuForAdminOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditMenuRoleMapForAdmin([FromRoute] MenuCode? id, [FromBody] IEnumerable<long> roleIds, [FromServices] IMenuRepository menuRepository)
        {
            if (id == null)
            {
                return JsonBadRequest("Menu Id is required.");
            }

            var menu = menuLogic.GetMenu(id.Value);
            if (menu == null)
            {
                return JsonNotFound($"Menu Id {id.Value} is not found.");
            }

            //まずは関係するロールマップをすべて削除
            menuRepository.DeleteMenuMap(menu);

            foreach (var roleId in roleIds)
            {
                var role = await roleRepository.GetRoleAsync(roleId);
                if (role == null)
                {
                    return JsonNotFound($"Role Id {roleId} is not found.");
                }

                if (role.IsSystemRole)
                {
                    if (menu.MenuType != MenuType.System)
                    {
                        //システムメニュー以外はシステムロールを紐づけできない
                        return JsonConflict($"A system menu is only attached to the system role {role.Id}");
                    }
                    menuRepository.AttachRole(menu, role);
                }
                else
                {
                    if (role.TenantId != null)
                    {
                        //テナント用カスタムロールを管理者が編集可能か、というのは議論があるが、今はUI的に表示していないハズなので、不正と見なして弾く
                        return JsonConflict($"Role {role.Name} is a custome tenant role for Tenant {role.TenantId}");
                    }
                    if (menu.MenuType != MenuType.Tenant)
                    {
                        //テナントメニュー以外は紐づけできない（Public/Internal/Unknownは紐づける必要がないから）
                        return JsonConflict($"A tenant menu is only attached to a tenant role.");
                    }
                    menuRepository.AttachRole(menu, role);
                }
            }

            unitOfWork.Commit();
            roleRepository.Refresh(); //キャッシュを破棄

            //表示情報を取得
            var result = new MenuForAdminOutputModel()
            {
                Id = menu.Code,
                Name = menu.Name,
                Description = menu.Description,
                MenuType = menu.MenuType,
                Roles = menuRepository.GetAttachedRoles(menu.Code).Select(r => new MenuForAdminOutputModel.RoleModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSystemRole = r.IsSystemRole
                })
            };

            return JsonOK(result);
        }

        /// <summary>
        /// 管理者向けメニュー種別一覧を取得
        /// </summary>
        [HttpGet("/api/v{api-version:apiVersion}/admin/menu-types")]
        [PermissionFilter(MenuCode.Menu)]
        [ProducesResponseType(typeof(IEnumerable<EnumInfo>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllTypesForSystem()
        {
            var menuTypes = Enum.GetValues(typeof(MenuType)) as MenuType[];

            return JsonOK(menuTypes.Where(m => m != MenuType.Unknown).Select(m => new EnumInfo() { Id = (int)m, Name = m.ToString() }));
        }
    }
}

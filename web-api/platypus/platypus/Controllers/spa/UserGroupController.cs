﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.UserGroupApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// ユーザグループ管理を扱うためのAPI集
    /// </summary>
    [ApiController]
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/admin/usergroup")]
    public class UserGroupController : PlatypusApiControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IUserGroupRepository userGroupRepository;
        private readonly ITenantRepository tenantRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserGroupController(
            IUserRepository userRepository,
            IUserGroupRepository userGroupRepository,
            ITenantRepository tenantRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.userRepository = userRepository;
            this.userGroupRepository = userGroupRepository;
            this.tenantRepository = tenantRepository;
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// ユーザグループ一覧を取得する
        /// </summary>
        [HttpGet]
        [Filters.PermissionFilter(MenuCode.UserGroup, MenuCode.Tenant)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var result = userGroupRepository.GetAll().OrderBy(u => u.Id);

            return JsonOK(result.Select(r => new IndexOutputModel(r)));
        }

        /// <summary>
        /// 指定されたIDのユーザグループ情報を取得する
        /// </summary>
        /// <param name="id">ユーザグループID</param>
        [HttpGet("{id}")]
        [Filters.PermissionFilter(MenuCode.UserGroup)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public IActionResult GetDetails(long? id)
        {
            // 入力チェック
            if (!id.HasValue)
            {
                return JsonBadRequest("UserGroup ID is required.");
            }
            // データの存在チェック
            var userGroup = userGroupRepository.GetUserGroupById(id.Value);
            if(userGroup == null)
            {
                return JsonNotFound($"UserGroup ID {id.Value} is not found.");
            }

            return JsonOK(new DetailsOutputModel(userGroup));
        }

        /// <summary>
        /// 新規にユーザグループを登録する
        /// </summary>
        /// <param name="model">ユーザグループ入力モデル</param>
        [HttpPost]
        [Filters.PermissionFilter(MenuCode.UserGroup)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateInputModel model)
        {
            // データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            // ロールの入力チェック
            var roles = await ValidateRoles(model.RoleIds);
            if (!roles.IsSuccess)
            {
                return JsonBadRequest(roles.Error);
            }

            var userGroup = new UserGroup()
            {
                Name = model.Name,
                Memo = model.Memo,
                IsGroup = model.IsGroup,
                Dn = model.Dn,
                IsDirect = model.IsDirect,
            };

            userGroupRepository.AttachRoleMap(userGroup, roles.Value);
            userGroupRepository.Add(userGroup);
            unitOfWork.Commit();

            return JsonOK(new DetailsOutputModel(userGroup));
        }

        /// <summary>
        /// 既存のユーザグループを編集する
        /// </summary>
        /// <param name="id">ユーザグループID</param>
        /// <param name="model">ユーザグループ入力モデル</param>
        [HttpPut("{id}")]
        [Filters.PermissionFilter(MenuCode.UserGroup)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody] CreateInputModel model)
        {
            // データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            // データの存在チェック
            var userGroup = await userGroupRepository.GetByIdAsync(id.Value);
            if (userGroup == null)
            {
                return JsonNotFound($"UserGroup ID {id.Value} is not found.");
            }

            // ロールの入力チェック
            var roles = await ValidateRoles(model.RoleIds);
            if (!roles.IsSuccess)
            {
                return JsonBadRequest(roles.Error);
            }

            userGroup.Id = id.Value;
            userGroup.Name = model.Name;
            userGroup.Memo = model.Memo;
            userGroup.IsGroup = model.IsGroup;
            userGroup.Dn = model.Dn;
            userGroup.IsDirect = model.IsDirect;

            userGroupRepository.AttachRoleMap(userGroup, roles.Value);
            userGroupRepository.Update(userGroup);

            // ユーザグループ自体の更新を確定させるため、更新内容を一旦コミットする
            unitOfWork.Commit();

            // 影響のあるテナントに所属するLDAPユーザのロール情報を更新する
            var tenants = userGroupRepository.GetTenantsByUserGroup(id.Value).ToList();
            foreach(var tenant in tenants)
            {
                var users = userRepository.GetLdapUsers(tenant.Id).ToList();
                foreach(var user in users)
                {
                    userRepository.UpdateLdapRole(user.Id, tenant.Id);
                }
            }

            unitOfWork.Commit();

            return JsonOK(new DetailsOutputModel(userGroup));
        }

        /// <summary>
        /// ユーザグループを削除する
        /// </summary>
        /// <param name="id">ユーザグループID</param>
        [HttpDelete("{id}")]
        [Filters.PermissionFilter(MenuCode.UserGroup)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id)
        {
            // データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            // データの存在チェック
            var userGroup = await userGroupRepository.GetByIdAsync(id.Value);
            if (userGroup == null)
            {
                return JsonNotFound($"UserGroup ID {id.Value} is not found.");
            }

            // 影響のあるテナントに所属するLDAPユーザの紐づけを解除する
            var tenants = userGroupRepository.GetTenantsByUserGroup(id.Value).ToList();
            foreach (var tenant in tenants)
            {
                var users = userRepository.GetLdapUsers(tenant.Id).ToList();
                foreach (var user in users)
                {
                    userRepository.DetachUserGroup(user, tenant.Id, userGroup.Id);
                }
            }

            userGroupRepository.Delete(userGroup);
            unitOfWork.Commit();
            // テナントで持っているユーザグループ情報を更新するため
            tenantRepository.Refresh();

            return JsonNoContent();
        }

        /// <summary>
        /// ロールIDの入力チェックを行い、ロール情報をリストで返す
        /// </summary>
        /// <param name="roleIds">ロールIDリスト</param>
        private async Task<Result<IEnumerable<Role>, string>> ValidateRoles(IEnumerable<long> roleIds)
        {
            // 空チェック
            if(roleIds.Count() == 0)
            {
                return Result<IEnumerable<Role>, string>.CreateErrorResult("The RoleIds field is required.");
            }

            List<Role> roles = new List<Role>();
            foreach(var roleId in roleIds)
            {
                // ロールの存在チェック
                var role = await roleRepository.GetRoleAsync(roleId);
                if (role == null)
                {
                    return Result<IEnumerable<Role>, string>.CreateErrorResult($"Role ID {roleId} is not found.");
                }
                // システムロールは登録できないようにする
                if (role.IsSystemRole)
                {
                    return Result<IEnumerable<Role>, string>.CreateErrorResult($"The system role {role.Name} is not assigned as a tenant role.");
                }
                roles.Add(role);
            }
            return Result<IEnumerable<Role>, string>.CreateResult(roles);
        }
    }
}

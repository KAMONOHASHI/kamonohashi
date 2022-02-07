using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.UserGroupApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System;
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
        private readonly IUserGroupRepository userGroupRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserGroupController(
            IUserGroupRepository userGroupRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.userGroupRepository = userGroupRepository;
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 登録されているユーザグループ一覧を取得する
        /// </summary>
        [HttpGet]
        [Filters.PermissionFilter(MenuCode.UserGroup)]
        [ProducesResponseType(typeof(IEnumerable<UserGroupOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var result = userGroupRepository.GetUserGroupsAllWithRoles();

            return JsonOK(result.Select(r => new UserGroupOutputModel(r)));
        }

        /// <summary>
        /// 新規にユーザグループを登録する
        /// </summary>
        /// <param name="model">ユーザグループ入力モデル</param>
        [HttpPost]
        [Filters.PermissionFilter(MenuCode.UserGroup)]
        [ProducesResponseType(typeof(UserGroupOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateInputModel model)
        {
            // データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            // ロールの入力チェック
            var error = await ValidateRoles(model.RoleIds);
            if (error != null)
            {
                return JsonBadRequest(error);
            }

            var userGroup = new UserGroup()
            {
                Name = model.Name,
                Memo = model.Memo,
                IsGroup = model.IsGroup,
                Dn = model.Dn,
                IsDirect = model.IsDirect,
            };

            userGroupRepository.AttachRoleMap(userGroup, model.RoleIds);
            userGroupRepository.Add(userGroup);
            unitOfWork.Commit();

            return JsonOK(new UserGroupOutputModel(userGroup));
        }

        /// <summary>
        /// 既存のユーザグループを編集する
        /// </summary>
        /// <param name="id">ユーザグループID</param>
        /// <param name="model">ユーザグループ入力モデル</param>
        [HttpPut("{id}")]
        [Filters.PermissionFilter(MenuCode.UserGroup)]
        [ProducesResponseType(typeof(UserGroupOutputModel), (int)HttpStatusCode.OK)]
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
            var error = await ValidateRoles(model.RoleIds);
            if (error != null)
            {
                return JsonBadRequest(error);
            }

            userGroup.Name = model.Name;
            userGroup.Memo = model.Memo;
            userGroup.IsGroup = model.IsGroup;
            userGroup.Dn = model.Dn;
            userGroup.IsDirect = model.IsDirect;

                userGroupRepository.AttachRoleMap(userGroup, model.RoleIds);
                userGroupRepository.Update(userGroup);
                unitOfWork.Commit();

            return JsonOK(new UserGroupOutputModel(userGroup));
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

            // TODO 関連テーブルのレコード削除

            userGroupRepository.Delete(userGroup);
            unitOfWork.Commit();

            return JsonNoContent();
        }

        /// <summary>
        /// ロール情報の入力チェックを行う
        /// </summary>
        private async Task<string> ValidateRoles(IEnumerable<long> roleIds)
        {
            //List<Role> roles = new List<Role>();
            foreach(var roleId in roleIds)
            {
                // ロールの存在チェック
                var role = await roleRepository.GetRoleAsync(roleId);
                if (role == null)
                {
                    return $"ロールID {roleId}が存在しません。";
                }
                if (role.IsSystemRole)
                {
                    // システムロールは登録できないようにする
                    return "システムロールは登録できません。";
                }
                //roles.Add(role);
            }
            return null;
        }
    }
}

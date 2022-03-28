using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.UserApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// ユーザ管理を扱うためのAPI集
    /// </summary>
    [ApiController]
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/admin/users")]
    public class UserController : PlatypusApiControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IUserGroupLogic userGroupLogic;

        public UserController(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IClusterManagementLogic clusterManagementLogic,
            IUserGroupLogic userGroupLogic,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
            this.clusterManagementLogic = clusterManagementLogic;
            this.userGroupLogic = userGroupLogic;
        }

        #region ユーザ管理

        /// <summary>
        /// 管理者向けにユーザの一覧を取得する。
        /// </summary>
        [HttpGet]
        [PermissionFilter(MenuCode.User)]
        [ProducesResponseType(typeof(IEnumerable<IndexForAdminOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllUsersForAdmin()
        {
            //ユーザ情報単体を取得
            var users = userRepository.GetAllUsersWithTenant();
            //ロール情報・テナント情報と紐づけて、返す
            var result = users.Select(u => CraeteIndexOutputModel(u)).ToList();
            return JsonOK(result);
        }

        /// <summary>
        /// 管理者向けに指定したユーザの情報を取得する。
        /// </summary>
        [HttpGet("{id}")]
        [PermissionFilter(MenuCode.User)]
        [ProducesResponseType(typeof(IndexForAdminOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserForAdmin(long? id)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("User ID is required.");
            }
            //データの存在チェック
            var user = await userRepository.GetByIdAsync(id.Value);
            if (user == null)
            {
                return JsonNotFound($"User ID {id} is not found.");
            }

            //ロール情報・テナント情報と紐づけて、返す
            return JsonOK(CraeteIndexOutputModel(user));
        }

        private IndexForAdminOutputModel CraeteIndexOutputModel(User user)
        {
            var userModel = new IndexForAdminOutputModel(user);

            //ユーザのテナント情報（ロール含む）を取得
            var info = userRepository.GetUserInfo(user);
            userModel.Tenants = info.TenantDic.Select(x => new TenantInfo(x.Key, x.Value, user.DefaultTenantId) { IsOrigin = userRepository.IsOriginMember(user.Id,x.Key.Id)}).OrderBy(t => t.DisplayName).ToList();

            //このユーザのシステムロールを取得
            userModel.SystemRoles = roleRepository.GetSystemRoles(user.Id);

            return userModel;
        }

        /// <summary>
        /// ユーザをローカルアカウントとして新規追加する
        /// </summary>
        [HttpPost]
        [PermissionFilter(MenuCode.User)]
        [ProducesResponseType(typeof(IndexForAdminOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateUser([FromBody] CreateInputModel model, [FromServices] ITenantRepository tenantRepository)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                //パスワードに空文字は許可しない
                return JsonBadRequest($"Password is NOT allowed to set empty string.");
            }

            // 名前の前後の空白は除去
            model.Name = model.Name.Trim();

            //データの存在チェック
            User user = userRepository.GetUser(model.Name);
            if (user != null)
            {
                //同名のユーザがいたら失敗
                return JsonNotFound($"Invalid user name.");
            }
            //デフォルトがあるかチェック（必然的にテナントが一つ以上あるかのチェックにもなる）
            var defaultTenant = model.Tenants.FirstOrDefault(x => x.Default);
            if (defaultTenant == null)
            {
                return JsonNotFound($"Invalid default tenant exists.");
            }

            user = new User()
            {
                Name = model.Name,
                Password = Infrastructure.Util.GenerateHash(model.Password, model.Name),
                ServiceType = AuthServiceType.Local,
                DefaultTenantId = defaultTenant.Id.Value
            };

            //システムロールの登録
            var addSystemRoleErrorResult = await AddSystemRolesAsync(user, model.SystemRoles, true, true);
            if (addSystemRoleErrorResult != null)
            {
                return addSystemRoleErrorResult;
            }

            foreach (var tenantInput in model.Tenants)
            {
                // テナントの存在確認
                Tenant tenant = tenantRepository.Get(tenantInput.Id.Value);
                if (tenant == null)
                {
                    // 指定したテナントが存在しなかったら失敗
                    return JsonNotFound($"Tenant ID {tenantInput.Id} is not found.");
                }
                // 関連 map の作成
                var addTenantErrorResult = await AddTenantAsync(user, tenant, tenantInput.Roles, true);
                if (addTenantErrorResult != null)
                {
                    return addTenantErrorResult;
                }
            }

            // ユーザの登録
            userRepository.AddUser(user);
            unitOfWork.Commit();

            return JsonCreated(CraeteIndexOutputModel(user));
        }

        /// <summary>
        /// 指定したユーザを削除する
        /// </summary>
        [HttpDelete("{id}")]
        [PermissionFilter(MenuCode.User)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteUserAsync(long? id)
        {
            if (id == null)
            {
                //IDが指定されていなければエラー
                return JsonBadRequest("Invalid inputs.");
            }
            if (id.Value == CurrentUserInfo.Id)
            {
                //自分自身を消すと、ログイン中の処理がおかしくなるので、却下
                return JsonBadRequest("Invalid inputs. Not allowed to delete yourself.");
            }

            //データの存在チェック
            User user = await userRepository.GetByIdAsync(id.Value);
            if (user == null)
            {
                return JsonNotFound($"User ID {id} is not found.");
            }
            if (user.Name == ApplicationConst.DefaultFirstAdminUserName)
            {
                //デフォルトのAdminは消せない
                return JsonNotFound($"{ApplicationConst.DefaultFirstAdminUserName} is a super user you can NOT delete.");
            }

            userRepository.DeleteUser(user);

            unitOfWork.Commit();

            return JsonNoContent();
        }


        /// <summary>
        /// 指定したユーザを編集する
        /// </summary>
        [HttpPut("{id}")]
        [PermissionFilter(MenuCode.User)]
        [ProducesResponseType(typeof(IndexForAdminOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditUser(long? id, [FromBody] EditInputModel model, [FromServices] ITenantRepository tenantRepository)
        {
            //データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var user = await userRepository.GetByIdAsync(id.Value);
            if (user == null)
            {
                return JsonNotFound($"User ID {id} is not found.");
            }
            //デフォルトがあるかチェック（必然的にテナントが一つ以上あるかのチェックにもなる）
            var defaultTenant = model.Tenants.FirstOrDefault(x => x.Default);
            if (defaultTenant == null)
            {
                return JsonNotFound($"Invalid default tenant exists.");
            }

            //とりあえずすべてのシステムロールを一度外す
            roleRepository.DetachSystemRole(user.Id);
            var addSystemRoleErrorResult = await AddSystemRolesAsync(user, model.SystemRoles, false, true);
            if (addSystemRoleErrorResult != null)
            {
                return addSystemRoleErrorResult;
            }

            //現在登録されているテナントを抽出
            var info = userRepository.GetUserInfo(user);
            var currentTenants = info.TenantDic.Keys.ToList();
            // テナントの登録
            foreach (var tenantInput in model.Tenants)
            {
                // テナントの存在確認
                Tenant tenant = tenantRepository.Get(tenantInput.Id.Value);
                if (tenant == null)
                {
                    //指定したテナントが存在しなかったら失敗
                    return JsonNotFound($"Tenant ID {tenantInput.Id} is not found.");
                }

                // ロールの空チェック(Ldap経由で紐づいているテナントはロールが空になっている)
                if(tenantInput.Roles.Count() <= 0)
                {
                    continue;
                }

                // このテナントが既に紐づけられているか確認
                Tenant currentTenant = currentTenants.FirstOrDefault(t => t.Id == tenantInput.Id);
                if (currentTenant != null)
                {
                    // すでに紐づけられている場合、ロールが変更されている可能性があるので、更新処理を行う
                    await UpdateTenantAsync(user, tenant, tenantInput.Roles, true);
                    //候補から外す
                    currentTenants.Remove(currentTenant);
                }

                // 新規の紐づけの場合はこちら。
                else
                {
                    var addTenantErrorResult = await AddTenantAsync(user, tenant, tenantInput.Roles, true);
                    if (addTenantErrorResult != null)
                    {
                        //ロールバックされるので、不整合は起こらない
                        return addTenantErrorResult;
                    }
                }
            }
            //残っているのは削除対象
            foreach (var removedTenant in currentTenants)
            {
                //自分自身を接続中のテナントから外そうとしていたらエラー（処理が継続できない）
                if (CurrentUserInfo.Id == user.Id && CurrentUserInfo.SelectedTenant.Id == removedTenant.Id)
                {
                    return JsonConflict($"You are NOT allowed removing yourself from the currently connected tenant.");
                }
                // KQI上の紐づけを外す
                userRepository.DetachOriginTenant(user, removedTenant.Id);
            }

            // デフォルトテナントの変更
            user.DefaultTenantId = defaultTenant.Id.Value;

            unitOfWork.Commit();

            var userModel = new IndexForAdminOutputModel(user);
            userModel.SystemRoles = roleRepository.GetSystemRoles(user.Id);
            return JsonOK(userModel);
        }

        /// <summary>
        /// 指定したユーザのパスワードを変更する
        /// </summary>
        [HttpPut("{id}/password")]
        [PermissionFilter(MenuCode.User)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ChangePassword([FromRoute] long id, [FromBody] string password)
        {
            //データの存在チェック
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return JsonNotFound($"User ID {id} is not found.");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                //新しいパスワードに空文字は許可しない
                return JsonBadRequest($"Password is NOT allowed to set empty string.");
            }

            //パスワードをハッシュ化
            string hash = Infrastructure.Util.GenerateHash(password, user.Name);
            user.Password = hash;

            unitOfWork.Commit();

            return JsonNoContent();
        }

        /// <summary>
        /// LDAPサーバに問い合わせを行い、各ユーザの権限を更新する
        /// </summary>
        /// <param name="model">LDAP認証情報入力モデル</param>
        [HttpPost("sync-ldap")]
        [Filters.PermissionFilter(MenuCode.User)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> SyncLdapUser([FromBody] LdapAuthenticationInputModel model)
        {
            // 全LDAPユーザの取得
            var users = userRepository.GetAllUsersWithTenant().Where(user => user.ServiceType == AuthServiceType.Ldap).ToList();

            foreach (var user in users)
            {
                // LDAPサーバ上のユーザ情報を取得
                var result = userGroupLogic.Authenticate(user, model.UserName, model.Password);
                if (!result.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        // 認証情報に誤りがあったときにエラーを返す
                        return JsonBadRequest(result.Error);
                    }
                    // ユーザ情報が取得できなかったとき、LDAP由来のテナントから脱退する
                    LogInformation($"LDAPサーバにユーザ: {user.Name} は見つかりませんでした。");
                    var tenants = userRepository.GetTenantByUser(user.Id).ToList();
                    if (tenants != null && tenants.Count > 0)
                    {
                        foreach (var tenant in tenants)
                        {
                            if (userRepository.IsOriginMember(user.Id, tenant.Id))
                            {
                                // KQIの紐づけがあるとき
                                // 関連テーブルのUserGroupTenantMapIdsをnullにする
                                userRepository.UpdateTenant(user, tenant.Id, null, false, null);
                                // ロールの更新
                                userRepository.UpdateLdapRole(user.Id, tenant.Id);
                            }
                            else
                            {
                                // KQIの紐づけがないとき
                                userRepository.DetachTenant(user.Id, tenant.Id, false);
                            }
                        }
                        LogInformation($"ユーザ: {user.Name} がLDAP経由で参加したテナントの紐づけを解除しました。");
                    }
                }
                else
                {
                    await userGroupLogic.AddTenantFromGroup(result.Value, user, model.UserName, model.Password);
                }
            }
            unitOfWork.Commit();

            return JsonNoContent();
        }

        /// <summary>
        /// 指定したユーザにシステムロールを新規に付与する。
        /// 途中でエラーが発生した場合、そのエラー結果が返る。NULLなら成功。
        /// 編集の場合、事前に削除処理を行っておくこと。
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        /// <param name="systemRoleIds">システムロールID</param>
        /// <param name="isCreate">ユーザが新規作成の状態(=ID未割当)ならtrue</param>
        /// <param name="isOrigin">KQI上での紐づけならtrue</param>
        private async Task<IActionResult> AddSystemRolesAsync(User user, IEnumerable<long> systemRoleIds, bool isCreate, bool isOrigin)
        {
            if (systemRoleIds != null && systemRoleIds.Count() > 0)
            {
                foreach (var roleId in systemRoleIds)
                {
                    var role = await roleRepository.GetRoleAsync(roleId);
                    if (role == null)
                    {
                        return JsonNotFound($"Role ID {roleId} is not found.");
                    }
                    if (role.IsSystemRole == false)
                    {
                        return JsonNotFound($"Role ID {roleId} is not a system role.");
                    }
                    //ロールを一つずつ追加
                    roleRepository.AttachRole(user, role, null, isCreate, isOrigin);
                }
            }
            return null;
        }

        /// <summary>
        /// 指定したユーザをテナントに新規登録する。
        /// 途中でエラーが発生した場合、そのエラー結果が返る。NULLなら成功。
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        /// <param name="tenant">対象テナント</param>
        /// <param name="tenantRoleIds">テナントロールID</param>
        /// <param name="isOrigin">KQI上での紐づけならtrue</param>
        private async Task<IActionResult> AddTenantAsync(User user, Tenant tenant, IEnumerable<long> tenantRoleIds, bool isOrigin)
        {
            //ロールについての存在＆入力チェック
            var roles = new List<Role>();
            if (tenantRoleIds != null)
            {
                foreach (long roleId in tenantRoleIds)
                {
                    var role = await roleRepository.GetRoleAsync(roleId);
                    if (role == null)
                    {
                        //ロールがない
                        return JsonNotFound($"Role ID {roleId} is not found.");
                    }
                    if (role.IsSystemRole)
                    {
                        //システムロールをテナントロールとして追加しようとしている
                        return JsonBadRequest($"The system role {role.Name} is not assigned to a user as a tenant role.");
                    }
                    roles.Add(role);
                }
            }

            var maps = userRepository.AttachTenant(user, tenant.Id, roles, isOrigin, null);
            if (maps != null)
            {
                foreach (var map in maps)
                {
                    //レジストリを登録
                    await clusterManagementLogic.RegistRegistryToTenantAsync(tenant.Name, map);
                }
            }

            return null;
        }

        /// <summary>
        /// 指定したユーザとテナントの紐づけを更新する。
        /// 途中でエラーが発生した場合、そのエラー結果が返る。NULLなら成功。
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        /// <param name="tenant">対象テナント</param>
        /// <param name="tenantRoleIds">テナントロールID</param>
        /// <param name="isOrigin">KQI上での紐づけならtrue</param>
        private async Task<IActionResult> UpdateTenantAsync(User user, Tenant tenant, IEnumerable<long> tenantRoleIds, bool isOrigin)
        {
            //ロールについての存在＆入力チェック
            var roles = new List<Role>();
            if (tenantRoleIds != null)
            {
                foreach (long roleId in tenantRoleIds)
                {
                    var role = await roleRepository.GetRoleAsync(roleId);
                    if (role == null)
                    {
                        //ロールがない
                        return JsonNotFound($"Role ID {roleId} is not found.");
                    }
                    if (role.IsSystemRole)
                    {
                        //システムロールをテナントロールとして追加しようとしている
                        return JsonBadRequest($"The system role {role.Name} is not assigned to a user as a tenant role.");
                    }
                    roles.Add(role);
                }
            }

            var maps = userRepository.UpdateTenant(user, tenant.Id, roles, isOrigin, null);
            if (maps != null)
            {
                foreach (var map in maps)
                {
                    //レジストリを登録
                    await clusterManagementLogic.RegistRegistryToTenantAsync(tenant.Name, map);
                }
            }

            return null;
        }

        /// <summary>
        /// テナントロールIDをロール情報に変換して返す。
        /// 何か不適切な状態が起こった際はnullを返す。
        /// </summary>
        private async Task<Role> GetRolesAsync(long tenantId, long roleId)
        {
            var role = await roleRepository.GetRoleAsync(roleId);
            if (role == null)
            {
                //ロールがない
                return null;
            }
            if (role.IsSystemRole)
            {
                //システムロールをテナントロールとして追加しようとしている
                LogWarning($"The system role {role.Name} is not assigned as a tenant role.");
                return null;
            }
            if (role.TenantId != null && role.TenantId != tenantId)
            {
                //他のテナント用のロールを追加しようとしている
                LogWarning($"The tenant role {role.Name} is only assigned in same tenant user not tenant {tenantId}.");
                return null;
            }
            return role;
        }

        #endregion

        #region テナントユーザ管理

        /// <summary>
        /// テナント向けに、所属しているユーザの一覧を取得する。
        /// </summary>
        [HttpGet("/api/v{api-version:apiVersion}/tenant/users")]
        [PermissionFilter(MenuCode.TenantUser)]
        [ProducesResponseType(typeof(IEnumerable<IndexForTenantOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllUsersForTenant()
        {
            var tenantId = CurrentUserInfo.SelectedTenant.Id;

            //ユーザ情報単体を取得
            //ToListを付けてSQLを即時発行させる。でないと次の処理でSQLが並行で発行されてしまい、EFの制約に引っかかる。
            var users = userRepository.GetUsers(tenantId).OrderBy(u => u.Name).ToList();

            //ロール情報紐づけて、返す
            return JsonOK(users.Select(u =>
            {
                var result = new IndexForTenantOutputModel(u);
                result.Roles = roleRepository.GetTenantRoles(u.Id, tenantId).OrderBy(r => r.SortOrder);
                return result;
            }));
        }

        /// <summary>
        /// テナント向けに指定したユーザの情報を取得する。
        /// </summary>
        [HttpGet("/api/v{api-version:apiVersion}/tenant/users/{id}")]
        [PermissionFilter(MenuCode.TenantUser)]
        [ProducesResponseType(typeof(IndexForTenantOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserForTenant(long? id)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("User ID is required.");
            }
            //データの存在チェック
            var user = await userRepository.GetByIdAsync(id.Value);
            if (user == null)
            {
                return JsonNotFound($"User ID {id} is not found.");
            }
            var tenantId = CurrentUserInfo.SelectedTenant.Id;
            if (await userRepository.IsMemberAsync(user.Id, tenantId) == false)
            {
                //指定したユーザが同じテナントに所属していない場合、404扱い
                LogWarning($"User {user.Name} is NOT a member of Tenant {tenantId} yet.");
                return JsonNotFound($"User ID {id} is not found.");
            }

            var result = new IndexForTenantOutputModel(user);
            result.Roles = roleRepository.GetTenantRoles(user.Id, tenantId);

            //ロール情報・テナント情報と紐づけて、返す
            return JsonOK(result);
        }

        /// <summary>
        /// 指定したユーザを接続中のテナントから削除する
        /// </summary>
        /// <param name="id">ユーザID</param>
        /// <param name="tenantRepository">DI用</param>
        [HttpDelete("/api/v{api-version:apiVersion}/tenant/users/{id}")]
        [PermissionFilter(MenuCode.TenantUser)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DetachTenantForTenant([FromRoute] long? id, [FromServices] ITenantRepository tenantRepository)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Tenant ID is required.");
            }
            if (id.Value == CurrentUserInfo.Id)
            {
                //自分自身を外すと、ログイン中の処理がおかしくなるので、却下
                return JsonBadRequest("Invalid inputs. Not allowed to remove yourself from your current tenant.");
            }
            //データの存在チェック
            var user = await userRepository.GetByIdAsync(id.Value);
            if (user == null)
            {
                return JsonNotFound($"User ID {id} is not found.");
            }
            var tenant = CurrentUserInfo.SelectedTenant;
            if ((await userRepository.IsMemberAsync(user.Id, tenant.Id)) == false)
            {
                //まだ当該テナントに所属していなかったら失敗
                LogWarning($"User {user.Name} is NOT a member of Tenant {tenant.Id} yet.");
                return JsonNotFound($"User ID {id} is not found.");
            }

            // KQI上の紐づけを外す
            userRepository.DetachOriginTenant(user, tenant.Id);

            if (user.DefaultTenantId == tenant.Id)
            {
                //デフォルトテナントを外した場合、デフォルトを他に付け替える

                //他のテナント情報を取得
                var userInfo = userRepository.GetUserInfo(user); //DBへの反映は遅延実行なので、まだこの時点では当該テナントに所属している状態になる
                var newDefaultTenant = userInfo.TenantDic.Keys.FirstOrDefault(d => d.Id != tenant.Id);
                if (newDefaultTenant == null)
                {
                    //付け替え先がないので、止む無くSandboxに新規紐づけする
                    userRepository.AttachSandbox(user);
                }
                else
                {
                    user.DefaultTenantId = newDefaultTenant.Id;
                }
            }
            unitOfWork.Commit();

            return JsonNoContent();
        }

        /// <summary>
        /// 指定したユーザについての接続中のテナントに対するロール情報を編集する
        /// </summary>
        /// <param name="id">ユーザID</param>
        /// <param name="roleIds">登録するロールID</param>
        /// <param name="tenantRepository">DI用</param>
        [HttpPut("/api/v{api-version:apiVersion}/tenant/users/{id}/roles")]
        [PermissionFilter(MenuCode.TenantUser)]
        [ProducesResponseType(typeof(IndexForAdminOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditUserTenantRoleForTenant([FromRoute] long id, [FromBody] IEnumerable<long> roleIds, [FromServices] ITenantRepository tenantRepository)
        {
            //データの存在チェック
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return JsonNotFound($"User ID {id} is not found.");
            }
            var tenant = CurrentUserInfo.SelectedTenant;
            if ((await userRepository.IsMemberAsync(user.Id, tenant.Id)) == false)
            {
                //まだ当該テナントに所属していなかったら失敗
                LogWarning($"User {user.Name} is NOT a member of Tenant {tenant.Id} yet.");
                return JsonNotFound($"User ID {id} is not found.");
            }
            var roles = new List<Role>();
            //ロールについての存在＆入力チェック
            if (roleIds != null)
            {
                foreach (long roleId in roleIds)
                {
                    var role = await GetRolesAsync(tenant.Id, roleId);
                    if (role == null)
                    {
                        return JsonNotFound($"Role ID {roleId} is not found.");
                    }
                    roles.Add(role);
                }
            }

            userRepository.ChangeTenantRole(id, tenant.Id, roles, true);

            if (user.DefaultTenantId == tenant.Id)
            {
                //デフォルトテナントのロールをすべて外した場合、デフォルトを他に付け替える

                if (!userRepository.IsOriginMember(user.Id, tenant.Id))
                {
                    //他のテナント情報を取得
                    var userInfo = userRepository.GetUserInfo(user); //DBへの反映は遅延実行なので、まだこの時点では当該テナントに所属している状態になる
                    var newDefaultTenant = userInfo.TenantDic.Keys.FirstOrDefault(d => d.Id != tenant.Id);
                    if (newDefaultTenant == null)
                    {
                        //付け替え先がないので、止む無くSandboxに新規紐づけする
                        userRepository.AttachSandbox(user);
                    }
                    else
                    {
                        user.DefaultTenantId = newDefaultTenant.Id;
                    }
                }
            }

            unitOfWork.Commit();

            var result = new IndexForTenantOutputModel(user);
            result.Roles = roleRepository.GetTenantRoles(user.Id, tenant.Id);

            //ロール情報・テナント情報と紐づけて、返す
            return JsonOK(result);
        }

        #endregion
    }
}

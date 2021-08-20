using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nssol.Platypus.ApiModels.TenantApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
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
    /// テナント管理を扱うためのAPI集
    /// </summary>
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/admin/tenants")]
    public class TenantController : PlatypusApiControllerBase
    {
        private readonly ITenantRepository tenantRepository;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IRegistryRepository registryRepository;
        private readonly IGitRepository gitRepository;
        private readonly ICommonDiLogic commonDiLogic;
        private readonly IStorageLogic storageLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly ContainerManageOptions containerManageOptions;
        private readonly IUnitOfWork unitOfWork;

        public TenantController(
            ITenantRepository tenantRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IRegistryRepository registryRepository,
            IGitRepository gitRepository,
            ICommonDiLogic commonDiLogic,
            IStorageLogic storageLogic,
            IClusterManagementLogic clusterManagementLogic,
            IOptions<ContainerManageOptions> containerManageOptions,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.tenantRepository = tenantRepository;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.registryRepository = registryRepository;
            this.gitRepository = gitRepository;
            this.commonDiLogic = commonDiLogic;
            this.storageLogic = storageLogic;
            this.clusterManagementLogic = clusterManagementLogic;
            this.containerManageOptions = containerManageOptions.Value;
            this.unitOfWork = unitOfWork;
        }

        #region Tenant管理
        /// <summary>
        /// テナント一覧を取得
        /// </summary>
        [HttpGet]
        [PermissionFilter(MenuCode.Tenant, MenuCode.User, MenuCode.Node)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var tenants = tenantRepository.GetAllTenants();

            return JsonOK(tenants.OrderBy(t => t.Name).Select(t => new IndexOutputModel(t)));
        }

        /// <summary>
        /// 指定されたIDのテナント情報を取得。
        /// </summary>
        /// <param name="id">テナントID</param>
        [HttpGet("{id}")]
        [PermissionFilter(MenuCode.Tenant)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public IActionResult GetDetails(long? id)
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

            var model = new DetailsOutputModel(tenant);

            return JsonOK(model);
        }

        /// <summary>
        /// 新規にテナントを登録する
        /// </summary>
        /// <param name="model">登録内容</param>
        /// <param name="nodeRepository">DI用</param>
        [HttpPost]
        [PermissionFilter(MenuCode.Tenant)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateForTenant([FromBody]CreateInputModel model, [FromServices] INodeRepository nodeRepository)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            var ignoreNamespaces = containerManageOptions.IgnoreNamespaces.Split(",");

            if 
                (model.TenantName.StartsWith(containerManageOptions.KqiNamespacePrefix) 
                || model.TenantName.StartsWith(containerManageOptions.KubernetesNamespacePrefix)
                || ignoreNamespaces.Contains(model.TenantName)
                )
            {
                // KqiNamespacePrefix または KubernetesNamespacePrefix で始まるテナント名、IgnoreNamespacesに含まれるテナント名は許可しないためエラー
                return JsonBadRequest($"Invalid inputs. 'TenantName' cannot be in ' { containerManageOptions.IgnoreNamespaces } ' or start with '{ containerManageOptions.KqiNamespacePrefix }' or '{ containerManageOptions.KubernetesNamespacePrefix }'.");
            }

            Tenant tenant = tenantRepository.GetFromTenantName(model.TenantName);
            if (tenant != null)
            {
                //テナント名の重複があるのでエラー
                return JsonConflict($"Tenant {model.TenantName} already exists: ID = {tenant.Id}");
            }
            if (model.DefaultGitId != null && model.GitIds.Contains(model.DefaultGitId.Value) == false)
            {
                //デフォルトGitがGit一覧の中になかったらエラー
                return JsonConflict($"Default Git ID {model.DefaultGitId.Value} does NOT exist in selected gits.");
            }
            if (model.DefaultRegistryId != null && model.RegistryIds.Contains(model.DefaultRegistryId.Value) == false)
            {
                //デフォルトレジストリがレジストリ一覧の中になかったらエラー
                return JsonConflict($"Default Registry ID {model.DefaultRegistryId.Value} does NOT exist in selected registries.");
            }

            tenant = new Tenant()
            {
                Name = model.TenantName,
                DisplayName = model.DisplayName,
                StorageBucket = model.TenantName,
                StorageId = model.StorageId,
                AvailableInfiniteTimeNotebook = model.AvailableInfiniteTimeNotebook
            };

            Git git = null;
            if (model.GitIds != null && model.GitIds.Count() > 0)
            {
                //デフォルトGitの設定（無ければ一個目）
                tenant.DefaultGitId = model.DefaultGitId == null ?
                    model.GitIds.ElementAt(0) : model.DefaultGitId.Value;

                foreach (long gitId in model.GitIds)
                {
                    //データの存在チェック
                    git = await gitRepository.GetByIdAsync(gitId);
                    if (git == null)
                    {
                        return JsonNotFound($"The selected git ID {gitId} is not found.");
                    }
                    await gitRepository.AttachGitToTenantAsync(tenant, git, true);
                }
            }
            Registry registry = null;
            if (model.RegistryIds != null && model.RegistryIds.Count() > 0)
            {
                //デフォルトレジストリの設定（無ければ一個目）
                tenant.DefaultRegistryId = model.DefaultRegistryId == null ?
                    model.RegistryIds.ElementAt(0) : model.DefaultRegistryId.Value;

                foreach (long registryId in model.RegistryIds)
                {
                    //データの存在チェック
                    registry = await registryRepository.GetByIdAsync(registryId);
                    if (registry == null)
                    {
                        return JsonNotFound($"The selected registry ID {registryId} is not found.");
                    }
                    await registryRepository.AttachRegistryToTenantAsync(tenant, registry, true);
                }
            }

            //データの存在チェック
            var storage = tenantRepository.GetStorage(model.StorageId.Value);
            if (storage == null)
            {
                return JsonNotFound($"The selected storage ID {model.StorageId.Value} is not found.");
            }

            //ObjectStorage に バケットを作成する
            bool isCreated = await storageLogic.CreateBucketAsync(tenant, storage);
            if (!isCreated)
            {
                // 既にバケットが存在していたならエラーとする
                return JsonNotFound($"Can not create because [{tenant.Name}] exists in the NFS server. Please delete it from the NFS server.");
            }

            tenantRepository.AddTenant(tenant);

            //コンテナ管理サービス作業
            //テナントを登録
            var tenantResult = await clusterManagementLogic.RegistTenantAsync(tenant.Name);
            if (tenantResult == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, "Couldn't create cluster master namespace. Please check the configuration to the connect cluster manager service.");
            }

            // アクセスレベルが "Public" のノードにアサイン
            var nodes = nodeRepository.GetAll().Where(n => n.AccessLevel == NodeAccessLevel.Public);
            foreach (Node node in nodes)
            {
                await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, tenant.Name, true);
            }

            //初期データ投入処理
            commonDiLogic.InitializeTenant(tenant);

            unitOfWork.Commit();
            tenantRepository.Refresh();
            roleRepository.Refresh();

            var result = new IndexOutputModel(tenant);

            return JsonOK(result);
        }

        /// <summary>
        /// テナントを削除する。(他のユーザが未ログイン状態の時間帯で実施するのが望ましい)
        /// </summary>
        /// <param name="id">テナントID</param>
        /// <param name="nodeRepository">DI用</param>
        [HttpDelete("{id}")]
        [PermissionFilter(MenuCode.Tenant)]
        [ProducesResponseType(typeof(DeleteOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteTenantAsync(long id, [FromServices] INodeRepository nodeRepository)
        {
            // 返却データ
            DeleteOutputModel result = new DeleteOutputModel();

            // 入力モデル・データのチェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest($"Invalid inputs: illegal input model");
            }
            // 引数 id のエントリーが存在しない
            Tenant tenant = tenantRepository.Get(id);
            if (tenant == null)
            {
                return JsonNotFound($"Invalid inputs: not found tanant id [{id}].");
            }

            // "sandbox"テナントは削除させない
            if (tenant.Name == ApplicationConst.DefaultFirstTenantName)
            {
                return JsonBadRequest($"Tenant [{ApplicationConst.DefaultFirstTenantName}] is not allowed to delete.");
            }
            // 自分自身の接続中のテナントが対象なら削除不可
            if (CurrentUserInfo.SelectedTenant.Id == id)
            {
                return JsonConflict($"Illegal state: CurrentUserInfo.SelectedTenant.Id is [{id}].");
            }
            // 削除対象のテナントでコンテナ稼働中の場合は削除しない
            var containers = await clusterManagementLogic.GetAllContainerDetailsInfosAsync();
            if (!containers.IsSuccess)
            {
                JsonError(HttpStatusCode.ServiceUnavailable, $"ClusterManagementLogic#GetAllContainerDetailsInfosAsync() retusns error. tenantName=[{tenant.Name}]");
            }
            else if (containers.Value.Count() > 0)
            {
                var runningCount = 0; // Where().Count() で個数を一括取得できるが、ステータスを確認するかもしれないので foreach 文とした。
                foreach (var c in containers.Value)
                {
                    // ステータスによらず、全て稼働中と見做す
                    if (c.TenantName.Equals(tenant.Name))
                    {
                        runningCount += 1;
                    }
                }

                if (runningCount > 0)
                {
                    return JsonConflict($"Running containers exists deleting tenant. tenant name=[{tenant.Name}], running container count=[{runningCount}]");
                }
                containers.Value.Where(x => x.TenantName.Equals(tenant.Name));
            }

            // 削除対象のテナントを所有するユーザ・リストを獲得
            IEnumerable<User> users = userRepository.GetUsers(id);
            foreach (User user in users)
            {
                UserInfo userInfo = userRepository.GetUserInfo(user);
                // 削除対象のテナントを、アクセス中のユーザが利用している場合がありうるが、判別できないので無視する

                // ユーザにおいて削除対象のテナントを detach
                userRepository.DetachTenant(user.Id, id, false);    // 第３引数は true/false どちらでもよい
                // DefaultTenant が削除対象のテナントなら変更
                if (user.DefaultTenantId == id)
                {
                    if (userInfo.TenantDic.Count() > 1)
                    {
                        // 他の登録テナントを DefaultTenant とする
                        Tenant anotherTenant = userInfo.TenantDic.Keys.FirstOrDefault(t => t.Id != id);
                        user.DefaultTenantId = anotherTenant.Id;
                    }
                    else
                    {
                        // サンドボックステナントを DefaultTenant とする
                        userRepository.AttachSandbox(user);
                    }
                }
            }

            // k8s の名前空間の抹消(削除)
            var k8sResult = await clusterManagementLogic.EraseTenantAsync(tenant.Name);
            if (!k8sResult)
            {
                // 削除に失敗したならメッセージを警告として格納し処理の中断は行わない
                var msg = $"Couldn't delete cluster master namespace [{tenant.Name}]. Please check the configuration to the connect cluster manager service.";
                LogWarning(msg);
            }

            // 全てのアサイン情報を削除する
            var nodes = nodeRepository.GetAll();
            foreach (Node node in nodes)
            {
                await clusterManagementLogic.UpdateTenantEnabledLabelAsync(node.Name, tenant.Name, false);
            }

            // テナントの削除（関連するDBのエントリも自動削除）
            tenantRepository.DeleteTenant(tenant);

            // コミットとリフレッシュ(Tenant, Role)
            unitOfWork.Commit();
            tenantRepository.Refresh();
            roleRepository.Refresh();

            // バケット(テナントのデータ)削除用のコンテナを起動する。
            var resultContainer = await clusterManagementLogic.RunDeletingTenantDataContainerAsync(tenant);
            if (resultContainer == null || resultContainer.Status.Succeed() == false)
            {
                // コンテナ起動に失敗した場合警告としてメッセージを格納し処理の中断は行わない。
                var msg = $"Failed to run container for delete minio bucket. You should delete bucket minio [{tenant.Name}] by manual operation.";
                LogWarning(msg);
                result.ContainerWarnMsg = msg;
            }

            // 結果の返却
            return JsonOK(result);
        }

        /// <summary>
        /// テナント情報の編集
        /// </summary>
        /// <param name="id">テナントID</param>
        /// <param name="model">変更内容</param>
        [HttpPut("{id}")]
        [PermissionFilter(MenuCode.Tenant)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody]EditInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var tenant = await tenantRepository.GetTenantWithStorageForUpdateAsync(id.Value);
            if (tenant == null)
            {
                return JsonNotFound($"Tenant ID {id.Value} is not found.");
            }

            if (model.DefaultGitId != null && model.GitIds.Contains(model.DefaultGitId.Value) == false)
            {
                //デフォルトGitがGit一覧の中になかったらエラー
                return JsonConflict($"Default Git ID {model.DefaultGitId.Value} does NOT exist in selected gits.");
            }
            if (model.DefaultRegistryId != null && model.RegistryIds.Contains(model.DefaultRegistryId.Value) == false)
            {
                //デフォルトレジストリがレジストリ一覧の中になかったらエラー
                return JsonConflict($"Default Registry ID {model.DefaultRegistryId.Value} does NOT exist in selected registries.");
            }
            if (model.StorageId != null)
            {
                //データの存在チェック
                var storage = tenantRepository.GetStorage(model.StorageId.Value);
                if (storage == null)
                {
                    return JsonNotFound($"The selected storage ID {model.StorageId.Value} is not found.");
                }

                //バケットを作成する
                await storageLogic.CreateBucketAsync(tenant, storage);
            }

            tenant.DisplayName = model.DisplayName;
            tenant.StorageId = model.StorageId;
            tenant.AvailableInfiniteTimeNotebook = model.AvailableInfiniteTimeNotebook;

            //コンテナ管理サービス作業
            //テナントを登録
            var tenantResult = await clusterManagementLogic.RegistTenantAsync(tenant.Name);
            if (tenantResult == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, "Couldn't create cluster master namespace. Please check the configuration to the connect cluster manager service.");
            }

            //テナントとGitを紐づけ
            //まずは現状のGitを取得して、そこから増減を判断する
            var currentGits = gitRepository.GetGitAll(tenant.Id).ToList();
            if (model.GitIds != null && model.GitIds.Count() > 0)
            {
                //デフォルトGitの設定（無ければ一個目）
                tenant.DefaultGitId = model.DefaultGitId == null ?
                    model.GitIds.ElementAt(0) : model.DefaultGitId.Value;

                foreach (long gitId in model.GitIds)
                {
                    Git currentGit = currentGits.FirstOrDefault(r => r.Id == gitId);
                    if (currentGit != null)
                    {
                        //以前も紐づいていたので、無視。
                        currentGits.Remove(currentGit);
                        continue;
                    }

                    //データの存在チェック
                    Git git = await gitRepository.GetByIdAsync(gitId);
                    if (git == null)
                    {
                        return JsonNotFound($"The selected git ID {gitId} is not found.");
                    }

                    await gitRepository.AttachGitToTenantAsync(tenant, git, false);
                }
            }
            //残っているのは削除された紐づけなので、消す
            foreach (var removedGit in currentGits)
            {
                gitRepository.DetachGitFromTenant(tenant, removedGit);
            }

            //テナントとレジストリを紐づけ
            //まずは現状のレジストリを取得して、そこから増減を判断する
            var currentRegistries = registryRepository.GetRegistryAll(tenant.Id).ToList();
            if (model.RegistryIds != null && model.RegistryIds.Count() > 0)
            {
                //デフォルトレジストリの設定（無ければ一個目）
                tenant.DefaultRegistryId = model.DefaultRegistryId == null ?
                    model.RegistryIds.ElementAt(0) : model.DefaultRegistryId.Value;

                foreach (long registryId in model.RegistryIds)
                {
                    Registry currentRegistry = currentRegistries.FirstOrDefault(r => r.Id == registryId);
                    if (currentRegistry != null)
                    {
                        //以前も紐づいていたので、無視。
                        currentRegistries.Remove(currentRegistry);
                        continue;
                    }

                    //データの存在チェック
                    Registry registry = await registryRepository.GetByIdAsync(registryId);
                    if (registry == null)
                    {
                        return JsonNotFound($"The selected registry ID {registryId} is not found.");
                    }

                    var maps = await registryRepository.AttachRegistryToTenantAsync(tenant, registry, false);
                    if (maps != null)
                    {
                        foreach (var map in maps)
                        {
                            //レジストリを登録
                            var registryResult = await clusterManagementLogic.RegistRegistryToTenantAsync(tenant.Name, map);
                            if (registryResult == false)
                            {
                                return JsonError(HttpStatusCode.ServiceUnavailable, "Couldn't map the tenant and the registry in a cluster management service. Please check the configuration to the connect cluster manager service.");
                            }
                        }
                    }
                }
            }
            //残っているのは削除された紐づけなので、消す
            foreach (var removedRegistry in currentRegistries)
            {
                registryRepository.DetachRegistryFromTenant(tenant, removedRegistry);
            }

            // 関連するクラスタトークンをリセット
            tenantRepository.DeleteClusterToken(tenant.Id);

            tenantRepository.Update(tenant, unitOfWork);

            return JsonOK(new IndexOutputModel(tenant));
        }
        #endregion

        #region 接続テナント設定
        /// <summary>
        /// 接続中のテナントの情報を取得。
        /// </summary>
        [HttpGet("/api/v{api-version:apiVersion}/tenant")]
        [PermissionFilter(MenuCode.TenantSetting)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public IActionResult GetDetailsForTenant()
        {
            return GetDetails(CurrentUserInfo.SelectedTenant.Id);
        }

        /// <summary>
        /// 接続中のテナントの情報の編集
        /// </summary>
        /// <param name="model">変更内容</param>
        [HttpPut("/api/v{api-version:apiVersion}/tenant")]
        [PermissionFilter(MenuCode.TenantSetting)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditForTenant([FromBody]EditInputModel model)
        {
            return await Edit(CurrentUserInfo.SelectedTenant.Id, model);
        }
        #endregion
    }
}

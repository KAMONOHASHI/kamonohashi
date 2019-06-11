using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.ServiceModels;
using Nssol.Platypus.Services.Interfaces;
using System.Linq;

namespace Nssol.Platypus.DataAccess.Core
{
    public class Seed
    {
        /// <summary>
        /// 接続先のデータソースを表すDBコンテキスト
        /// </summary>
        private CommonDbContext dbContext;

        private readonly DeployOptions deployOptions;
        private readonly IObjectStorageService objectStorageService;
        private readonly IClusterManagementService clusterManagementService;
        private readonly ILogger logger;

        public Seed(
            CommonDbContext context,
            IOptions<DeployOptions> deployOptions,
            IObjectStorageService objectStorageService,
            IClusterManagementService clusterManagementService,
            ILogger<Seed> logger
            )
        {
            this.dbContext = context;
            this.deployOptions = deployOptions.Value;
            this.objectStorageService = objectStorageService;
            this.clusterManagementService = clusterManagementService;
            this.logger = logger;
        }

        /// <summary>
        /// DBのマイグレーション
        /// </summary>
        public void Migrate()
        {
            //DBのマイグレーションを実行（最新のものまでが自動で反映される）
            //Databaseが未作成であれば、作成する
            dbContext.Database.Migrate();
        }

        /// <summary>
        /// DBが初期化されているかどうかを判定する。
        /// 初期化されているなら true を、初期化されていないなら false を返却する。
        /// </summary>
        public bool IsInitializedDB()
        {
            //ユーザ数を確認（初期化済みであれば必ずAdminがいる）
            int count = dbContext.Set<User>().ToList().Count;
            return count != 0;
        }

        public bool IsValidDeployOptions()
        {
            // 設定値のチェック
            bool ret = true;
            if (string.IsNullOrEmpty(deployOptions.Password))
            {
                LogError("DeployOptions の Password が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(deployOptions.GpuNodes))
            {
                LogError("DeployOptions の GpuNodes が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(deployOptions.ObjectStorageNode))
            {
                LogError("DeployOptions の ObjectStorageNode が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(deployOptions.ObjectStoragePort))
            {
                LogError("DeployOptions の ObjectStoragePort が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(deployOptions.ObjectStorageAccessKey))
            {
                LogError("DeployOptions の ObjectStorageAccessKey が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(deployOptions.ObjectStorageSecretKey))
            {
                LogError("DeployOptions の ObjectStorageSecretKey が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(deployOptions.NfsStorage))
            {
                LogError("DeployOptions の NfsStorage が設定されていません。");
                ret = false;
            }
            if (string.IsNullOrEmpty(deployOptions.NfsPath))
            {
                LogError("DeployOptions の NfsPath が設定されていません。");
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// DBを初期化する。
        /// </summary>
        public bool InitializeDB()
        {
            // DBを初期化したか
            bool isInitializeDB = false;
            // 初期化済みではない場合、DBを初期化
            if (!IsInitializedDB())
            {
                CreateInitialDB();
                isInitializeDB = true;
            }
            return isInitializeDB;
        }

        private void LogError(string msg)
        {
            LogUtil.WriteLLog(logger.LogError, "-", "-", msg);
        }

        private void LogWarn(string msg)
        {
            LogUtil.WriteLLog(logger.LogWarning, "-", "-", msg);
        }

        private void LogDebug(string msg)
        {
            LogUtil.WriteLLog(logger.LogDebug, "-", "-", msg);
        }

        #region 共通の初期データ投入

        private int CreateInitialDB()
        {
            //ロール作成
            Role userRole = AddNewRecordForInit(new Role() { Name = "users", DisplayName = "User", SortOrder = 10, IsSystemRole = false });
            Role researcherRole = AddNewRecordForInit(new Role() { Name = "researchers", DisplayName = "Researcher", SortOrder = 20, IsSystemRole = false });
            Role managerRole = AddNewRecordForInit(new Role() { Name = "managers", DisplayName = "Manager", SortOrder = 30, IsSystemRole = false });
            Role adminRole = AddNewRecordForInit(new Role() { Name = "admins", DisplayName = "Admin", SortOrder = 100, IsSystemRole = true });

            //メニュー作成
            AddNewRecordForInit(new MenuRoleMap() { Role = userRole, MenuCode = Logic.MenuLogic.DataMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = userRole, MenuCode = Logic.MenuLogic.DataSetMenu.Code.ToString() });

            AddNewRecordForInit(new MenuRoleMap() { Role = researcherRole, MenuCode = Logic.MenuLogic.DataMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = researcherRole, MenuCode = Logic.MenuLogic.DataSetMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = researcherRole, MenuCode = Logic.MenuLogic.PreprocessMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = researcherRole, MenuCode = Logic.MenuLogic.TrainingMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = researcherRole, MenuCode = Logic.MenuLogic.InferenceMenu.Code.ToString() });

            AddNewRecordForInit(new MenuRoleMap() { Role = managerRole, MenuCode = Logic.MenuLogic.TenantSettingMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = managerRole, MenuCode = Logic.MenuLogic.TenantRoleMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = managerRole, MenuCode = Logic.MenuLogic.TenantUserMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = managerRole, MenuCode = Logic.MenuLogic.TenantMenuAccessMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = managerRole, MenuCode = Logic.MenuLogic.TenantResourceMenu.Code.ToString() });

            AddNewRecordForInit(new MenuRoleMap() { Role = adminRole, MenuCode = Logic.MenuLogic.TenantMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = adminRole, MenuCode = Logic.MenuLogic.GitMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = adminRole, MenuCode = Logic.MenuLogic.RegistryMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = adminRole, MenuCode = Logic.MenuLogic.StorageMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = adminRole, MenuCode = Logic.MenuLogic.RoleMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = adminRole, MenuCode = Logic.MenuLogic.QuotaMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = adminRole, MenuCode = Logic.MenuLogic.UserMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = adminRole, MenuCode = Logic.MenuLogic.NodeMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = adminRole, MenuCode = Logic.MenuLogic.ResourceMenu.Code.ToString() });
            AddNewRecordForInit(new MenuRoleMap() { Role = adminRole, MenuCode = Logic.MenuLogic.MenuAccessMenu.Code.ToString() });

            //初期Git作成
            Git git = AddNewRecordForInit(new Git() { Name = "GitHub", RepositoryUrl = "https://github.com", ServiceType = GitServiceType.GitHub, ApiUrl = "https://api.github.com" });
            //初期レジストリ作成
            Registry registry = AddNewRecordForInit(new Registry() { Name = "official-docker-hub", Host = "registry.hub.docker.com", PortNo = 80, ServiceType = RegistryServiceType.DockerHub, ApiUrl = "https://registry.hub.docker.com/", RegistryUrl = "https://registry.hub.docker.com/" });

            // 初期ノードの作成
            string[] nodeNames = deployOptions.GpuNodes.Split(',');
            foreach (string nodeName in nodeNames)
            {
                AddNewRecordForInit(new Node()
                {
                    Name = nodeName,
                    AccessLevel = NodeAccessLevel.Public,
                    TensorBoardEnabled = true
                });
            }

            // 初期ストレージの作成
            Storage storage = AddNewRecordForInit(new Storage()
            {
                Name = ApplicationConst.DefaultFirstStorageName,
                ServerAddress = deployOptions.ObjectStorageNode + ":" + deployOptions.ObjectStoragePort,
                AccessKey = deployOptions.ObjectStorageAccessKey,
                SecretKey = deployOptions.ObjectStorageSecretKey,
                NfsServer = deployOptions.NfsStorage,
                NfsRoot = deployOptions.NfsPath
            });

            //初期テナント作成
            Tenant tenant = AddNewRecordForInit(new Tenant()
            {
                Name = ApplicationConst.DefaultFirstTenantName,
                DisplayName = ApplicationConst.DefaultFirstTenantDisplayName,
                DefaultGit = git,
                DefaultRegistryId = registry.Id,
                StorageBucket = ApplicationConst.DefaultFirstTenantName,
                StorageId = storage.Id
            });
            //GitとTenantの対応付け
            TenantGitMap tenantGitMap = AddNewRecordForInit(new TenantGitMap() { Tenant = tenant, Git = git });
            //RegistryとTenantの対応付け
            TenantRegistryMap tenantRegistryMap = AddNewRecordForInit(new TenantRegistryMap() { Tenant = tenant, Registry = registry });

            //初期ユーザ作成
            User user = AddNewRecordForInit(new User()
            {
                Name = ApplicationConst.DefaultFirstAdminUserName,
                ServiceType = AuthServiceType.Local,
                DefaultTenant = tenant,
                Password = Util.GenerateHash(deployOptions.Password, ApplicationConst.DefaultFirstAdminUserName)
            });

            //初期ユーザの所属登録
            UserTenantMap userTenantMap = AddNewRecordForInit(new UserTenantMap() { Tenant = tenant, User = user });
            AddNewRecordForInit(new UserTenantGitMap() { User = user, TenantGitMap = tenantGitMap });
            AddNewRecordForInit(new UserTenantRegistryMap() { User = user, TenantRegistryMap = tenantRegistryMap });

            // ロール明細の登録

            AddNewRecordForInit(new UserRoleMap() { Role = researcherRole, User = user, TenantMap = userTenantMap });
            AddNewRecordForInit(new UserRoleMap() { Role = managerRole, User = user, TenantMap = userTenantMap });
            AddNewRecordForInit(new UserRoleMap() { Role = adminRole, User = user });

            // テナント系DBの初期化
            InitTenant(tenant);

            //コミット
            int result = dbContext.SaveChanges(user.Name);

            // テナント環境の生成
            return result;
        }

        private T AddNewRecordForInit<T>(T model) where T : ModelBase
        {
            dbContext.Set<T>().Add(model);
            return model;
        }

        #endregion

        #region テナント用の初期データ投入
        /// <summary>
        /// テナント用テーブルに指定したテナント用の初期データを追加する
        /// </summary>
        public void InitTenant(Tenant tenant)
        {
            AddNewRecordForInit(tenant, new DataType() { Name = "training", SortOrder = 1 });
            AddNewRecordForInit(tenant, new DataType() { Name = "testing", SortOrder = 2 });
            AddNewRecordForInit(tenant, new DataType() { Name = "validation", SortOrder = 3 });
        }

        private T AddNewRecordForInit<T>(Tenant tenant, T model) where T : TenantModelBase
        {
            model.Tenant = tenant;
            dbContext.Set<T>().Add(model);
            return model;
        }

        /// <summary>
        /// 初期生成テナントと ObjectStore を同期させる。
        /// </summary>
        public void SyncInitialObjectStore()
        {
            StorageConfigModel storageConfigModel = new StorageConfigModel()
            {
                AccessKey = deployOptions.ObjectStorageAccessKey,
                SecretKey = deployOptions.ObjectStorageSecretKey,
                StorageServer = deployOptions.ObjectStorageNode + ":" + deployOptions.ObjectStoragePort,
                Bucket = ApplicationConst.DefaultFirstTenantName
            };
            bool isCreated = objectStorageService.CreateBucketAsync(storageConfigModel).Result;
            if (isCreated)
            {
                LogDebug($"ObjectStore のバケット {ApplicationConst.DefaultFirstTenantName} は既に作成済みです。");
            }
        }

        /// <summary>
        /// 初期生成テナントと ClusterManager(k8s) を同期させる。
        /// 成功したなら true を返却する。
        /// </summary>
        public bool SyncInitialClusterManager()
        {
            bool ret = clusterManagementService.RegistTenantAsync(ApplicationConst.DefaultFirstTenantName).Result;
            if (!ret)
            {
                LogWarn($"Cluster(k8s) のネームスペース {ApplicationConst.DefaultFirstTenantName} の作成に失敗しました。");
            }
            return ret;
        }
        #endregion

        /// <summary>
        /// オブジェクトをクリーンアップします。
        /// </summary>
        /// <remarks>
        /// Disposeパターンの実装
        /// </remarks>
        public void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
                dbContext = null;
            }
        }
    }
}

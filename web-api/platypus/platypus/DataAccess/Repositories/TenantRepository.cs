using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories
{
    /// <summary>
    /// Tenantテーブルにアクセスするためのリポジトリ。
    /// Tenant情報はキャッシュしており、クローンせずに取得可能なので、変更しないように注意。
    /// </summary>
    /// <remarks>
    /// 3種類のテーブルをキャッシュする必要があるので、今はMemoryCacheを使っていない。
    /// </remarks>
    public class TenantRepository : RepositoryBase<Tenant>, ITenantRepository
    {
        private readonly DbSet<Tenant> dbsetTenant;
        private readonly DbSet<Storage> dbsetStorage;

        private static List<Tenant> _tenants;
        private static List<Storage> _storages;

        /// <summary>
        /// キャッシュのリセットとキャッシュの読み込みが同時処理されないようにセマフォを設ける。
        /// 同一スレッドであればlockはネスト可能なので、セマフォは一つだけ。
        /// </summary>
        private static readonly object semaphore = new object();

        private ILogger<TenantRepository> logger;

        /// <summary>
        /// キャッシュされたStorageリスト
        /// </summary>
        private List<Storage> storages
        {
            get
            {
                lock (semaphore)
                {
                    if (_storages == null)
                    {
                        Log("ストレージ情報をDBからキャッシュに読み込み");
                        _storages = dbsetStorage.OrderBy(r => r.Id).ToList();
                    }
                }
                return _storages;
            }
        }

        /// <summary>
        /// キャッシュされたテナントリスト
        /// </summary>
        private List<Tenant> tenants
        {
            get
            {
                lock (semaphore)
                {
                    if (_tenants == null)
                    {
                        Log("テナント情報をDBからキャッシュに読み込み");
                        //Git/Registry/Storageまとめて読み込む。キャッシュされているものと別のインスタンスになるが、これを編集したり比較したりすることはないハズ
                        _tenants = dbsetTenant
                            .Include(t => t.DefaultGit)
                            .Include(t => t.GitMaps).ThenInclude(map => map.Git)
                            .Include(t => t.Storage)
                            .Include(t => t.DefaultRegistry)
                            .Include(t => t.RegistryMaps).ThenInclude(map => map.Registry)
                            .OrderBy(t => t.Name).ToList();
                    }
                }
                return _tenants;
            }
        }

        /// <summary>
        /// 接続先のデータソースを表すDBコンテキスト
        /// </summary>
        private CommonDbContext DataContext { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TenantRepository(CommonDbContext context,
            ILogger<TenantRepository> logger) : base(context)
        {
            DataContext = context;
            dbsetTenant = DataContext.Set<Tenant>();
            dbsetStorage = DataContext.Set<Storage>();
            this.logger = logger;
        }

        /// <summary>
        /// キャッシュを破棄する
        /// </summary>
        public void Refresh()
        {
            lock (semaphore)
            {
                Log("テナント情報、Git情報、レジストリ情報をキャッシュから破棄");
                _tenants = null;
                _storages = null;
            }
        }

        #region Tenant
        /// <summary>
        /// Idからテナントを取得する。
        /// 対応するテナントが見つからない場合はNULLを返す。
        /// </summary>
        public Tenant Get(long id)
        {
            return tenants.FirstOrDefault(d => d.Id == id);
        }

        /// <summary>
        /// テナント名(<see cref="Tenant.Name"/>)からテナントを取得する。
        /// 対応するテナントが見つからない場合はNULLを返す。
        /// </summary>
        public Tenant GetFromTenantName(string tenantName)
        {
            if (string.IsNullOrEmpty(tenantName))
            {
                return null;
            }
            return tenants.FirstOrDefault(d => d.Name.Equals(tenantName, StringComparison.CurrentCulture));
        }

        /// <summary>
        /// 全テナントを取得する。
        /// </summary>
        public IEnumerable<Tenant> GetAllTenants()
        {
            return tenants;
        }

        /// <summary>
        /// テナントを追加する
        /// </summary>
        public void AddTenant(Tenant tenant)
        {
            dbsetTenant.Add(tenant);
        }

        /// <summary>
        /// 更新用のテナント情報を取得する。
        /// 普段はキャッシュからデータをとるが、それだとEntityFrameworkがキャッシュされたオブジェクトのIDを見失って、編集ではなく新規追加になる恐れがある。
        /// </summary>
        public async Task<Tenant> GetTenantForUpdateAsync(long id)
        {
            return await dbsetTenant.FindAsync(id);
        }

        /// <summary>
        /// 更新用のテナント情報を取得する。ストレージ情報も併せて取得する
        /// 普段はキャッシュからデータをとるが、それだとEntityFrameworkがキャッシュされたオブジェクトのIDを見失って、編集ではなく新規追加になる恐れがある。
        /// </summary>
        public async Task<Tenant> GetTenantWithStorageForUpdateAsync(long id)
        {
            return await dbsetTenant.Include(t => t.Storage).FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// テナント情報を更新する。
        /// 引数のTenantはキャッシュからではなく、<see cref="GetTenantForUpdateAsync(long)"/>で直接DBから取得したものを使うこと。
        /// </summary>
        public void Update(Tenant tenant, IUnitOfWork unitOfWork)
        {
            //tenantを適切に取得していれば、Attachする必要なく、そのまま普通に更新できるはず。

            unitOfWork.Commit();

            Refresh();
        }

        /// <summary>
        /// 指定したテナントを削除する。
        /// </summary>
        public void DeleteTenant(Tenant tenant)
        {
            // 関連エントリの削除
            // Roles エントリの削除は Cascade 指定ではない。

            // テナントの削除
            Delete(tenant);
        }

        /// <summary>
        /// 指定したテナントのClusterTokenをすべて削除する
        /// </summary>
        public void DeleteClusterToken(long tenantId)
        {
            foreach (var map in FindModelAll<UserTenantMap>(map => map.TenantId == tenantId))
            {
                map.ClusterToken = null;
            }
        }

        #endregion Tenant

        #region Storage
        /// <summary>
        /// 全Git情報を取得する。
        /// </summary>
        public IEnumerable<Storage> GetStorageAll()
        {
            return storages;
        }

        /// <summary>
        /// 指定したStorage情報を取得する。
        /// </summary>
        public Storage GetStorage(long storageId)
        {
            return storages.FirstOrDefault(s => s.Id == storageId);
        }

        /// <summary>
        /// Storage情報を追加する
        /// </summary>
        public void AddStorage(Storage storage)
        {
            dbsetStorage.Add(storage);
        }

        /// <summary>
        /// 更新用のStorage情報を取得する。
        /// 普段はキャッシュからデータをとるが、それだとEntityFrameworkがキャッシュされたオブジェクトのIDを見失って、編集ではなく新規追加になる恐れがある。
        /// </summary>
        public async Task<Storage> GetStorageForUpdateAsync(long id)
        {
            return await dbsetStorage.FindAsync(id);
        }

        /// <summary>
        /// Storage情報を更新する
        /// </summary>
        public void UpdateStorage(Storage storage)
        {
            dbsetStorage.Attach(storage);
            DataContext.Entry(storage).State = EntityState.Modified;
        }

        /// <summary>
        /// Storage情報を削除する
        /// </summary>
        public void DeleteStorage(Storage storage)
        {
            dbsetStorage.Remove(storage);
        }
        #endregion

        private void Log(string message)
        {
            LogUtil.WriteLLog(logger.LogDebug, "-", "-", message);
        }
    }
}

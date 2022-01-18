using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Tenantテーブルにアクセスするためのリポジトリ
    /// </summary>
    public interface ITenantRepository
    {
        /// <summary>
        /// キャッシュを破棄する
        /// </summary>
        void Refresh();

        /// <summary>
        /// テナント名(<see cref="Tenant.Name"/>)からテナントを取得する。
        /// 対応するテナントが見つからない場合はNULLを返す。
        /// </summary>
        Tenant GetFromTenantName(string tenantName);
        /// <summary>
        /// Idからテナントを取得する。
        /// 対応するテナントが見つからない場合はNULLを返す。
        /// </summary>
        Tenant Get(long id);

        /// <summary>
        /// 全テナントを取得する。
        /// </summary>
        IEnumerable<Tenant> GetAllTenants();
        /// <summary>
        /// テナントを追加する
        /// </summary>
        void AddTenant(Tenant tenant);

        /// <summary>
        /// 更新用のテナント情報を取得する。
        /// 普段はキャッシュからデータをとるが、それだとEntityFrameworkがキャッシュされたオブジェクトのIDを見失って、編集ではなく新規追加になる恐れがある。
        /// </summary>
        Task<Tenant> GetTenantForUpdateAsync(long id);

        /// <summary>
        /// 更新用のテナント情報を取得する。ストレージ情報も併せて取得する
        /// 普段はキャッシュからデータをとるが、それだとEntityFrameworkがキャッシュされたオブジェクトのIDを見失って、編集ではなく新規追加になる恐れがある。
        /// </summary>
        Task<Tenant> GetTenantWithStorageForUpdateAsync(long id);

        /// <summary>
        /// テナント情報を更新する。
        /// 引数のRoleはキャッシュからではなく、<see cref="GetTenantForUpdateAsync(long)"/>で直接DBから取得したものを使うこと。
        /// </summary>
        void Update(Tenant tenant, IUnitOfWork unitOfWork);

        /// <summary>
        /// 指定したテナントを削除する。
        /// </summary>
        void DeleteTenant(Tenant tenant);

        /// <summary>
        /// 指定したテナントのClusterTokenをすべて削除する
        /// </summary>
        void DeleteClusterToken(long tenantId);

        #region Storage管理
        /// <summary>
        /// 全Storage情報を取得する。
        /// </summary>
        IEnumerable<Storage> GetStorageAll();

        /// <summary>
        /// 指定したStorage情報を取得する。
        /// </summary>
        Storage GetStorage(long storageId);

        /// <summary>
        /// Storage情報を追加する
        /// </summary>
        void AddStorage(Storage storage);

        /// <summary>
        /// 更新用のStorage情報を取得する。
        /// 普段はキャッシュからデータをとるが、それだとEntityFrameworkがキャッシュされたオブジェクトのIDを見失って、編集ではなく新規追加になる恐れがある。
        /// </summary>
        Task<Storage> GetStorageForUpdateAsync(long id);

        /// <summary>
        /// Storage情報を更新する
        /// </summary>
        void UpdateStorage(Storage storage);

        /// <summary>
        /// Storage情報を削除する
        /// </summary>
        void DeleteStorage(Storage storage);
        #endregion
    }
}

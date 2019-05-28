using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    public interface IRegistryRepository : IRepository<Registry>
    {
        /// <summary>
        /// 全レジストリ情報を取得する。
        /// </summary>
        IEnumerable<Registry> GetRegistryAll();

        /// <summary>
        /// 指定したテナントに紐づく全レジストリを取得する
        /// </summary>
        IEnumerable<Registry> GetRegistryAll(long tenantId);

        /// <summary>
        /// テナントにレジストリを紐づける。
        /// 結果として、作成したすべての<see cref="UserTenantRegistryMap"/>を返す。
        /// テナントの新規作成時であれば、<paramref name="isCreate"/>をTrueにする。
        /// </summary>
        Task<IEnumerable<UserTenantRegistryMap>> AttachRegistryToTenantAsync(Tenant tenant, Registry registry, bool isCreate);

        /// <summary>
        /// テナントのレジストリ紐づけを解除する
        /// </summary>
        void DetachRegistryFromTenant(Tenant tenant, Registry registry);

        /// <summary>
        /// 指定したテナント・ユーザに対応するマッピング情報を取得する
        /// </summary>
        IEnumerable<UserTenantRegistryMap> GetUserTenantRegistryMapAll(long tenantId, long userId);

        /// <summary>
        /// 指定したテナント・ユーザ・レジストリに対応するマッピング情報を取得する
        /// </summary>
        UserTenantRegistryMap GetUserTenantRegistryMap(long userId, long tenantId, long registryId);

        /// <summary>
        /// 指定したレジストリを使用しているテナントを最大一件返す。
        /// </summary>
        Tenant GetTenant(long registryId);

        /// <summary>
        /// ユーザ・テナント・レジストリに対応する全てのマッピング情報を取得する
        /// </summary>
        IEnumerable<UserTenantRegistryMap> GetUserTenantRegistryMapAll();
    }
}

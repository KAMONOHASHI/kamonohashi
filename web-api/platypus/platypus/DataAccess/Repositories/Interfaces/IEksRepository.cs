using System.Collections.Generic;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    public interface IEksRepository : IRepository<Eks>
    {
        /// <summary>
        /// EKSのノードへのアサイン状況をリセットする
        /// </summary>
        /// <param name="eksId">対象のEKSのデータのID</param>
        void ResetEksToTenant(long eksId);

        /// <summary>
        /// EKSの情報とテナントの情報をマッピングする
        /// テナントの存在は確認しない
        /// </summary>
        /// <param name="tenants">テナント</param>
        /// <param name="eks">EKS</param>
        /// <param name="isCreate">EKSの情報が新規登録か否か</param>
        /// <returns></returns>
        void AttachEksToTenant(IEnumerable<Tenant> tenants, Eks eks, bool isCreate);
    }
}

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
        /// 対象のテナントで利用可能なEKSの一覧を取得する
        /// </summary>
        IEnumerable<Eks> GetEksByTenantId(long tenantId);

        /// <summary>
        /// EKSの情報とテナントの情報をマッピングする
        /// テナントの存在は確認しない
        /// </summary>
        /// <param name="tenants">テナント</param>
        /// <param name="eks">EKS</param>
        /// <param name="isCreate">EKSの情報が新規登録か否か</param>
        /// <returns></returns>
        void AttachEksToTenant(IEnumerable<Tenant> tenants, Eks eks, bool isCreate);

        /// <summary>
        /// ユーザー、テナント、EKSのデータからユーザーのトークンを取得する
        /// 存在しない場合は、nullが返る
        /// </summary>
        string GetUserToken(long userId, long tenantId, long eksId);
    }
}

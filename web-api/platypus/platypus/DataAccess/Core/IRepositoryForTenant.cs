using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.DataAccess.Core
{
    /// <summary>
    /// テナント向けの拡張インターフェース
    /// </summary>
    /// <typeparam name="TModel">テナント用モデルの基底クラス</typeparam>
    public interface IRepositoryForTenant<TModel> : IRepository<TModel>
        where TModel : TenantModelBase
    {
    }
}

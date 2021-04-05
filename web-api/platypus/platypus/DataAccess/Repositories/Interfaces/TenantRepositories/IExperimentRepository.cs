using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// 実験テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IExperimentRepository : IRepositoryForTenant<Experiment> { }
}

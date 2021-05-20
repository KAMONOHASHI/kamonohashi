using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels.Aquarium;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// アクアリウムデータセットテーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IAquariumDataSetRepository : IRepositoryForTenant<DataSet> { }
}

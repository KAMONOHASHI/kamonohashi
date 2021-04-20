using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels.Aquarium;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// アクアリウムデータセットバージョンテーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IAquariumDataSetVersionRepository : IRepositoryForTenant<DataSetVersion> { }
}

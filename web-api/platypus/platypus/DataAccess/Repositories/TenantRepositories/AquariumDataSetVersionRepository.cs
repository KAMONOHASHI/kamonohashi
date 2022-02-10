using Microsoft.AspNetCore.Http;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels.Aquarium;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// アクアリウムデータセットバージョンテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class AquariumDataSetVersionRepository : RepositoryForTenantBase<DataSetVersion>, IAquariumDataSetVersionRepository
    {
        public AquariumDataSetVersionRepository(CommonDbContext context, IHttpContextAccessor accessor)
        : base(context, accessor) { }
    }
}

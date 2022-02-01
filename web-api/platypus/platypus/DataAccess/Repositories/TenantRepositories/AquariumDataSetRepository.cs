using Microsoft.AspNetCore.Http;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels.Aquarium;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// アクアリウムデータセットテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class AquariumDataSetRepository : RepositoryForTenantBase<DataSet>, IAquariumDataSetRepository
    {
        public AquariumDataSetRepository(CommonDbContext context, IHttpContextAccessor accessor)
        : base(context, accessor) { }
    }
}

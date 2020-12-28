using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels.Aquarium;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// アクアリウムデータセット関連テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class AquariumDataSetRepository : RepositoryForTenantBase<DataSet>, IAquariumDataSetRepository
    {
        public AquariumDataSetRepository(CommonDbContext context, IHttpContextAccessor accessor)
        : base(context, accessor) {}

        public async Task<DataSet> GetDataSetWithVersionsAsync(long id)
            => await GetAll().Include(x => x.DataSetVersions).SingleOrDefaultAsync(x => x.Id == id);

        public async Task<DataSetVersion> GetDataSetVersionWithDataAsync(long aquariumDatasetId, long versionId)
            => await GetModelAll<DataSetVersion>()
            .Include(x => x.DataSet)
            .ThenInclude(d => d.DataSetEntries)
            .ThenInclude(d => d.Data)
            .SingleOrDefaultAsync(x => x.Id == versionId && x.AquariumDataSetId == aquariumDatasetId);

        public void Add<T>(T model) where T : Models.TenantModels.TenantModelBase => AddModel(model);
    }
}

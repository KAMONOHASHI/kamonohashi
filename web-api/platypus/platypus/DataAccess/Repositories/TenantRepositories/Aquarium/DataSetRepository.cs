using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories.Aquarium;
using Nssol.Platypus.Models.TenantModels.Aquarium;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories.Aquarium
{
    /// <summary>
    /// アクアリウムデータセット関連テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class DataSetRepository : RepositoryForTenantBase<DataSet>, IDataSetRepository
    {
        public DataSetRepository(CommonDbContext context, IHttpContextAccessor accessor)
        : base(context, accessor) {}

        public async Task<DataSet> GetDataSetWithVersionsAsync(long id)
            => await GetAll().Include(x => x.DataSetVersions).SingleOrDefaultAsync(x => x.Id == id);

        public async Task<DataSetVersion> GetDataSetVersionWithFilesAsync(long datasetId, long versionId)
            => await GetModelAll<DataSetVersion>()
            .Include(x => x.DataSetVersionEntries)
            .ThenInclude(x => x.Data)
            .ThenInclude(d => d.DataProperties)
            .ThenInclude(d => d.DataFile)
            .SingleOrDefaultAsync(x => x.Id == versionId && x.DataSetId == datasetId);

        public void Add<T>(T model) where T : Models.TenantModels.TenantModelBase => AddModel(model);
    }
}

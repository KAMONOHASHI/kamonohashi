using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels;
using System.Linq;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// 実験関連テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class ExperimentRepository : RepositoryForTenantBase<Experiment>, IExperimentRepository
    {
        public ExperimentRepository(CommonDbContext context, IHttpContextAccessor accessor)
            : base(context, accessor) {}

        public IQueryable<ExperimentPreprocess> GetExperimentPreprocess(long templateId, long templateVersionId,
            long dataSetId, long dataSetVersionId)
            => GetModelAll<ExperimentPreprocess>()
            .Include(x => x.TrainingHistory)
            .Where(x => x.TemplateId == templateId && x.TemplateVersionId == templateVersionId
            && x.DataSetId == dataSetId && x.DataSetVersionId == dataSetVersionId);

        public void Add<T>(T model) where T : TenantModelBase => AddModel(model);
    }
}

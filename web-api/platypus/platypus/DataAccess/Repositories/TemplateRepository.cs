using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Models;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories
{
    /// <summary>
    /// テンプレートテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class TemplateRepository : RepositoryBase<Template>, ITemplateRepository
    {
        public TemplateRepository(CommonDbContext context) : base(context) { }

        public async Task<TemplateVersion> GetTemplateVersionAsync(long templateId, long templateVersionId)
            => await GetModelAll<TemplateVersion>()
            .Include(x => x.PreprocessContainerRegistry)
            .Include(x => x.TrainingContainerRegistry)
            .Include(x => x.EvaluationContainerRegistry)
            .SingleOrDefaultAsync(x => x.Id == templateVersionId && x.TemplateId == templateId);

        public void Add<T>(T model) where T : ModelBase => AddModel(model);
    }
}

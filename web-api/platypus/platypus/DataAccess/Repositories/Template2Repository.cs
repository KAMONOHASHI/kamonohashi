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
    /// <seealso cref="Nssol.Platypus.DataAccess.Repositories.Interfaces.ITemplateRepository" />
    public class Template2Repository : RepositoryBase<Template>, ITemplate2Repository
    {
        public Template2Repository(CommonDbContext context) : base(context) { }

        public async Task<Template> GetTemplateWithVersionsAsync(long id)
            => await GetAll().Include(x => x.TemplateVersions).SingleOrDefaultAsync(x => x.Id == id);

        public async Task<TemplateVersion> GetTemplateVersionAsync(long templateId, long templateVersionId)
            => await GetModelAll<TemplateVersion>()
            .Include(x => x.PreprocessContainerRegistry)
            .Include(x => x.TrainingContainerRegistry)
            .Include(x => x.EvaluationContainerRegistry)
            .SingleOrDefaultAsync(x => x.Id == templateVersionId && x.TemplateId == templateId);

        public void Add<T>(T model) where T : ModelBase => AddModel(model);
    }
}

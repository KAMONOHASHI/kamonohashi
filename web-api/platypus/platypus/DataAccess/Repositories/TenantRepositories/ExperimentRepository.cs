using Microsoft.AspNetCore.Http;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// 実験テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class ExperimentRepository : RepositoryForTenantBase<Experiment>, IExperimentRepository
    {
        public ExperimentRepository(CommonDbContext context, IHttpContextAccessor accessor)
            : base(context, accessor) {}
    }
}

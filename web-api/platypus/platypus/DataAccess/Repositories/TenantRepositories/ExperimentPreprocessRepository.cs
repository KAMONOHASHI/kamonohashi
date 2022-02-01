using Microsoft.AspNetCore.Http;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// 実験前処理テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class ExperimentPreprocessRepository : RepositoryForTenantBase<ExperimentPreprocess>, IExperimentPreprocessRepository
    {
        public ExperimentPreprocessRepository(CommonDbContext context, IHttpContextAccessor accessor)
            : base(context, accessor) { }
        public Task<bool> ExistsAsync(Expression<Func<ExperimentPreprocess, bool>> where, bool force)
            => ExistsModelAsync(where, force);
    }
}

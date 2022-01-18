using Microsoft.AspNetCore.Http;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels.Aquarium;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// アクアリウム推論テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class AquariumEvaluationRepository : RepositoryForTenantBase<Evaluation>, IAquariumEvaluationRepository
    {
        public AquariumEvaluationRepository(CommonDbContext context, IHttpContextAccessor accessor)
            : base(context, accessor) { }

        public Task<bool> ExistsAsync(Expression<Func<Evaluation, bool>> where, bool force)
            => ExistsModelAsync(where, force);
    }
}

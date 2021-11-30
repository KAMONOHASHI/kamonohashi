using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels.Aquarium;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// アクアリウム推論テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IAquariumEvaluationRepository : IRepositoryForTenant<Evaluation>
    {
        /// <summary>
        /// 条件を満たすエントリが存在するか確認する
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<Evaluation, bool>> where, bool force);
    }
}

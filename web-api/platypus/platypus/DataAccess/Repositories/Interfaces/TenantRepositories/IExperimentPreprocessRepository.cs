using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// 実験前処理テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IExperimentPreprocessRepository : IRepositoryForTenant<ExperimentPreprocess>
    {
        /// <summary>
        /// 条件を満たすエントリが存在するか確認する
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<ExperimentPreprocess, bool>> where, bool force);
    }
}

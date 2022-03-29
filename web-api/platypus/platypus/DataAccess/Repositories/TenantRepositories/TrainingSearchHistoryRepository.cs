using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// 学習検索履歴テーブルにアクセスするためのリポジトリ
    /// </summary>
    public class TrainingSearchHistoryRepository : RepositoryForTenantBase<TrainingSearchHistories>, ITrainingSearchHistoryRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainingSearchHistoryRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// 前処理テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class PreprocessRepository : RepositoryForTenantBase<Preprocess>, IPreprocessRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PreprocessRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }

        /// <summary>
        /// 指定された前処理IDの前処理エンティティ（外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">前処理ID</param>
        public async Task<Preprocess> GetIncludeAllAsync(long id)
        {
            return await FindAll(t => t.Id == id)
                .Include(t => t.ContainerRegistry)
                .SingleOrDefaultAsync();
        }
    }
}

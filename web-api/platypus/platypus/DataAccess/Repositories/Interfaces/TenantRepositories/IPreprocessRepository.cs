using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// 前処理テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IPreprocessRepository : IRepositoryForTenant<Preprocess>
    {
        /// <summary>
        /// 指定された前処理IDの前処理エンティティ（外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">前処理ID</param>
        Task<Preprocess> GetIncludeAllAsync(long id);
    }
}

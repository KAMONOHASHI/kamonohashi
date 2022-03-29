using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// 学習検索履歴テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface ITrainingSearchHistoryRepository : IRepositoryForTenant<TrainingSearchHistories>
    {
    }
}

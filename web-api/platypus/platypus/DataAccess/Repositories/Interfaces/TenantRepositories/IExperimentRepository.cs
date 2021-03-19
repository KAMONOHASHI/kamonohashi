using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System.Linq;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// 実験関連テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IExperimentRepository : IRepositoryForTenant<Experiment>
    {
        /// <summary>
        /// 実験前処理を取得する
        /// </summary>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="templateVersionId">テンプレートバージョンID</param>
        /// <param name="dataSetId">アクアリウムデータセットID</param>
        /// <param name="dataSetVersionId">アクアリウムデータセットバージョンID</param>
        /// <returns>ExperimentPreprocess</returns>
        IQueryable<ExperimentPreprocess> GetExperimentPreprocess(long templateId, long templateVersionId,
            long dataSetId, long dataSetVersionId);

        /// <summary>
        /// 新規エントリを追加する
        /// </summary>
        /// <param name="model">追加するエントリ</param>
        void Add<T>(T model) where T : TenantModelBase;
    }
}

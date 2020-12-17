using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels.Aquarium;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories.Aquarium
{
    /// <summary>
    /// アクアリウムデータセット関連テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IDataSetRepository : IRepositoryForTenant<DataSet>
    {
        /// <summary>
        /// アクアリウムデータセットとそのバージョンを取得する
        /// </summary>
        /// <param name="id">アクアリウムデータセットID</param>
        /// <returns>アクアリウムデータセット</returns>
        Task<DataSet> GetDataSetWithVersionsAsync(long id);

        /// <summary>
        /// アクアリウムデータセットバージョンとそのファイルを取得する
        /// </summary>
        /// <param name="datasetId">アクアリウムデータセットID</param>
        /// <param name="versionId">アクアリウムデータセットバージョンID</param>
        /// <returns>アクアリウムデータセット</returns>
        Task<DataSetVersion> GetDataSetVersionWithFilesAsync(long datasetId, long versionId);

        /// <summary>
        /// 新規エントリを追加する
        /// </summary>
        /// <param name="model">追加するエントリ</param>
        void Add<T>(T model) where T : Models.TenantModels.TenantModelBase;
    }
}

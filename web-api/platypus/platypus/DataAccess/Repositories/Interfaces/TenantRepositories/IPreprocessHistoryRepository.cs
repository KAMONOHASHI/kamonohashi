using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// 前処理履歴テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IPreprocessHistoryRepository : IRepositoryForTenant<PreprocessHistory>
    {
        /// <summary>
        /// 全前処理履歴（データと前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <returns>前処理履歴エンティティリスト</returns>
        Task<IEnumerable<PreprocessHistory>> GetAllIncludeDataAndPreprocessAsync();

        /// <summary>
        /// 全前処理履歴（データと前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <returns>前処理履歴エンティティリスト</returns>
        IQueryable<PreprocessHistory> GetAllIncludeDataAndPreprocess();

        /// <summary>
        /// 指定した前処理履歴IDのデータ（データ、前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">前処理履歴ID</param>
        /// <param name="force">選択中のテナント以外も対象とするか</param>
        /// <returns>前処理履歴エンティティ</returns>
        PreprocessHistory GetPreprocessHistoryIncludeDataAndPreprocess(long id, bool force);

        /// <summary>
        /// 指定したデータIDから派生したデータを取得する。
        /// </summary>
        /// <param name="dataId">派生元データID</param>
        IEnumerable<PreprocessHistory> GetPreprocessIncludePreprocessByInputDataId(long dataId);

        /// <summary>
        /// 指定したデータIDと前処理IDに紐づく前処理履歴（データ、前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <param name="preprocessId">前処理ID</param>
        /// <param name="dataId">派生元データID</param>
        Task<PreprocessHistory> GetPreprocessIncludeDataAndPreprocessAsync(long preprocessId, long dataId);

        /// <summary>
        /// 指定した前処理IDに紐づく前処理履歴（データ、前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <param name="preprocessId">前処理ID</param>
        Task<IEnumerable<PreprocessHistory>> GetPreprocessAllIncludeDataAndPreprocessAsync(long preprocessId);

        /// <summary>
        /// 指定したデータIDと前処理名に紐づく前処理履歴（前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <param name="dataId">派生元データID</param>
        /// <param name="preprocessName">前処理名</param>
        Task<PreprocessHistory> GetPreprocessIncludePreprocessAsync(long dataId, string preprocessName);

        /// <summary>
        /// 指定したデータIDを元にした前処理履歴が存在するかチェックします。
        /// </summary>
        /// <param name="id">データID</param>
        /// <returns>True：存在する　False:存在しない</returns>
        Task<bool> ExistsByInputDataIdAsync(long id);

        /// <summary>
        /// 指定したデータIDの派生元データを取得する。
        /// </summary>
        /// <param name="id">データID</param>
        Task<Data> GetInputDataAsync(long id);

        /// <summary>
        /// 指定した前処理履歴の結果の中で、編集不能になっている結果を一件返す。
        /// この結果がnullでない場合は、この前処理履歴結果は削除できない
        /// </summary>
        PreprocessHistoryOutput GetLockedOutput(long id);

        /// <summary>
        /// 指定した前処理履歴から生成されたデータのID一覧を取得する。
        /// </summary>
        IEnumerable<long> GetPreprocessOutputs(long id);

        /// <summary>
        /// 指定した前処理履歴の出力結果として、データを一件追加する。
        /// </summary>
        void AddOutputData(long historyId, Data newData);

    }
}

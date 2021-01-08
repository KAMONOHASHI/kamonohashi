using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// 実験履歴テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    /// TODO 前処理分も追加
    public interface IExperimentPreprocessHistoryRepository : IRepositoryForTenant<ExperimentPreprocessHistory>
    {
        /// <summary>
        /// 全実験の前処理履歴（データセットを含む）を取得します。
        /// </summary>
        IQueryable<ExperimentPreprocessHistory> GetAllIncludeDataSet();


        /// <summary>
        /// 全実験履歴の名前とIDのみ取得する
        /// </summary>
        Task<IEnumerable<ExperimentPreprocessHistory>> GetAllNameAsync();

        /// <summary>
        /// 指定された実験履歴IDの実験履歴エンティティ（全ての外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        Task<ExperimentPreprocessHistory> GetIncludeAllAsync(long id);

        /// <summary>
        /// 指定したデータセットIDから派生したデータを取得する。
        /// </summary>
        /// <param name="dataSetId">派生元データセットID</param>
        IEnumerable<ExperimentPreprocessHistory> GetPreprocecssedDataIncludePreprocessByInputDataSetId(long dataSetId);

        /// <summary>
        /// 指定したデータセットIDとテンプレートIDに紐づく実験前処理履歴（データ、前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="dataSetId">派生元データセットID</param>
        Task<ExperimentPreprocessHistory> GetPreprocessIncludeDataSetAndTemplateAsync(long templateId, long dataSetId);

        /// <summary>
        /// データセットIDに紐づく実験の前処理履歴が存在するかチェックします。
        /// </summary>
        /// <param name="datasetId">データセットID</param>
        /// <returns>True:存在する　False:存在しない</returns>
        Task<bool> ExistsByDataSetIdAsync(long datasetId);

        /// <summary>
        /// 検索条件に合致するデータを一件取得します。
        /// </summary>
        /// <param name="where">検索条件</param>
        /// <param name="force">選択中のテナント以外も対象とするか</param>
        ExperimentPreprocessHistory Find(Expression<Func<ExperimentPreprocessHistory, bool>> where, bool force);

        /// <summary>
        /// 実験履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, bool force);

        /// <summary>
        /// 実験履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="startedAt">開始日時</param>
        /// <param name="completedAt">停止日時</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task UpdateStatusAsync(long id, ContainerStatus status, DateTime? startedAt, DateTime completedAt, bool force);

        /// <summary>
        /// 指定した実験前処理履歴から生成されたデータのID一覧を取得する。
        /// </summary>
        IEnumerable<long> GetExperimentPreprocessOutputs(long id);

        /// <summary>
        /// 指定した実験前処理履歴の出力結果として、データを一件追加する。
        /// </summary>
        void AddOutputData(long historyId, Data newData);
        #region 添付ファイル操作

        /// <summary>
        /// 指定したIDの実験履歴に紐づく全ての添付ファイルを取得します。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <returns>添付ファイル一覧</returns>
        Task<IEnumerable<ExperimentPreprocessHistoryAttachedFile>> GetAllAttachedFilesAsync(long id);

        /// <summary>
        /// 指定したIDの実験履歴添付ファイルを取得します。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <returns>添付ファイル</returns>
        Task<ExperimentPreprocessHistoryAttachedFile> GetAttachedFileAsync(long id);

        /// <summary>
        /// 指定したIDの実験履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns> True:登録済み　False:未登録</returns>
        Task<bool> ExistsAttachedFileAsync(long id, string fileName);

        /// <summary>
        /// 実験履歴添付ファイルを追加します。
        /// </summary>
        /// <param name="file">追加対象のファイル</param>
        void AddAttachedFile(ExperimentPreprocessHistoryAttachedFile file);

        /// <summary>
        /// 指定した実験履歴添付ファイルを削除します。
        /// </summary>
        /// <param name="file">削除対象のファイル</param>
        void DeleteAttachedFile(ExperimentPreprocessHistoryAttachedFile file);
        #endregion
    }
}

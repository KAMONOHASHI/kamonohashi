using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// データセットテーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IDataSetRepository : IRepositoryForTenant<DataSet>
    {

        /// <summary>
        /// すべてのデータセット（データセットエントリの外部参照を含む）を取得します。
        /// </summary>
        /// <returns>データセットエンティティリスト</returns>
        Task<IEnumerable<DataSet>> GetAllIncludeDataSetEntryAsync();

        /// <summary>
        /// 指定したデータセットIDのデータセット（データセットエントリの外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">データセットID</param>
        /// <returns>データセットエンティティ</returns>
        Task<DataSet> GetDataSetIncludeDataSetEntryAsync(long id);

        /// <summary>
        /// 指定したデータセットIDのデータセット（データセットエントリからデータファイルの外部参照までを含む）を取得します。
        /// </summary>
        /// <param name="id">データセットID</param>
        /// <returns>データセットエンティティ</returns>
        Task<DataSet> GetDataSetIncludeDataSetEntryAndDataAsync(long id);

        /// <summary>
        /// 指定したデータセットのエントリを全て削除する。
        /// </summary>
        void DeleteAllEntries(long dataSetId);

        /// <summary>
        /// 新規にデータセットエントリを追加する
        /// </summary>
        void AddEntry(DataSet dataSet, long dataTypeId, long dataId, bool isCreate);

        /// <summary>
        /// 指定したデータが含まれる、更新不能なデータセットを取得する
        /// </summary>
        DataSet GetLockedDataSetByData(long dataId);


        /// <summary>
        /// 指定したデータを、全データセットから外す。
        /// データ自体の削除はしない。
        /// 事前に削除可能か、判定しておくこと。
        /// </summary>
        void RemoveDataFromDataSet(long dataId);
    }
}

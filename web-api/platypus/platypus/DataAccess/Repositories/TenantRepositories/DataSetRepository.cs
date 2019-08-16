using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// データセットテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class DataSetRepository : RepositoryForTenantBase<DataSet>, IDataSetRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataSetRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }

        /// <summary>
        /// すべてのデータセット（データセットエントリの外部参照を含む）を取得します。
        /// <see cref="DataSet.DataSetEntries"/>の<see cref="DataSetEntry.Data"/>からの外部参照は含まれません。
        /// </summary>
        /// <returns>
        /// データセットエンティティリスト
        /// </returns>
        public async Task<IEnumerable<DataSet>> GetAllIncludeDataSetEntryAsync()
        {
            return await GetAll().Include(d => d.DataSetEntries).ThenInclude(d => d.DataType)
                                             .OrderByDescending(t => t.Id).ToListAsync();
        }

        /// <summary>
        /// 指定したデータセットIDのデータセット（データセットエントリの外部参照を含む）を取得します。
        /// <see cref="DataSet.DataSetEntries"/>の<see cref="DataSetEntry.Data"/>からの外部参照は含まれません。
        /// </summary>
        /// <param name="id">データセットID</param>
        /// <returns>
        /// データセットエンティティ
        /// </returns>
        public async Task<DataSet> GetDataSetIncludeDataSetEntryAsync(long id)
        {
            return await GetAll().Include(d => d.DataSetEntries).ThenInclude(d => d.Data)
                .ThenInclude(d => d.TagMaps).ThenInclude(tm => tm.Tag)
                .Include(d => d.DataSetEntries).ThenInclude(d => d.DataType)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// 指定したデータセットIDのデータセット（データセットエントリからデータファイルの外部参照までを含む）を取得します。
        /// </summary>
        /// <param name="id">データセットID</param>
        /// <returns>
        /// データセットエンティティ
        /// </returns>
        public async Task<DataSet> GetDataSetIncludeDataSetEntryAndDataAsync(long id)
        {
            return await GetAll().Include(d => d.DataSetEntries).ThenInclude(d => d.Data)
                .ThenInclude(d => d.DataProperties).ThenInclude(d => d.DataFile)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// 指定したデータセットのエントリを全て削除する。
        /// </summary>
        public void DeleteAllEntries(long dataSetId)
        {
            DeleteModelAll<DataSetEntry>(d => d.DataSetId == dataSetId);
        }

        /// <summary>
        /// 新規にデータセットエントリを追加する
        /// </summary>
        public void AddEntry(DataSet dataSet, long dataTypeId, long dataId, bool isCreate)
        {
            DataSetEntry entry = new DataSetEntry()
            {
                DataTypeId = dataTypeId,
                DataId = dataId
            };
            if (isCreate)
            {
                entry.DataSet = dataSet;
            }
            else
            {
                entry.DataSetId = dataSet.Id;
            }
            AddModel<DataSetEntry>(entry);
        }

        /// <summary>
        /// 指定したデータが含まれる、更新不能なデータセットを取得する
        /// </summary>
        public DataSet GetLockedDataSetByData(long dataId)
        {
            var lockedEntry = GetModelAll<DataSetEntry>().Include(e => e.DataSet).Where(entry => entry.DataId == dataId && entry.DataSet.IsLocked == true).FirstOrDefault();
            if(lockedEntry == null)
            {
                return null;
            }
            return lockedEntry.DataSet;
        }

        /// <summary>
        /// 指定したデータを、全データセットから外す。
        /// データ自体の削除はしない。
        /// 事前に削除可能か、判定しておくこと。
        /// </summary>
        public void RemoveDataFromDataSet(long dataId)
        {
            DeleteModelAll<DataSetEntry>(entry => entry.DataId == dataId);
        }
    }
}

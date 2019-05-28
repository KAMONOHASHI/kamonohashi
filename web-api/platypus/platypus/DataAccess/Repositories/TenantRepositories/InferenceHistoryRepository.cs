using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// 推論履歴テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class InferenceHistoryRepository : RepositoryForTenantBase<InferenceHistory>, IInferenceHistoryRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InferenceHistoryRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }

        /// <summary>
        /// 全推論履歴（データセットを含む）を取得します。
        /// </summary>
        public IQueryable<InferenceHistory> GetAllIncludeDataSet()
        {
            return GetAll().Include(t => t.DataSet);
        }

        /// <summary>
        /// 全学習履歴（データセットを含む）を並べ替えありで取得します。
        /// </summary>
        public IQueryable<InferenceHistory> GetAllIncludeDataSetWithOrdering()
        {
            return GetAll().OrderByDescending(t => t.Favorite).ThenByDescending(t => t.Id).Include(t => t.DataSet).Include(t => t.Parent);
        }

        /// <summary>
        /// 全学習履歴の名前とIDのみ取得する
        /// </summary>
        public async Task<IEnumerable<InferenceHistory>> GetAllNameAsync()
        {
            return await GetAll()
                .Select(t => new InferenceHistory() { Id = t.Id, Name = t.Name, Memo = t.Memo, Status = t.Status })
                .OrderByDescending(t => t.Id).ToListAsync();
        }

        /// <summary>
        /// 指定された推論履歴IDの推論履歴エンティティ（外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        public async Task<InferenceHistory> GetIncludeAllAsync(long id)
        {
            return await FindAll(t => t.Id == id).Include(t => t.DataSet)
                .Include(t => t.Parent)
                .Include(t => t.InferenceHistoryAttachedFile)
                .Include(t => t.ContainerRegistry)
                .SingleOrDefaultAsync();
        }


        /// <summary>
        /// データセットIDに紐づく推論履歴が存在するかチェックします。
        /// </summary>
        /// <param name="datasetId">データセットID</param>
        /// <returns>
        /// True:存在する　False:存在しない
        /// </returns>
        public async Task<bool> ExistsByDataSetIdAsync(long datasetId)
        {
            return await ExistsAsync(x => x.DataSetId == datasetId);
        }

        public override void Add(InferenceHistory entity)
        {
            if (entity.OptionDic != null && entity.OptionDic.Count > 0)
            {
                entity.Options = JsonConvert.SerializeObject(entity.OptionDic);
            }
            base.Add(entity);
        }

        /// <summary>
        /// 検索条件に合致するデータを一件取得します。
        /// </summary>
        public InferenceHistory Find(Expression<Func<InferenceHistory, bool>> where, bool force)
        {
            return FindModel<InferenceHistory>(where, force);
        }

        /// <summary>
        /// 学習履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        public async Task UpdateStatusAsync(long id, ContainerStatus status, bool force)
        {
            var history = await this.GetByIdAsync(id, force);
            history.Status = status.Key;
            UpdateModel<InferenceHistory>(history, force);
        }

        /// <summary>
        /// 学習履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        public async Task UpdateStatusAsync(long id, ContainerStatus status, DateTime? startedAt, DateTime completedAt, bool force)
        {
            var history = await this.GetByIdAsync(id, force);
            history.CompletedAt = completedAt;
            if (history.StartedAt == null)
            {
                history.StartedAt = startedAt;
            }
            history.Status = status.Key;
            UpdateModel<InferenceHistory>(history, force);
        }

        public override void Delete(InferenceHistory entity)
        {
            DeleteAsync(entity).Wait();
        }

        public async Task DeleteAsync(InferenceHistory entity)
        {
            base.Delete(entity);

            //自分以外に同じデータセットを使っている履歴がなければ、データセットのロック状態を解除する
            var other = await ExistsAsync(th => th.DataSetId == entity.DataSetId && th.Id != entity.Id);
            if (other == false)
            {
                var dataSet = await GetModelByIdAsync<DataSet>(entity.DataSetId);
                dataSet.IsLocked = false;
            }
        }

        #region 添付ファイル操作

        /// <summary>
        /// 指定したIDの推論履歴に紐づく全ての添付ファイルを取得します。
        /// </summary>
        public async Task<IEnumerable<InferenceHistoryAttachedFile>> GetAllAttachedFilesAsync(long id)
        {
            return await FindModelAll<InferenceHistoryAttachedFile>(x => x.InferenceHistoryId == id).OrderBy(x => x.FileName).ToListAsync();
        }

        /// <summary>
        /// 指定したIDの推論履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        public async Task<bool> ExistsAttachedFileAsync(long id, string fileName)
        {
            return await ExistsModelAsync<InferenceHistoryAttachedFile>(x => x.InferenceHistoryId == id && x.FileName == fileName);
        }

        /// <summary>
        /// 指定したIDの推論履歴添付ファイルを取得します。
        /// </summary>
        public async Task<InferenceHistoryAttachedFile> GetAttachedFileAsync(long id)
        {
            return await GetModelByIdAsync<InferenceHistoryAttachedFile>(id);
        }

        /// <summary>
        /// 推論履歴添付ファイルを追加します。
        /// </summary>
        public void AddAttachedFile(InferenceHistoryAttachedFile file)
        {
            AddModel<InferenceHistoryAttachedFile>(file);
        }

        /// <summary>
        /// 指定した推論履歴添付ファイルを削除します。
        /// </summary>
        public void DeleteAttachedFile(InferenceHistoryAttachedFile file)
        {
            DeleteModel<InferenceHistoryAttachedFile>(file);
        }
        #endregion
    }
}

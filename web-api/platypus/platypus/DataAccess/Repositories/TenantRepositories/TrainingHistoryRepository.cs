using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// 学習履歴テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class TrainingHistoryRepository : RepositoryForTenantBase<TrainingHistory>, ITrainingHistoryRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainingHistoryRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }

        /// <summary>
        /// 全学習履歴（データセットを含む）を取得します。
        /// </summary>
        public IQueryable<TrainingHistory> GetAllIncludeDataSet()
        {
            return GetAll().Include(t => t.DataSet);
        }

        /// <summary>
        /// 全学習履歴（データセットを含む）を並べ替えありで取得します。
        /// </summary>
        public IQueryable<TrainingHistory> GetAllIncludeDataSetWithOrdering()
        {
            return GetAll().OrderByDescending(t => t.Favorite).ThenByDescending(t => t.Id).Include(t => t.DataSet);
        }

        /// <summary>
        /// 全学習履歴の名前とIDのみ取得する
        /// </summary>
        public async Task<IEnumerable<TrainingHistory>> GetAllNameAsync()
        {
            return await GetAll()
                .Select(t => new TrainingHistory() { Id = t.Id, Name = t.Name, Memo = t.Memo, Status = t.Status })
                .OrderByDescending(t => t.Id).ToListAsync();
        }

        /// <summary>
        /// 指定された学習履歴IDの学習履歴エンティティ（外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        public async Task<TrainingHistory> GetIncludeAllAsync(long id)
        {
            return await FindAll(t => t.Id == id).Include(t => t.DataSet)
                .Include(t => t.Parent)
                .Include(t => t.TrainingHistoryAttachedFile)
                .Include(t => t.ContainerRegistry)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// データセットIDに紐づく学習履歴が存在するかチェックします。
        /// </summary>
        /// <param name="datasetId">データセットID</param>
        /// <returns>
        /// True:存在する　False:存在しない
        /// </returns>
        public async Task<bool> ExistsByDataSetIdAsync(long datasetId)
        {
            return await ExistsAsync(x => x.DataSetId == datasetId);
        }

        public override void Add(TrainingHistory entity)
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
        public TrainingHistory Find(Expression<Func<TrainingHistory, bool>> where, bool force)
        {
            return FindModel<TrainingHistory>(where, force);
        }

        /// <summary>
        /// 学習履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        public async Task UpdateStatusAsync(long id, ContainerStatus status, bool force)
        {
            var history = await this.GetByIdAsync(id, force);
            history.Status = status.Key;
            UpdateModel<TrainingHistory>(history, force);
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
            UpdateModel<TrainingHistory>(history, force);
        }

        public override void Delete(TrainingHistory entity)
        {
            DeleteAsync(entity).Wait();
        }

        public async Task DeleteAsync(TrainingHistory entity)
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
        /// 指定したIDの学習履歴に紐づく全ての添付ファイルを取得します。
        /// </summary>
        public async Task<IEnumerable<TrainingHistoryAttachedFile>> GetAllAttachedFilesAsync(long id)
        {
            return await FindModelAll<TrainingHistoryAttachedFile>(x => x.TrainingHistoryId == id).OrderBy(x => x.FileName).ToListAsync();
        }

        /// <summary>
        /// 指定したIDの学習履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        public async Task<bool> ExistsAttachedFileAsync(long id, string fileName)
        {
            return await ExistsModelAsync<TrainingHistoryAttachedFile>(x => x.TrainingHistoryId == id && x.FileName == fileName);
        }

        /// <summary>
        /// 指定したIDの学習履歴添付ファイルを取得します。
        /// </summary>
        public async Task<TrainingHistoryAttachedFile> GetAttachedFileAsync(long id)
        {
            return await GetModelByIdAsync<TrainingHistoryAttachedFile>(id);
        }

        /// <summary>
        /// 学習履歴添付ファイルを追加します。
        /// </summary>
        public void AddAttachedFile(TrainingHistoryAttachedFile file)
        {
            AddModel<TrainingHistoryAttachedFile>(file);
        }

        /// <summary>
        /// 指定した学習履歴添付ファイルを削除します。
        /// </summary>
        public void DeleteAttachedFile(TrainingHistoryAttachedFile file)
        {
            DeleteModel<TrainingHistoryAttachedFile>(file);
        }
        #endregion
    }
}

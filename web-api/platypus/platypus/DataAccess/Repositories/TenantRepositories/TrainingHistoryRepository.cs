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
        /// 全学習履歴（データセット、親学習を含む）を並べ替えありで取得します。
        /// </summary>
        public IQueryable<TrainingHistory> GetAllIncludeDataSetAndParentWithOrdering()
        {
            return GetAll().OrderByDescending(t => t.Favorite).ThenByDescending(t => t.Id)
                .Include(t => t.DataSet)
                .Include(t => t.ParentMaps).ThenInclude(map => map.Parent);
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
                .Include(t => t.ParentMaps).ThenInclude(map => map.Parent)
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

        /// <summary>
        /// 学習履歴を追加
        /// </summary>
        /// <param name="entity">学習履歴</param>
        public override void Add(TrainingHistory entity)
        {
            if (entity.OptionDic != null && entity.OptionDic.Count > 0)
            {
                entity.Options = JsonConvert.SerializeObject(entity.OptionDic);
            }
            if(entity.PortList != null && entity.PortList.Count > 0)
            {
                entity.Ports = JsonConvert.SerializeObject(entity.PortList);
            }
            base.Add(entity);
        }

        /// <summary>
        /// 検索条件に合致するデータを一件取得します。
        /// </summary>
        /// <param name="where">検索条件</param>
        /// <param name="force">選択中のテナント以外も対象とするか</param>
        public TrainingHistory Find(Expression<Func<TrainingHistory, bool>> where, bool force)
        {
            return FindModel<TrainingHistory>(where, force);
        }

        /// <summary>
        /// 学習履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
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
        /// <param name="id">学習履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="startedAt">開始日時</param>
        /// <param name="completedAt">停止日時</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
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

        /// <summary>
        /// 学習履歴を削除
        /// </summary>
        /// <param name="entity">学習履歴</param>
        public override void Delete(TrainingHistory entity)
        {
            DeleteAsync(entity).Wait();
        }

        /// <summary>
        /// 学習履歴を削除
        /// </summary>
        /// <param name="entity">学習履歴</param>
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

        /// <summary>
        /// 派生した学習履歴を取得する
        /// </summary>
        /// <param name="id">親学習ID</param>
        public async Task<IEnumerable<TrainingHistoryParentMap>> GetChildrenAsync(long id)
        {
            return await FindModelAll<TrainingHistoryParentMap>(x => x.ParentId == id).Include(t => t.TrainingHistory).OrderBy(x => x.Id).ToListAsync();
        }

        /// <summary>
        /// 学習履歴に親学習を紐づける
        /// </summary>
        /// <param name="trainingHistory">学習履歴</param>
        /// <param name="parent">親学習履歴</param>
        public TrainingHistoryParentMap AttachParentAsync(TrainingHistory trainingHistory, TrainingHistory parent)
        {
            if (parent == null)
            {
                //指定がなければ何もしない
                return null;
            }

            TrainingHistoryParentMap map = new TrainingHistoryParentMap()
            {
                TrainingHistoryId = trainingHistory.Id,
                ParentId = parent.Id
            };

            AddModel(map);
            return map;
        }

        #region 添付ファイル操作

        /// <summary>
        /// 指定したIDの学習履歴に紐づく全ての添付ファイルを取得します。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <returns>添付ファイル一覧</returns>
        public async Task<IEnumerable<TrainingHistoryAttachedFile>> GetAllAttachedFilesAsync(long id)
        {
            return await FindModelAll<TrainingHistoryAttachedFile>(x => x.TrainingHistoryId == id).OrderBy(x => x.FileName).ToListAsync();
        }

        /// <summary>
        /// 指定したIDの学習履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns> True:登録済み　False:未登録</returns>
        public async Task<bool> ExistsAttachedFileAsync(long id, string fileName)
        {
            return await ExistsModelAsync<TrainingHistoryAttachedFile>(x => x.TrainingHistoryId == id && x.FileName == fileName);
        }

        /// <summary>
        /// 指定したIDの学習履歴添付ファイルを取得します。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <returns>添付ファイル</returns>
        public async Task<TrainingHistoryAttachedFile> GetAttachedFileAsync(long id)
        {
            return await GetModelByIdAsync<TrainingHistoryAttachedFile>(id);
        }

        /// <summary>
        /// 学習履歴添付ファイルを追加します。
        /// </summary>
        /// <param name="file">追加対象のファイル</param>
        public void AddAttachedFile(TrainingHistoryAttachedFile file)
        {
            AddModel<TrainingHistoryAttachedFile>(file);
        }

        /// <summary>
        /// 指定した学習履歴添付ファイルを削除します。
        /// </summary>
        /// <param name="file">削除対象のファイル</param>
        public void DeleteAttachedFile(TrainingHistoryAttachedFile file)
        {
            DeleteModel<TrainingHistoryAttachedFile>(file);
        }
        #endregion
    }
}

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
        /// 全推論履歴（データセット、親学習を含む）を並べ替えありで取得します。
        /// </summary>
        public IQueryable<InferenceHistory> GetAllIncludeDataSetAndParentWithOrdering()
        {
            return GetAll().OrderByDescending(t => t.Favorite).ThenByDescending(t => t.Id)
                .Include(t => t.DataSet)
                .Include(t => t.ParentMaps).ThenInclude(map => map.Parent);
        }

        /// <summary>
        /// 全推論履歴の名前とIDのみ取得する
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
        /// <param name="id">推論履歴ID</param>
        public async Task<InferenceHistory> GetIncludeAllAsync(long id)
        {
            return await FindAll(t => t.Id == id).Include(t => t.DataSet)
                .Include(t => t.ParentMaps).ThenInclude(map => map.Parent)
                                           .ThenInclude(p => p.DataSet)
                .Include(t => t.ParentMaps).ThenInclude(map => map.Parent)
                                           .ThenInclude(p => p.TagMaps)
                                           .ThenInclude(map => map.Tag)
                .Include(t => t.ParentInferenceMaps).ThenInclude(map => map.Parent)
                                            .ThenInclude(p => p.DataSet)
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

        /// <summary>
        /// 推論履歴を追加
        /// </summary>
        /// <param name="entity">推論履歴</param>
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
        /// <param name="where">検索条件</param>
        /// <param name="force">選択中のテナント以外も対象とするか</param>
        public InferenceHistory Find(Expression<Func<InferenceHistory, bool>> where, bool force)
        {
            return FindModel<InferenceHistory>(where, force);
        }

        /// <summary>
        /// 推論履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public async Task UpdateStatusAsync(long id, ContainerStatus status, bool force)
        {
            var history = await this.GetByIdAsync(id, force);
            history.Status = status.Key;
            UpdateModel<InferenceHistory>(history, force);
        }

        /// <summary>
        /// 推論履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
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
            UpdateModel<InferenceHistory>(history, force);
        }

        /// <summary>
        /// 推論履歴を削除
        /// </summary>
        /// <param name="entity">推論履歴</param>
        public override void Delete(InferenceHistory entity)
        {
            DeleteAsync(entity).Wait();
        }

        /// <summary>
        /// 推論履歴を削除
        /// </summary>
        /// <param name="entity">推論履歴</param>
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

        /// <summary>
        /// 指定したIDの学習履歴を利用した推論履歴を取得する
        /// </summary>
        /// <param name="id">マウントされた学習ID</param>
        public async Task<IEnumerable<InferenceHistoryParentMap>> GetMountedTrainingAsync(long id)
        {
            return await FindModelAll<InferenceHistoryParentMap>(x => x.ParentId == id).Include(t => t.InferenceHistory).OrderBy(x => x.Id).ToListAsync();
        }

        /// <summary>
        /// 推論履歴に親学習を紐づける
        /// </summary>
        /// <param name="history">推論履歴</param>
        /// <param name="parent">親学習履歴</param>
        public InferenceHistoryParentMap AttachParentAsync(InferenceHistory history, TrainingHistory parent)
        {
            if (parent == null)
            {
                //指定がなければ何もしない
                return null;
            }

            InferenceHistoryParentMap map = new InferenceHistoryParentMap()
            {
                InferenceHistoryId = history.Id,
                ParentId = parent.Id
            };

            AddModel(map);
            return map;
        }

        /// <summary>
        /// 推論履歴IDに親推論履歴IDを紐づける
        /// </summary>
        /// <param name="inferenceHistory">推論履歴履歴</param>
        /// <param name="parentInference">親推論履歴</param>
        public InferenceHistoryParentInferenceMap AttachParentInferenceToInferenceAsync(InferenceHistory inferenceHistory, InferenceHistory parentInference)
        {
            if (parentInference == null)
            {
                //指定がなければ何もしない
                return null;
            }

            InferenceHistoryParentInferenceMap map = new InferenceHistoryParentInferenceMap()
            {
                InferenceHistoryId = inferenceHistory.Id,
                ParentId = parentInference.Id
            };

            AddModel(map);
            return map;
        }

        /// <summary>
        /// 推論履歴IDに紐づいている親推論履歴IDを解除する
        /// </summary>
        /// <param name="inferenceHistory">推論履歴</param>
        public void DetachParentInferenceToInferenceAsync(InferenceHistory inferenceHistory)
        {
            DeleteModelAll<InferenceHistoryParentInferenceMap>(map => map.InferenceHistoryId == inferenceHistory.Id);
        }

        #region 添付ファイル操作

        /// <summary>
        /// 指定したIDの推論履歴に紐づく全ての添付ファイルを取得します。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        /// <returns>添付ファイル一覧</returns>
        public async Task<IEnumerable<InferenceHistoryAttachedFile>> GetAllAttachedFilesAsync(long id)
        {
            return await FindModelAll<InferenceHistoryAttachedFile>(x => x.InferenceHistoryId == id).OrderBy(x => x.FileName).ToListAsync();
        }

        /// <summary>
        /// 指定したIDの推論履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns> True:登録済み　False:未登録</returns>
        public async Task<bool> ExistsAttachedFileAsync(long id, string fileName)
        {
            return await ExistsModelAsync<InferenceHistoryAttachedFile>(x => x.InferenceHistoryId == id && x.FileName == fileName);
        }

        /// <summary>
        /// 指定したIDの推論履歴添付ファイルを取得します。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        /// <returns>添付ファイル</returns>
        public async Task<InferenceHistoryAttachedFile> GetAttachedFileAsync(long id)
        {
            return await GetModelByIdAsync<InferenceHistoryAttachedFile>(id);
        }

        /// <summary>
        /// 推論履歴添付ファイルを追加します。
        /// </summary>
        /// <param name="file">追加対象のファイル</param>
        public void AddAttachedFile(InferenceHistoryAttachedFile file)
        {
            AddModel<InferenceHistoryAttachedFile>(file);
        }

        /// <summary>
        /// 指定した推論履歴添付ファイルを削除します。
        /// </summary>
        /// <param name="file">削除対象のファイル</param>
        public void DeleteAttachedFile(InferenceHistoryAttachedFile file)
        {
            DeleteModel<InferenceHistoryAttachedFile>(file);
        }
        #endregion
    }
}

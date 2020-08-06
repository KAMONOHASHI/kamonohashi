﻿using Microsoft.EntityFrameworkCore;
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
    /// ノートブック履歴テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class NotebookHistoryRepository : RepositoryForTenantBase<NotebookHistory>, INotebookHistoryRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NotebookHistoryRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }

        /// <summary>
        /// 全ノートブック履歴を並べ替えありで取得する
        /// </summary>
        public IQueryable<NotebookHistory> GetAllWithOrdering()
        {
            return GetAll().OrderByDescending(t => t.Favorite).ThenByDescending(t => t.Id);
        }

        /// <summary>
        /// 全ノートブック履歴の名前とIDのみ取得する
        /// </summary>
        public async Task<IEnumerable<NotebookHistory>> GetAllNameAsync()
        {
            return await GetAll()
                .Select(t => new NotebookHistory() { Id = t.Id, Name = t.Name, Memo = t.Memo, Status = t.Status })
                .OrderByDescending(t => t.Id).ToListAsync();
        }

        /// <summary>
        /// 指定されたノートブック履歴IDのノートブック履歴エンティティ（外部参照を含む）を取得する
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        public async Task<NotebookHistory> GetIncludeAllAsync(long id)
        {
            return await FindAll(t => t.Id == id)
                            .Include(t => t.DataSet)
                            .Include(t => t.ContainerRegistry)
                            .Include(t => t.ParentTrainingMaps).ThenInclude(map => map.Parent)
                                                               .ThenInclude(p => p.DataSet)
                            .Include(t => t.ParentTrainingMaps).ThenInclude(map => map.Parent)
                                                               .ThenInclude(p => p.TagMaps)
                                                               .ThenInclude(map => map.Tag)
                            .Include(t => t.ParentInferenceMaps).ThenInclude(map => map.Parent)
                                                                .ThenInclude(p => p.DataSet)
                            .SingleOrDefaultAsync();
        }

        /// <summary>
        /// テナント横断で全データ（外部参照を含む）をすべて取得する。（取得結果はキャッシュされない）
        /// ソートはIDの逆順。
        /// </summary>
        public async Task<IEnumerable<NotebookHistory>> GetAllIncludeTenantAsNoTrackingAsync()
        {
            return await GetModelAll<NotebookHistory>(true).Include(t => t.Tenant)
                .OrderByDescending(t => t.Id).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 検索条件に合致するデータを一件取得する
        /// </summary>
        /// <param name="where">検索条件</param>
        /// <param name="force">選択中のテナント以外も対象とするか</param>
        public NotebookHistory Find(Expression<Func<NotebookHistory, bool>> where, bool force)
        {
            return FindModel<NotebookHistory>(where, force);
        }

        /// <summary>
        /// ノートブック履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public async Task UpdateStatusAsync(long id, ContainerStatus status, bool force)
        {
            var history = await this.GetByIdAsync(id, force);
            history.Status = status.Key;
            UpdateModel<NotebookHistory>(history, force);
        }

        /// <summary>
        /// ノートブック履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="completedAt">停止日時</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public async Task UpdateStatusAsync(long id, ContainerStatus status, DateTime completedAt, bool force)
        {
            var history = await this.GetByIdAsync(id, force);
            history.CompletedAt = completedAt;
            history.Status = status.Key;
            UpdateModel<NotebookHistory>(history, force);
        }

        /// <summary>
        /// ノートブック履歴を追加
        /// </summary>
        /// <param name="entity">ノートブック履歴</param>
        public override void Add(NotebookHistory entity)
        {
            if (entity.OptionDic != null && entity.OptionDic.Count > 0)
            {
                entity.Options = JsonConvert.SerializeObject(entity.OptionDic);
            }
            base.Add(entity);
        }

        /// <summary>
        /// ノートブック履歴を削除
        /// </summary>
        /// <param name="entity">ノートブック履歴</param>
        public override void Delete(NotebookHistory entity)
        {
            base.Delete(entity);
        }

        /// <summary>
        /// ノートブック履歴IDに親学習履歴IDを紐づける
        /// </summary>
        /// <param name="notebookHistory">ノートブック履歴</param>
        /// <param name="parent">親学習履歴</param>
        public NotebookHistoryParentTrainingMap AttachParentToNotebookAsync(NotebookHistory notebookHistory, TrainingHistory parent)
        {
            if (parent == null)
            {
                //指定がなければ何もしない
                return null;
            }

            NotebookHistoryParentTrainingMap map = new NotebookHistoryParentTrainingMap()
            {
                NotebookHistoryId = notebookHistory.Id,
                ParentId = parent.Id
            };

            AddModel(map);
            return map;
        }

        /// <summary>
        /// ノートブック履歴IDに紐づいている親学習履歴IDを解除する
        /// </summary>
        /// <param name="notebookHistory">ノートブック履歴</param>
        public void DetachParentToNotebookAsync(NotebookHistory notebookHistory)
        {
            DeleteModelAll<NotebookHistoryParentTrainingMap>(map => map.NotebookHistoryId == notebookHistory.Id);
        }

        /// <summary>
        /// ノートブック履歴IDに親推論履歴IDを紐づける
        /// </summary>
        /// <param name="notebookHistory">ノートブック履歴</param>
        /// <param name="parentInference">親推論履歴</param>
        public NotebookHistoryParentInferenceMap AttachParentInferenceToNotebookAsync(NotebookHistory notebookHistory, InferenceHistory parentInference)
        {
            if (parentInference == null)
            {
                //指定がなければ何もしない
                return null;
            }

            NotebookHistoryParentInferenceMap map = new NotebookHistoryParentInferenceMap()
            {
                NotebookHistoryId = notebookHistory.Id,
                ParentId = parentInference.Id
            };

            AddModel(map);
            return map;
        }

        /// <summary>
        /// ノートブック履歴IDに紐づいている親推論履歴IDを解除する
        /// </summary>
        /// <param name="notebookHistory">ノートブック履歴</param>
        public void DetachParentInferenceToNotebookAsync(NotebookHistory notebookHistory)
        {
            DeleteModelAll<NotebookHistoryParentInferenceMap>(map => map.NotebookHistoryId == notebookHistory.Id);
        }
    }
}

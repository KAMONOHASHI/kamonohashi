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
    /// 実験履歴テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class ExperimentHistoryRepository : RepositoryForTenantBase<ExperimentHistory>, IExperimentHistoryRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExperimentHistoryRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }

        /// <summary>
        /// 全実験履歴（データセットを含む）を取得します。
        /// </summary>
        public IQueryable<ExperimentHistory> GetAllIncludeDataSet()
        {
            return GetAll().Include(t => t.DataSet);
        }

        /// <summary>
        /// 全実験履歴（データセット、テンプレートを含む）を並べ替えありで取得します。
        /// </summary>
        public IQueryable<ExperimentHistory> GetAllIncludeDataSetAndParentWithOrdering()
        {
            return GetAll().OrderByDescending(t => t.Id)
                .Include(t => t.DataSet)
                .Include(t => t.Template);
        }

        /// <summary>
        /// 全実験履歴の名前とIDのみ取得する
        /// </summary>
        public async Task<IEnumerable<ExperimentHistory>> GetAllNameAsync()
        {
            return await GetAll()
                .Select(t => new ExperimentHistory() { Id = t.Id, Name = t.Name, Status = t.Status })
                .OrderByDescending(t => t.Id).ToListAsync();
        }

        /// <summary>
        /// 指定された実験履歴IDの実験履歴エンティティ（外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        public async Task<ExperimentHistory> GetIncludeAllAsync(long id)
        {
            return await FindAll(t => t.Id == id).Include(t => t.DataSet)
                .Include(t => t.Template)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// データセットIDに紐づく実験履歴が存在するかチェックします。
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
        /// 実験履歴を追加
        /// </summary>
        /// <param name="entity">実験履歴</param>
        public override void Add(ExperimentHistory entity)
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
        public ExperimentHistory Find(Expression<Func<ExperimentHistory, bool>> where, bool force)
        {
            return FindModel<ExperimentHistory>(where, force);
        }

        /// <summary>
        /// 実験履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public async Task UpdateStatusAsync(long id, ContainerStatus status, bool force)
        {
            var history = await this.GetByIdAsync(id, force);
            history.Status = status.Key;
            UpdateModel<ExperimentHistory>(history, force);
        }

        /// <summary>
        /// 実験履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
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
            UpdateModel<ExperimentHistory>(history, force);
        }

        /// <summary>
        /// 実験履歴を削除
        /// </summary>
        /// <param name="entity">実験履歴</param>
        public override void Delete(ExperimentHistory entity)
        {
            DeleteAsync(entity).Wait();
        }

        /// <summary>
        /// 実験履歴を削除
        /// </summary>
        /// <param name="entity">実験履歴</param>
        public async Task DeleteAsync(ExperimentHistory entity)
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
        /// 指定したIDの実験履歴に紐づく全ての添付ファイルを取得します。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <returns>添付ファイル一覧</returns>
        public async Task<IEnumerable<ExperimentHistoryAttachedFile>> GetAllAttachedFilesAsync(long id)
        {
            return await FindModelAll<ExperimentHistoryAttachedFile>(x => x.ExperimentHistoryId == id).OrderBy(x => x.FileName).ToListAsync();
        }

        /// <summary>
        /// 指定したIDの実験履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns> True:登録済み　False:未登録</returns>
        public async Task<bool> ExistsAttachedFileAsync(long id, string fileName)
        {
            return await ExistsModelAsync<ExperimentHistoryAttachedFile>(x => x.ExperimentHistoryId == id && x.FileName == fileName);
        }

        /// <summary>
        /// 指定したIDの実験履歴添付ファイルを取得します。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <returns>添付ファイル</returns>
        public async Task<ExperimentHistoryAttachedFile> GetAttachedFileAsync(long id)
        {
            return await GetModelByIdAsync<ExperimentHistoryAttachedFile>(id);
        }

        /// <summary>
        /// 実験履歴添付ファイルを追加します。
        /// </summary>
        /// <param name="file">追加対象のファイル</param>
        public void AddAttachedFile(ExperimentHistoryAttachedFile file)
        {
            AddModel<ExperimentHistoryAttachedFile>(file);
        }

        /// <summary>
        /// 指定した実験履歴添付ファイルを削除します。
        /// </summary>
        /// <param name="file">削除対象のファイル</param>
        public void DeleteAttachedFile(ExperimentHistoryAttachedFile file)
        {
            DeleteModel<ExperimentHistoryAttachedFile>(file);
        }
        #endregion
    }
}

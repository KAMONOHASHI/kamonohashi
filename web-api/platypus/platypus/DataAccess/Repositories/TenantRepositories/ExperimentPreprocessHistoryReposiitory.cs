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
    public class ExperimentPreprocessHistoryRepository : RepositoryForTenantBase<ExperimentPreprocessHistory>, IExperimentPreprocessHistoryRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExperimentPreprocessHistoryRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }

        /// <summary>
        /// 全実験履歴（データセットを含む）を取得します。
        /// </summary>
        public IQueryable<ExperimentPreprocessHistory> GetAllIncludeDataSet()
        {
            return GetAll().Include(t => t.DataSet);
        }


        /// <summary>
        /// 全実験履歴の名前とIDのみ取得する
        /// </summary>
        public async Task<IEnumerable<ExperimentPreprocessHistory>> GetAllNameAsync()
        {
            return await GetAll()
                .Select(t => new ExperimentPreprocessHistory() { Id = t.Id, Name = t.Name, Status = t.Status })
                .OrderByDescending(t => t.Id).ToListAsync();
        }

        /// <summary>
        /// 指定された実験履歴IDの前処理履歴エンティティ（外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        public async Task<ExperimentPreprocessHistory> GetIncludeAllAsync(long id)
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
        /// 指定したデータIDから派生したデータを取得する。
        /// </summary>
        /// <param name="dataSetId">派生元データセットID</param>
        public IEnumerable<ExperimentPreprocessHistory> GetPreprocecssedDataIncludePreprocessByInputDataSetId(long dataSetId)
        {
            return FindAll(x => x.DataSetId == dataSetId).Include(x => x.Template);
        }

        /// <summary>
        /// 指定したデータセットIDとテンプレートIDに紐づく実験前処理履歴（データ、前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="dataSetId">派生元データセットID</param>
        public async Task<ExperimentPreprocessHistory> GetPreprocessIncludeDataSetAndTemplateAsync(long templateId, long dataSetId)
        {
            return await GetAll()
                .Include(p => p.DataSet).Include(p => p.Template).FirstOrDefaultAsync(d => d.DataSetId == dataSetId && d.TemplateId == templateId);
        }


        /// <summary>
        /// 指定した実験前処理履歴の出力結果として、データを一件追加する。
        /// </summary> 
        public void AddOutputData(long historyId, Data newData)
        {
            ExperimentPreprocessHistoryOutput image = new ExperimentPreprocessHistoryOutput()
            {
                ExperimentHistoryId = historyId,
                OutputData = newData
            };
            AddModel<ExperimentPreprocessHistoryOutput>(image);
        }

        /// <summary>
        /// 実験の前処理履歴を追加
        /// </summary>
        /// <param name="entity">実験の前処理履歴</param>
        public override void Add(ExperimentPreprocessHistory entity)
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
        public ExperimentPreprocessHistory Find(Expression<Func<ExperimentPreprocessHistory, bool>> where, bool force)
        {
            return FindModel<ExperimentPreprocessHistory>(where, force);
        }

        /// <summary>
        /// 実験前処理履歴のステータスを変更する。
        /// 存在チェックは行わない。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public async Task UpdateStatusAsync(long id, ContainerStatus status, bool force)
        {
            var history = await this.GetByIdAsync(id, force);
            history.Status = status.Key;
            UpdateModel<ExperimentPreprocessHistory>(history, force);
        }

        /// <summary>
        /// 実験前処理履歴のステータスを変更する。
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
            UpdateModel<ExperimentPreprocessHistory>(history, force);
        }

        /// <summary>
        /// 指定した実験前処理履歴から生成されたデータのID一覧を取得する。
        /// </summary>
        public IEnumerable<long> GetExperimentPreprocessOutputs(long id)
        {
            return FindModelAll<ExperimentPreprocessHistoryOutput>(p => p.ExperimentHistoryId == id).Select(p => p.OutputDataId);
        }

        /// <summary>
        /// 実験前処理履歴を削除
        /// </summary>
        /// <param name="entity">実験履歴</param>
        public override void Delete(ExperimentPreprocessHistory entity)
        {
            DeleteAsync(entity).Wait();
        }

        /// <summary>
        /// 実験前処理履歴を削除
        /// </summary>
        /// <param name="entity">実験履歴</param>
        public async Task DeleteAsync(ExperimentPreprocessHistory entity)
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
        public async Task<IEnumerable<ExperimentPreprocessHistoryAttachedFile>> GetAllAttachedFilesAsync(long id)
        {
            return await FindModelAll<ExperimentPreprocessHistoryAttachedFile>(x => x.ExperimentPreprocessHistoryId == id).OrderBy(x => x.FileName).ToListAsync();
        }

        /// <summary>
        /// 指定したIDの実験履歴に、指定した名前の添付ファイルが登録済みか。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns> True:登録済み　False:未登録</returns>
        public async Task<bool> ExistsAttachedFileAsync(long id, string fileName)
        {
            return await ExistsModelAsync<ExperimentPreprocessHistoryAttachedFile>(x => x.ExperimentPreprocessHistoryId == id && x.FileName == fileName);
        }

        /// <summary>
        /// 指定したIDの実験履歴添付ファイルを取得します。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <returns>添付ファイル</returns>
        public async Task<ExperimentPreprocessHistoryAttachedFile> GetAttachedFileAsync(long id)
        {
            return await GetModelByIdAsync<ExperimentPreprocessHistoryAttachedFile>(id);
        }

        /// <summary>
        /// 実験履歴添付ファイルを追加します。
        /// </summary>
        /// <param name="file">追加対象のファイル</param>
        public void AddAttachedFile(ExperimentPreprocessHistoryAttachedFile file)
        {
            AddModel<ExperimentPreprocessHistoryAttachedFile>(file);
        }

        /// <summary>
        /// 指定した実験履歴添付ファイルを削除します。
        /// </summary>
        /// <param name="file">削除対象のファイル</param>
        public void DeleteAttachedFile(ExperimentPreprocessHistoryAttachedFile file)
        {
            DeleteModel<ExperimentPreprocessHistoryAttachedFile>(file);
        }
        #endregion
    }
}

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
    /// 前処理履歴テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class PreprocessHistoryRepository : RepositoryForTenantBase<PreprocessHistory>, IPreprocessHistoryRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PreprocessHistoryRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }

        /// <summary>
        /// 全前処理履歴（データと前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <returns>
        /// 前処理履歴エンティティリスト
        /// </returns>
        public async Task<IEnumerable<PreprocessHistory>> GetAllIncludeDataAndPreprocessAsync()
        {
            return await GetAll()
                .Include(p => p.InputData).Include(p => p.Preprocess)
                .OrderByDescending(d => d.Id).ToListAsync();
        }

        /// <summary>
        /// 全前処理履歴（データと前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <returns>
        /// 前処理履歴エンティティリスト
        /// </returns>
        public IQueryable<PreprocessHistory> GetAllIncludeDataAndPreprocess()
        {
            return GetAll()
                .Include(p => p.InputData).Include(p => p.Preprocess)
                .OrderByDescending(d => d.Id);
        }

        /// <summary>
        /// 指定した前処理履歴IDのデータ（データ、前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">前処理履歴ID</param>
        /// <param name="force">選択中のテナント以外も対象とするか</param>
        /// <returns>
        /// 前処理履歴エンティティ
        /// </returns>
        public PreprocessHistory GetPreprocessHistoryIncludeDataAndPreprocess(long id, bool force)
        {
            return GetModelAll<PreprocessHistory>(force)
                .Include(p => p.InputData).Include(p => p.Preprocess).FirstOrDefault(d => d.Id == id);
        }

        /// <summary>
        /// 指定したデータIDから派生したデータを取得する。
        /// </summary>
        /// <param name="dataId">派生元データID</param>
        public IEnumerable<PreprocessHistory> GetPreprocessIncludePreprocessByInputDataId(long dataId)
        {
            return FindAll(p => p.InputDataId == dataId).Include(p => p.Preprocess);
        }

        /// <summary>
        /// 指定したデータIDと前処理IDに紐づく前処理履歴（データ、前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <param name="preprocessId">前処理ID</param>
        /// <param name="dataId">派生元データID</param>
        public async Task<PreprocessHistory> GetPreprocessIncludeDataAndPreprocessAsync(long preprocessId, long dataId)
        {
            return await GetAll()
                .Include(p => p.InputData).Include(p => p.Preprocess).FirstOrDefaultAsync(d => d.InputDataId == dataId && d.PreprocessId == preprocessId);
        }

        /// <summary>
        /// 指定した前処理IDに紐づく前処理履歴（データ、前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <param name="preprocessId">前処理ID</param>
        public async Task<IEnumerable<PreprocessHistory>> GetPreprocessAllIncludeDataAndPreprocessAsync(long preprocessId)
        {
            return await GetAll()
                .Where(d => d.PreprocessId == preprocessId).OrderByDescending(t => t.CreatedAt).Include(p => p.InputData).Include(p => p.Preprocess).ToListAsync();
        }

        /// <summary>
        /// 指定したデータIDと前処理名に紐づく前処理履歴（前処理の外部参照を含む）を取得します。
        /// </summary>
        /// <param name="dataId">派生元データID</param>
        /// <param name="preprocessName">前処理名</param>
        public async Task<PreprocessHistory> GetPreprocessIncludePreprocessAsync(long dataId, string preprocessName)
        {
            return await GetAll().Include(p => p.Preprocess)
                .SingleOrDefaultAsync(d => d.InputDataId == dataId && d.Preprocess.Name == preprocessName);
        }

        /// <summary>
        /// 指定したデータIDを元にした前処理履歴が存在するかチェックします。
        /// </summary>
        /// <param name="id">データID</param>
        /// <returns>
        /// True：存在する　False:存在しない
        /// </returns>
        public async Task<bool> ExistsByInputDataIdAsync(long id)
        {
            return await GetAll().AnyAsync(x => x.InputDataId == id);
        }

        /// <summary>
        /// 指定したデータIDの派生元データを取得する。
        /// </summary>
        /// <param name="id">データID</param>
        public async Task<Data> GetInputDataAsync(long id)
        {
            return await FindModelAll<PreprocessHistoryOutput>(p => p.OutputDataId == id).Include(p => p.PreprocessHistory)
                .ThenInclude(p => p.InputData).Select(p => p.PreprocessHistory.InputData).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 指定した前処理履歴から生成されたデータのID一覧を取得する。
        /// </summary>
        public IEnumerable<long> GetPreprocessOutputs(long id)
        {
            return FindModelAll<PreprocessHistoryOutput>(p => p.PreprocessHistoryId == id).Select(p => p.OutputDataId);
        }

        /// <summary>
        /// 指定した前処理履歴の結果の中で、編集不能になっている結果を一件返す。
        /// この結果がnullでない場合は、この前処理履歴結果は削除できない
        /// </summary>
        public PreprocessHistoryOutput GetLockedOutput(long id)
        {
            //判定1: ロックされたデータセットに含まれているか
            PreprocessHistoryOutput lockedOutput = FindModelAll<PreprocessHistoryOutput>(o => o.PreprocessHistoryId == id).Where(o => 
                GetDbSet<DataSetEntry>().Include(e => e.DataSet) //出力結果を含み、編集不可なデータセットが1件以上あるか
                .Where(e => e.DataSet.IsLocked)
                .Where(e => e.DataId == o.OutputDataId).Count() > 0
            ).FirstOrDefault();

            if(lockedOutput != null)
            {
                return lockedOutput;
            }

            //判定2: 前処理出力が更に前処理されているか
            lockedOutput = FindModelAll<PreprocessHistoryOutput>(o => o.PreprocessHistoryId == id).Where(o => 
                GetDbSet<PreprocessHistory>().Where(history => history.InputDataId == o.OutputDataId).Count() > 0
            ).FirstOrDefault();

            return lockedOutput;
        }

        /// <summary>
        /// 指定した前処理履歴の出力結果として、データを一件追加する。
        /// </summary>
        public void AddOutputData(long historyId, Data newData)
        {
            PreprocessHistoryOutput image = new PreprocessHistoryOutput()
            {
                PreprocessHistoryId = historyId,
                OutputData = newData
            };
            AddModel<PreprocessHistoryOutput>(image);
        }

        public override void Add(PreprocessHistory entity)
        {
            if (entity.OptionDic != null && entity.OptionDic.Count > 0)
            {
                entity.Options = Newtonsoft.Json.JsonConvert.SerializeObject(entity.OptionDic);
            }
            base.Add(entity);
        }

        /// <summary>
        /// 前処理履歴一件を削除する。
        /// 削除可否は呼び出し側で判定しておくこと。
        /// またDataの削除はこの中では行わない。
        /// </summary>
        public override void Delete(PreprocessHistory entity)
        {
            //履歴出力を削除
            DeleteModelAll<PreprocessHistoryOutput>(p => p.PreprocessHistoryId == entity.Id);
            
            //前処理履歴自体をDBから削除
            base.Delete(entity);
        }

        /// <summary>
        /// 前処理履歴一件を削除する。
        /// 削除可否は呼び出し側で判定しておくこと。
        /// またDataの削除はこの中では行わない。
        /// </summary>
        public new void Delete(PreprocessHistory preprocessHistory, bool force)
        {
            //履歴出力を削除
            DeleteModelAll<PreprocessHistoryOutput>(p => p.PreprocessHistoryId == preprocessHistory.Id);

            //前処理履歴自体をDBから削除
            base.Delete(preprocessHistory, force);
        }
    }
}

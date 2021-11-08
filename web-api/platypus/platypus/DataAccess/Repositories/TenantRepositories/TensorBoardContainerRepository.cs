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
    /// TensorBoardコンテナテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    /// <seealso cref="Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories.ITensorBoardContainerRepository" />
    public class TensorBoardContainerRepository : RepositoryForTenantBase<TensorBoardContainer>, ITensorBoardContainerRepository
    {        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TensorBoardContainerRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }
        /// <summary>
        /// テナント横断で全データ（外部参照を含む）をすべて取得する。
        /// ソートはIDの逆順。
        /// </summary>
        public async Task<IEnumerable<TensorBoardContainer>> GetAllIncludePortAndTenantAsync()
        {
            return await GetModelAll<TensorBoardContainer>(true).Include(t => t.Tenant)
                .OrderByDescending(t => t.Id).ToListAsync();
        }

        /// <summary>
        /// 検索条件に合致するデータを一件取得します。
        /// </summary>
        public TensorBoardContainer Find(Expression<Func<TensorBoardContainer, bool>> where, bool force)
        {
            return FindModel<TensorBoardContainer>(where, force);
        }

        /// <summary>
        /// 現在のテナント内で、学習履歴IDが等しく、Disable状態じゃないコンテナを取得します。
        /// </summary>
        public TensorBoardContainer GetAvailableContainer(long trainingHistoryId)
        {
            TensorBoardContainer container = FindAll(x =>
                x.TrainingHistoryId == trainingHistoryId
            ).Include(t => t.Tenant)
            .AsEnumerable().Where(x =>
                ContainerStatus.IsAvailable(x.Status))
            .FirstOrDefault()
            ;
            return container;
        }

        /// <summary>
        /// 指定したIDのTensorBoardコンテナのステータスを更新する。
        /// </summary>
        public bool UpdateStatus(long id, string status, bool force)
        {
            //コンテナ情報を取得する
            TensorBoardContainer container = Find(x => x.Id == id, force);

            if (container == null)
            {
                //並列処理で、ここに来るまでの間に対象が削除されている場合がある
                //その場合は何もしない
                return false;
            }

            if (container.Status != status)
            {
                container.Status = status;
                Update(container);
            }
            return true;
        }

        /// <summary>
        /// tensorBoardコンテナ情報を追加
        /// </summary>
        /// <param name="entity">tensorBoardコンテナ情報</param>
        public override void Add(TensorBoardContainer entity)
        {
            if (entity.MountedTrainingHistoryIdList != null && entity.MountedTrainingHistoryIdList.Count > 0)
            {
                entity.MountedTrainingHistoryIds = JsonConvert.SerializeObject(entity.MountedTrainingHistoryIdList);
            }
            base.Add(entity);
        }
    }
}

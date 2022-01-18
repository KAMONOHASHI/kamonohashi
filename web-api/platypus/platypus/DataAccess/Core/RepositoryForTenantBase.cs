using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Core
{
    /// <summary>
    /// テナント向けテーブルにアクセスするための基本リポジトリクラス
    /// </summary>
    public abstract class RepositoryForTenantBase<TModel> : IRepositoryForTenant<TModel>
        where TModel : TenantModelBase
    {
        /// <summary>
        /// 現在選択中のテナントID
        /// 新規作成時、TModel.Tenant に TenantオブジェクトそのものをAddするとTenantも新規作成をしようとするので、IDだけを保持させる。
        /// </summary>
        protected long CurrentTenantId { get; private set; }

        /// <summary>
        /// 接続先のデータソースを表すDBコンテキスト
        /// </summary>
        private CommonDbContext DataContext;

        /// <summary>
        /// TModelに対応するテーブルにアクセスするためのアクセッサ
        /// </summary>
        private readonly DbSet<TModel> dbset;

        /// <summary>
        /// サブクラスから他のテナント用テーブルにアクセスするためのアクセッサ。
        /// この結果を用いたDBアクセスは、テナントIDの制約が入らない（そのままだと他のテナント情報にアクセスできる）ので、取り扱いには注意。
        /// </summary>
        protected DbSet<T> GetDbSet<T>() where T : TenantModelBase
        {
            return DataContext.Set<T>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        /// <param name="accessor">HTTPコンテキストアクセサ</param>
        protected RepositoryForTenantBase(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
        {
            DataContext = context;
            dbset = DataContext.Set<TModel>();
            CurrentTenantId = accessor.HttpContext.GetClaims().GetTenantId();
        }

        /// <summary>
        /// 新規エントリを追加
        /// </summary>
        /// <param name="entity">追加するエントリ</param>
        public virtual void Add(TModel entity)
        {
            AddModel(entity, false);
        }

        /// <summary>
        /// 新規エントリを追加
        /// </summary>
        /// <param name="entity">追加するエントリ</param>
        /// <param name="force">他テナントに対する追加を許可するか</param>
        public void Add(TModel entity, bool force)
        {
            AddModel(entity, force);
        }

        /// <summary>
        /// 新規エントリを追加
        /// </summary>
        /// <param name="entity">追加するエントリ</param>
        /// <param name="force">他テナントに対する追加を許可するか</param>
        protected void AddModel<T>(T entity, bool force = false) where T : TenantModelBase
        {
            if (force == false)
            {
                if (entity.TenantId <= 0)
                {
                    entity.TenantId = CurrentTenantId;
                }
                else if (force == false && entity.TenantId != CurrentTenantId)
                {
                    throw new UnauthorizedAccessException("異なるテナントのデータを追加しようとしています。");
                }
            }
            DataContext.Set<T>().Add(entity);
        }

        /// <summary>
        /// 新規エントリを一括追加
        /// </summary>
        /// <param name="entities">追加するエントリ</param>
        public virtual void AddRange(IQueryable<TModel> entities)
        {
            AddModelRange(entities);
        }

        /// <summary>
        /// 新規エントリを一括追加
        /// </summary>
        /// <param name="entities">追加するエントリ</param>
        protected void AddModelRange<T>(IQueryable<T> entities) where T : TenantModelBase
        {
            if (entities == null)
            {
                return;
            }
            foreach (T entity in entities)
            {
                AddModel(entity);
            }
        }

        /// <summary>
        /// 既存エントリを更新
        /// </summary>
        /// <param name="entity">更新するエントリ</param>
        public virtual void Update(TModel entity)
        {
            UpdateModel(entity, false);
        }

        /// <summary>
        /// 既存エントリを更新
        /// </summary>
        /// <param name="entity">更新するエントリ</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        public void Update(TModel entity, bool force)
        {
            UpdateModel(entity, force);
        }

        /// <summary>
        /// 既存エントリを更新
        /// </summary>
        /// <param name="entity">更新するエントリ</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        protected void UpdateModel<T>(T entity, bool force = false) where T : TenantModelBase
        {
            if (!force && entity.TenantId != CurrentTenantId)
            {
                throw new UnauthorizedAccessException("異なるテナントのデータを更新しようとしています。");
            }
            DataContext.Set<T>().Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 既存エントリを削除
        /// </summary>
        /// <param name="entity">削除するエントリ</param>
        public virtual void Delete(TModel entity)
        {
            this.DeleteModel(entity, false);
        }

        /// <summary>
        /// 既存エントリを削除
        /// </summary>
        /// <param name="id">削除対象のID</param>
        public async virtual Task DeleteByIdAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            this.DeleteModel(entity, false);
        }

        /// <summary>
        /// 既存エントリを削除
        /// </summary>
        /// <param name="entity">削除するエントリ</param>
        /// <param name="force">他テナントに対する削除を許可するか</param>
        public void Delete(TModel entity, bool force)
        {
            this.DeleteModel(entity, force);
        }

        /// <summary>
        /// 既存エントリを削除
        /// </summary>
        /// <param name="id">削除対象のID</param>
        /// <param name="force">他テナントに対する削除を許可するか</param>
        public async Task DeleteByIdAsync(long id, bool force)
        {
            var entity = await GetByIdAsync(id);
            this.DeleteModel(entity, force);
        }

        /// <summary>
        /// 既存エントリを削除
        /// </summary>
        /// <param name="entity">削除するエントリ</param>
        /// <param name="force">他テナントに対する削除を許可するか</param>
        protected void DeleteModel<T>(T entity, bool force = false) where T : TenantModelBase
        {
            if (!force && entity.TenantId != CurrentTenantId)
            {
                throw new UnauthorizedAccessException("異なるテナントのデータを削除しようとしています。");
            }
            DataContext.Set<T>().Remove(entity);
        }

        /// <summary>
        /// 条件を満たす既存エントリを一括削除
        /// </summary>
        /// <param name="where">削除条件</param>
        public virtual void DeleteAll(Expression<Func<TModel, bool>> where)
        {
            this.DeleteModelAll(where, false);
        }

        /// <summary>
        /// 条件を満たす既存エントリを一括削除
        /// </summary>
        /// <param name="where">削除条件</param>
        /// <param name="force">他テナントに対する削除を許可するか</param>
        public void DeleteAll(Expression<Func<TModel, bool>> where, bool force)
        {
            this.DeleteModelAll(where, force);
        }

        /// <summary>
        /// 条件を満たす既存エントリを一括削除
        /// </summary>
        /// <param name="where">削除条件</param>
        /// <param name="force">他テナントに対する削除を許可するか</param>
        protected void DeleteModelAll<T>(Expression<Func<T, bool>> where, bool force = false) where T : TenantModelBase
        {
            IQueryable<T> entries = GetModelAll<T>(force).Where(where);
            DataContext.Set<T>().RemoveRange(entries);
        }

        /// <summary>
        /// 既存エントリを一括削除
        /// </summary>
        /// <param name="entities">削除するエントリ</param>
        public virtual void DeleteRange(IQueryable<TModel> entities)
        {
            DeleteModelRange(entities);
        }

        /// <summary>
        /// 既存エントリを一括削除
        /// </summary>
        /// <param name="entities">削除するエントリ</param>
        protected void DeleteModelRange<T>(IQueryable<T> entities) where T : TenantModelBase
        {
            if (entities == null)
            {
                return;
            }
            foreach (T entity in entities)
            {
                DeleteModel(entity);
            }
        }

        /// <summary>
        /// エントリの数を取得する。
        /// </summary>
        public async Task<long> Count()
        {
            return await GetAll().LongCountAsync();
        }

        /// <summary>
        /// 条件を満たすエントリの数を取得する。
        /// </summary>
        /// <param name="where">取得条件</param>
        public async Task<long> Count(Expression<Func<TModel, bool>> where)
        {
            return await FindAll(where).LongCountAsync();
        }

        /// <summary>
        /// 条件を満たすエントリが存在するか確認する。
        /// </summary>
        /// <param name="where">取得条件</param>
        public async virtual Task<bool> ExistsAsync(Expression<Func<TModel, bool>> where)
        {
            return await GetAll().AnyAsync(where);
        }
        /// <summary>
        /// 条件を満たすエントリが存在するか確認する。
        /// </summary>
        /// <param name="where">取得条件</param>
        /// <param name="force">他テナントに対する取得を許可するか</param>
        protected async Task<bool> ExistsModelAsync<T>(Expression<Func<T, bool>> where, bool force = false) where T : TenantModelBase
        {
            return await GetModelAll<T>(force).AnyAsync(where);
        }

        /// <summary>
        /// PKから既存エントリを取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// </summary>
        /// <param name="id">取得対象のID</param>
        public virtual async Task<TModel> GetByIdAsync(long id)
        {
            return await GetModelByIdAsync<TModel>(id);
        }

        /// <summary>
        /// PKから既存エントリを取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// </summary>
        /// <param name="id">取得対象のID</param>
        /// <param name="force">他テナントに対する取得を許可するか</param>
        public async Task<TModel> GetByIdAsync(long id, bool force)
        {
            return await GetModelByIdAsync<TModel>(id, force);
        }

        /// <summary>
        /// PKから既存エントリを取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// </summary>
        /// <param name="id">取得対象のID</param>
        /// <param name="force">他テナントに対する取得を許可するか</param>
        protected async Task<T> GetModelByIdAsync<T>(long id, bool force = false) where T : TenantModelBase
        {
            T model = await DataContext.Set<T>().FindAsync(id);
            if (model == null)
            {
                return null;
            }
            if (!force && model != null && model.TenantId != CurrentTenantId)
            {
                throw new UnauthorizedAccessException("異なるテナントのデータを取得しようとしています。");
            }
            return model;
        }

        /// <summary>
        /// 現在のテナントに属する、既存エントリをすべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        public virtual IQueryable<TModel> GetAll()
        {
            return this.GetModelAll<TModel>(false);
        }

        /// <summary>
        /// 現在のテナントに属する、既存エントリをすべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="force">他テナントに対する取得を許可するか</param>
        protected IQueryable<T> GetModelAll<T>(bool force = false) where T : TenantModelBase
        {
            if (force)
            {
                return DataContext.Set<T>();
            }
            else
            {
                return DataContext.Set<T>().Where(model => model.Tenant.Id == CurrentTenantId);
            }
        }

        /// <summary>
        /// 既存エントリを指定した順番ですべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="asc">昇順の場合はtrue, 降順ならfalse</param>
        /// <param name="keySelector">並び順を表す式</param>
        public virtual IQueryable<TModel> GetAllWithOrderby<TKey>(Expression<Func<TModel, TKey>> keySelector, bool asc)
        {
            return this.GetModelAllWithOrderby(keySelector, asc, false);
        }

        /// <summary>
        /// 既存エントリを指定した順番ですべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="asc">昇順の場合はtrue, 降順ならfalse</param>
        /// <param name="keySelector">並び順を表す式</param>
        /// <param name="force">ログイン中のテナント以外も含めるか</param>
        protected IQueryable<T> GetModelAllWithOrderby<T, TKey>(Expression<Func<T, TKey>> keySelector, bool asc, bool force) where T : TenantModelBase
        {
            IQueryable<T> result = GetModelAll<T>(force);
            if (asc)
            {
                return result.OrderBy(keySelector);
            }
            else
            {
                return result.OrderByDescending(keySelector);
            }
        }

        /// <summary>
        /// 条件を満たす既存エントリを一つ取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// </summary>
        /// <param name="where">取得条件</param>
        public virtual TModel Find(Expression<Func<TModel, bool>> where)
        {
            return FindModel<TModel>(where);
        }

        /// <summary>
        /// 条件を満たす既存エントリを一つ取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// </summary>
        /// <param name="where">取得条件</param>
        /// <param name="force">他テナントに対する取得を許可するか</param>
        protected T FindModel<T>(Expression<Func<T, bool>> where, bool force = false) where T : TenantModelBase
        {
            return FindModelAll(where, force).FirstOrDefault<T>();
        }

        /// <summary>
        /// 条件を満たす既存エントリをすべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="where">取得条件</param>
        public virtual IQueryable<TModel> FindAll(Expression<Func<TModel, bool>> where)
        {
            return FindModelAll<TModel>(where);
        }

        /// <summary>
        /// 条件を満たす既存エントリをすべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="where">取得条件</param>
        /// <param name="force">他テナントに対する取得を許可するか</param>
        public IQueryable<TModel> FindAll(Expression<Func<TModel, bool>> where, bool force)
        {
            return FindModelAll<TModel>(where, force);
        }

        /// <summary>
        /// 条件を満たす既存エントリをすべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="where">取得条件</param>
        /// <param name="force">他テナントに対する取得を許可するか</param>
        protected IQueryable<T> FindModelAll<T>(Expression<Func<T, bool>> where, bool force = false) where T : TenantModelBase
        {
            return GetModelAll<T>(force).Where(where);
        }

        /// <summary>
        /// 条件を満たす既存エントリを指定した順番で取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="where">取得条件</param>
        /// <param name="keySelector">並び順を表す式</param>
        /// <param name="asc">昇順の場合はtrue, 降順ならfalse</param>
        public virtual IQueryable<TModel> FindAllWithOrderby<TKey>(Expression<Func<TModel, bool>> where, Expression<Func<TModel, TKey>> keySelector, bool asc)
        {
            return FindModelAllWithOrderby<TModel, TKey>(where, keySelector, asc);
        }

        /// <summary>
        /// 条件を満たす既存エントリを指定した順番で取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="where">取得条件</param>
        /// <param name="keySelector">並び順を表す式</param>
        /// <param name="asc">昇順の場合はtrue, 降順ならfalse</param>
        /// <param name="force">他テナントに対する取得を許可するか</param>
        public IQueryable<TModel> FindAllWithOrderby<TKey>(Expression<Func<TModel, bool>> where, Expression<Func<TModel, TKey>> keySelector, bool asc, bool force)
        {
            return FindModelAllWithOrderby<TModel, TKey>(where, keySelector, asc, force);
        }

        /// <summary>
        /// 条件を満たす既存エントリを指定した順番で取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="where">取得条件</param>
        /// <param name="keySelector">並び順を表す式</param>
        /// <param name="asc">昇順の場合はtrue, 降順ならfalse</param>
        /// <param name="force">他テナントに対する取得を許可するか</param>
        protected IQueryable<T> FindModelAllWithOrderby<T, TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> keySelector, bool asc, bool force = false) where T : TenantModelBase
        {
            var set = FindModelAll(where, force);
            if (asc)
            {
                return set.OrderBy(keySelector);
            }
            else
            {
                return set.OrderByDescending(keySelector);
            }
        }
    }
}

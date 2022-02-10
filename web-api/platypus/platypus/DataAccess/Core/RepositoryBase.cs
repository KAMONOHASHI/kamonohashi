using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Core
{
    /// <summary>
    /// テナント別データベースの各テーブルにアクセスするためのリポジトリの基本クラス。
    /// 基本的なCRUDはこのクラスで実装し、Includeなど他のテーブルの情報を必要とするものはサブクラスで実装する。
    /// </summary>
    /// <typeparam name="TModel">テーブルに対応するモデル</typeparam>
    public class RepositoryBase<TModel> : IRepository<TModel>
        where TModel : ModelBase
    {
        /// <summary>
        /// TModelに対応するテーブルにアクセスするためのアクセッサ
        /// </summary>
        private readonly DbSet<TModel> dbset;

        /// <summary>
        /// 接続先のデータソースを表すDBコンテキスト
        /// </summary>
        private CommonDbContext DataContext;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RepositoryBase(CommonDbContext dataContext)
        {
            DataContext = dataContext;
            dbset = DataContext.Set<TModel>();
        }

        /// <summary>
        /// 新規エントリを追加
        /// </summary>
        public virtual void Add(TModel entity)
        {
            dbset.Add(entity);
        }

        /// <summary>
        /// 新規エントリを追加
        /// </summary>
        protected void AddModel<T>(T entity) where T : ModelBase
        {
            DataContext.Set<T>().Add(entity);
        }

        /// <summary>
        /// 新規エントリを一括追加
        /// </summary>
        public void AddRange(IQueryable<TModel> entities)
        {
            AddModelRange(entities);
        }

        /// <summary>
        /// 新規エントリを一括追加
        /// </summary>
        protected void AddModelRange<T>(IQueryable<T> entities) where T : ModelBase
        {
            if (entities == null)
            {
                return;
            }
            foreach (T entity in entities)
            {
                AddModel<T>(entity);
            }
        }

        /// <summary>
        /// 既存エントリを更新
        /// </summary>
        public virtual void Update(TModel entity)
        {
            UpdateModel(entity);
        }
        /// <summary>
        /// 既存エントリを更新
        /// </summary>
        protected void UpdateModel<T>(T entity) where T : ModelBase
        {
            DataContext.Set<T>().Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 既存エントリを削除
        /// </summary>
        public virtual void Delete(TModel entity)
        {
            dbset.Remove(entity);
        }

        /// <summary>
        /// 既存エントリを削除
        /// </summary>
        public async virtual Task DeleteByIdAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            this.DeleteModel(entity);
        }

        /// <summary>
        /// 既存エントリを削除
        /// </summary>
        protected void DeleteModel<T>(T entity) where T : ModelBase
        {
            DataContext.Set<T>().Remove(entity);
        }

        /// <summary>
        /// 条件を満たす既存エントリを一括削除
        /// </summary>
        public virtual void DeleteAll(Expression<Func<TModel, bool>> where)
        {
            dbset.RemoveRange(dbset.Where<TModel>(where));
        }

        /// <summary>
        /// 条件を満たす既存エントリを一括削除
        /// </summary>
        protected void DeleteModelAll<T>(Expression<Func<T, bool>> where) where T : ModelBase
        {
            var entries = DataContext.Set<T>().Where<T>(where);
            DataContext.Set<T>().RemoveRange(entries);
        }

        /// <summary>
        /// 既存エントリを一括削除
        /// </summary>
        public void DeleteRange(IQueryable<TModel> entities)
        {
            DeleteModelRange(entities);
        }

        /// <summary>
        /// 既存エントリを一括削除
        /// </summary>
        protected void DeleteModelRange<T>(IQueryable<T> entities) where T : ModelBase
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
        public async Task<long> Count(Expression<Func<TModel, bool>> where)
        {
            return await FindAll(where).LongCountAsync();
        }

        /// <summary>
        /// 条件を満たすエントリが存在するか確認する。
        /// </summary>
        public async Task<bool> ExistsAsync(Expression<Func<TModel, bool>> where)
        {
            return await GetAll().AnyAsync(where);
        }
        /// <summary>
        /// 条件を満たすエントリが存在するか確認する。
        /// </summary>
        protected async Task<bool> ExistsModelAsync<T>(Expression<Func<T, bool>> where) where T : ModelBase
        {
            return await GetModelAll<T>().AnyAsync(where);
        }

        /// <summary>
        /// PKから既存エントリを取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// </summary>
        public virtual async Task<TModel> GetByIdAsync(long id)
        {
            return await dbset.FindAsync(id);
        }

        /// <summary>
        /// PKから既存エントリを取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// </summary>
        protected async Task<T> GetModelByIdAsync<T>(long id) where T : ModelBase
        {
            return await DataContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// 既存エントリをすべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        public virtual IQueryable<TModel> GetAll()
        {
            return DataContext.Set<TModel>();
        }

        /// <summary>
        /// 対応するテーブルへのアクセッサを取得する。
        /// <see cref="DataContext"/>.Xxx の代わりに、GetAll{T}を使用すること
        /// </summary>
        protected IQueryable<T> GetModelAll<T>() where T : ModelBase
        {
            return DataContext.Set<T>();
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
            return GetModelAllWithOrderby<TModel, TKey>(keySelector, asc);
        }

        /// <summary>
        /// 既存エントリを指定した順番ですべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="asc">昇順の場合はtrue, 降順ならfalse</param>
        /// <param name="keySelector">並び順を表す式</param>
        protected IQueryable<T> GetModelAllWithOrderby<T, TKey>(Expression<Func<T, TKey>> keySelector, bool asc) where T : ModelBase
        {
            var set = DataContext.Set<T>();
            if (asc)
            {
                return set.OrderBy(keySelector);
            }
            else
            {
                return set.OrderByDescending(keySelector);
            }
        }

        /// <summary>
        /// 条件を満たす既存エントリを一つ取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// </summary>
        public virtual TModel Find(Expression<Func<TModel, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault<TModel>();
        }

        /// <summary>
        /// 条件を満たす既存エントリを一つ取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// </summary>
        protected T FindModel<T>(Expression<Func<T, bool>> where) where T : ModelBase
        {
            return DataContext.Set<T>().Where(where).FirstOrDefault<T>();
        }

        /// <summary>
        /// 条件を満たす既存エントリをすべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        public virtual IQueryable<TModel> FindAll(Expression<Func<TModel, bool>> where)
        {
            return dbset.Where(where);
        }

        /// <summary>
        /// 条件を満たす既存エントリをすべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        protected IQueryable<T> FindModelAll<T>(Expression<Func<T, bool>> where) where T : ModelBase
        {
            return DataContext.Set<T>().Where(where);
        }

        /// <summary>
        /// 条件を満たす既存エントリを指定した順番で取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        public virtual IQueryable<TModel> FindAllWithOrderby<TKey>(Expression<Func<TModel, bool>> where, Expression<Func<TModel, TKey>> keySelector, bool asc)
        {
            return FindModelAllWithOrderby<TModel, TKey>(where, keySelector, asc);
        }

        /// <summary>
        /// 条件を満たす既存エントリを指定した順番で取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        protected IQueryable<T> FindModelAllWithOrderby<T, TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> keySelector, bool asc) where T : ModelBase
        {
            var set = FindModelAll(where);
            if (asc)
            {
                return set.OrderBy(keySelector);
            }
            else
            {
                return set.OrderByDescending(keySelector);
            }
        }

        /// <summary>
        /// DB・またはキャッシュから、型引数に対応するテーブルの全レコードを取得する。
        /// </summary>
        /// <param name="memoryCache">メモリキャッシュ</param>
        /// <param name="span">キャッシュ期間</param>
        /// <param name="logger">ロガー</param>
        protected async Task<List<T>> GetFromCacheAsync<T>(IMemoryCache memoryCache, TimeSpan span, ILogger logger) where T : ModelBase
        {
            return await GetFromCacheAsync<T>(memoryCache, span, logger, null);
        }

        /// <summary>
        /// DB・またはキャッシュから、型引数に対応するテーブルの全レコードを並び順を指定して取得する。
        /// このメソッド自体はスレッドセーフではないので、必要なら使う側で良しなに排他制御する。
        /// </summary>
        /// <param name="memoryCache">メモリキャッシュ</param>
        /// <param name="span">キャッシュ期間</param>
        /// <param name="logger">ロガー</param>
        /// <param name="keySelector">並び順を決めるのに使う項目を返す関数</param>
        /// <param name="asc">昇順か</param>
        protected async Task<List<T>> GetFromCacheAsync<T>(IMemoryCache memoryCache, TimeSpan span, ILogger logger, Expression<Func<T, object>> keySelector, bool asc = true) where T : ModelBase
        {
            string key = typeof(T).Name;
            var records = await memoryCache.GetOrCreateAsync(key, async entry =>
            {
                //指定した期間キャッシュする
                if (logger != null)
                {
                    LogUtil.WriteLLog(logger.LogDebug, "-", "-", $"{key} のデータをキャッシュに読み込みます。");
                }
                entry.AbsoluteExpiration = DateTimeOffset.Now + span;
                if (keySelector == null)
                {
                    return await DataContext.Set<T>().ToListAsync();
                }
                else if (asc)
                {
                    return await DataContext.Set<T>().OrderBy(keySelector).ToListAsync();
                }
                else
                {
                    return await DataContext.Set<T>().OrderByDescending(keySelector).ToListAsync();
                }
            });

            return records;
        }

        /// <summary>
        /// DB・またはキャッシュから、指定したクエリを使ってT型の複数レコードを取得する。
        /// クエリは<see cref="IQueryable{T}"/>ではなく<see cref="IEnumerable{T}"/>で返すこと。でないとDB接続がDisposeされた後に問い合わせすることになってエラーが起きうる。
        /// </summary>
        /// <param name="memoryCache">メモリキャッシュ</param>
        /// <param name="span">キャッシュ期間</param>
        /// <param name="logger">ロガー</param>
        /// <param name="key">キー</param>
        /// <param name="query">クエリ</param>
        protected IEnumerable<T> GetFromCache<T>(IMemoryCache memoryCache, TimeSpan span, ILogger logger, string key, Func<IEnumerable<T>> query) where T : ModelBase
        {
            var records = memoryCache.GetOrCreate(key, entry =>
            {
                //指定した期間キャッシュする
                if (logger != null)
                {
                    LogUtil.WriteLLog(logger.LogDebug, "-", "-", $"{key} のデータをキャッシュに読み込みます。");
                }
                entry.AbsoluteExpiration = DateTimeOffset.Now + span;
                return query();
            });

            return records;
        }

        /// <summary>
        /// インメモリキャッシュから、型引数に対応するテーブルのデータを破棄する。
        /// </summary>
        protected void ClearCache<T>(IMemoryCache memoryCache, ILogger logger)
        {
            string key = typeof(T).Name;
            LogUtil.WriteLLog(logger.LogDebug, "-", "-", $"{key} のキャッシュデータを破棄しました。");
            memoryCache.Remove(key);
        }
    }
}

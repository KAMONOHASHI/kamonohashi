using Nssol.Platypus.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Core
{
    /// <summary>
    /// リポジトリパターンのインターフェイス
    /// </summary>
    /// <typeparam name="TModel">エンティティ</typeparam>
    public interface IRepository<TModel> where TModel : ModelBase
    {
        /// <summary>
        /// エンティティを追加します。
        /// </summary>
        /// <param name="entity">追加するエンティティ</param>
        void Add(TModel entity);

        /// <summary>
        /// 新規エントリを一括追加
        /// </summary>
        void AddRange(IQueryable<TModel> entities);

        /// <summary>
        /// エンティティを更新します。
        /// </summary>
        /// <param name="entity">更新するエンティティ</param>
        void Update(TModel entity);

        /// <summary>
        /// エンティティを削除します。
        /// </summary>
        /// <param name="entity">削除するエンティティ</param>
        void Delete(TModel entity);

        /// <summary>
        /// エンティティを削除します。
        /// </summary>
        /// <param name="id">削除するエンティティのID(PK)</param>
        Task DeleteByIdAsync(long id);

        /// <summary>
        /// 指定した条件に一致するエンティティを削除します。
        /// </summary>
        /// <param name="where">条件Funcデリゲート</param>
        void DeleteAll(Expression<Func<TModel, bool>> where);

        /// <summary>
        /// 既存エントリを一括削除
        /// </summary>
        void DeleteRange(IQueryable<TModel> entities);

        /// <summary>
        /// エントリの数を取得する。
        /// </summary>
        Task<long> Count();

        /// <summary>
        /// 条件を満たすエントリの数を取得する。
        /// </summary>
        Task<long> Count(Expression<Func<TModel, bool>> where);

        /// <summary>
        /// 条件を満たすエントリが存在するか確認する。
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<TModel, bool>> where);

        /// <summary>
        /// PKから既存エントリを取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// </summary>
        Task<TModel> GetByIdAsync(long id);

        /// <summary>
        /// 既存エントリをすべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <returns>エンティティリスト</returns>
        IQueryable<TModel> GetAll();

        /// <summary>
        /// 既存エントリを指定した順番ですべて取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        /// <param name="asc">昇順の場合はtrue, 降順ならfalse</param>
        /// <param name="keySelector">並び順を表す式</param>
        /// <returns>エンティティリスト</returns>
        IQueryable<TModel> GetAllWithOrderby<TKey>(Expression<Func<TModel, TKey>> keySelector, bool asc);

        /// <summary>
        /// 指定した条件に一致するエンティティを１件取得します。
        /// 存在しない場合はnullが返ります。
        /// </summary>
        /// <param name="where">条件Funcデリゲート</param>
        /// <returns>一致したエンティティ</returns>
        TModel Find(Expression<Func<TModel, bool>> where);

        /// <summary>
        /// 指定した条件に一致する複数エンティティを取得します。
        /// </summary>
        /// <param name="where">条件Funcデリゲート</param>
        /// <returns>一致したエンティティリスト</returns>
        IQueryable<TModel> FindAll(Expression<Func<TModel, bool>> where);

        /// <summary>
        /// 条件を満たす既存エントリを指定した順番で取得する。
        /// 外部参照がある場合、そのインスタンスは含まれない。
        /// エントリが存在しない場合、nullではなく空のコレクションが返る。
        /// </summary>
        IQueryable<TModel> FindAllWithOrderby<TKey>(Expression<Func<TModel, bool>> where, Expression<Func<TModel, TKey>> keySelector, bool asc);
    }
}

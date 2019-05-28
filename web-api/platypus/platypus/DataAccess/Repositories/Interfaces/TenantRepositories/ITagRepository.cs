using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// タグテーブルにアクセスするためのレポジトリ
    /// </summary>
    public interface ITagRepository: IRepositoryForTenant<Tag>
    {
        /// <summary>
        /// タグ付でタグマップを取得する
        /// </summary>
        IEnumerable<Tag> GetAllTag(long dataId);

        /// <summary>
        /// タグからタグマップを取得する
        /// </summary>
        IEnumerable<DataTagMap> GetByTags(IEnumerable<Tag> tags);

        /// <summary>
        /// 指定したデータにタグをつける。
        /// </summary>
        void Add(Data data, string tagName);

        /// <summary>
        /// 指定したデータにタグをつける。
        /// データIDの存在チェックは行わない。
        /// タグを付与したらtrueを返す。
        /// </summary>
        /// <param name="dataId">データID</param>
        /// <param name="tagString">タグ名</param>
        /// <param name="checkExists">既に付与済みかチェックするか</param>
        Task<bool> AddAsync(long dataId, string tagString, bool checkExists = true);

        /// <summary>
        /// 指定されたタグ名のリストに一致するタグIDのリストを返す。
        /// 存在しないタグ名が含まれていた場合、そのタグ名は無視する。
        /// </summary>
        IEnumerable<long> GetTagIds(IEnumerable<string> tagNames);

        /// <summary>
        /// 指定されたデータIDに紐づく全てのタグを削除する。
        /// データIDの存在チェックは行わない。
        /// </summary>
        void DeleteAll(long dataId);

        /// <summary>
        /// 指定されたデータIDに紐づくタグを削除する。
        /// dataIdの存在チェックはしない。
        /// </summary>
        bool Delete(long dataId, string tagString);
        
        /// <summary>
        /// 未使用のタグをすべて削除する
        /// </summary>
        /// <returns>削除したタグの数</returns>
        int DeleteUnUsedTags();
    }
}

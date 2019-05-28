using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Microsoft.EntityFrameworkCore;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// タグテーブルにアクセスするためのレポジトリ
    /// </summary>
    public class TagRepository : RepositoryForTenantBase<Tag>, ITagRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TagRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }

        /// <summary>
        /// 指定したデータにタグをつける。
        /// データIDの存在チェックは行わない。
        /// Dataを新規登録する場合に使う。既存Dataなら<see cref="AddAsync(long, string, bool)"/>を使用する。
        /// </summary>
        public void Add(Data data, string tagString)
        {
            //既存のタグがあるか
            Tag tag = Find(t => t.Name == tagString);
            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagString,
                };
                Add(tag);
            }
            AddModel<DataTagMap>(new DataTagMap()
            {
                Data = data,
                Tag = tag,
            });
        }

        /// <summary>
        /// 指定したデータにタグをつける。
        /// データIDの存在チェックは行わない。
        /// タグを付与したらtrueを返す。
        /// </summary>
        /// <param name="dataId">データID</param>
        /// <param name="tagString">タグ名</param>
        /// <param name="checkExists">既に付与済みかチェックするか</param>
        public async Task<bool> AddAsync(long dataId, string tagString, bool checkExists = true)
        {
            //既存のタグがあるか
            Tag tag = Find(t => t.Name == tagString);
            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagString,
                };
                Add(tag);
            }
            else if(checkExists)
            {
                bool exists = await ExistsModelAsync<DataTagMap>(map => map.DataId == dataId && map.TagId == tag.Id);
                if(exists)
                {
                    return false;
                }
            }
            AddModel<DataTagMap>(new DataTagMap()
            {
                DataId = dataId,
                Tag = tag,
            });
            return true;
        }

        /// <summary>
        /// 指定したデータIDに紐づく全てのタグを取得する
        /// </summary>
        public IEnumerable<Tag> GetAllTag(long dataId)
        {
            return FindModelAll<DataTagMap>(m => m.DataId == dataId).Include(m => m.Tag).Select(dm => dm.Tag);
        }

        /// <summary>
        /// タグからタグマップを取得する
        /// </summary>
        public IEnumerable<DataTagMap> GetByTags(IEnumerable<Tag> tags)
        {
            return tags.SelectMany(t => GetModelAll<DataTagMap>().Where(m => m.TagId == t.Id));
        }

        /// <summary>
        /// 指定されたタグ名のリストに一致するタグIDのリストを返す。
        /// 存在しないタグ名が含まれていた場合、そのタグ名は無視する。
        /// </summary>
        public IEnumerable<long> GetTagIds(IEnumerable<string> tagNames)
        {
            return FindAll(t => tagNames.Contains(t.Name)).Select(t => t.Id);
        }

        /// <summary>
        /// 指定されたデータIDに紐づく全てのタグを削除する。
        /// dataIdの存在チェックはしない。
        /// </summary>
        public void DeleteAll(long dataId)
        {
            DeleteModelAll<DataTagMap>(m => m.DataId == dataId);
        }

        /// <summary>
        /// 指定されたデータIDに紐づくタグを削除する。
        /// dataIdの存在チェックはしない。
        /// </summary>
        public bool Delete(long dataId, string tagString)
        {
            DeleteModelAll<DataTagMap>(m => m.DataId == dataId);

            //既存のタグがあるか
            Tag tag = Find(t => t.Name == tagString);
            if (tag == null)
            {
                //タグ自体が存在しなければ、当然紐づいていない
                return false;
            }

            var tagMap = FindModel<DataTagMap>(map => map.DataId == dataId && map.TagId == tag.Id);
            if (tagMap == null)
            {
                //タグとの紐づけはない
                return false;
            }

            DeleteModel<DataTagMap>(tagMap);

            return true;
        }

        /// <summary>
        /// 未使用のタグをすべて削除する
        /// </summary>
        /// <returns>削除したタグの数</returns>
        public int DeleteUnUsedTags()
        {
            var targets = GetModelAll<Tag>(true).Include(t => t.DataMaps).Where(t => t.DataMaps.Count == 0);
            foreach(var target in targets)
            {
                Delete(target, true);
            }
            return targets.Count();
        }
    }
}

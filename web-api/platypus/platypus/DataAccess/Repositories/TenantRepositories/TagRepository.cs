using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        #region データ

        /// <summary>
        /// 指定したデータIDに紐づく全てのタグを取得する
        /// </summary>
        /// <param name="dataId">データID</param>
        public IEnumerable<Tag> GetAllDataTag(long dataId)
        {
            return FindModelAll<DataTagMap>(m => m.DataId == dataId).Include(m => m.Tag).Select(dm => dm.Tag);
        }

        /// <summary>
        /// タグからタグマップを取得する
        /// </summary>
        /// <param name="tags">タグ</param>
        public IEnumerable<DataTagMap> GetDataTagMapsByTags(IEnumerable<Tag> tags)
        {
            return tags.SelectMany(t => GetModelAll<DataTagMap>().Where(m => m.TagId == t.Id));
        }

        /// <summary>
        /// 指定したデータにタグをつける。
        /// データIDの存在チェックは行わない。
        /// Dataを新規登録する場合に使う。既存Dataなら<see cref="AddAsync(long, string, bool)"/>を使用する。
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="tagName">タグ名</param>
        public void AddDataTag(Data data, string tagName)
        {
            //既存のタグがあるか
            Tag tag = Find(t => t.Name == tagName && t.Type == TagType.Data);
            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagName,
                    Type = TagType.Data
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
        /// <param name="tagName">タグ名</param>
        /// <param name="checkExists">既に付与済みかチェックするか</param>
        public async Task<bool> AddDataTagAsync(long dataId, string tagName, bool checkExists = true)
        {
            //既存のタグがあるか
            Tag tag = Find(t => t.Name == tagName && t.Type == TagType.Data);
            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagName,
                    Type = TagType.Data
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
        /// 指定されたタグ名のリストに一致するタグIDのリストを返す。
        /// 存在しないタグ名が含まれていた場合、そのタグ名は無視する。
        /// </summary>
        /// <param name="tagNames">タグ名</param>
        public IEnumerable<long> GetDataTagIds(IEnumerable<string> tagNames)
        {
            return FindAll(t => tagNames.Contains(t.Name)).Select(t => t.Id);
        }

        /// <summary>
        /// 指定されたデータIDに紐づく全てのタグを削除する。
        /// データIDの存在チェックはしない。
        /// </summary>
        /// <param name="dataId">データID</param>
        public void DeleteAllDataTag(long dataId)
        {
            DeleteModelAll<DataTagMap>(m => m.DataId == dataId);
        }

        /// <summary>
        /// 指定されたデータIDに紐づくタグを削除する。
        /// データIDの存在チェックはしない。
        /// </summary>
        /// <param name="dataId">データID</param>
        /// <param name="tagName">タグ名</param>
        public bool DeleteDataTag(long dataId, string tagName)
        {
            DeleteModelAll<DataTagMap>(m => m.DataId == dataId);

            //既存のタグがあるか
            Tag tag = Find(t => t.Name == tagName && t.Type == TagType.Data);
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
        /// データで使用するタグの内、未使用のタグをすべて削除する
        /// </summary>
        /// <returns>削除したタグの数</returns>
        public int DeleteUnUsedDataTags()
        {
            var targets = GetModelAll<Tag>(true).Where(t => t.Type == TagType.Data).Include(t => t.DataMaps).Where(t => t.DataMaps.Count == 0);
            foreach (var target in targets)
            {
                Delete(target, true);
            }
            return targets.Count();
        }

        #endregion

        #region 学習履歴

        /// <summary>
        /// 指定した学習履歴IDに紐づく全てのタグを取得する
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        public IEnumerable<Tag> GetAllTrainingHistoryTag(long trainingHistoryId)
        {
            return FindModelAll<TrainingHistoryTagMap>(m => m.TrainingHistoryId == trainingHistoryId).Include(m => m.Tag).Select(tm => tm.Tag);
        }

        /// <summary>
        /// 指定した学習履歴にタグをつける。
        /// 学習履歴IDの存在チェックは行わない。
        /// 学習履歴を新規登録する場合に使う。既存学習履歴なら<see cref="AddTrainingHistoryTagsAsync(long, string, bool)"/>を使用する。
        /// </summary>
        /// <param name="trainingHistory">学習履歴</param>
        /// <param name="tagName">タグ名</param>
        public void AddTrainingHistoryTag(TrainingHistory trainingHistory, string tagName)
        {
            //既存のタグがあるか
            Tag tag = Find(t => t.Name == tagName && t.Type == TagType.Training);
            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagName,
                    Type = TagType.Training
                };
                Add(tag);
            }
            AddModel<TrainingHistoryTagMap>(new TrainingHistoryTagMap()
            {
                TrainingHistory = trainingHistory,
                Tag = tag,
            });
        }

        /// <summary>
        /// 指定した学習履歴にタグをつける。
        /// 学習履歴IDの存在チェックは行わない。
        /// タグを付与したらtrueを返す。
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        /// <param name="tagName">タグ名</param>
        /// <param name="checkExists">既に付与済みかチェックするか</param>
        public async Task<bool> AddTrainingHistoryTagAsync(long trainingHistoryId, string tagName, bool checkExists = true)
        {
            //既存のタグがあるか
            Tag tag = Find(t => t.Name == tagName && t.Type == TagType.Training);
            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagName,
                    Type = TagType.Training
                };
                Add(tag);
            }
            else if (checkExists)
            {
                bool exists = await ExistsModelAsync<TrainingHistoryTagMap>(map => map.TrainingHistoryId == trainingHistoryId && map.TagId == tag.Id);
                if (exists)
                {
                    return false;
                }
            }
            AddModel<TrainingHistoryTagMap>(new TrainingHistoryTagMap()
            {
                TrainingHistoryId = trainingHistoryId,
                Tag = tag,
            });
            return true;
        }

        /// <summary>
        /// 指定された学習履歴IDに紐づく全てのタグを削除する。
        /// 学習履歴IDの存在チェックはしない。
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        public void DeleteAllTrainingHistoryTag(long trainingHistoryId)
        {
            DeleteModelAll<TrainingHistoryTagMap>(m => m.TrainingHistoryId == trainingHistoryId);
        }

        /// <summary>
        /// 指定された学習履歴IDに紐づくタグを削除する。
        /// 学習履歴IDの存在チェックはしない。
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        /// <param name="tagName">タグ名</param>
        public bool DeleteTrainingHistoryTag(long trainingHistoryId, string tagName)
        {
            DeleteModelAll<TrainingHistoryTagMap>(m => m.TrainingHistoryId == trainingHistoryId);

            //既存のタグがあるか
            Tag tag = Find(t => t.Name == tagName && t.Type == TagType.Training);
            if (tag == null)
            {
                //タグ自体が存在しなければ、当然紐づいていない
                return false;
            }

            var tagMap = FindModel<TrainingHistoryTagMap>(map => map.TrainingHistoryId == trainingHistoryId && map.TagId == tag.Id);
            if (tagMap == null)
            {
                //タグとの紐づけはない
                return false;
            }

            DeleteModel<TrainingHistoryTagMap>(tagMap);

            return true;
        }

        /// <summary>
        /// 学習履歴で使用するタグの内、未使用のタグをすべて削除する
        /// </summary>
        /// <returns>削除したタグの数</returns>
        public int DeleteUnUsedTrainingHistoryTags()
        {
            var targets = GetModelAll<Tag>(true).Where(t => t.Type == TagType.Training).Include(t => t.TrainingHistoryMaps).Where(t => t.TrainingHistoryMaps.Count == 0);
            foreach (var target in targets)
            {
                Delete(target, true);
            }
            return targets.Count();
        }

        #endregion
    }
}

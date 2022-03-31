using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// タグロジック
    /// </summary>
    public class TagLogic : PlatypusLogicBase, ITagLogic
    {
        private ITagRepository tagRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TagLogic(
            ITagRepository tagRepository,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.tagRepository = tagRepository;
        }

        /// <summary>
        /// 選択中のテナントのすべてのタグを取得する
        /// </summary>
        public IEnumerable<Tag> GetAllTags()
        {
            return tagRepository.GetAll();
        }

        /// <summary>
        /// タグ名の配列から、対応するタグ情報の配列を取得する。
        /// 一つでも存在しないタグ名が含まれていた場合、nullが返る。
        /// </summary>
        /// <param name="tagNames">タグ名</param>
        public IEnumerable<long> GetTagIds(IEnumerable<string> tagNames)
        {
            var tagIds = tagRepository.GetDataTagIds(tagNames);
            if (tagIds.Count() != tagNames.Count())
            {
                //数が一致しない＝存在しないタグが含まれていた
                return null;
            }
            return tagIds;
        }

        #region データ

        /// <summary>
        /// 指定したデータIDに紐づくすべてのタグを取得する
        /// </summary>
        /// <param name="dataId">データID</param>
        public IEnumerable<Tag> GetAllDataTag(long dataId)
        {
            return tagRepository.GetAllDataTag(dataId);
        }

        /// <summary>
        /// 指定した新規データにタグを追加する。
        /// </summary>
        /// <param name="data">新規データ</param>
        /// <param name="inputTags">関連付けるタグ</param>
        public void CreateDataTags(Data data, IEnumerable<string> inputTags)
        {
            foreach (var inputTag in inputTags.Distinct())
            {
                if (string.IsNullOrEmpty(inputTag) == false)
                {
                    tagRepository.AddDataTag(data, inputTag);
                }
            }
        }

        /// <summary>
        /// 指定した既存データIDとタグを関連付ける。
        /// 既存の関連付け状況に関わらず、指定されたタグのみが紐づいている状況にする（他の紐づけはすべて削除する）
        /// </summary>
        /// <remarks>
        /// 親無しになるタグのチェックは行わない。
        /// </remarks>
        /// <param name="dataId">データID</param>
        /// <param name="inputTags">関連付けるタグ</param>
        public async Task EditDataTagsAsync(long dataId, IEnumerable<string> inputTags)
        {
            //まずは既存のタグをすべて削除
            tagRepository.DeleteAllDataTag(dataId);

            foreach (var inputTag in inputTags.Distinct())
            {
                if (string.IsNullOrEmpty(inputTag) == false)
                {
                    //タグを付与する。既存タグは削除済みなので、重複チェックはしない。
                    await tagRepository.AddDataTagAsync(dataId, inputTag, false);
                }
            }
        }

        /// <summary>
        /// 指定されたデータIDに紐づく全てのタグを削除する。
        /// データIDの存在チェックはしない。
        /// </summary>
        /// <param name="dataId">データID</param>
        public void DeleteDataTags(long dataId)
        {
            //登録済みのタグマップ
            tagRepository.DeleteAllDataTag(dataId);
        }

        #endregion

        #region 学習履歴

        /// <summary>
        /// 指定した学習履歴IDに紐づくすべてのタグを取得する
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        public IEnumerable<Tag> GetAllTrainingHistoryTag(long trainingHistoryId)
        {
            return tagRepository.GetAllTrainingHistoryTag(trainingHistoryId);
        }

        /// <summary>
        /// 指定した新規学習履歴にタグを追加する。
        /// </summary>
        /// <param name="trainingHistory">学習履歴</param>
        /// <param name="inputTags">関連付けるタグ</param>
        public void CreateTrainingHistoryTags(TrainingHistory trainingHistory, IEnumerable<string> inputTags)
        {
            foreach (var inputTag in inputTags.Distinct())
            {
                if (string.IsNullOrEmpty(inputTag) == false)
                {
                    tagRepository.AddTrainingHistoryTag(trainingHistory, inputTag);
                }
            }
        }

        /// <summary>
        /// 指定した既存学習履歴IDとタグを関連付ける。
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        /// <param name="inputTags">関連付けるタグ</param>
        public async Task AddTrainingHistoryTagsAsync(long trainingHistoryId, IEnumerable<string> inputTags)
        {
            foreach (var inputTag in inputTags.Distinct())
            {
                if (string.IsNullOrEmpty(inputTag) == false)
                {
                    //タグを付与する。
                    await tagRepository.AddTrainingHistoryTagAsync(trainingHistoryId, inputTag, true);
                }
            }
        }

        /// <summary>
        /// 指定した既存学習履歴IDとタグを関連付ける。
        /// 既存の関連付け状況に関わらず、指定されたタグのみが紐づいている状況にする（他の紐づけはすべて削除する）
        /// </summary>
        /// <remarks>
        /// 親無しになるタグのチェックは行わない。
        /// </remarks>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        /// <param name="inputTags">関連付けるタグ</param>
        public async Task EditTrainingHistoryTagsAsync(long trainingHistoryId, IEnumerable<string> inputTags)
        {
            //まずは既存のタグをすべて削除
            tagRepository.DeleteAllTrainingHistoryTag(trainingHistoryId);

            foreach (var inputTag in inputTags.Distinct())
            {
                if (string.IsNullOrEmpty(inputTag) == false)
                {
                    //タグを付与する。既存タグは削除済みなので、重複チェックはしない。
                    await tagRepository.AddTrainingHistoryTagAsync(trainingHistoryId, inputTag, false);
                }
            }
        }

        /// <summary>
        /// 指定された学習履歴IDに紐づく全てのタグを削除する。
        /// 学習履歴IDの存在チェックはしない。
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        public void DeleteTrainingHistoryTags(long trainingHistoryId)
        {
            //登録済みのタグマップ
            tagRepository.DeleteAllTrainingHistoryTag(trainingHistoryId);
        }

        #endregion
    }
}

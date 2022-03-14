using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// タグロジックインタフェース
    /// </summary>
    public interface ITagLogic
    {
        /// <summary>
        /// 選択中のテナントのすべてのタグを取得する
        /// </summary>
        IEnumerable<Tag> GetAllTags();

        /// <summary>
        /// タグ名の配列から、対応するタグ情報の配列を取得する。
        /// 一つでも存在しないタグ名が含まれていた場合、nullが返る。
        /// </summary>
        /// <param name="tagNames">タグ名</param>
        IEnumerable<long> GetTagIds(IEnumerable<string> tagNames);

        #region データ

        /// <summary>
        /// 指定したデータIDに紐づくすべてのタグを取得する
        /// </summary>
        /// <param name="dataId">データID</param>
        IEnumerable<Tag> GetAllDataTag(long dataId);

        /// <summary>
        /// 指定した新規データにタグを追加する。
        /// </summary>
        /// <param name="data">新規データ</param>
        /// <param name="inputTags">関連付けるタグ</param>
        void CreateDataTags(Data data, IEnumerable<string> inputTags);

        /// <summary>
        /// 指定した既存データIDとタグを関連付ける。
        /// 既存の関連付け状況に関わらず、指定されたタグのみが紐づいている状況にする（他の紐づけはすべて削除する）
        /// </summary>
        /// <remarks>
        /// 親無しになるタグのチェックは行わない。
        /// </remarks>
        /// <param name="dataId">データID</param>
        /// <param name="inputTags">関連付けるタグ</param>
        Task EditDataTagsAsync(long dataId, IEnumerable<string> inputTags);

        /// <summary>
        /// 指定されたデータIDに紐づく全てのタグを削除する。
        /// データIDの存在チェックはしない。
        /// </summary>
        /// <param name="dataId">データID</param>
        void DeleteDataTags(long dataId);

        #endregion

        #region 学習履歴

        /// <summary>
        /// 指定した学習履歴IDに紐づくすべてのタグを取得する
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        IEnumerable<Tag> GetAllTrainingHistoryTag(long trainingHistoryId);

        /// <summary>
        /// 指定した新規学習履歴にタグを追加する。
        /// </summary>
        /// <param name="trainingHistory">学習履歴</param>
        /// <param name="inputTags">関連付けるタグ</param>
        void CreateTrainingHistoryTags(TrainingHistory trainingHistory, IEnumerable<string> inputTags);

        /// <summary>
        /// 指定した既存データIDとタグを関連付ける。
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        /// <param name="inputTags">関連付けるタグ</param>
        Task AddTrainingHistoryTagsAsync(long trainingHistoryId, IEnumerable<string> inputTags);

        /// <summary>
        /// 指定した既存データIDとタグを関連付ける。
        /// 既存の関連付け状況に関わらず、指定されたタグのみが紐づいている状況にする（他の紐づけはすべて削除する）
        /// </summary>
        /// <remarks>
        /// 親無しになるタグのチェックは行わない。
        /// </remarks>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        /// <param name="inputTags">関連付けるタグ</param>
        Task EditTrainingHistoryTagsAsync(long trainingHistoryId, IEnumerable<string> inputTags);

        /// <summary>
        /// 指定されたデータIDに紐づく全てのタグを削除する。
        /// 学習履歴IDの存在チェックはしない。
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        void DeleteTrainingHistoryTags(long trainingHistoryId);

        #endregion
    }
}

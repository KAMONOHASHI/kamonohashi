using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// タグテーブルにアクセスするためのレポジトリ
    /// </summary>
    public interface ITagRepository : IRepositoryForTenant<Tag>
    {
        #region データ

        /// <summary>
        /// 指定したデータIDに紐づく全てのタグを取得する
        /// </summary>
        /// <param name="dataId">データID</param>
        IEnumerable<Tag> GetAllDataTag(long dataId);

        /// <summary>
        /// タグからタグマップを取得する
        /// </summary>
        /// <param name="tags">タグ</param>
        IEnumerable<DataTagMap> GetDataTagMapsByTags(IEnumerable<Tag> tags);

        /// <summary>
        /// 指定したデータにタグをつける。
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="tagName">タグ名</param>
        void AddDataTag(Data data, string tagName);

        /// <summary>
        /// 指定したデータにタグをつける。
        /// データIDの存在チェックは行わない。
        /// タグを付与したらtrueを返す。
        /// </summary>
        /// <param name="dataId">データID</param>
        /// <param name="tagName">タグ名</param>
        /// <param name="checkExists">既に付与済みかチェックするか</param>
        Task<bool> AddDataTagAsync(long dataId, string tagName, bool checkExists = true);

        /// <summary>
        /// 指定されたタグ名のリストに一致するタグIDのリストを返す。
        /// 存在しないタグ名が含まれていた場合、そのタグ名は無視する。
        /// </summary>
        /// <param name="tagNames">タグ名</param>
        IEnumerable<long> GetDataTagIds(IEnumerable<string> tagNames);

        /// <summary>
        /// 指定されたデータIDに紐づく全てのタグを削除する。
        /// データIDの存在チェックは行わない。
        /// </summary>
        /// <param name="dataId">データID</param>
        void DeleteAllDataTag(long dataId);

        /// <summary>
        /// 指定されたデータIDに紐づくタグを削除する。
        /// データIDの存在チェックはしない。
        /// </summary>
        /// <param name="dataId">データID</param>
        /// <param name="tagName">タグ名</param>
        bool DeleteDataTag(long dataId, string tagName);

        /// <summary>
        /// データで使用するタグの内、未使用のタグをすべて削除する
        /// </summary>
        /// <returns>削除したタグの数</returns>
        int DeleteUnUsedDataTags();

        #endregion

        #region 学習履歴

        /// <summary>
        /// 指定した学習履歴IDに紐づく全てのタグを取得する
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        IEnumerable<Tag> GetAllTrainingHistoryTag(long trainingHistoryId);

        /// <summary>
        /// 指定した学習履歴にタグをつける。
        /// </summary>
        /// <param name="trainingHistory">学習履歴</param>
        /// <param name="tagName">タグ名</param>
        void AddTrainingHistoryTag(TrainingHistory trainingHistory, string tagName);

        /// <summary>
        /// 指定した学習履歴にタグをつける。
        /// 学習履歴IDの存在チェックは行わない。
        /// タグを付与したらtrueを返す。
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        /// <param name="tagName">タグ名</param>
        /// <param name="checkExists">既に付与済みかチェックするか</param>
        Task<bool> AddTrainingHistoryTagAsync(long trainingHistoryId, string tagName, bool checkExists = true);

        /// <summary>
        /// 指定された学習履歴IDに紐づく全てのタグを削除する。
        /// 学習履歴IDの存在チェックは行わない。
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        void DeleteAllTrainingHistoryTag(long trainingHistoryId);

        /// <summary>
        /// 指定された学習履歴IDに紐づくタグを削除する。
        /// 学習履歴IDの存在チェックはしない。
        /// </summary>
        /// <param name="trainingHistoryId">学習履歴ID</param>
        /// <param name="tagName">タグ名</param>
        bool DeleteTrainingHistoryTag(long trainingHistoryId, string tagName);

        /// <summary>
        /// 学習履歴で使用するタグの内、未使用のタグをすべて削除する
        /// </summary>
        /// <returns>削除したタグの数</returns>
        int DeleteUnUsedTrainingHistoryTags();

        #endregion
    }
}

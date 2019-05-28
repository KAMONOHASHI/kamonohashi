using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 指定したデータIDに紐づくすべてのタグを取得する
        /// </summary>
        IEnumerable<Tag> GetAllTags(long dataId);

        /// <summary>
        /// 指定した新規データにタグを追加する。
        /// </summary>
        void Create(Data data, IEnumerable<string> inputTags);

        /// <summary>
        /// 指定した既存データIDとタグを関連付ける。
        /// 既存の関連付け状況に関わらず、指定されたタグのみが紐づいている状況にする（他の紐づけはすべて削除する）
        /// </summary>
        /// <remarks>
        /// 親無しになるタグのチェックは行わない。
        /// </remarks>
        Task EditAsync(long dataId, IEnumerable<string> inputTags);

        /// <summary>
        /// タグ名の配列から、対応するタグ情報の配列を取得する。
        /// 一つでも存在しないタグ名が含まれていた場合、nullが返る。
        /// </summary>
        IEnumerable<long> GetTagIds(IEnumerable<string> tagNames);

        /// <summary>
        /// 指定されたデータIDに紐づく全てのタグを削除する。
        /// dataIdの存在チェックはしない。
        /// </summary>
        void Delete(long dataId);
    }
}

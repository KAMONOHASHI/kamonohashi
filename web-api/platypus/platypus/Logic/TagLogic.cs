using Microsoft.AspNetCore.Http;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        /// 指定したデータIDに紐づくすべてのタグを取得する
        /// </summary>
        public IEnumerable<Tag> GetAllTags(long dataId)
        {
            return tagRepository.GetAllTag(dataId);
        }

        /// <summary>
        /// 指定した新規データにタグを追加する。
        /// </summary>
        public void Create(Data data, IEnumerable<string> inputTags)
        {
            foreach (var inputTag in inputTags.Distinct())
            {
                if (string.IsNullOrEmpty(inputTag) == false)
                {
                    tagRepository.Add(data, inputTag);
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
        public async Task EditAsync(long dataId, IEnumerable<string> inputTags)
        {
            //まずは既存のタグをすべて削除
            tagRepository.DeleteAll(dataId);
            
            foreach (var inputTag in inputTags.Distinct())
            {
                if (string.IsNullOrEmpty(inputTag) == false)
                {
                    //タグを付与する。既存タグは削除済みなので、重複チェックはしない。
                    await tagRepository.AddAsync(dataId, inputTag, false);
                }
            }
        }

        /// <summary>
        /// タグ名の配列から、対応するタグ情報の配列を取得する。
        /// 一つでも存在しないタグ名が含まれていた場合、nullが返る。
        /// </summary>
        public IEnumerable<long> GetTagIds(IEnumerable<string> tagNames)
        {
            var tagIds = tagRepository.GetTagIds(tagNames);
            if(tagIds.Count() != tagNames.Count())
            {
                //数が一致しない＝存在しないタグが含まれていた
                return null;
            }
            return tagIds;
        }

        /// <summary>
        /// 指定されたデータIDに紐づく全てのタグを削除する。
        /// dataIdの存在チェックはしない。
        /// </summary>
        public void Delete(long dataId)
        {
            //登録済みのタグマップ
            tagRepository.DeleteAll(dataId);
        }
    }
}

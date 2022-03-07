using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.UserGroupApiModels
{
    /// <summary>
    /// ユーザグループ一覧出力モデル
    /// </summary>
    public class IndexOutputModel : OutputModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userGroup">ユーザグループ</param>
        public IndexOutputModel(UserGroup userGroup) : base(userGroup)
        {
            Id = userGroup.Id;
            Name = userGroup.Name;
            Memo = userGroup.Memo;
            IsGroup = userGroup.IsGroup;
            Dn = userGroup.Dn;
        }

        /// <summary>
        /// ユーザグループID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ユーザグループ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 対象ユーザグループがグループか、OUか。
        /// </summary>
        /// <remarks>
        /// True：グループ、False：OU
        /// </remarks>
        public bool IsGroup { get; set; }

        /// <summary>
        /// 対象ユーザグループのDN情報
        /// </summary>
        public string Dn { get; set; }
    }
}

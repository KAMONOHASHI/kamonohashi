using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.UserGroupApiModels
{
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(UserGroup userGroup) : base(userGroup)
        {
            Id = userGroup.Id;
            Name = userGroup.Name;
            Memo = userGroup.Memo;
            IsGroup = userGroup.IsGroup;
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
        public bool IsGroup { get; set; }
    }
}

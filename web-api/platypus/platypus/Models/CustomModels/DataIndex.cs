using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.Models.CustomModels
{
    public class DataIndex : TenantModelBase
    {

        /// <summary>
        /// 登録者表示名
        /// </summary>
        public string DisplayNameCreatedBy { get; set; }

        /// <summary>
        /// 表示用ID
        /// </summary>
        public long? DisplayId { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// メモ。非構造データ用。
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 元データ名
        /// </summary>
        public string ParentDataName { get; set; }

        /// <summary>
        /// 元データID
        /// </summary>
        public long? ParentDataId { get; set; }

        /// <summary>
        /// タグをカンマ区切りで連結したもの
        /// </summary>
        public string Tag { get; set; }
    }
}

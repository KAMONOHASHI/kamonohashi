using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    /// <summary>
    /// Index用モデル。
    /// タグ情報を含んでいる。
    /// </summary>
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(Data data) : base(data)
        {
            Id = data.Id;
            DisplayId = data.DisplayId;
            Name = data.Name;
            Memo = data.Memo;
            IsRaw = data.ParentDataId == null;
            Tags = data.Tags?.ToList();
        }

        public IndexOutputModel(Models.CustomModels.DataIndex data) : base()
        {
            Id = data.Id;
            DisplayId = data.DisplayId;
            Name = data.Name;
            Memo = data.Memo;
            IsRaw = data.ParentDataId == null;
            ParentDataId = data.ParentDataId;
            ParentDataName = data.ParentDataName;
            if (string.IsNullOrEmpty(data.Tag) == false)
            {
                Tags = data.Tag.Split(',').OrderBy(t => t);
            }

            CreatedBy = data.CreatedBy;
            CreatedAt = data.CreatedAt.ToFormatedString();
            ModifiedBy = data.ModifiedBy;
            ModifiedAt = data.ModifiedAt.ToFormatedString();
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
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
        /// 未加工のデータか
        /// </summary>
        public bool IsRaw { get; set; }

        /// <summary>
        /// 元データ名
        /// </summary>
        public string ParentDataName { get; set; }

        /// <summary>
        /// 元データID
        /// </summary>
        public long? ParentDataId { get; set; }
        /// <summary>
        /// タグ
        /// </summary>
        public IEnumerable<string> Tags { get; set; }
    }
}

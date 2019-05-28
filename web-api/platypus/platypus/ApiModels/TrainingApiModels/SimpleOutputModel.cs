using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// 学習履歴のうち、コスト最小で取得できる情報だけを保持する
    /// </summary>
    public class SimpleOutputModel : Components.OutputModelBase
    {
        public SimpleOutputModel(TrainingHistory history) : base(history)
        {
            Id = history.Id;
            DisplayId = history.DisplayId;
            Name = history.Name;
            Memo = history.Memo;
            Status = history.GetStatus().ToString();
            FullName = $"{Id}:{Name}";
            Favorite = history.Favorite;
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
        /// メモ
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// お気に入り
        /// </summary>
        public bool Favorite { get; set; }

        /// <summary>
        /// 表示用
        /// </summary>
        public string FullName { get; set; }
    }
}

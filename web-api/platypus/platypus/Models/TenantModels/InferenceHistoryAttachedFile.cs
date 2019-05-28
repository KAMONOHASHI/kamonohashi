using Nssol.Platypus.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 推論履歴用添付ファイル
    /// </summary>
    public class InferenceHistoryAttachedFile : TenantModelBase
    {
        /// <summary>
        /// 推論履歴ID
        /// </summary>
        [Required]
        public long InferenceHistoryId { get; set; }

        /// <summary>
        /// ファイルを識別するためのキー
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// ファイル名
        /// </summary>
        [Required]
        public string FileName { get; set; }
        /// <summary>
        /// 保存先パス
        /// </summary>
        [Required]
        public string StoredPath { get; set; }
        /// <summary>
        /// 推論履歴
        /// </summary>
        [ForeignKey(nameof(InferenceHistoryId))]
        public virtual InferenceHistory InferenceHistory { get; set; }
    }
}

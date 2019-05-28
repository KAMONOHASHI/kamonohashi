using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 学習履歴用添付ファイル
    /// </summary>
    public class TrainingHistoryAttachedFile : TenantModelBase
    {
        /// <summary>
        /// 学習履歴ID
        /// </summary>
        [Required]
        public long TrainingHistoryId { get; set; }

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
        /// 学習履歴
        /// </summary>
        [ForeignKey(nameof(TrainingHistoryId))]
        public virtual TrainingHistory TrainingHistory { get; set; }

        /// <summary>
        /// 添付ファイル
        /// </summary>
        [NotMapped]
        public ResourceFileInfo AttachedFile
        {
            get
            {
                return new ResourceFileInfo(StoredPath, FileName, ResourceType.TrainingHistoryAttachedFiles);
            }
            set
            {
                if(value.Type != ResourceType.TrainingHistoryAttachedFiles)
                {
                    throw new ArgumentException($"Unexpected resource type: expected = {ResourceType.TrainingHistoryAttachedFiles}, actual = {value.Type}");
                }
                this.StoredPath = value.StoredPath;
                this.FileName = value.FileName;
            }
        }
    }
}

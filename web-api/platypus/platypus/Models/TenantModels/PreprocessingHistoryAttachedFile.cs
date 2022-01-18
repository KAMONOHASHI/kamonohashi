using Nssol.Platypus.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 前処理履歴用添付ファイル
    /// </summary>
    public class PreprocessHistoryAttachedFile : TenantModelBase
    {
        /// <summary>
        /// 前処理ID
        /// </summary>
        [Required]
        public long PreprocessHistoryId { get; set; }

        /// <summary>
        /// データID
        /// </summary>
        [Required]
        public long PreprocessDataId { get; set; }

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
        [ForeignKey(nameof(PreprocessHistory))]
        public virtual PreprocessHistory PreprocessHistory { get; set; }

        /// <summary>
        /// 添付ファイル
        /// </summary>
        [NotMapped]
        public ResourceFileInfo AttachedFile
        {
            get
            {
                return new ResourceFileInfo(StoredPath, FileName, ResourceType.PreprocContainerAttachedFiles);
            }
            set
            {
                if (value.Type != ResourceType.PreprocContainerAttachedFiles)
                {
                    throw new ArgumentException($"Unexpected resource type: expected = {ResourceType.PreprocContainerAttachedFiles}, actual = {value.Type}");
                }
                this.StoredPath = value.StoredPath;
                this.FileName = value.FileName;
            }
        }
    }
}

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
    /// 実験履歴用添付ファイル
    /// </summary>
    public class ExperimentPreprocessHistoryAttachedFile : TenantModelBase
    {
        /// <summary>
        /// 実験の前処理履歴ID
        /// </summary>
        [Required]
        public long ExperimentPreprocessHistoryId { get; set; }

        /// <summary>
        /// アクアリウムデータセットID
        /// </summary>
        [Required]
        public long DataSetId { get; set; }

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
        /// 実験履歴
        /// </summary>
        [ForeignKey(nameof(ExperimentHistory))]
        public virtual ExperimentHistory ExperimentHistory { get; set; }

        /// <summary>
        /// 添付ファイル
        /// </summary>
        [NotMapped]
        public ResourceFileInfo AttachedFile
        {
            get
            {
                return new ResourceFileInfo(StoredPath, FileName, ResourceType.ExperimentContainerAttachedFiles);
            }
            set
            {
                if(value.Type != ResourceType.ExperimentContainerAttachedFiles)
                {
                    throw new ArgumentException($"Unexpected resource type: expected = {ResourceType.ExperimentContainerAttachedFiles}, actual = {value.Type}");
                }
                this.StoredPath = value.StoredPath;
                this.FileName = value.FileName;
            }
        }
    }
}

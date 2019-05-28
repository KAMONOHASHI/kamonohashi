using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    public class AttachedFileOutputModel
    {
        public AttachedFileOutputModel()
        {
        }

        public AttachedFileOutputModel(long id, string fileName, long fileId)
        {
            this.Id = id;
            this.FileName = fileName;
            this.FileId = fileId;
        }

        /// <summary>
        /// 学習履歴ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 添付ファイルID
        /// </summary>
        public long FileId { get; set; }
        /// <summary>
        /// ファイルURL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 削除可能か
        /// </summary>
        public bool IsLocked { get; set; }
    }
}

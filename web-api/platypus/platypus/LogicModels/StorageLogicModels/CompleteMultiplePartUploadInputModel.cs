using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.LogicModels.StorageLogicModels
{
    /// <summary>
    /// アップロード完了処理モデル
    /// </summary>
    public class CompleteMultiplePartUploadInputModel
    {
        /// <summary>
        /// 分割アップロード完了時の'PartNumber+ETag'
        /// </summary>
        [Required]
        public IEnumerable<string> PartETags { get; set; }

        /// <summary>
        /// 分割アップロードID
        /// </summary>
        [Required]
        public string UploadId { get; set; }

        /// <summary>
        /// キー
        /// </summary>
        [Required]
        public string Key { get; set; }
    }
}

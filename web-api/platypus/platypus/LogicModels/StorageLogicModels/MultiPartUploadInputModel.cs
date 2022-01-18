using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.LogicModels.StorageLogicModels
{
    /// <summary>
    /// 分割アップロードの入力モデル
    /// </summary>
    public class MultiPartUploadInputModel
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        [Required]
        [FromQuery(Name = "fileName")]
        public string FileName { get; set; }
        /// <summary>
        /// 分割数
        /// </summary>
        [Required]
        [FromQuery(Name = "partSum")]
        public int PartSum { get; set; }
        /// <summary>
        /// リソース名
        /// </summary>
        [Required]
        [FromQuery(Name = "type")]
        public string Type { get; set; }
    }
}

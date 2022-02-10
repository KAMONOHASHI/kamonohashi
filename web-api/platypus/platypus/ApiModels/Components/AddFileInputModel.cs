using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.Components
{
    public class AddFileInputModel
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        [Required]
        public string FileName { get; set; }
        /// <summary>
        /// 保存パス
        /// </summary>
        [Required]
        public string StoredPath { get; set; }
    }
}

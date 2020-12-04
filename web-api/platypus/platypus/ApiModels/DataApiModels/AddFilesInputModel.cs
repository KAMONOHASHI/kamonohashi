using Nssol.Platypus.ApiModels.Components;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    public class AddFilesInputModel
    {
        /// <summary>
        /// ファイル名と保存パスのペアのListを受け取る
        /// </summary>
        [Required]
        public List<AddFileInputModel> Files { get; set; }
    }
}

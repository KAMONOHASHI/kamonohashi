using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    public class AddFilesInputModel
    {
        public class File
        {
            /// <summary>
            /// ファイル名
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 保存パス
            /// </summary>
            public string StoredPath { get; set; }

        }
        /// <summary>
        /// ファイル名と保存パスのペアのListを受け取る
        /// </summary>
        [Required]
        public List<File> Files { get; set; }

    }
}

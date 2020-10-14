using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    // FileNameとStoredPathはv2.1.1以前との互換用
    // FileNameとStoredPathのペアまたはFilesがないといけない
    // 将来的には OpenAPIの oneOfに移行したいが、そのためには swashbuckleとasp dotnet coreの
    // バージョンアップが必要
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

        public List<File> Files { get; set; }


        public string FileName { get; set; }
        /// <summary>
        /// 保存パス
        /// </summary>
        public string StoredPath { get; set; }
    }
}

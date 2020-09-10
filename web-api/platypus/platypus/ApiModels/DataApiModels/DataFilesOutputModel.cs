using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    public class DataFilesOutputModel
    {
        public class File
        {
            public File(long fileId, string key, string fileName)
            {
                this.FileId = fileId;
                this.Key = key;
                this.FileName = fileName;
            }
            /// <summary>
            /// データファイルID
            /// </summary>
            public long FileId { get; set; }
            /// <summary>
            /// ファイル種別キー
            /// </summary>
            public string Key { get; set; }
            /// <summary>
            /// ファイル名
            /// </summary>
            public string FileName { get; set; }

        }
        /// <summary>
        /// データID
        /// </summary>
        public long Id { get; set; }
        public List<File> Files { get; set; }

    }
}

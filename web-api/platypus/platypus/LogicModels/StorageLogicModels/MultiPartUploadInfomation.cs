using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.LogicModels.StorageLogicModels
{
    /// <summary>
    /// 分割アップロード情報
    /// </summary>
    public class MultiPartUploadModel
    {
        /// <summary>
        /// 分割URL
        /// </summary>
        public IEnumerable<Uri> Uris { get; set; }
        /// <summary>
        /// 分割合計数
        /// </summary>
        public int PartsSum { get; set; }
        /// <summary>
        /// アップロードID
        /// </summary>
        public string UploadId { get; set; }
        /// <summary>
        /// キー
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 保存先パス
        /// </summary>
        public string StoredPath { get; set; }
    }
}

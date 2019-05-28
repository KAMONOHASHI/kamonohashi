using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Nssol.Platypus.Infrastructure.Infos
{
    /// <summary>
    /// オブジェクトストレージで管理されるオブジェクトファイルのモデル。
    /// minioで使用
    /// </summary>
    public class StorageFileInfo
    {
        /// <summary>
        /// オブジェクトストレージ上でのファイルパス
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime LastModified { get; }

        /// <summary>
        /// サイズ
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// ダウンロードURL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// コンストラクタ。システム内からのみ利用可能。
        /// </summary>
        internal StorageFileInfo(string key, DateTime lastModified, long size)
        {
            Key = key;
            FileName = Path.GetFileName(key);
            LastModified = lastModified;
            Size = size;
        }
    }
}

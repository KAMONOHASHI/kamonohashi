using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// オブジェクトストレージで管理されるリソースのモデル。
    /// リソース種別、ストレージ上でのリソース名、元のリソース名などが纏まっていないと煩雑なので、これで整理。
    /// cephで使用
    /// </summary>
    public class ResourceFileInfo
    {
        /// <summary>
        /// オブジェクトストレージ上でのファイルパス（ファイル名）
        /// </summary>
        public string StoredPath { get; }

        /// <summary>
        /// アップロードされたときのファイル名
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// リソース種別
        /// </summary>
        public ResourceType Type { get; }

        /// <summary>
        /// コンストラクタ。システム内からのみ利用可能。
        /// </summary>
        internal ResourceFileInfo(string storedPath, string fileName, ResourceType type)
        {
            StoredPath = storedPath;
            FileName = Path.GetFileName(fileName);
            Type = type;
        }
    }
}

using Nssol.Platypus.Infrastructure.Infos;
using System.Collections.Generic;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// オブジェクトストレージにディレクトリ直下を問い合わせた結果をまとめたもの
    /// </summary>
    public class StorageListResultInfo
    {
        /// <summary>
        /// searchDirPath 配下のディレクトリ
        /// </summary>
        public List<StorageDirInfo> Dirs { get; }
        /// <summary>
        /// searchDirPath 配下のファイル
        /// </summary>
        public List<StorageFileInfo> Files { get; }
        /// <summary>
        /// ディレクトリ直下に1000件の結果があった場合true
        /// </summary>
        public bool Exceeded { get; }

        /// <summary>
        /// コンストラクタ。システム内からのみ利用可能。
        /// </summary>
        internal StorageListResultInfo(List<StorageDirInfo> dirs, List<StorageFileInfo> files, bool exceeded)
        {
            Dirs = dirs;
            Files = files;
            Exceeded = exceeded;
        }
    }
}

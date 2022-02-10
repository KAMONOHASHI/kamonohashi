namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// オブジェクトストレージのディレクトリのモデル。
    /// minioで使用
    /// </summary>
    public class StorageDirInfo
    {
        /// <summary>
        /// オブジェクトストレージ上でのディレクトリパス
        /// </summary>
        public string DirPath { get; }

        /// <summary>
        /// ディレクトリ名
        /// </summary>
        public string DirName { get; }

        /// <summary>
        /// コンストラクタ。システム内からのみ利用可能。
        /// </summary>
        internal StorageDirInfo(string dirPath)
        {
            var splited = dirPath.Split("/");
            DirPath = dirPath;
            DirName = splited[splited.Length - 2]; // 「/a/b/」→ ["", "a", "b", ""] となるので後ろから2番目
        }
    }
}

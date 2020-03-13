namespace Nssol.Platypus.ServiceModels
{
    /// <summary>
    /// KAMONOHASHIのバージョン情報を表すクラス
    /// </summary>
    public class VersionModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="version">バージョン番号</param>
        public VersionModel(string version)
        {
            Version = version;
            Support = false;
        }

        /// <summary>
        /// バージョン番号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// <see cref="Version"/>のリリース日
        /// </summary>
        public string ReleaseDate { get; set; }


        /// <summary>
        /// <see cref="Version"/>のサポート有無。
        /// true：サポート対象　false：サポート対象外
        /// </summary>
        public bool Support { get; set; }

        /// <summary>
        /// 最新バージョン番号
        /// </summary>
        public string LatestVersion { get; set; }
    }
}

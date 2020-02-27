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
        /// 最新バージョン番号
        /// </summary>
        public string LatestVersion { get; set; }
    }
}

namespace Nssol.Platypus.ServiceModels.GitHubModels
{
    /// <summary>
    /// リリース情報を表すクラス
    /// </summary>
    public class ReleaseModel
    {
        /// <summary>
        /// タグ名（バージョン）
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// リリース名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ドラフトか否か
        /// </summary>
        public bool Draft { get; set; }

        /// <summary>
        /// プレリリースか否か
        /// </summary>
        public bool Prerelease { get; set; }
    }
}

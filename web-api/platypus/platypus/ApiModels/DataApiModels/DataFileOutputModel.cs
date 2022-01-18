namespace Nssol.Platypus.ApiModels.DataApiModels
{
    public class DataFileOutputModel
    {
        /// <summary>
        /// データID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// データファイルID
        /// </summary>
        public long FileId { get; set; }
        /// <summary>
        /// ファイル種別キー
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// ファイルURL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// ファイルサイズ
        /// </summary>
        public long FileSize { get; set; }
    }
}

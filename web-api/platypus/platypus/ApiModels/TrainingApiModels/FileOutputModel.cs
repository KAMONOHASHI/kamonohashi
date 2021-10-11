namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// 学習履歴の添付ファイル出力モデル
    /// </summary>
    public class FileOutputModel
    {
        /// <summary>
        /// 学習履歴ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 添付ファイルID
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

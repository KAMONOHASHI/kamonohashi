namespace Nssol.Platypus.ApiModels.ResourceApiModels
{
    /// <summary>
    /// 履歴メタデータの出力モデル
    /// </summary>
    public class HistoryMetadataOutputModel
    {
        /// <summary>
        /// 件数
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// 開始日
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 終了日
        /// </summary>
        public string EndDate { get; set; }
    }
}

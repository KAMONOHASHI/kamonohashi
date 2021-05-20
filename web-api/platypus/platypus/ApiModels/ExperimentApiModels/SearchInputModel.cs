namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// 実験一覧の検索モデル
    /// </summary>
    public class SearchInputModel
    {
        /// <summary>
        /// IDの検索条件。
        /// 比較文字列＋数値の形式。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 実行時刻の検索条件。
        /// 比較文字列＋時刻の形式。
        /// e.g.（比較文字列は半角でOK）
        /// "2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
        /// "＞2018/01/01" → 2018/01/01 00:00:00 以降
        /// "＜2018/01/01" → 2018/01/01 00:00:00 以前
        /// </summary>
        public string StartedAt { get; set; }
    }
}

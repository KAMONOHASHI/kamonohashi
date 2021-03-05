namespace Nssol.Platypus.ApiModels.Aquarium.DataSetApiModels
{
    /// <summary>
    /// アクアリウムデータセットの検索モデル
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
        /// 作成時刻の検索条件。
        /// 比較文字列＋時刻の形式。
        /// e.g.（比較文字列は半角でOK）
        /// "2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
        /// "＞2018/01/01" → 2018/01/01 00:00:00 以降
        /// "＜2018/01/01" → 2018/01/01 00:00:00 以前
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 更新時刻の検索条件。
        /// 比較文字列＋時刻の形式。
        /// e.g.（比較文字列は半角でOK）
        /// "2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
        /// "＞2018/01/01" → 2018/01/01 00:00:00 以降
        /// "＜2018/01/01" → 2018/01/01 00:00:00 以前
        /// </summary>
        public string ModifiedAt { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}

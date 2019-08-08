namespace Nssol.Platypus.ApiModels.NotebookApiModels
{
    /// <summary>
    /// ノートブック履歴検索の入力モデル。
    /// 複数のアクションメソッドで使われる可能性がある。
    /// 受取は全てクエリパラメータ。
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
        /// 作成日時
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }
        
        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }
    }
}

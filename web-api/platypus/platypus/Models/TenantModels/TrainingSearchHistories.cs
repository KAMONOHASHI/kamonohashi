using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 学習検索履歴モデル
    /// </summary>
    public class TrainingSearchHistories : TenantModelBase
    {
        /// <summary>
        /// 表示用ID
        /// </summary>
        public long? DisplayId { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// IDの検索条件。
        /// この数値以上のIDが検索される。
        /// </summary>
        public long? IdLower { get; set; }

        /// <summary>
        /// IDの検索条件。
        /// この数値以下のIDが検索される。
        /// </summary>
        public long? IdUpper { get; set; }

        /// <summary>
        /// 学習名
        /// </summary>
        public string TrainingName { get; set; }

        /// <summary>
        /// 学習名がor検索かand検索か
        /// </summary>
        public bool? TrainingNameOr { get; set; }

        /// <summary>
        /// 親学習名
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 親学習名がor検索かand検索か
        /// </summary>
        public bool? ParentNameOr { get; set; }

        /// <summary>
        /// 実行時刻の検索の期間の開始の条件。日付の形式。
        /// "2018/01/01" → 2018/01/01 00:00:00 以降が検索される。
        /// </summary>
        public string StartedAtLower { get; set; }

        /// <summary>
        /// 実行時刻の検索の期間の終了の条件。日付の形式。
        /// "2018/01/01" → 2018/01/01 23:59:59 以前が検索される。
        /// </summary>
        public string StartedAtUpper { get; set; }

        /// <summary>
        /// 実行者
        /// </summary>
        public string StartedBy { get; set; }

        /// <summary>
        /// 実行者の検索がor検索かand検索か
        /// </summary>
        public bool? StartedByOr { get; set; }

        /// <summary>
        /// データセット名
        /// </summary>
        public string DataSet { get; set; }

        /// <summary>
        /// データセット名がor検索かand検索か
        /// </summary>
        public bool? DataSetOr { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// メモがor検索かand検索か
        /// </summary>
        public bool? MemoOr { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ステータスがor検索かand検索か
        /// </summary>
        public bool? StatusOr { get; set; }

        /// <summary>
        /// 実行コマンド
        /// </summary>
        public string EntryPoint { get; set; }

        /// <summary>
        /// 実行コマンドがor検索かand検索か
        /// </summary>
        public bool? EntryPointOr { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// タグがor検索かand検索か
        /// </summary>
        public bool? TagsOr { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// 学習履歴検索の入力モデル。
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
        /// 親学習ID
        /// </summary>
        public string ParentId { get; set; }


        /// <summary>
        /// 親学習名
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 実行時刻の検索条件。
        /// 比較文字列＋時刻の形式。
        /// e.g.（比較文字列は半角でOK）
        /// "2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
        /// "＞2018/01/01" → 2018/01/01 00:00:00 以降
        /// "＜2018/01/01" → 2018/01/01 00:00:00 以前
        /// </summary>
        public string StartedAt { get; set; }
        
        /// <summary>
        /// データセット名
        /// </summary>
        public string DataSet { get; set; }
        
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }
        
        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 実行コマンド
        /// </summary>
        public string EntryPoint { get; set; }
    }
}

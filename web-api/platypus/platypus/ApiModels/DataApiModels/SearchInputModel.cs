using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    /// <summary>
    /// データ検索の入力モデル。
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
        /// メモ
        /// </summary>
        public string Memo { get; set; }
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
        /// タグ。
        /// </summary>
        [FromQuery(Name = "tag")]
        public IEnumerable<string> Tags { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// 設定値を保存するテーブル
    /// </summary>
    /// <remarks>
    /// アンチパターンのEAVを避け、単一行テーブルによる実装とする。
    /// https://softwareengineering.stackexchange.com/questions/163606/configuration-data-single-row-table-vs-name-value-pair-table
    /// </remarks>
    public class Setting : ModelBase
    {
        /// <summary>
        /// デフォルト値とユニーク制約を付け、単一行となることを保証するための制御列
        /// </summary>
        [Required]
        public int EnsureSingleRow { get; set; } = 1;

        [Required]
        public string ApiSecurityTokenPass { get; set; }
    }
}

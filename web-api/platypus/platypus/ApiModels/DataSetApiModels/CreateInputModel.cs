using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.DataSetApiModels
{
    /// <summary>
    /// データセット作成の入力モデル
    /// </summary>
    /// <remarks>
    /// CLIで既存データセットからの新規作成を容易に行えるよう、出力したデータセット詳細情報と同じフォーマットを取っている。
    /// </remarks>
    public class CreateInputModel
    {
        /// <summary>
        /// データセット名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// キーにデータ種別、値にデータIDの集合を取るディクショナリ
        /// </summary>
        [Required]
        public Dictionary<string, IEnumerable<Entry>> Entries { get; set; }

        public class Entry
        {
            /// <summary>
            /// データID
            /// </summary>
            [Required]
            public long Id { get; set; }
        }
    }
}

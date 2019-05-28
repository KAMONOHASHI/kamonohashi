using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.DataSetApiModels
{
    /// <summary>
    /// データセット編集の入力モデル
    /// </summary>
    /// <remarks>
    /// CLIで既存データセットからの新規作成を容易に行えるよう、出力したデータセット詳細情報と同じフォーマットを取っている。
    /// </remarks>
    public class EditEntriesInputModel : EditInputModel
    {
        /// <summary>
        /// キーにデータ種別、値にデータIDの集合を取るディクショナリ
        /// </summary>
        public Dictionary<string, IEnumerable<CreateInputModel.Entry>> Entries { get; set; }
    }
}

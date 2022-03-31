using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// 付与または削除するタグ入力モデル
    /// </summary>
    public class TagsInputModel
    {
        /// <summary>
        /// タグを付与、または、削除したい対象の学習履歴のId
        /// </summary>
        [Required]
        public IEnumerable<long> Id { get; set; }

        /// <summary>
        /// 付与、または、削除したいタグ
        /// </summary>
        [Required]
        public IEnumerable<string> Tags { get; set; }
    }
}

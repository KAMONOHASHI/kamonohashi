using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// 検索履歴入力モデル
    /// </summary>
    public class SearchHistoryInputModel
    {
        /// <summary>
        /// 検索履歴の名前
        /// </summary>
        [Required]
        [MinLength(4)]
        public string Name { set; get; }

        /// <summary>
        /// 詳細検索入力モデル
        /// </summary>
        public SearchDetailInputModel SearchDetailInputModel { set; get; }
    }
}

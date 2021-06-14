using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// アクアリウム推論作成の入力モデル
    /// </summary>
    public class EvaluationCreateInputModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// アクアリウムデータセットID
        /// </summary>
        public long DataSetId { get; set; }

        /// <summary>
        /// アクアリウムデータセットバージョンID
        /// </summary>
        public long DataSetVersionId { get; set; }
    }
}
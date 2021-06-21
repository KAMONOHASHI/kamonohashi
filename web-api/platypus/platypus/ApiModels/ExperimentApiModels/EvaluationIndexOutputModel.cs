using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels.Aquarium;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// アクアリウム推論の出力モデル
    /// </summary>
    public class EvaluationIndexOutputModel : EvaluationSimpleOutputModel
    {
        public EvaluationIndexOutputModel(Evaluation evaluation, string status) : base(evaluation)
        {
            StartedAt = evaluation.CreatedAt.ToFormatedString();
            CompletedAt = evaluation.TrainingHistory?.CompletedAt?.ToFormatedString();
            Status = status;
            if (evaluation.TrainingHistory != null)
            {
                Training = new TrainingApiModels.SimpleOutputModel(evaluation.TrainingHistory);
            }
            DataSet = new Aquarium.DataSetApiModels.IndexOutputModel(evaluation.DataSet);
            DataSetVersion = new Aquarium.DataSetApiModels.VersionIndexOutputModel(evaluation.DataSetVersion);
        }

        /// <summary>
        /// 開始日時
        /// </summary>
        public string StartedAt { get; set; }

        /// <summary>
        /// 完了日時
        /// </summary>
        public string CompletedAt { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 学習
        /// </summary>
        public TrainingApiModels.SimpleOutputModel Training { get; set; }

        /// <summary>
        /// データセット
        /// </summary>
        public Aquarium.DataSetApiModels.IndexOutputModel DataSet { get; set; }

        /// <summary>
        /// データセットバージョン
        /// </summary>
        public Aquarium.DataSetApiModels.VersionIndexOutputModel DataSetVersion { get; set; }
    }
}

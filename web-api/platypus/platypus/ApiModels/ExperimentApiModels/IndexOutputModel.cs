using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// 実験の出力モデル
    /// </summary>
    public class IndexOutputModel : SimpleOutputModel
    {
        public IndexOutputModel(Experiment experiment, string status) : base(experiment)
        {
            StartedAt = experiment.CreatedAt.ToFormatedString();
            CompletedAt = experiment.TrainingHistory?.CompletedAt?.ToFormatedString();
            Status = status;
            DataSet = new Aquarium.DataSetApiModels.IndexOutputModel(experiment.DataSet);
            Template = new TemplateApiModels.IndexOutputModel(experiment.Template);
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
        /// データセット
        /// </summary>
        public Aquarium.DataSetApiModels.IndexOutputModel DataSet { get; set; }

        /// <summary>
        /// テンプレート
        /// </summary>
        public TemplateApiModels.IndexOutputModel Template { get; set; }
    }
}

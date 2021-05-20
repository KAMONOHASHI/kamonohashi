using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// 実験の詳細出力モデル
    /// </summary>
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Experiment experiment, string status) 
            : base(experiment, status) 
        {
            DataSetVersion = new Aquarium.DataSetApiModels.VersionIndexOutputModel(experiment.DataSetVersion);
            TemplateVersion = new TemplateApiModels.VersionIndexOutputModel(experiment.TemplateVersion);
            if (experiment.ExperimentPreprocess?.TrainingHistory != null)
            {
                Preprocess = new TrainingApiModels.SimpleOutputModel(experiment.ExperimentPreprocess.TrainingHistory);
            }
            if (experiment.TrainingHistory != null)
            {
                Training = new TrainingApiModels.SimpleOutputModel(experiment.TrainingHistory);
            }
        }

        /// <summary>
        /// データセットバージョン
        /// </summary>
        public Aquarium.DataSetApiModels.VersionIndexOutputModel DataSetVersion { get; set; }

        /// <summary>
        /// テンプレートバージョン
        /// </summary>
        public TemplateApiModels.VersionIndexOutputModel TemplateVersion { get; set; }

        /// <summary>
        /// 前処理
        /// </summary>
        public TrainingApiModels.SimpleOutputModel Preprocess { get; set; }

        /// <summary>
        /// 学習
        /// </summary>
        public TrainingApiModels.SimpleOutputModel Training { get; set; }
    }
}

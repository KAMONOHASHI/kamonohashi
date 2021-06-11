using Nssol.Platypus.Models.TenantModels.Aquarium;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// アクアリウム推論のコスト最小出力モデル
    /// </summary>
    public class EvaluationSimpleOutputModel : Components.OutputModelBase
    {
        public EvaluationSimpleOutputModel(Evaluation evaluation) : base(evaluation)
        {
            Id = evaluation.Id;
            Name = evaluation.Name;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
    }
}

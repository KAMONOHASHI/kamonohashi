using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// 実験のコスト最小出力モデル
    /// </summary>
    public class SimpleOutputModel : Components.OutputModelBase
    {
        public SimpleOutputModel(Experiment history) : base(history)
        {
            Id = history.Id;
            Name = history.Name;
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

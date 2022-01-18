using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 実験前処理
    /// </summary>
    public class ExperimentPreprocess : TenantModelBase
    {
        /// <summary>
        /// アクアリウムデータセットID
        /// </summary>
        public long DataSetId { get; set; }

        /// <summary>
        /// アクアリウムデータセットバージョンID
        /// </summary>
        public long DataSetVersionId { get; set; }

        /// <summary>
        /// テンプレートID
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        /// テンプレートバージョンID
        /// </summary>
        public long TemplateVersionId { get; set; }

        /// <summary>
        /// 学習履歴ID
        /// </summary>
        public long TrainingHistoryId { get; set; }

        /// <summary>
        /// アクアリウムデータセット
        /// </summary>
        [ForeignKey(nameof(DataSetId))]
        public virtual Aquarium.DataSet DataSet { get; set; }

        /// <summary>
        /// アクアリウムデータセットバージョン
        /// </summary>
        [ForeignKey(nameof(DataSetVersionId))]
        public virtual Aquarium.DataSetVersion DataSetVersion { get; set; }

        /// <summary>
        /// テンプレート
        /// </summary>
        [ForeignKey(nameof(TemplateId))]
        public Template Template { get; set; }

        /// <summary>
        /// テンプレートバージョン
        /// </summary>
        [ForeignKey(nameof(TemplateVersionId))]
        public TemplateVersion TemplateVersion { get; set; }

        /// <summary>
        /// 学習履歴
        /// </summary>
        [ForeignKey(nameof(TrainingHistoryId))]
        public TrainingHistory TrainingHistory { get; set; }
    }
}

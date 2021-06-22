using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels.Aquarium
{
    /// <summary>
    /// アクアリウム推論
    /// </summary>
    public class Evaluation : TenantModelBase
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

        /// <summary>
        /// 実験ID
        /// </summary>
        public long ExperimentId { get; set; }

        /// <summary>
        /// 学習履歴ID
        /// </summary>
        public long? TrainingHistoryId { get; set; }

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
        /// 学習履歴
        /// </summary>
        [ForeignKey(nameof(TrainingHistoryId))]
        public TrainingHistory TrainingHistory { get; set; }

        /// <summary>
        /// 実験
        /// </summary>
        [ForeignKey(nameof(ExperimentId))]
        public Experiment Experiment { get; set; }
    }
}


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// 実験開始の入力モデル
    /// </summary>
    public class CreateInputModel
    {
        /// <summary>
        /// 実験名
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
        ///　テンプレートID
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        ///　テンプレートバージョンID
        /// </summary>
        public long TemplateVersionId { get; set; }
    }
}
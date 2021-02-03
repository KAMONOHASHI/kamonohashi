using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// 新規実験実行モデル
    /// </summary>
    public class CreateInputModel
    {
        /// <summary>
        /// 識別名
        /// </summary>
        [Required]
        [MinLength(1)]
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
        /// 追加環境変数
        /// </summary>
        public Dictionary<string, string> Options { get; set; }
    }
}
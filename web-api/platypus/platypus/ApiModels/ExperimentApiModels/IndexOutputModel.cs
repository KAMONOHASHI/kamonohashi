using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// 実験履歴のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : SimpleOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="history">実験履歴</param>
        public IndexOutputModel(ExperimentHistory history) : base(history)
        {
            DataSet = new DataSetApiModels.IndexOutputModel(history.DataSet);
            Template = new TemplateApiModels.IndexOutputModel(history.Template);
        }

        /// <summary>
        /// データセット
        /// </summary>
        public DataSetApiModels.IndexOutputModel DataSet { get; set; }

        /// <summary>
        /// テンプレート
        /// </summary>
        public TemplateApiModels.IndexOutputModel Template { get; set; }
    }
}

using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.InferenceApiModels
{
    /// <summary>
    /// 推論履歴のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class InferenceIndexOutputModel : InferenceSimpleOutputModel
    {
        public InferenceIndexOutputModel(InferenceHistory history) : base(history)
        {
            DataSet = new DataSetApiModels.IndexOutputModel(history.DataSet);
            EntryPoint = history.EntryPoint;
            if (history.Parent != null)
            {
                ParentName = history.Parent.Name;
            }
        }

        /// <summary>
        /// データセット
        /// </summary>
        public DataSetApiModels.IndexOutputModel DataSet { get; set; }
        /// <summary>
        /// ジョブ実行コマンド
        /// </summary>
        public string EntryPoint { get; set; }
        /// <summary>
        /// 親学習ジョブ名
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 出力値
        /// </summary>
        public string OutputValue { get; set; }
    }
}

using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// 学習履歴のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : SimpleOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="history">学習履歴</param>
        public IndexOutputModel(TrainingHistory history) : base(history)
        {
            DataSet = new DataSetApiModels.IndexOutputModel(history.DataSet);
            EntryPoint = history.EntryPoint;
            ParentMaps = history.ParentMaps;
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
        /// 親学習
        /// </summary>
        public System.Collections.Generic.ICollection<TrainingHistoryParentMap> ParentMaps { get; set;  }
    }
}

using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Infrastructure;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// 実験の前処理履歴情報のうち、Indexで表示する最低情報だけを保持する。
    /// </summary>
    /// <remarks>
    /// 前処理履歴IDは外部には晒さない
    /// </remarks>
    public class PreprocessHistoriesOutputModel
    {
        public PreprocessHistoriesOutputModel(ExperimentPreprocessHistory history)
        {
            Key = history.Name;
            Status = ContainerStatus.Convert(history.Status).Name;
            CreatedAt = history.CreatedAt.ToFormatedString();

            DataSetId = history.DataSetId;
            DataName = history.DataSet.AquariumDataSet.Name;
            PreprocessId = history.Template.Id;
            PreprocessName = history.Template.Name;
        }
        /// <summary>
        /// コンテナ名になる一意識別文字列
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ステータスの種類。
        /// None: 存在しない。
        /// Running: ジョブが正常に実行されている。
        /// Error: ジョブが異常な状態で実行されている。
        /// Closed: ジョブ実行が正常に完了し、実行結果が保存された。
        /// Failed: ジョブ実行が異常終了した。
        /// </summary>
        public string StatusType { get; set; }

        /// <summary>
        /// 登録日
        /// </summary>
        public string CreatedAt { get; set; }
        /// <summary>
        /// 元データID
        /// </summary>
        public long DataSetId { get; set; }
        /// <summary>
        /// 元データ名
        /// </summary>
        public string DataName { get; set; }
        /// <summary>
        /// 前処理ID
        /// </summary>
        public long? PreprocessId { get; set; }
        /// <summary>
        /// 前処理名
        /// </summary>
        public string PreprocessName { get; set; }

    }
}

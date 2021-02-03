using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Infrastructure;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// 実験の前処理履歴情報のうち、Indexで表示する最低情報だけを保持する。
    /// </summary>
    public class PreprocessHistoryIndexOutputModel : Components.OutputModelBase
    {
        public PreprocessHistoryIndexOutputModel(ExperimentPreprocessHistory history) 
            : base(history)
        {
            Id = history.Id;
            DataSetId = history.DataSetId;
            DataSetVersionId = history.DataSetVersionId;
            TemplateId = history.TemplateId;
            StartedAt = history.StartedAt.ToString();
            CompletedAt = history.CompletedAt.ToString();
            var containerStatus = history.GetStatus();
            Status = containerStatus.Name;
            StatusType = containerStatus.StatusType;
            Key = history.Key;
            OutputDataSetId = history.OutputDataSetId;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

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
        public long? TemplateId { get; set; }

        /// <summary>
        /// 実行開始日時
        /// </summary>
        public string StartedAt { get; set; }

        /// <summary>
        /// 実行完了日時
        /// </summary>
        public string CompletedAt { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ステータスの種類
        /// </summary>
        public string StatusType { get; set; }

        /// <summary>
        /// コンテナキー
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 前処理結果データセットID
        /// </summary>
        public long? OutputDataSetId { get; set; }
    }
}

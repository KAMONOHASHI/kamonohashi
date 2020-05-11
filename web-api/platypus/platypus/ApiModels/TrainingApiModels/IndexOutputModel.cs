using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;

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
            if (history.ParentMaps != null && history.ParentMaps.Count > 0)
            {
                List<string> parentFullNameList = new List<string>();
                foreach (TrainingHistoryParentMap parentMap in history.ParentMaps)
                {
                    parentFullNameList.Add($"{parentMap.Parent.Id}:{parentMap.Parent.Name}");
                }
                ParentFullNameList = parentFullNameList;
            }
            Tags = history.Tags;
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
        /// 親学習名（表示用）
        /// </summary>
        public List<string> ParentFullNameList { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        public IEnumerable<string> Tags { get; set; }
    }
}

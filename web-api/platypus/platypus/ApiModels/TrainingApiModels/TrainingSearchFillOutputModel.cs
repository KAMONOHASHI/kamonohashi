using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// 学習検索項目補完出力モデル
    /// </summary>
    public class TrainingSearchFillOutputModel
    {
        /// <summary>
        /// 実行者名の一覧
        /// </summary>
        public IEnumerable<string> CreatedBy { get; set; }

        /// <summary>
        /// ステータスの一覧
        /// </summary>
        public IEnumerable<string> Status { get; set; }

        /// <summary>
        /// タグの一覧
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// データセットの補完
        /// </summary>
        public IEnumerable<string> Datasets { get; set; }
    }
}

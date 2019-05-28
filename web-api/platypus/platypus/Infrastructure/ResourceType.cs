using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// オブジェクトストレージにファイルを格納する際のリソース種別の列挙値
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// 不正値
        /// </summary>
        Unknown = 0,
        /// <summary>データ</summary>
        Data,
        /// <summary>ユーザーの添付した学習履歴添付ファイル</summary>
        TrainingHistoryAttachedFiles,
        /// <summary>コンテナからKAMONOHASHIの前処理実行ログ添付ファイル</summary>
        PreprocContainerAttachedFiles,
        /// <summary>コンテナからKAMONOHASHIの添付した学習履歴添付ファイル</summary>
        TrainingContainerAttachedFiles,
        /// <summary>学習ジョブのコンテナ出力ファイル結果</summary>
        TrainingContainerOutputFiles,
        /// <summary>ユーザの添付した推論履歴添付ファイル</summary>
        InferenceHistoryAttachedFiles,
        /// <summary>コンテナが添付した推論履歴添付ファイル</summary>
        InferenceContainerAttachedFiles,
        /// <summary>推論ジョブのコンテナ出力ファイル結果</summary>
        InferenceContainerOutputFiles,
    }
}

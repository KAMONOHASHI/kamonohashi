using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 実験履歴
    /// </summary>
    public class ExperimentHistory : TenantModelBase
    {
        /// <summary>
        /// アクアリウムデータセットID
        /// </summary>
        public long DataSetId { get; set; }

        /// <summary>
        /// アクアリウムデータセットバージョンID
        /// </summary>
        public long DataSetVersionId { get; set; }

        /// <summary>
        /// 学習の入力となるkamonohashiデータセットID
        /// (前処理があるなら前処理後のデータセット)
        /// </summary>
        public long? InputDataSetId { get; set; }

        /// <summary>
        /// テンプレートID
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        /// 実行開始日時
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// 実行完了日時
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }


        /// <summary>
        /// ユーザが定義可能なKeyValue値のJson。
        /// Keyが環境変数名、Valueがその値になる。
        /// </summary>
        /// <remarks>
        /// 例：
        /// {
        ///   "MODEL_TYPE" : "hoge",
        ///   "VALID_ARGUMENT" : "--num_classes=3"
        /// }
        /// </remarks>
        public string Options { get; set; }

        /// <summary>
        /// <see cref="Options"/> のディクショナリ表現。
        /// </summary>
        /// <remarks>
        /// <see cref="DataAccess.Repositories.TenantRepositories.ExperimentHistoryRepository.Add(ExperimentHistory)"/> の実行時、
        /// <see cref="Options"/> への変換を行う。更新はされないので、そこだけでいいはず。
        /// </remarks>
        [NotMapped]
        public Dictionary<string, string> OptionDic { get; set; }

        /// <summary>
        /// アクアリウムデータセット
        /// </summary>
        [ForeignKey(nameof(DataSetId))]
        public virtual Models.TenantModels.Aquarium.DataSet DataSet { get; set; }

        /// <summary>
        /// アクアリウムデータセットバージョン
        /// </summary>
        [ForeignKey(nameof(DataSetVersionId))]
        public virtual Models.TenantModels.Aquarium.DataSetVersion DataSetVersion { get; set; }

        /// <summary>
        /// テンプレート
        /// </summary>
        [ForeignKey(nameof(TemplateId))]
        public virtual ModelTemplate Template{ get; set; }

        /// <summary>
        /// 学習の入力となるkamonohashiデータセット
        /// </summary>
        [ForeignKey(nameof(InputDataSetId))]
        public virtual DataSet InputDataSet { get; set; }


        /// <summary>
        /// コンテナ起動時に使用する名前
        /// </summary>
        public string Key
        {
            get
            {
                //実験履歴IDはユニークなので、それをもとに名前を決める
                return $"experiment-{Id}";
            }
        }


        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 環境変数のディクショナリ表現
        /// </summary>
        /// <returns>環境変数</returns>
        public Dictionary<string, string> GetOptionDic()
        {
            if (Options == null)
            {
                return new Dictionary<string, string>();
            }
            OptionDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(Options);
            return OptionDic;
        }

        /// <summary>
        /// コンテナステータスの取得
        /// </summary>
        /// <returns>コンテナのステータス詳細</returns>
        public ContainerStatus GetStatus()
        {
            return ContainerStatus.Convert(Status);
        }
    }
}

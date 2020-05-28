using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// 学習履歴の詳細情報モデル
    /// </summary>
    public class DetailsOutputModel : IndexOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="history">学習履歴</param>
        public DetailsOutputModel(TrainingHistory history) : base(history)
        {
            Key = history.Key;
            Options = new List<KeyValuePair<string, string>>();
            Ports = new List<int>();
            NodePorts = new List<KeyValuePair<string, string>>();
            GitModel = new GitCommitOutputModel()
            {
                GitId = history.ModelGitId,
                Repository = history.ModelRepository,
                Owner = history.ModelRepositoryOwner,
                Branch = history.ModelBranch,
                CommitId = history.ModelCommitId
            };
            ContainerImage = new ContainerImageOutputModel()
            {
                RegistryId = history.ContainerRegistryId.Value,
                RegistryName = history.ContainerRegistry.Name,
                Image = history.ContainerImage,
                Tag = history.ContainerTag
            };
            CompletedAt = history.CompletedAt?.ToFormatedString();
            StartedAt = history.StartedAt?.ToFormatedString();
            LogSummary = history.LogSummary;

            if (history.ParentMaps != null && history.ParentMaps.Count > 0)
            {
                List<SimpleOutputModel> parents = new List<SimpleOutputModel>();
                foreach (TrainingHistoryParentMap parentMap in history.ParentMaps)
                {
                    parents.Add(new SimpleOutputModel(parentMap.Parent));
                }
                Parents = parents;
            }

            Node = history.Node;

            EntryPoint = history.EntryPoint;

            Cpu = history.Cpu;
            Memory = history.Memory;
            Gpu = history.Gpu;
            Partition = history.Partition;

            Zip = history.Zip;
            LocalDataSet = history.LocalDataSet;

            foreach (var option in history.GetOptionDic())
            {
                Options.Add(new KeyValuePair<string, string>(option.Key, option.Value));
            }

            foreach(var port in history.GetPortList())
            {
                Ports.Add(port);
            }

            // 待機時間と実行時間の設定
            setWaitingAndExecutionTimes(history);
        }

        /// <summary>
        /// コンテナ名になる一意識別文字列
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 学習モデルGit情報
        /// </summary>
        public GitCommitOutputModel GitModel { get; set; }

        /// <summary>
        /// オプション。
        /// ViewModelではDictionaryを使わないという規約のため、KVPのリストで返す。
        /// </summary>
        public List<KeyValuePair<string, string>> Options { get; set; }

        /// <summary>
        /// コンテナイメージ
        /// </summary>
        public ContainerImageOutputModel ContainerImage { get; set; }

        /// <summary>
        /// 親学習履歴情報。
        /// </summary>
        /// <remarks>
        /// <see cref="IndexOutputModel"/>にするとDataSetの再問い合わせが必要になるため、簡略化
        /// </remarks>
        public List<SimpleOutputModel> Parents { get; set; }

        /// <summary>
        /// 完了日時
        /// </summary>
        public string CompletedAt { get; set; }

        /// <summary>
        /// 開始日時
        /// </summary>
        public string StartedAt { get; set; }

        /// <summary>
        /// コンテナが実行されたノード名
        /// </summary>
        public string Node { get; set; }

        /// <summary>
        /// ログ要約
        /// </summary>
        public string LogSummary { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public int Cpu { get; set; }

        /// <summary>
        /// メモリ容量（GiB）
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        public int Gpu { get; set; }

        /// <summary>
        /// パーティション
        /// </summary>
        public string Partition { get; set; }

        /// <summary>
        /// 開放ポート番号
        /// </summary>
        public List<int> Ports { get; set; }

        /// <summary>
        /// TargetPortとNodePortのペア
        /// </summary>
        public List<KeyValuePair<string, string>> NodePorts { get; set; }


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
        /// コンテナの状態に対する注釈。何か異常が発生している際は注釈が表示される。
        /// </summary>
        public string ConditionNote { get; set; }

        /// <summary>
        /// 待機時間
        /// status 
        /// </summary>
        public string WaitingTime { get; set; }

        /// <summary>
        /// 実行時間
        /// </summary>
        public string ExecutionTime { get; set;  }

        /// <summary>
        /// zip圧縮するか否か。
        /// true：zip圧縮する　false：zip圧縮しない
        /// </summary>
        public bool Zip { get; set; }

        /// <summary>
        /// データセットをローカルコピーするか否か。
        /// true：ローカルコピーする　false：ローカルコピーしない(シンボリックリンクを作成する)
        /// </summary>
        public bool LocalDataSet { get; set; }

        /// <summary>
        /// 引数 TrainingHistory history の属性 CreatedAt/StartedA/CompletedAt の値に従い、
        ///   待機時間(WaitingTime)と実行時間(ExecutionTime)を設定する。
        /// StartedAt == null 時におては、CompletedAt == null なら Pending 中、
        ///   CompletedAt != null なら Pending 中にキャンセルしたと判定する。
        /// 
        /// 状態                                      | 待機時間                | 実行時間
        /// StartedAt == null and CompletedAt == null | 現時刻      - CreatedAt | null
        /// StartedAt == null and CompletedAt != null | CompletedAt - CreatedAt | null
        /// CompletedAt == null                       | StartedAt   - CreatedAt | 現時刻      - StartedAt
        /// その他(両方とも値有り)                    | StartedAt   - CreatedAt | CompletedAt - StartedAt
        /// 
        /// </summary>
        /// <param name="history">学習履歴</param>
        private void setWaitingAndExecutionTimes(TrainingHistory history)
        {
            if (history.StartedAt == null)
            {
                if (history.CompletedAt == null)
                {
                    // 学習コンテナの起動前 (すなわち Pending 中)
                    WaitingTime = getElapsedTime(DateTime.Now, history.CreatedAt);
                    ExecutionTime = null;
                }
                else
                {
                    // 学習コンテナの起動前(Pending 中)にジョブをキャンセルした場合
                    WaitingTime = getElapsedTime(history.CompletedAt, history.CreatedAt);
                    ExecutionTime = null;
                }
            }
            else if (history.CompletedAt == null)
            {
                // 学習コンテナの起動中
                WaitingTime = getElapsedTime(history.StartedAt, history.CreatedAt);
                ExecutionTime = getElapsedTime(DateTime.Now, history.StartedAt);
            }
            else
            {
                // 学習コンテナの起動完了
                WaitingTime = getElapsedTime(history.StartedAt, history.CreatedAt);
                ExecutionTime = getElapsedTime(history.CompletedAt, history.StartedAt);
            }
        }

        /// <summary>
        /// DateTime の差の時刻を文字列で返却 (DateTime が null なら結果も null)
        /// </summary>
        /// <param name="finshedTime">終了時刻</param>
        /// <param name="startingTime">開始時刻</param>
        private string getElapsedTime(DateTime? finshedTime, DateTime? startingTime)
        {
            if (finshedTime == null || startingTime == null)
            {
                // 時刻が null なら結果も null
                return null;
            }
            TimeSpan span = finshedTime.Value - startingTime.Value;
            return span.ToString(@"%d'd '%h'h '%m'm'", CultureInfo.InvariantCulture);
        }
    }
}

using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.ApiModels.TrainingApiModels;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.InferenceApiModels
{
    /// <summary>
    /// 推論履歴の詳細情報モデル
    /// </summary>
    public class InferenceDetailsOutputModel : InferenceIndexOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="history">推論履歴</param>
        public InferenceDetailsOutputModel(InferenceHistory history) : base(history)
        {
            Key = history.Key;
            Options = new List<KeyValuePair<string, string>>();
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
                List<IndexOutputModel> parents = new List<IndexOutputModel>();
                foreach (InferenceHistoryParentMap parentMap in history.ParentMaps)
                {
                    parents.Add(new IndexOutputModel(parentMap.Parent));
                }
                Parents = parents;
            }
            if (history.ParentInferenceMaps != null)
            {
                Inferences = new List<InferenceApiModels.InferenceIndexOutputModel>();
                foreach (var parentInferenceMap in history.ParentInferenceMaps)
                {
                    var parentInference = new InferenceApiModels.InferenceIndexOutputModel(parentInferenceMap.Parent);
                    Inferences.Add(parentInference);
                }
                Inferences.Sort(delegate (InferenceApiModels.InferenceIndexOutputModel parentInference1, InferenceApiModels.InferenceIndexOutputModel parentInference2)
                {
                    return parentInference1.Id.CompareTo(parentInference2.Id);
                });
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
        public List<IndexOutputModel> Parents { get; set; }

        /// <summary>
        /// 親推論履歴
        /// </summary>
        public List<InferenceApiModels.InferenceIndexOutputModel> Inferences { get; set; }

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
        /// メモリ容量（GB）
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
        /// zip圧縮するか否か。
        /// true：zip圧縮する　false：zip圧縮しない
        /// </summary>
        public bool Zip { get; set; }

        /// <summary>
        /// データセットをローカルコピーするか否か。
        /// true：ローカルコピーする　false：ローカルコピーしない(シンボリックリンクを作成する)
        /// </summary>
        public bool LocalDataSet { get; set; }
    }

}

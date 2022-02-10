namespace Nssol.Platypus.LogicModels
{
    /// <summary>
    /// 評価用コンテナ起動情報一覧
    /// </summary>
    public class EvalContainerModel
    {
        /// <summary>
        /// コンテナのID。他のプロパティ値から自動生成される。
        /// </summary>
        public string ContainerId
        {
            get
            {
                return $"eval-{TrainingHistoryId}-from-{DataSetIdForEval}";
            }
        }

        /// <summary>
        /// 評価に使うデータセットのID
        /// </summary>
        public long DataSetIdForEval { get; set; }

        /// <summary>
        /// 学習履歴ID
        /// </summary>
        public long TrainingHistoryId { get; set; }

        /// <summary>
        /// 取得対象のスクリプトのコミットID
        /// </summary>
        public string CommitId { get; set; }

        /// <summary>
        /// 評価モデルのスクリプトに渡す引数
        /// </summary>
        public string Argument { get; set; }

        /// <summary>
        /// スクリプトのエントリポイント
        /// </summary>
        public string EntryPoint { get; set; }

        /// <summary>
        /// 取得対象のスクリプトのリポジトリ
        /// </summary>
        public string Repository { get; set; }

        /// <summary>
        /// コンテナイメージ名
        /// </summary>
        public string ContainerImage { get; set; }

        /// <summary>
        /// CPU利用数（最大値）
        /// </summary>
        public int Cpu { get; set; }

        /// <summary>
        /// メモリ容量
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// GPU利用数
        /// </summary>
        public int Gpu { get; set; }

        /// <summary>
        /// GPUの型番
        /// </summary>
        public string GpuType { get; set; }
    }
}

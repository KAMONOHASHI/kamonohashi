using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// テンプレートバージョン作成の入力モデル
    /// </summary>
    public class VersionCreateInputModel
    {
        #region 前処理コンテナ
        /// <summary>
        /// エントリポイント
        /// </summary>
        public string PreprocessEntryPoint { get; set; }

        /// <summary>
        /// コンテナ情報
        /// </summary>
        public ContainerImageInputModel PreprocessContainerImage { get; set; }

        /// <summary>
        /// 前処理ソースコードGit情報
        /// </summary>
        public GitCommitNullableInputModel PreprocessGitModel { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public int PreprocessCpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）
        /// </summary>
        public int PreprocessMemory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        public int PreprocessGpu { get; set; }
        #endregion

        #region 学習コンテナ
        /// <summary>
        /// エントリポイント
        /// </summary>
        [Required]
        public string TrainingEntryPoint { get; set; }

        /// <summary>
        /// コンテナ情報
        /// </summary>
        [Required]
        public ContainerImageInputModel TrainingContainerImage { get; set; }

        /// <summary>
        /// ソースコードGit情報
        /// </summary>
        [Required]
        public GitCommitNullableInputModel TrainingGitModel { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        [Required]
        public int TrainingCpu { get; set; }

        /// <summary>
        /// メモリ容量
        /// </summary>
        [Required]
        public int TrainingMemory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        [Required]
        public int TrainingGpu { get; set; }
        #endregion

        #region 評価コンテナ
        /// <summary>
        /// エントリポイント
        /// </summary>
        public string EvaluationEntryPoint { get; set; }

        /// <summary>
        /// コンテナ情報
        /// </summary>
        public ContainerImageInputModel EvaluationContainerImage { get; set; }

        /// <summary>
        /// ソースコードGit情報
        /// </summary>
        public GitCommitNullableInputModel EvaluationGitModel { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public int EvaluationCpu { get; set; }

        /// <summary>
        /// メモリ容量
        /// </summary>
        public int EvaluationMemory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        public int EvaluationGpu { get; set; }
        #endregion
    }
}

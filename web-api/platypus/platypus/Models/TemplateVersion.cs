using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// テンプレートバージョン
    /// </summary>
    public class TemplateVersion : ModelBase
    {
        /// <summary>
        /// テンプレートID
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        /// テンプレート
        /// </summary>
        [ForeignKey(nameof(TemplateId))]
        public Template Template { get; set; }

        /// <summary>
        /// バージョン番号
        /// </summary>
        public long Version { get; set; }

        #region 前処理コンテナ
        /// <summary>
        /// エントリポイント
        /// </summary>
        public string PreprocessEntryPoint { get; set; }

        /// <summary>
        /// Git
        /// </summary>
        public long? PreprocessRepositoryGitId { get; set; }

        /// <summary>
        /// リポジトリ名
        /// </summary>
        public string PreprocessRepositoryName { get; set; }

        /// <summary>
        /// リポジトリオーナー
        /// </summary>
        public string PreprocessRepositoryOwner { get; set; }

        /// <summary>
        /// ブランチ
        /// </summary>
        public string PreprocessRepositoryBranch { get; set; }

        /// <summary>
        /// コミットID
        /// </summary>
        public string PreprocessRepositoryCommitId { get; set; }

        /// <summary>
        /// Gitトークン
        /// </summary>
        public string PreprocessRepositoryToken { get; set; }

        /// <summary>
        /// Dockerリポジトリ
        /// </summary>
        public long? PreprocessContainerRegistryId { get; set; }

        /// <summary>
        /// コンテナ
        /// </summary>
        public string PreprocessContainerImage { get; set; }

        /// <summary>
        /// コンテナタグ（＝バージョン）
        /// </summary>
        public string PreprocessContainerTag { get; set; }

        /// <summary>
        /// レジストリトークン
        /// </summary>
        public string PreprocessContainerToken { get; set; }

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

        /// <summary>
        /// Git
        /// </summary>
        [ForeignKey(nameof(PreprocessRepositoryGitId))]
        public Git PreprocessRepositoryGit { get; set; }

        /// <summary>
        /// コンテナレジストリ
        /// </summary>
        [ForeignKey(nameof(PreprocessContainerRegistryId))]
        public Registry PreprocessContainerRegistry { get; set; }
        #endregion

        #region 学習コンテナ用
        /// <summary>
        /// エントリポイント
        /// </summary>
        [Required]
        public string TrainingEntryPoint { get; set; }

        /// <summary>
        /// Git
        /// </summary>
        public long TrainingRepositoryGitId { get; set; }

        /// <summary>
        /// リポジトリ名
        /// </summary>
        [Required]
        public string TrainingRepositoryName { get; set; }

        /// <summary>
        /// リポジトリオーナー
        /// </summary>
        [Required]
        public string TrainingRepositoryOwner { get; set; }

        /// <summary>
        /// ブランチ
        /// </summary>
        [Required]
        public string TrainingRepositoryBranch { get; set; }

        /// <summary>
        /// コミットID
        /// </summary>
        [Required]
        public string TrainingRepositoryCommitId { get; set; }

        /// <summary>
        /// Gitトークン
        /// </summary>
        public string TrainingRepositoryToken { get; set; }

        /// <summary>
        /// Dockerリポジトリ
        /// </summary>
        public long TrainingContainerRegistryId { get; set; }

        /// <summary>
        /// コンテナ
        /// </summary>
        [Required]
        public string TrainingContainerImage { get; set; }

        /// <summary>
        /// コンテナタグ（＝バージョン）
        /// </summary>
        [Required]
        public string TrainingContainerTag { get; set; }

        /// <summary>
        /// レジストリトークン
        /// </summary>
        public string TrainingContainerToken { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public int TrainingCpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）
        /// </summary>
        public int TrainingMemory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        public int TrainingGpu { get; set; }

        /// <summary>
        /// Git
        /// </summary>
        [ForeignKey(nameof(TrainingRepositoryGitId))]
        public Git TrainingRepositoryGit { get; set; }

        /// <summary>
        /// コンテナレジストリ
        /// </summary>
        [ForeignKey(nameof(TrainingContainerRegistryId))]
        public Registry TrainingContainerRegistry { get; set; }
        #endregion

        #region 評価コンテナ用
        /// <summary>
        /// エントリポイント
        /// </summary>
        public string EvaluationEntryPoint { get; set; }

        /// <summary>
        /// Git
        /// </summary>
        public long? EvaluationRepositoryGitId { get; set; }

        /// <summary>
        /// リポジトリ名
        /// </summary>
        public string EvaluationRepositoryName { get; set; }

        /// <summary>
        /// リポジトリオーナー
        /// </summary>
        public string EvaluationRepositoryOwner { get; set; }

        /// <summary>
        /// ブランチ
        /// </summary>
        public string EvaluationRepositoryBranch { get; set; }

        /// <summary>
        /// コミットID
        /// </summary>
        public string EvaluationRepositoryCommitId { get; set; }

        /// <summary>
        /// Gitトークン
        /// </summary>
        public string EvaluationRepositoryToken { get; set; }

        /// <summary>
        /// Dockerリポジトリ
        /// </summary>
        public long? EvaluationContainerRegistryId { get; set; }

        /// <summary>
        /// コンテナ
        /// </summary>
        public string EvaluationContainerImage { get; set; }

        /// <summary>
        /// コンテナタグ（＝バージョン）
        /// </summary>
        public string EvaluationContainerTag { get; set; }

        /// <summary>
        /// レジストリトークン
        /// </summary>
        public string EvaluationContainerToken { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public int EvaluationCpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）
        /// </summary>
        public int EvaluationMemory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        public int EvaluationGpu { get; set; }

        /// <summary>
        /// Git
        /// </summary>
        [ForeignKey(nameof(EvaluationRepositoryGitId))]
        public Git EvaluationRepositoryGit { get; set; }

        /// <summary>
        /// コンテナレジストリ
        /// </summary>
        [ForeignKey(nameof(EvaluationContainerRegistryId))]
        public Registry EvaluationContainerRegistry { get; set; }
        #endregion
    }
}

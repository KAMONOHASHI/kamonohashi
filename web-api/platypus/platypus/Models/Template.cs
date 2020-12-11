using Nssol.Platypus.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// テンプレート
    /// </summary>
    public class ModelTemplate : ModelBase
    {
        /// <summary>
        /// 表示用ID
        /// </summary>
        public long? DisplayId { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// バージョン
        /// </summary>
        public long? Version { get; set; }


        /// <summary>
        /// グループID
        /// </summary>
        public long? GroupId { get; set; }

        /// <summary>
        /// 公開設定
        /// </summary>
        [Required]
        public TemplateAccessLevel AccessLevel { get; set; }


        #region 前処理コンテナ
        /// <summary>
        /// エントリポイント。
        /// </summary>
        public string PreprocessEntryPoint { get; set; }

        /// <summary>
        /// Git
        /// </summary>
        /// <remarks>前処理はGitを必須としない</remarks>
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
        /// ブランチ。
        /// </summary>
        public string PreprocessRepositoryBranch { get; set; }

        /// <summary>
        /// コミットID
        /// </summary>
        public string PreprocessRepositoryCommitId { get; set; }

        /// <summary>
        /// Dockerリポジトリ。
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
        /// CPUコア数のデフォルト値
        /// </summary>
        public int PreprocessCpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）のデフォルト値
        /// </summary>
        public int PreprocessMemory { get; set; }

        /// <summary>
        /// GPU数のデフォルト値
        /// </summary>
        public int PreprocessGpu { get; set; }

        /// <summary>
        /// コンテナレジストリ
        /// </summary>
        [ForeignKey(nameof(PreprocessContainerRegistryId))]
        public virtual Registry PreprocessContainerRegistry { get; set; }
        #endregion

        #region 学習コンテナ用
        /// <summary>
        /// エントリポイント。
        /// </summary>
        public string TrainingEntryPoint { get; set; }

        /// <summary>
        /// Git
        /// </summary>
        /// <remarks>前処理はGitを必須としない</remarks>
        public long? TrainingRepositoryGitId { get; set; }

        /// <summary>
        /// リポジトリ名
        /// </summary>
        public string TrainingRepositoryName { get; set; }

        /// <summary>
        /// リポジトリオーナー
        /// </summary>
        public string TrainingRepositoryOwner { get; set; }

        /// <summary>
        /// ブランチ。
        /// </summary>
        public string TrainingRepositoryBranch { get; set; }

        /// <summary>
        /// コミットID
        /// </summary>
        public string TrainingRepositoryCommitId { get; set; }

        /// <summary>
        /// Dockerリポジトリ。
        /// </summary>
        public long? TrainingContainerRegistryId { get; set; }

        /// <summary>
        /// コンテナ
        /// </summary>
        public string TrainingContainerImage { get; set; }

        /// <summary>
        /// コンテナタグ（＝バージョン）
        /// </summary>
        public string TrainingContainerTag { get; set; }

        /// <summary>
        /// CPUコア数のデフォルト値
        /// </summary>
        public int TrainingCpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）のデフォルト値
        /// </summary>
        public int TrainingMemory { get; set; }

        /// <summary>
        /// GPU数のデフォルト値
        /// </summary>
        public int TrainingGpu { get; set; }

        
        /// <summary>
        /// コンテナレジストリ
        /// </summary>
        [ForeignKey(nameof(TrainingContainerRegistryId))]
        public virtual Registry TrainingContainerRegistry { get; set; }
        #endregion
    }
}

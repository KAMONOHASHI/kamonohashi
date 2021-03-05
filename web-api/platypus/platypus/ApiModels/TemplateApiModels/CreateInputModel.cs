using Nssol.Platypus.Infrastructure;
using System.Collections.Generic;
using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.Controllers.Util;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// テンプレート作成の入力モデル
    /// </summary>
    public class CreateInputModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        /// </remarks>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// アクセスレベル
        /// </summary>
        [Required]
        public TemplateAccessLevel? AccessLevel { get; set; }

        /// <summary>
        /// バージョン
        /// </summary>
        [Required]
        public long? Version { get; set; }


        /// <summary>
        /// グループID
        /// </summary>
        public long? GroupId { get; set; }

        /// <summary>
        /// このテンプレートを使用できるテナントのID。
        /// <see cref="AccessLevel"/>が<see cref="TemplateAccessLevel.Private"/>の時のみ指定可能。
        /// それ以外の場合は無視される。
        /// </summary>
        public long? AssignedTenantId { get; set; }

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
        #endregion

        #region 学習・推論コンテナ
        /// <summary>
        /// エントリポイント
        /// </summary>
        public string TrainingEntryPoint { get; set; }

        /// <summary>
        /// コンテナ情報
        /// </summary>
        public ContainerImageInputModel TrainingContainerImage { get; set; }

        /// <summary>
        /// 前処理ソースコードGit情報
        /// </summary>
        public GitCommitNullableInputModel TrainingGitModel { get; set; }

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
        #endregion
    }
}

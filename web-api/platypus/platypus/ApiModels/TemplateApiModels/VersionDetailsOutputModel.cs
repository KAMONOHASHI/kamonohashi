using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    public class VersionDetailsOutputModel : VersionIndexOutputModel
    {
        public VersionDetailsOutputModel(TemplateVersion templateVersion) : base(templateVersion)
        {
            PreprocessEntryPoint = templateVersion.PreprocessEntryPoint;
            if (templateVersion.PreprocessRepositoryGitId != null)
            {
                PreprocessGitModel = new GitCommitOutputModel()
                {
                    GitId = templateVersion.PreprocessRepositoryGitId,
                    Repository = templateVersion.PreprocessRepositoryName,
                    Owner = templateVersion.PreprocessRepositoryOwner,
                    Branch = templateVersion.PreprocessRepositoryBranch,
                    CommitId = templateVersion.PreprocessRepositoryCommitId
                };
            }
            if (!string.IsNullOrEmpty(templateVersion.PreprocessContainerImage))
            {
                PreprocessContainerImage = new ContainerImageOutputModel()
                {
                    RegistryId = templateVersion.PreprocessContainerRegistryId,
                    RegistryName = templateVersion.PreprocessContainerRegistry.Name,
                    Image = templateVersion.PreprocessContainerImage,
                    Tag = templateVersion.PreprocessContainerTag
                };
            }
            PreprocessCpu = templateVersion.PreprocessCpu;
            PreprocessMemory = templateVersion.PreprocessMemory;
            PreprocessGpu = templateVersion.PreprocessGpu;

            TrainingEntryPoint = templateVersion.TrainingEntryPoint;
            TrainingGitModel = new GitCommitOutputModel()
            {
                GitId = templateVersion.TrainingRepositoryGitId,
                Repository = templateVersion.TrainingRepositoryName,
                Owner = templateVersion.TrainingRepositoryOwner,
                Branch = templateVersion.TrainingRepositoryBranch,
                CommitId = templateVersion.TrainingRepositoryCommitId
            };
            TrainingContainerImage = new ContainerImageOutputModel()
            {
                RegistryId = templateVersion.TrainingContainerRegistryId,
                RegistryName = templateVersion.TrainingContainerRegistry.Name,
                Image = templateVersion.TrainingContainerImage,
                Tag = templateVersion.TrainingContainerTag
            };
            TrainingCpu = templateVersion.TrainingCpu;
            TrainingMemory = templateVersion.TrainingMemory;
            TrainingGpu = templateVersion.TrainingGpu;

            EvaluationEntryPoint = templateVersion.EvaluationEntryPoint;
            if (templateVersion.EvaluationRepositoryGitId != null)
            {
                EvaluationGitModel = new GitCommitOutputModel()
                {
                    GitId = templateVersion.EvaluationRepositoryGitId,
                    Repository = templateVersion.EvaluationRepositoryName,
                    Owner = templateVersion.EvaluationRepositoryOwner,
                    Branch = templateVersion.EvaluationRepositoryBranch,
                    CommitId = templateVersion.EvaluationRepositoryCommitId
                };
            }
            if (!string.IsNullOrEmpty(templateVersion.EvaluationContainerImage))
            {
                EvaluationContainerImage = new ContainerImageOutputModel()
                {
                    RegistryId = templateVersion.EvaluationContainerRegistryId,
                    RegistryName = templateVersion.EvaluationContainerRegistry.Name,
                    Image = templateVersion.EvaluationContainerImage,
                    Tag = templateVersion.EvaluationContainerTag
                };
            }
            EvaluationCpu = templateVersion.EvaluationCpu;
            EvaluationMemory = templateVersion.EvaluationMemory;
            EvaluationGpu = templateVersion.EvaluationGpu;
        }

        #region 前処理
        /// <summary>
        /// エントリポイント
        /// </summary>
        public string PreprocessEntryPoint { get; set; }

        /// <summary>
        /// ソースコードGit情報
        /// </summary>
        public GitCommitOutputModel PreprocessGitModel { get; set; }

        /// <summary>
        /// コンテナイメージ
        /// </summary>
        public ContainerImageOutputModel PreprocessContainerImage { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public int? PreprocessCpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）
        /// </summary>
        public int? PreprocessMemory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        public int? PreprocessGpu { get; set; }
        #endregion

        #region 学習
        /// <summary>
        /// エントリポイント
        /// </summary>
        public string TrainingEntryPoint { get; set; }

        /// <summary>
        /// ソースコードGit情報
        /// </summary>
        public GitCommitOutputModel TrainingGitModel { get; set; }

        /// <summary>
        /// コンテナイメージ
        /// </summary>
        public ContainerImageOutputModel TrainingContainerImage { get; set; }

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
        #endregion

        #region 評価
        /// <summary>
        /// エントリポイント
        /// </summary>
        public string EvaluationEntryPoint { get; set; }

        /// <summary>
        /// ソースコードGit情報
        /// </summary>
        public GitCommitOutputModel EvaluationGitModel { get; set; }

        /// <summary>
        /// コンテナイメージ
        /// </summary>
        public ContainerImageOutputModel EvaluationContainerImage { get; set; }

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
        #endregion

    }
}

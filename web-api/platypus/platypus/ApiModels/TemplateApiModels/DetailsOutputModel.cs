using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.ApiModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(ModelTemplate template) : base(template)
        {
            if (template.PreprocessRepositoryGitId != null)
            {
                PreprocessGitModel = new GitCommitOutputModel()
                {
                    GitId = template.PreprocessRepositoryGitId,
                    Repository = template.PreprocessRepositoryName,
                    Owner = template.PreprocessRepositoryOwner,
                    Branch = template.PreprocessRepositoryBranch,
                    CommitId = template.PreprocessRepositoryCommitId
                };
            }
            if (!string.IsNullOrEmpty(template.PreprocessContainerImage) && !string.IsNullOrEmpty(template.PreprocessContainerTag))
            {
                PreprocessContainerImage = new ContainerImageOutputModel()
                {
                    RegistryId = template.PreprocessContainerRegistryId,
                    RegistryName = template.PreprocessContainerRegistry.Name,
                    Image = template.PreprocessContainerImage,
                    Tag = template.PreprocessContainerTag
                };
            }
            PreprocessEntryPoint = template.PreprocessEntryPoint;

            if (template.TrainingRepositoryGitId != null)
            {
                TrainingGitModel = new GitCommitOutputModel()
                {
                    GitId = template.TrainingRepositoryGitId,
                    Repository = template.TrainingRepositoryName,
                    Owner = template.TrainingRepositoryOwner,
                    Branch = template.TrainingRepositoryBranch,
                    CommitId = template.TrainingRepositoryCommitId
                };
            }
            if (!string.IsNullOrEmpty(template.TrainingContainerImage) && !string.IsNullOrEmpty(template.TrainingContainerTag))
            {
                TrainingContainerImage = new ContainerImageOutputModel()
                {
                    RegistryId = template.TrainingContainerRegistryId,
                    RegistryName = template.TrainingContainerRegistry.Name,
                    Image = template.TrainingContainerImage,
                    Tag = template.TrainingContainerTag
                };
            }
            TrainingEntryPoint = template.TrainingEntryPoint;
            PreprocessCpu = template.PreprocessCpu;
            PreprocessMemory = template.PreprocessMemory;
            PreprocessGpu = template.PreprocessGpu;
            TrainingCpu = template.TrainingCpu;
            TrainingMemory = template.TrainingMemory;
            TrainingGpu = template.TrainingGpu;
        }
        #region 前処理
        /// <summary>
        /// ソースコードGit情報
        /// </summary>
        public GitCommitOutputModel PreprocessGitModel { get; set; }
        /// <summary>
        /// コンテナイメージ
        /// </summary>
        public ContainerImageOutputModel PreprocessContainerImage { get; set; }

        /// <summary>
        /// エントリポイント。
        /// </summary>
        public string PreprocessEntryPoint { get; set; }
        #endregion

        #region 前処理
        /// <summary>
        /// ソースコードGit情報
        /// </summary>
        public GitCommitOutputModel TrainingGitModel { get; set; }
        /// <summary>
        /// コンテナイメージ
        /// </summary>
        public ContainerImageOutputModel TrainingContainerImage { get; set; }

        /// <summary>
        /// エントリポイント。
        /// </summary>
        public string TrainingEntryPoint { get; set; }
        #endregion
        /// <summary>
        /// <see cref="IndexOutputModel.AccessLevel"/>が<see cref="TemplateAccessLevel.Private"/>の時、このノードを使用できるテナントの一覧。
        /// </summary>
        public IEnumerable<AssignedTenant> AssignedTenants { get; set; }

        public class AssignedTenant
        {
            /// <summary>
            /// テナントID
            /// </summary>
            public long Id { get; set; }
            /// <summary>
            /// テナント名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// テナント表示名
            /// </summary>
            public string DisplayName { get; set; }


        }

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


    }
}

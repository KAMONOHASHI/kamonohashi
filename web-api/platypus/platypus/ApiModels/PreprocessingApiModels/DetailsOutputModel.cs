using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.ApiModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.PreprocessingApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Preprocess preprocessing) : base(preprocessing)
        {
            if (preprocessing.RepositoryGitId != null)
            {
                GitModel = new GitCommitOutputModel()
                {
                    GitId = preprocessing.RepositoryGitId,
                    Repository = preprocessing.RepositoryName,
                    Owner = preprocessing.RepositoryOwner,
                    Branch = preprocessing.RepositoryBranch,
                    CommitId = preprocessing.RepositoryCommitId
                };
            }
            if (!string.IsNullOrEmpty(preprocessing.ContainerImage) && !string.IsNullOrEmpty(preprocessing.ContainerTag))
            {
                ContainerImage = new ContainerImageOutputModel()
                {
                    RegistryId = preprocessing.ContainerRegistryId,
                    RegistryName = preprocessing.ContainerRegistry.Name,
                    Image = preprocessing.ContainerImage,
                    Tag = preprocessing.ContainerTag
                };
            }
            EntryPoint = preprocessing.EntryPoint;
        }
        /// <summary>
        /// ソースコードGit情報
        /// </summary>
        public GitCommitOutputModel GitModel { get; set; }
        /// <summary>
        /// コンテナイメージ
        /// </summary>
        public ContainerImageOutputModel ContainerImage { get; set; }

        /// <summary>
        /// エントリポイント。
        /// </summary>
        public string EntryPoint { get; set; }

        /// <summary>
        /// 実行済みか
        /// </summary>
        public bool IsLocked { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.ApiModels.ExperimentApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Models.TenantModels.Aquarium;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// 実験API
    /// </summary>
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/experiment")]
    public class ExperimentController : PlatypusApiControllerBase
    {
        private readonly IExperimentRepository experimentRepository;
        private readonly IExperimentPreprocessRepository experimentPreprocessRepository;
        private readonly IAquariumDataSetRepository aquariumDataSetRepository;
        private readonly IDataSetRepository dataSetRepository;
        private readonly IGitRepository gitRepository;
        private readonly IRegistryRepository registryRepository;
        private readonly ITemplateRepository templateRepository;
        private readonly ITemplateVersionRepository templateVersionRepository;
        private readonly ITenantRepository tenantRepository;
        private readonly ITemplateLogic templateLogic;
        private readonly ITagLogic tagLogic;
        private readonly INodeRepository nodeRepository;
        private readonly IGitLogic gitLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IRegistryLogic registryLogic;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITrainingHistoryRepository trainingHistoryRepository;

        public ExperimentController(
            IExperimentRepository experimentRepository,
            IExperimentPreprocessRepository experimentPreprocessRepository,
            IAquariumDataSetRepository aquariumDataSetRepository,
            IDataSetRepository dataSetRepository,
            IGitRepository gitRepository,
            IRegistryRepository registryRepository,
            ITrainingHistoryRepository trainingHistoryRepository,
            ITemplateRepository templateRepository,
            ITemplateVersionRepository templateVersionRepository,
            ITenantRepository tenantRepository,
            INodeRepository nodeRepository,
            ITemplateLogic templateLogic,
            ITagLogic tagLogic,
            IGitLogic gitLogic,
            IClusterManagementLogic clusterManagementLogic,
            IRegistryLogic registryLogic,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.trainingHistoryRepository = trainingHistoryRepository;
            this.experimentRepository = experimentRepository;
            this.experimentPreprocessRepository = experimentPreprocessRepository;
            this.aquariumDataSetRepository = aquariumDataSetRepository;
            this.dataSetRepository = dataSetRepository;
            this.gitRepository = gitRepository;
            this.registryRepository = registryRepository;
            this.templateRepository = templateRepository;
            this.templateVersionRepository = templateVersionRepository;
            this.tenantRepository = tenantRepository;
            this.nodeRepository = nodeRepository;
            this.templateLogic = templateLogic;
            this.tagLogic = tagLogic;
            this.gitLogic = gitLogic;
            this.clusterManagementLogic = clusterManagementLogic;
            this.registryLogic = registryLogic;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 指定された条件でページングされた状態で、実験一覧を取得する
        /// </summary>
        /// <param name="page">ページ番号。デフォルトは1。</param>
        /// <param name="perPage">表示件数。指定がない場合は上限(1000件)。</param>
        /// <param name="filter">検索条件</param>
        [HttpGet]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] SearchInputModel filter, [FromQuery] int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            //未指定、あるいは1000件以上であれば、1000件に指定
            var pageCount = (perPage.HasValue && perPage.Value < 1000) ? perPage.Value : 1000;

            var experiments = experimentRepository
                .GetAllWithOrderby(x => x.Id, false)
                .Include(x => x.Template)
                .Include(x => x.DataSet)
                .Include(x => x.ExperimentPreprocess).ThenInclude(x => x.TrainingHistory).ThenInclude(x => x.DataSet)
                .Include(x => x.TrainingHistory).ThenInclude(x => x.DataSet)
                .SearchLong(d => d.Id, filter.Id)
                .SearchString(d => d.Name, filter.Name)
                .SearchTime(d => d.CreatedAt, filter.StartedAt);

            if (withTotal)
            {
                SetTotalCountToHeader(experiments.Count());
            }

            var result = new List<IndexOutputModel>();
            foreach (var x in experiments.Paging(page, pageCount))
            {
                var preprocessStatus = await UpdateStatus(x.ExperimentPreprocess?.TrainingHistory);
                var trainingStatus = await UpdateStatus(x.TrainingHistory);
                var status = GetExperimentStatus(preprocessStatus, trainingStatus);
                result.Add(new IndexOutputModel(x, status.ToString()));
            }

            return JsonOK(result);
        }

        /// <summary>
        /// エンドユーザ表示用の実験ステータスを作成する
        /// </summary>
        private static ContainerStatus GetExperimentStatus(ContainerStatus preprocessStatus, ContainerStatus trainingStatus)
        {
            if (trainingStatus.Type == ContainerStatus.ContainerStatusType.None
                && (preprocessStatus.Type == ContainerStatus.ContainerStatusType.Failed
                || preprocessStatus.Type == ContainerStatus.ContainerStatusType.Error))
            {
                return preprocessStatus;
            }
            return trainingStatus;
        }

        /// <summary>
        /// kamonohashi学習のステータスを更新し、最新ステータスを返す
        /// </summary>
        private async Task<ContainerStatus> UpdateStatus(TrainingHistory history)
            => history != null
            ? await TrainingController.DoGetUpdatedIndexOutputModelAsync(history,
                    clusterManagementLogic, CurrentUserInfo, trainingHistoryRepository, unitOfWork)
            : ContainerStatus.None;

        /// <summary>
        /// 実験を取得する
        /// </summary>
        /// <param name="id">実験ID</param>
        [HttpGet("{id}")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long id)
        {
            var experiment = await experimentRepository
                .GetAll()
                .Include(x => x.Template)
                .Include(x => x.TemplateVersion)
                .Include(x => x.DataSet)
                .Include(x => x.DataSetVersion)
                .Include(x => x.TrainingHistory).ThenInclude(x => x.DataSet)
                .Include(x => x.ExperimentPreprocess).ThenInclude(x => x.TrainingHistory).ThenInclude(x => x.DataSet)
                .SingleOrDefaultAsync(x => x.Id == id); 
            if (experiment == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }

            var preprocessStatus = await UpdateStatus(experiment.ExperimentPreprocess?.TrainingHistory);
            var trainingStatus = await UpdateStatus(experiment.TrainingHistory);
            var status = GetExperimentStatus(preprocessStatus, trainingStatus);
            var model = new DetailsOutputModel(experiment, status.ToString());

            return JsonOK(model);
        }

        /// <summary>
        /// 前処理なし実験の学習を開始する
        /// </summary>
        private async Task<IActionResult> RunExperimentTrainingWithoutPreprocess(CreateInputModel model,
            TemplateVersion templateVersion, DataSetVersion dataSetVersion)
        {
            var registryTokenKey = await RegistRegistryToTenantAsync(templateVersion.TrainingContainerRegistry, templateVersion.TrainingContainerToken);
            if (registryTokenKey == null)
            {
                return JsonBadRequest("Cannot register registry token");
            }

            var gitToken = templateVersion.TrainingRepositoryToken ?? UserGitToken(templateVersion.TrainingRepositoryGitId);

            // kamonohashi学習に必要な情報を設定
            var trainingCreateInputModel = new ApiModels.TrainingApiModels.CreateInputModel
            {
                Name = model.Name,
                ContainerImage = new ContainerImageInputModel
                {
                    RegistryId = templateVersion.TrainingContainerRegistryId,
                    Image = templateVersion.TrainingContainerImage,
                    Tag = templateVersion.TrainingContainerTag,
                },
                DataSetId = dataSetVersion.DataSetId,
                ParentIds = null,
                GitModel = new GitCommitInputModel
                {
                    GitId = templateVersion.TrainingRepositoryGitId,
                    Repository = templateVersion.TrainingRepositoryName,
                    Owner = templateVersion.TrainingRepositoryOwner,
                    Branch = templateVersion.TrainingRepositoryBranch,
                    CommitId = templateVersion.TrainingRepositoryCommitId,
                },
                EntryPoint = templateVersion.TrainingEntryPoint,
                Options = null,
                Cpu = templateVersion.TrainingCpu,
                Memory = templateVersion.TrainingMemory,
                Gpu = templateVersion.TrainingGpu,
                Partition = null,
                Ports = null,
                Memo = null,
                Tags = null,
                Zip = false,
                LocalDataSet = true,
            };

            // kamonohashi学習を開始
            (var trainingHistory, var result) = await TrainingController.DoCreate(trainingCreateInputModel,
                dataSetRepository, nodeRepository, tenantRepository, trainingHistoryRepository,
                clusterManagementLogic, gitLogic, tagLogic, unitOfWork,
                CurrentUserInfo, ModelState, RequestUrl, "training", registryTokenKey, gitToken);

            // 実験の学習とkamonohashi学習を結び付ける
            if (trainingHistory != null)
            {
                var experiment = new Experiment
                {
                    Name = model.Name,
                    DataSetId = model.DataSetId,
                    DataSetVersionId = model.DataSetVersionId,
                    TemplateId = model.TemplateId,
                    TemplateVersionId = model.TemplateVersionId,
                    TrainingHistoryId = trainingHistory.Id,
                };
                experimentRepository.Add(experiment);
                unitOfWork.Commit();

                ((JsonResult)result).Value = new SimpleOutputModel(experiment);
            }

            return result;
        }

        /// <summary>
        /// 前処理あり実験の前処理を開始する
        /// </summary>
        private async Task<IActionResult> RunExperimentPreprocess(CreateInputModel model,
            TemplateVersion templateVersion, DataSetVersion dataSetVersion)
        {
            var registryTokenKey = await RegistRegistryToTenantAsync(templateVersion.PreprocessContainerRegistry, templateVersion.PreprocessContainerToken);
            if (registryTokenKey == null)
            {
                return JsonBadRequest("Cannot register registry token");
            }

            var gitToken = templateVersion.PreprocessRepositoryToken ?? UserGitToken(templateVersion.PreprocessRepositoryGitId);

            // 実験IDを発行するため、Experimentをここで作成
            var experiment = new Experiment
            {
                Name = model.Name,
                DataSetId = model.DataSetId,
                DataSetVersionId = model.DataSetVersionId,
                TemplateId = model.TemplateId,
                TemplateVersionId = model.TemplateVersionId,
            };
            experimentRepository.Add(experiment);
            unitOfWork.Commit();

            // kamonohashi学習に必要な情報を設定
            var trainingCreateInputModel = new ApiModels.TrainingApiModels.CreateInputModel
            {
                Name = model.Name,
                ContainerImage = new ContainerImageInputModel
                {
                    RegistryId = templateVersion.PreprocessContainerRegistryId,
                    Image = templateVersion.PreprocessContainerImage,
                    Tag = templateVersion.PreprocessContainerTag,
                },
                DataSetId = dataSetVersion.DataSetId,
                ParentIds = null,
                GitModel = new GitCommitInputModel
                {
                    GitId = templateVersion.PreprocessRepositoryGitId,
                    Repository = templateVersion.PreprocessRepositoryName,
                    Owner = templateVersion.PreprocessRepositoryOwner,
                    Branch = templateVersion.PreprocessRepositoryBranch,
                    CommitId = templateVersion.PreprocessRepositoryCommitId,
                },
                EntryPoint = templateVersion.PreprocessEntryPoint,
                Options = new Dictionary<string, string>
                {
                    { "EXPERIMENT_ID", experiment.Id.ToString() },
                },
                Cpu = templateVersion.PreprocessCpu,
                Memory = templateVersion.PreprocessMemory,
                Gpu = templateVersion.PreprocessGpu,
                Partition = null,
                Ports = null,
                Memo = null,
                Tags = null,
                Zip = false,
                LocalDataSet = true,
            };

            // kamonohashi学習を開始
            (var trainingHistory, var result) = await TrainingController.DoCreate(trainingCreateInputModel,
                dataSetRepository, nodeRepository, tenantRepository, trainingHistoryRepository,
                clusterManagementLogic, gitLogic, tagLogic, unitOfWork,
                CurrentUserInfo, ModelState, RequestUrl, "experiment_preproc", registryTokenKey, gitToken);

            // 実験の前処理とkamonohashi学習を結び付ける
            if (trainingHistory != null)
            {
                var experimentPreprocess = new ExperimentPreprocess
                {
                    DataSetId = dataSetVersion.AquariumDataSetId,
                    DataSetVersionId = dataSetVersion.Id,
                    TemplateId = templateVersion.TemplateId,
                    TemplateVersionId = templateVersion.Id,
                    TrainingHistoryId = trainingHistory.Id,
                };
                experimentPreprocessRepository.Add(experimentPreprocess);
                experiment.ExperimentPreprocess = experimentPreprocess;
                experimentRepository.Update(experiment);
                unitOfWork.Commit();

                ((JsonResult)result).Value = new SimpleOutputModel(experiment);
            }
            else
            {
                experimentRepository.Delete(experiment);
                unitOfWork.Commit();
            }

            return result;
        }

        /// <summary>
        /// 前処理あり実験の学習を開始する
        /// </summary>
        private async Task<IActionResult> RunExperimentTrainingWithPreprocess(CreateInputModel model,
            TemplateVersion templateVersion, DataSetVersion dataSetVersion, ExperimentPreprocess experimentPreprocess)
        {
            var registryTokenKey = await RegistRegistryToTenantAsync(templateVersion.TrainingContainerRegistry, templateVersion.TrainingContainerToken);
            if (registryTokenKey == null)
            {
                return JsonBadRequest("Cannot register registry token");
            }

            var gitToken = templateVersion.TrainingRepositoryToken ?? UserGitToken(templateVersion.TrainingRepositoryGitId);

            // kamonohashi学習に必要な情報を設定
            var trainingCreateInputModel = new ApiModels.TrainingApiModels.CreateInputModel
            {
                Name = model.Name,
                ContainerImage = new ContainerImageInputModel
                {
                    RegistryId = templateVersion.TrainingContainerRegistryId,
                    Image = templateVersion.TrainingContainerImage,
                    Tag = templateVersion.TrainingContainerTag,
                },
                // 前処理あり実験の学習はデータセットを使用しないが、
                // kamonohashi学習ではデータセットを省略できないので、ダミーを指定している。
                DataSetId = dataSetVersion.DataSetId,
                ParentIds = new List<long> { experimentPreprocess.TrainingHistoryId, },
                GitModel = new GitCommitInputModel
                {
                    GitId = templateVersion.TrainingRepositoryGitId,
                    Repository = templateVersion.TrainingRepositoryName,
                    Owner = templateVersion.TrainingRepositoryOwner,
                    Branch = templateVersion.TrainingRepositoryBranch,
                    CommitId = templateVersion.TrainingRepositoryCommitId,
                },
                EntryPoint = templateVersion.TrainingEntryPoint,
                Options = null,
                Cpu = templateVersion.TrainingCpu,
                Memory = templateVersion.TrainingMemory,
                Gpu = templateVersion.TrainingGpu,
                Partition = null,
                Ports = null,
                Memo = null,
                Tags = null,
                Zip = false,
                LocalDataSet = true,
            };

            // kamonohashi学習を開始
            (var trainingHistory, var result) = await TrainingController.DoCreate(trainingCreateInputModel,
                dataSetRepository, nodeRepository, tenantRepository, trainingHistoryRepository,
                clusterManagementLogic, gitLogic, tagLogic, unitOfWork,
                CurrentUserInfo, ModelState, RequestUrl, "experiment_training_after_preproc", registryTokenKey, gitToken);

            // 実験の学習とkamonohashi学習を結び付ける
            if (trainingHistory != null)
            {
                var experiment = new Experiment
                {
                    Name = model.Name,
                    DataSetId = model.DataSetId,
                    DataSetVersionId = model.DataSetVersionId,
                    TemplateId = model.TemplateId,
                    TemplateVersionId = model.TemplateVersionId,
                    TrainingHistoryId = trainingHistory.Id,
                    ExperimentPreprocessId = experimentPreprocess.Id,
                };
                experimentRepository.Add(experiment);
                unitOfWork.Commit();

                ((JsonResult)result).Value = new SimpleOutputModel(experiment);
            }

            return result;
        }

        /// <summary>
        /// 実験を開始する
        /// </summary>
        [HttpPost("run")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Run([FromBody] CreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            var dataSet = await aquariumDataSetRepository.GetDataSetWithVersionsAsync(model.DataSetId);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet ID {model.DataSetId} is not found.");
            }

            var dataSetVersion = dataSet.DataSetVersions.SingleOrDefault(x => x.Id == model.DataSetVersionId);
            if (dataSetVersion == null)
            {
                return JsonNotFound($"DataSetVersion (DataSetId {model.DataSetId} and VersionId {model.DataSetVersionId}) is not found.");
            }

            var template = await templateRepository.GetByIdAsync(model.TemplateId);
            if (template == null)
            {
                return JsonNotFound($"Template ID {model.TemplateId} is not found.");
            }
            if (!templateLogic.Accessible(template, CurrentUserInfo.SelectedTenant))
            {
                return JsonBadRequest($"Template ID {model.TemplateId} is not accesible.");
            }

            var templateVersion = await templateVersionRepository
                .GetAll()
                .Include(x => x.PreprocessContainerRegistry)
                .Include(x => x.TrainingContainerRegistry)
                .Include(x => x.EvaluationContainerRegistry)
                .SingleOrDefaultAsync(x => x.TemplateId == model.TemplateId && x.Id == model.TemplateVersionId);
            if (templateVersion == null)
            {
                return JsonNotFound($"TemplateVersion (TemplateID {model.TemplateId} and VersionId {model.TemplateVersionId} is not found.");
            }

            var registryMaps = registryRepository.GetUserTenantRegistryMapAll(CurrentUserInfo.SelectedTenant.Id, CurrentUserInfo.Id);
            if (templateVersion.PreprocessContainerRegistryId.HasValue
                && !registryMaps.Any(x => x.TenantRegistryMap.RegistryId == templateVersion.PreprocessContainerRegistryId.Value))
            {
                return JsonBadRequest($"Prprocess Container Registry ID {templateVersion.PreprocessContainerRegistryId.Value} is not accesible.");
            }
            if (!registryMaps.Any(x => x.TenantRegistryMap.RegistryId == templateVersion.TrainingContainerRegistryId))
            {
                return JsonBadRequest($"Training Container Registry ID {templateVersion.TrainingContainerRegistryId} is not accesible.");
            }
            if (templateVersion.EvaluationContainerRegistryId.HasValue
                && !registryMaps.Any(x => x.TenantRegistryMap.RegistryId == templateVersion.EvaluationContainerRegistryId.Value))
            {
                return JsonBadRequest($"Evaluation Container Registry ID {templateVersion.EvaluationContainerRegistryId.Value} is not accesible.");
            }

            var gitMaps = gitRepository.GetUserTenantGitMapAll(CurrentUserInfo.SelectedTenant.Id, CurrentUserInfo.Id);
            if (templateVersion.PreprocessRepositoryGitId.HasValue
                && !gitMaps.Any(x => x.TenantGitMap.GitId == templateVersion.PreprocessRepositoryGitId.Value))
            {
                return JsonBadRequest($"Prprocess Repository Git ID {templateVersion.PreprocessRepositoryGitId.Value} is not accesible.");
            }
            if (!gitMaps.Any(x => x.TenantGitMap.GitId == templateVersion.TrainingRepositoryGitId))
            {
                return JsonBadRequest($"Training Repository Git ID {templateVersion.TrainingRepositoryGitId} is not accesible.");
            }
            if (templateVersion.EvaluationRepositoryGitId.HasValue
                && !gitMaps.Any(x => x.TenantGitMap.GitId == templateVersion.EvaluationRepositoryGitId.Value))
            {
                return JsonBadRequest($"Evaluation Repository Git ID {templateVersion.EvaluationRepositoryGitId.Value} is not accesible.");
            }


            if (templateVersion.PreprocessContainerRegistryId == null)
            {
                // テンプレートに前処理がない
                return await RunExperimentTrainingWithoutPreprocess(model, templateVersion, dataSetVersion);
            }

            var experimentPreprocess = experimentPreprocessRepository
                .GetAll()
                .Include(x => x.TrainingHistory)
                .Where(x => x.TemplateId == model.TemplateId && x.TemplateVersionId == model.TemplateVersionId
                && x.DataSetId == model.DataSetId && x.DataSetVersionId == model.DataSetVersionId)
                .AsEnumerable()
                .FirstOrDefault(x => x.TrainingHistory.GetStatus() == ContainerStatus.Completed);

            if (experimentPreprocess == null)
            {
                // テンプレートに前処理があり、前処理が実行されていない
                return await RunExperimentPreprocess(model, templateVersion, dataSetVersion);
            }

            // テンプレートに前処理があり、前処理が別実験で実行ずみ
            return await RunExperimentTrainingWithPreprocess(model, templateVersion, dataSetVersion, experimentPreprocess);
        }

        /// <summary>
        /// 前処理を正常終了させ、後続の学習を開始する
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpPost("{id}/preprocessing/complete")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CompletePreprocess(long id)
        {
            var experiment = await experimentRepository
                .GetAll()
                .Include(x => x.TemplateVersion).ThenInclude(x => x.TrainingContainerRegistry)
                .Include(x => x.DataSetVersion)
                .Include(x => x.ExperimentPreprocess)
                .SingleOrDefaultAsync(x => x.Id == id);
            // 実験がない
            if (experiment == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }
            // 実験に前処理がない
            if (experiment.ExperimentPreprocessId == null)
            {
                return JsonBadRequest($"Experiment ID {id} does not have preprocess.");
            }
            // 実験の学習が既に開始している
            if (experiment.TrainingHistoryId != null)
            {
                return JsonBadRequest($"Experiment ID {id} already has started training.");
            }

            // kamonohashi学習に必要な情報を設定
            var templateVersion = experiment.TemplateVersion;
            var dataSetVersion = experiment.DataSetVersion;
            var experimentPreprocess = experiment.ExperimentPreprocess;

            var registryTokenKey = await RegistRegistryToTenantAsync(templateVersion.TrainingContainerRegistry, templateVersion.TrainingContainerToken);
            if (registryTokenKey == null)
            {
                return JsonBadRequest("Cannot register registry token");
            }

            var gitToken = templateVersion.TrainingRepositoryToken ?? UserGitToken(templateVersion.TrainingRepositoryGitId);

            var trainingCreateInputModel = new ApiModels.TrainingApiModels.CreateInputModel
            {
                Name = experiment.Name,
                ContainerImage = new ContainerImageInputModel
                {
                    RegistryId = templateVersion.TrainingContainerRegistryId,
                    Image = templateVersion.TrainingContainerImage,
                    Tag = templateVersion.TrainingContainerTag,
                },
                // 前処理あり実験の学習はデータセットを使用しないが、
                // kamonohashi学習ではデータセットを省略できないので、ダミーを指定している。
                DataSetId = dataSetVersion.DataSetId,
                ParentIds = new List<long> { experimentPreprocess.TrainingHistoryId, },
                GitModel = new GitCommitInputModel
                {
                    GitId = templateVersion.TrainingRepositoryGitId,
                    Repository = templateVersion.TrainingRepositoryName,
                    Owner = templateVersion.TrainingRepositoryOwner,
                    Branch = templateVersion.TrainingRepositoryBranch,
                    CommitId = templateVersion.TrainingRepositoryCommitId,
                },
                EntryPoint = templateVersion.TrainingEntryPoint,
                Options = null,
                Cpu = templateVersion.TrainingCpu,
                Memory = templateVersion.TrainingMemory,
                Gpu = templateVersion.TrainingGpu,
                Partition = null,
                Ports = null,
                Memo = null,
                Tags = null,
                Zip = false,
                LocalDataSet = true,
            };

            // kamonohashi学習を開始
            (var trainingHistory, var result) = await TrainingController.DoCreate(trainingCreateInputModel,
                dataSetRepository, nodeRepository, tenantRepository, trainingHistoryRepository,
                clusterManagementLogic, gitLogic, tagLogic, unitOfWork,
                CurrentUserInfo, ModelState, RequestUrl, "experiment_training_after_preproc", registryTokenKey, gitToken);

            // 実験の学習とkamonohashi学習を結び付ける
            if (trainingHistory != null)
            {
                experiment.TrainingHistoryId = trainingHistory.Id;
                experimentRepository.Update(experiment);
                unitOfWork.Commit();

                ((JsonResult)result).Value = new SimpleOutputModel(experiment);
            }

            return result;
        }

        /// <summary>
        /// アクアリウム用レジストリシークレットをk8sに登録する
        /// </summary>
        /// <returns></returns>
        private async Task<string> RegistRegistryToTenantAsync(Registry registry, string token)
        {
            var registryMap = registryLogic.GetCurrentRegistryMap(registry.Id);
            var tokenKey = $"aqiarium-registry-{registry.Name}-{CurrentUserInfo.Id}";
            var url = registry.RegistryUrl;
            var selectedTenantName = CurrentUserInfo.SelectedTenant.Name;
            var userName = "";
            var password = token ?? registryMap?.RegistryPassword ?? "";
            
            var result = await clusterManagementLogic.RegistRegistryToTenantAsync(tokenKey, url, registry, selectedTenantName,
                userName, password);
            return result ? tokenKey : null;
        }

        private string UserGitToken(long? gitId)
        {
            return gitId.HasValue ? gitRepository.GetUserTenantGitMap(CurrentUserInfo.Id, CurrentUserInfo.SelectedTenant.Id, gitId.Value).GitToken : "";
        }
    }
}

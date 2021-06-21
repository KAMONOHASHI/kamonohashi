using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.ApiModels.TemplateApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// テンプレートAPI
    /// </summary>
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}")]
    public class TemplateController : PlatypusApiControllerBase
    {
        private readonly IExperimentRepository experimentRepository;
        private readonly IExperimentPreprocessRepository experimentPreprocessRepository;
        private readonly ITemplateRepository templateRepository;
        private readonly ITemplateVersionRepository templateVersionRepository;
        private readonly ITemplateLogic templateLogic;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGitLogic gitLogic;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TemplateController(
            IExperimentRepository experimentRepository,
            IExperimentPreprocessRepository experimentPreprocessRepository,
            ITemplateRepository templateRepository,
            ITemplateVersionRepository templateVersionRepository,
            ITemplateLogic templateLogic,
            IGitLogic gitLogic,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.experimentRepository = experimentRepository;
            this.experimentPreprocessRepository = experimentPreprocessRepository;
            this.templateRepository = templateRepository;
            this.templateVersionRepository = templateVersionRepository;
            this.templateLogic = templateLogic;
            this.unitOfWork = unitOfWork;
            this.gitLogic = gitLogic;
        }

        private async Task<IActionResult> SetTemplateDetails(TemplateVersion templateVersion, Template template, VersionCreateInputModel model)
        {
            long? preprocessGitId = null;
            string preprocessRepository = null;
            string preprocessOwner = null;
            string preprocessBranch = null;
            string preprocessCommitId = null;
            string preprocessGitToken = null;
            long trainingGitId = 0;
            string trainingRepository = null;
            string trainingOwner = null;
            string trainingBranch = null;
            string trainingCommitId = null;
            string trainingGitToken = null;
            long? evaluationGitId = null;
            string evaluationRepository = null;
            string evaluationOwner = null;
            string evaluationBranch = null;
            string evaluationCommitId = null;
            string evaluationGitToken = null;

            if (model.PreprocessGitModel != null)
            {
                preprocessGitId = model.PreprocessGitModel.GitId ?? CurrentUserInfo.SelectedTenant.DefaultGit?.Id;
                preprocessRepository = model.PreprocessGitModel.Repository;
                preprocessOwner = model.PreprocessGitModel.Owner;
                preprocessBranch = model.PreprocessGitModel.Branch;
                preprocessCommitId = model.PreprocessGitModel.CommitId;
                // コミットIDが指定されていなければ、ブランチのHEADからコミットIDを取得する
                if (string.IsNullOrEmpty(preprocessCommitId))
                {
                    preprocessCommitId = await gitLogic.GetCommitIdAsync(preprocessGitId.Value, model.PreprocessGitModel.Repository, model.PreprocessGitModel.Owner, model.PreprocessGitModel.Branch);
                    if (string.IsNullOrEmpty(preprocessCommitId))
                    {
                        // コミットIDが特定できなかったらエラー
                        return JsonNotFound($"The branch {preprocessBranch} for {preprocessGitId.Value}/{model.PreprocessGitModel.Owner}/{ model.PreprocessGitModel.Repository} is not found.");
                    }
                }
                preprocessGitToken = model.PreprocessGitModel.Token;
            }
            if (model.TrainingGitModel != null)
            {
                trainingGitId = (model.TrainingGitModel.GitId ?? CurrentUserInfo.SelectedTenant.DefaultGit?.Id).Value;
                trainingRepository = model.TrainingGitModel.Repository;
                trainingOwner = model.TrainingGitModel.Owner;
                trainingBranch = model.TrainingGitModel.Branch;
                trainingCommitId = model.TrainingGitModel.CommitId;
                // コミットIDが指定されていなければ、ブランチのHEADからコミットIDを取得する
                if (string.IsNullOrEmpty(trainingCommitId))
                {
                    trainingCommitId = await gitLogic.GetCommitIdAsync(trainingGitId, model.TrainingGitModel.Repository, model.TrainingGitModel.Owner, model.TrainingGitModel.Branch);
                    if (string.IsNullOrEmpty(trainingCommitId))
                    {
                        // コミットIDが特定できなかったらエラー
                        return JsonNotFound($"The branch {trainingBranch} for {trainingGitId}/{model.TrainingGitModel.Owner}/{ model.TrainingGitModel.Repository} is not found.");
                    }
                }
                trainingGitToken = model.TrainingGitModel.Token;
            }
            if (model.EvaluationGitModel != null)
            {
                evaluationGitId = (model.EvaluationGitModel.GitId ?? CurrentUserInfo.SelectedTenant.DefaultGit?.Id).Value;
                evaluationRepository = model.EvaluationGitModel.Repository;
                evaluationOwner = model.EvaluationGitModel.Owner;
                evaluationBranch = model.EvaluationGitModel.Branch;
                evaluationCommitId = model.EvaluationGitModel.CommitId;
                // コミットIDが指定されていなければ、ブランチのHEADからコミットIDを取得する
                if (string.IsNullOrEmpty(evaluationCommitId))
                {
                    evaluationCommitId = await gitLogic.GetCommitIdAsync(evaluationGitId.Value, model.EvaluationGitModel.Repository, model.EvaluationGitModel.Owner, model.EvaluationGitModel.Branch);
                    if (string.IsNullOrEmpty(evaluationCommitId))
                    {
                        // コミットIDが特定できなかったらエラー
                        return JsonNotFound($"The branch {evaluationBranch} for {evaluationGitId}/{model.EvaluationGitModel.Owner}/{ model.EvaluationGitModel.Repository} is not found.");
                    }
                }
                evaluationGitToken = model.EvaluationGitModel.Token;
            }

            long? preprocessRegistryId = null;
            string preprocessImage = null;
            string preprocessTag = null;
            string preprocessRegistryToken = null;
            if (model.PreprocessContainerImage != null)
            {
                preprocessRegistryId = model.PreprocessContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id;
                preprocessImage = model.PreprocessContainerImage.Image;
                preprocessTag = model.PreprocessContainerImage.Tag;
                preprocessRegistryToken = model.PreprocessContainerImage.Token;
            }
            long trainingRegistryId = 0;
            string trainingImage = null;
            string trainingTag = null;
            string trainingRegistryToken = null;
            if (model.TrainingContainerImage != null)
            {
                trainingRegistryId = (model.TrainingContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id).Value;
                trainingImage = model.TrainingContainerImage.Image;
                trainingTag = model.TrainingContainerImage.Tag;
                trainingRegistryToken = model.TrainingContainerImage.Token;
            }
            long? evaluationRegistryId = null;
            string evaluationImage = null;
            string evaluationTag = null;
            string evaluationRegistryToken = null;
            if (model.EvaluationContainerImage != null)
            {
                evaluationRegistryId = (model.EvaluationContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id).Value;
                evaluationImage = model.EvaluationContainerImage.Image;
                evaluationTag = model.EvaluationContainerImage.Tag;
                evaluationRegistryToken = model.EvaluationContainerImage.Token;
            }

            templateVersion.TemplateId = template.Id;
            templateVersion.Version = template.LatestVersion;

            templateVersion.PreprocessEntryPoint = model.PreprocessEntryPoint;
            templateVersion.PreprocessContainerRegistryId = preprocessRegistryId;
            templateVersion.PreprocessContainerImage = preprocessImage;
            templateVersion.PreprocessContainerTag = preprocessTag; // latestは運用上使用されていないハズなので、そのまま直接代入
            templateVersion.PreprocessContainerToken = preprocessRegistryToken;
            templateVersion.PreprocessRepositoryGitId = preprocessGitId;
            templateVersion.PreprocessRepositoryName = preprocessRepository;
            templateVersion.PreprocessRepositoryOwner = preprocessOwner;
            templateVersion.PreprocessRepositoryBranch = preprocessBranch;
            templateVersion.PreprocessRepositoryCommitId = preprocessCommitId;
            templateVersion.PreprocessRepositoryToken = preprocessGitToken;
            templateVersion.PreprocessCpu = model.PreprocessCpu;
            templateVersion.PreprocessMemory = model.PreprocessMemory;
            templateVersion.PreprocessGpu = model.PreprocessGpu;

            templateVersion.TrainingEntryPoint = model.TrainingEntryPoint;
            templateVersion.TrainingContainerRegistryId = trainingRegistryId;
            templateVersion.TrainingContainerImage = trainingImage;
            templateVersion.TrainingContainerTag = trainingTag; // latestは運用上使用されていないハズなので、そのまま直接代入
            templateVersion.TrainingContainerToken = trainingRegistryToken;
            templateVersion.TrainingRepositoryGitId = trainingGitId;
            templateVersion.TrainingRepositoryName = trainingRepository;
            templateVersion.TrainingRepositoryOwner = trainingOwner;
            templateVersion.TrainingRepositoryBranch = trainingBranch;
            templateVersion.TrainingRepositoryCommitId = trainingCommitId;
            templateVersion.TrainingRepositoryToken = trainingGitToken;
            templateVersion.TrainingCpu = model.TrainingCpu;
            templateVersion.TrainingMemory = model.TrainingMemory;
            templateVersion.TrainingGpu = model.TrainingGpu;

            templateVersion.EvaluationEntryPoint = model.EvaluationEntryPoint;
            templateVersion.EvaluationContainerRegistryId = evaluationRegistryId;
            templateVersion.EvaluationContainerImage = evaluationImage;
            templateVersion.EvaluationContainerTag = evaluationTag; // latestは運用上使用されていないハズなので、そのまま直接代入
            templateVersion.EvaluationContainerToken = evaluationRegistryToken;
            templateVersion.EvaluationRepositoryGitId = evaluationGitId;
            templateVersion.EvaluationRepositoryName = evaluationRepository;
            templateVersion.EvaluationRepositoryOwner = evaluationOwner;
            templateVersion.EvaluationRepositoryBranch = evaluationBranch;
            templateVersion.EvaluationRepositoryCommitId = evaluationCommitId;
            templateVersion.EvaluationRepositoryToken = evaluationGitToken;
            templateVersion.EvaluationCpu = model.EvaluationCpu;
            templateVersion.EvaluationMemory = model.EvaluationMemory;
            templateVersion.EvaluationGpu = model.EvaluationGpu;

            return null;
        }

        /// <summary>
        /// 接続中のテナントに有効なテンプレート一覧を取得する
        /// </summary>
        [HttpGet("tenant/templates")]
        [PermissionFilter(MenuCode.Template, MenuCode.Experiment)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllByTenant(bool withTotal = false)
        {
            var templates = templateRepository
                .GetAll()
                .OrderByDescending(x => x.Id)
                .Where(x => templateLogic.Accessible(x, CurrentUserInfo.SelectedTenant));
            if (withTotal)
            {
                SetTotalCountToHeader(templates.Count());
            }
            return JsonOK(templates.Select(t => new IndexOutputModel(t)));
        }

        /// <summary>
        /// 接続中のテナントで作成されたテンプレート一覧を取得する
        /// </summary>
        [HttpGet("templates")]
        [PermissionFilter(MenuCode.Template, MenuCode.Experiment)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllCreatedByTenant(bool withTotal = false)
        {
            var templates = templateRepository
                .GetAll()
                .OrderByDescending(x => x.Id)
                .Where(x => templateLogic.IsCreatedTenant(x, CurrentUserInfo.SelectedTenant));
            if (withTotal)
            {
                SetTotalCountToHeader(templates.Count());
            }
            return JsonOK(templates.Select(t => new IndexOutputModel(t)));
        }


        /// <summary>
        /// テンプレートを作成する
        /// </summary>
        [HttpPost("admin/templates")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public IActionResult Create([FromBody] CreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            if (model.AccessLevel != TemplateAccessLevel.Disabled
                && model.AccessLevel != TemplateAccessLevel.Private
                && model.AccessLevel != TemplateAccessLevel.Public)
            {
                return JsonBadRequest("Invalid access level");
            }

            var template = new Template
            {
                Name = model.Name,
                Memo = model.Memo,
                LatestVersion = 0,
                AccessLevel = model.AccessLevel,
                CreaterUserId = CurrentUserInfo.Id,
                CreaterTenantId = CurrentUserInfo.SelectedTenant.Id,
            };
            templateRepository.Add(template);
            unitOfWork.Commit();
            return JsonCreated(new IndexOutputModel(template));
        }

        /// <summary>
        /// テンプレートを取得する
        /// </summary>
        /// <param name="id">テンプレートID</param>
        [HttpGet("admin/templates/{id}")]
        [PermissionFilter(MenuCode.Template, MenuCode.Experiment)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long id)
        {
            var template = await templateRepository.GetByIdAsync(id);
            if (template == null)
            {
                return JsonNotFound($"Template ID {id} is not found.");
            }

            // テンプレートが公開テンプレートでない かつ
            // 作成したテナント以外のテナントからのリクエストの場合はBadrequestを返却
            if (template.AccessLevel != TemplateAccessLevel.Public 
                && template.CreaterTenantId != CurrentUserInfo.SelectedTenant.Id)
            {
                return JsonBadRequest("Invalid access level");
            }

            var result = new IndexOutputModel(template);
            return JsonOK(result);
        }

        /// <summary>
        /// テンプレートバージョンを作成する
        /// </summary>
        /// <param name="id">テンプレートID</param>
        [HttpPost("admin/templates/{id}/versions")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType(typeof(VersionIndexOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateTemplateVersion(long id, [FromBody] VersionCreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            if (model.PreprocessGitModel != null && !model.PreprocessGitModel.IsValid())
            {
                return JsonBadRequest($"The input about Preprocess Git is not valid.");
            }
            if (model.TrainingGitModel != null && !model.TrainingGitModel.IsValid())
            {
                return JsonBadRequest($"The input about Training Git is not valid.");
            }
            if (model.EvaluationGitModel != null && !model.EvaluationGitModel.IsValid())
            {
                return JsonBadRequest($"The input about Evaluation Git is not valid.");
            }

            var template = await templateRepository.GetByIdAsync(id);
            if (template == null)
            {
                return JsonNotFound($"Template ID {id} is not found.");
            }

            // 作成したテナント以外のテナントからのリクエストの場合はBadrequestを返却
            if (template.CreaterTenantId != CurrentUserInfo.SelectedTenant.Id)
            {
                return JsonBadRequest("Invalid access level");
            }

            template.LatestVersion += 1;
            templateRepository.Update(template);

            var templateVersion = new TemplateVersion();
            var errorResult = await SetTemplateDetails(templateVersion, template, model);
            if (errorResult != null)
            {
                return errorResult;
            }

            templateVersionRepository.Add(templateVersion);
            unitOfWork.Commit();
            return JsonCreated(new VersionIndexOutputModel(templateVersion));
        }

        /// <summary>
        /// テンプレートバージョン一覧を取得する
        /// </summary>
        /// <param name="id">テンプレートID</param>
        [HttpGet("admin/templates/{id}/versions")]
        [PermissionFilter(MenuCode.Template, MenuCode.Experiment)]
        [ProducesResponseType(typeof(IEnumerable<VersionIndexOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllTemplateVersion(long id)
        {
            var template = await templateRepository.GetAll().Include(x => x.TemplateVersions).SingleOrDefaultAsync(x => x.Id == id);
            if (template == null)
            {
                return JsonNotFound($"Template Id {id} is not found.");
            }

            // テンプレートが公開テンプレートでない かつ
            // 作成したテナント以外のテナントからのリクエストの場合はBadrequestを返却
            if (template.AccessLevel != TemplateAccessLevel.Public
                && template.CreaterTenantId != CurrentUserInfo.SelectedTenant.Id)
            {
                return JsonBadRequest("Invalid access level");
            }

            return JsonOK(template.TemplateVersions
                .OrderByDescending(x => x.Version)
                .Select(x => new VersionIndexOutputModel(x)));
        }

        /// <summary>
        /// テンプレートバージョンを取得する
        /// </summary>
        /// <param name="id">テンプレートID</param>
        /// <param name="versionId">テンプレートバージョンID</param>
        [HttpGet("admin/templates/{id}/versions/{versionId}")]
        [PermissionFilter(MenuCode.Template, MenuCode.Experiment)]
        [ProducesResponseType(typeof(VersionDetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetailTemplateVersion(long id, long versionId)
        {
            var template = await templateRepository.GetByIdAsync(id);
            if (template == null)
            {
                return JsonNotFound($"Template ID {id} is not found.");
            }
            // テンプレートが公開テンプレートでない かつ
            // 作成したテナント以外のテナントからのリクエストの場合はBadrequestを返却
            if (template.AccessLevel != TemplateAccessLevel.Public
                && template.CreaterTenantId != CurrentUserInfo.SelectedTenant.Id)
            {
                return JsonBadRequest("Invalid access level");
            }

            var templateVersion = await templateVersionRepository
                .GetAll()
                .Include(x => x.PreprocessContainerRegistry)
                .Include(x => x.TrainingContainerRegistry)
                .Include(x => x.EvaluationContainerRegistry)
                .SingleOrDefaultAsync(x => x.Id == versionId && x.TemplateId == id);

            if (templateVersion == null)
            {
                return JsonNotFound($"TemplateVersion (Id {id} VersionId {versionId}) is not found.");
            }

            var model = new VersionDetailsOutputModel(templateVersion);

            // Gitの表示用URLを作る
            if (templateVersion.PreprocessRepositoryGitId != null)
            {
                model.PreprocessGitModel.Url = gitLogic.GetTreeUiUrl(templateVersion.PreprocessRepositoryGitId.Value, templateVersion.PreprocessRepositoryName, templateVersion.PreprocessRepositoryOwner, templateVersion.PreprocessRepositoryCommitId);
            }
            model.TrainingGitModel.Url = gitLogic.GetTreeUiUrl(templateVersion.TrainingRepositoryGitId, templateVersion.TrainingRepositoryName, templateVersion.TrainingRepositoryOwner, templateVersion.TrainingRepositoryCommitId);
            if (templateVersion.EvaluationRepositoryGitId != null)
            {
                model.EvaluationGitModel.Url = gitLogic.GetTreeUiUrl(templateVersion.EvaluationRepositoryGitId.Value, templateVersion.EvaluationRepositoryName, templateVersion.EvaluationRepositoryOwner, templateVersion.EvaluationRepositoryCommitId);
            }

            return JsonOK(model);
        }

        /// <summary>
        /// テンプレートを編集する
        /// </summary>
        /// <param name="id">テンプレートID</param>
        [HttpPut("admin/templates/{id}")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long id, [FromBody] EditInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            var template = await templateRepository.GetByIdAsync(id);
            if (template == null)
            {
                return JsonNotFound($"Template ID {id} is not found.");
            }
            
            // 作成したテナント以外のテナントからのリクエストの場合はBadrequestを返却
            if (template.CreaterTenantId != CurrentUserInfo.SelectedTenant.Id)
            {
                return JsonBadRequest("Invalid access level");
            }

            if (model.AccessLevel != null)
            {
                if (model.AccessLevel != TemplateAccessLevel.Disabled
                    && model.AccessLevel != TemplateAccessLevel.Private
                    && model.AccessLevel != TemplateAccessLevel.Public)
                {
                    return JsonBadRequest("Invalid access level");
                }
                template.AccessLevel = model.AccessLevel.Value;
            }
            template.Name = EditColumnNotEmpty(model.Name, template.Name);
            template.Memo = EditColumn(model.Memo, template.Memo);
            unitOfWork.Commit();

            return JsonOK(new IndexOutputModel(template));
        }

        /// <summary>
        /// テンプレートを削除する
        /// </summary>
        /// <param name="id">テンプレートID</param>
        [HttpDelete("admin/templates/{id}")]
        [Filters.PermissionFilter(MenuCode.Template)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long id)
        {
            var template = await templateRepository.GetByIdAsync(id);
            if (template == null)
            {
                return JsonNotFound($"Template ID {id} is not found.");
            }
            
            // 作成したテナント以外のテナントからのリクエストの場合はBadrequestを返却
            if (template.CreaterTenantId != CurrentUserInfo.SelectedTenant.Id)
            {
                return JsonBadRequest("Invalid access level");
            }

            if (await experimentRepository.ExistsAsync(x => x.TemplateId == id, true))
            {
                return JsonConflict($"Template {id} has been used by experiment.");
            }
            if (await experimentPreprocessRepository.ExistsAsync(x => x.TemplateId == id, true))
            {
                return JsonConflict($"Template {id} has been used by experiment preprocess.");
            }

            templateRepository.Delete(template);
            unitOfWork.Commit();
            return JsonNoContent();
        }

        /// <summary>
        /// テンプレートバージョンを削除する
        /// </summary>
        /// <param name="id">テンプレートID</param>
        /// <param name="versionId">テンプレートバージョンID</param>
        [HttpDelete("admin/templates/{id}/versions/{versionId}")]
        [Filters.PermissionFilter(MenuCode.Template)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteTemplateVersion(long id, long versionId)
        {
            var template = await templateRepository.GetByIdAsync(id);
            if (template == null)
            {
                return JsonNotFound($"Template ID {id} is not found.");
            }
            // 作成したテナント以外のテナントからのリクエストの場合はBadrequestを返却
            if (template.CreaterTenantId != CurrentUserInfo.SelectedTenant.Id)
            {
                return JsonBadRequest("Invalid access level");
            }

            var templateVersion = await templateVersionRepository
                .GetAll()
                .SingleOrDefaultAsync(x => x.Id == versionId && x.TemplateId == id);
            if (templateVersion == null)
            {
                return JsonNotFound($"TemplateVersion (Id {id} VersionId {versionId}) is not found.");
            }

            if (await experimentRepository.ExistsAsync(x => x.TemplateId == id && x.TemplateVersionId == versionId, true))
            {
                return JsonConflict($"TemplateVersion (Id {id} VersionId {versionId}) has been used by experiment.");
            }
            if (await experimentPreprocessRepository.ExistsAsync(x => x.TemplateId == id && x.TemplateVersionId == versionId, true))
            {
                return JsonConflict($"TemplateVersion (Id {id} VersionId {versionId}) has been used by experiment preprocess.");
            }

            // 最新バージョンを削除する場合
            if (template.LatestVersion == templateVersion.Version)
            {
                var templateVersionsByTemplateId = templateVersionRepository
                    .GetAll()
                    .Where(x => x.TemplateId == id && x != templateVersion)
                    .DefaultIfEmpty();
                template.LatestVersion = templateVersionsByTemplateId?.Max(x => x.Version) ?? 0;
                templateRepository.Update(template);
            }

            templateVersionRepository.Delete(templateVersion);
            unitOfWork.Commit();
            return JsonNoContent();
        }
    }
}
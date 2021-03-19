using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.ApiModels.TemplateApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
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
        private readonly ITemplateRepository templateRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGitLogic gitLogic;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TemplateController(
            ITemplateRepository template2Repository,
            IGitLogic gitLogic,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.templateRepository = template2Repository;
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
            long trainingGitId = 0;
            string trainingRepository = null;
            string trainingOwner = null;
            string trainingBranch = null;
            string trainingCommitId = null;
            long? evaluationGitId = null;
            string evaluationRepository = null;
            string evaluationOwner = null;
            string evaluationBranch = null;
            string evaluationCommitId = null;

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
            }

            long? preprocessRegistryId = null;
            string preprocessImage = null;
            string preprocessTag = null;
            if (model.PreprocessContainerImage != null)
            {
                preprocessRegistryId = model.PreprocessContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id;
                preprocessImage = model.PreprocessContainerImage.Image;
                preprocessTag = model.PreprocessContainerImage.Tag;
            }
            long trainingRegistryId = 0;
            string trainingImage = null;
            string trainingTag = null;
            if (model.TrainingContainerImage != null)
            {
                trainingRegistryId = (model.TrainingContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id).Value;
                trainingImage = model.TrainingContainerImage.Image;
                trainingTag = model.TrainingContainerImage.Tag;
            }
            long? evaluationRegistryId = null;
            string evaluationImage = null;
            string evaluationTag = null;
            if (model.EvaluationContainerImage != null)
            {
                evaluationRegistryId = (model.EvaluationContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id).Value;
                evaluationImage = model.EvaluationContainerImage.Image;
                evaluationTag = model.EvaluationContainerImage.Tag;
            }

            templateVersion.TemplateId = template.Id;
            templateVersion.Version = template.LatestVersion;

            templateVersion.PreprocessEntryPoint = model.PreprocessEntryPoint;
            templateVersion.PreprocessContainerRegistryId = preprocessRegistryId;
            templateVersion.PreprocessContainerImage = preprocessImage;
            templateVersion.PreprocessContainerTag = preprocessTag; // latestは運用上使用されていないハズなので、そのまま直接代入
            templateVersion.PreprocessRepositoryGitId = preprocessGitId;
            templateVersion.PreprocessRepositoryName = preprocessRepository;
            templateVersion.PreprocessRepositoryOwner = preprocessOwner;
            templateVersion.PreprocessRepositoryBranch = preprocessBranch;
            templateVersion.PreprocessRepositoryCommitId = preprocessCommitId;
            templateVersion.PreprocessCpu = model.PreprocessCpu;
            templateVersion.PreprocessMemory = model.PreprocessMemory;
            templateVersion.PreprocessGpu = model.PreprocessGpu;

            templateVersion.TrainingEntryPoint = model.TrainingEntryPoint;
            templateVersion.TrainingContainerRegistryId = trainingRegistryId;
            templateVersion.TrainingContainerImage = trainingImage;
            templateVersion.TrainingContainerTag = trainingTag; // latestは運用上使用されていないハズなので、そのまま直接代入
            templateVersion.TrainingRepositoryGitId = trainingGitId;
            templateVersion.TrainingRepositoryName = trainingRepository;
            templateVersion.TrainingRepositoryOwner = trainingOwner;
            templateVersion.TrainingRepositoryBranch = trainingBranch;
            templateVersion.TrainingRepositoryCommitId = trainingCommitId;
            templateVersion.TrainingCpu = model.TrainingCpu;
            templateVersion.TrainingMemory = model.TrainingMemory;
            templateVersion.TrainingGpu = model.TrainingGpu;

            templateVersion.EvaluationEntryPoint = model.EvaluationEntryPoint;
            templateVersion.EvaluationContainerRegistryId = evaluationRegistryId;
            templateVersion.EvaluationContainerImage = evaluationImage;
            templateVersion.EvaluationContainerTag = evaluationTag; // latestは運用上使用されていないハズなので、そのまま直接代入
            templateVersion.EvaluationRepositoryGitId = evaluationGitId;
            templateVersion.EvaluationRepositoryName = evaluationRepository;
            templateVersion.EvaluationRepositoryOwner = evaluationOwner;
            templateVersion.EvaluationRepositoryBranch = evaluationBranch;
            templateVersion.EvaluationRepositoryCommitId = evaluationCommitId;
            templateVersion.EvaluationCpu = model.EvaluationCpu;
            templateVersion.EvaluationMemory = model.EvaluationMemory;
            templateVersion.EvaluationGpu = model.EvaluationGpu;

            return null;
        }

        /// <summary>
        /// 全テンプレート一覧を取得する
        /// </summary>
        [HttpGet("admin/templates")]
        [PermissionFilter(MenuCode.Template, MenuCode.Experiment)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll([FromQuery] int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            var templates = templateRepository.GetAll();
            templates = templates.OrderByDescending(t => t.Id);
            if (withTotal)
            {
                //テンプレートの場合は件数が少ない想定なので、別のSQLを投げずにカウントしてしまう
                SetTotalCountToHeader(templates.Count());
            }

            if (perPage.HasValue)
            {
                templates = templates.Paging(page, perPage.Value);
            }

            return JsonOK(templates.Select(t => new IndexOutputModel(t)));
        }

        /// <summary>
        /// 接続中のテナントに有効なテンプレート一覧を取得する
        /// </summary>
        [HttpGet("tenant/templates")]
        [PermissionFilter(MenuCode.Template, MenuCode.Experiment)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllByTenant(bool withTotal = false)
        {
            var templates = templateRepository.GetAll()
                .Where(x => (x.AccessLevel == TemplateAccessLevel.Private && x.CreaterTenantId == CurrentUserInfo.SelectedTenant.Id)
                || (x.AccessLevel == TemplateAccessLevel.Public));
            templates = templates.OrderByDescending(t => t.Id);
            if (withTotal)
            {
                //テンプレートの場合は件数が少ない想定なので、別のSQLを投げずにカウントしてしまう
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

            var result = new IndexOutputModel(template);
            return JsonOK(result);
        }

        /// <summary>
        /// テンプレートバージョンを作成する
        /// </summary>
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
            template.LatestVersion += 1;
            templateRepository.Update(template);

            var templateVersion = new TemplateVersion();
            var errorResult = await SetTemplateDetails(templateVersion, template, model);
            if (errorResult != null)
            {
                return errorResult;
            }

            templateRepository.Add(templateVersion);
            unitOfWork.Commit();
            return JsonCreated(new VersionIndexOutputModel(templateVersion));
        }

        /// <summary>
        /// テンプレートバージョン一覧を取得する
        /// </summary>
        /// <param name="id">取得するテンプレートのID</param>
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
            var tempalteVersion = await templateRepository.GetTemplateVersionAsync(id, versionId);
            if (tempalteVersion == null)
            {
                return JsonNotFound($"TemplateVersion (Id {id} VersionId {versionId}) is not found.");
            }

            var model = new VersionDetailsOutputModel(tempalteVersion);

            // Gitの表示用URLを作る
            if (tempalteVersion.PreprocessRepositoryGitId != null)
            {
                model.PreprocessGitModel.Url = gitLogic.GetTreeUiUrl(tempalteVersion.PreprocessRepositoryGitId.Value, tempalteVersion.PreprocessRepositoryName, tempalteVersion.PreprocessRepositoryOwner, tempalteVersion.PreprocessRepositoryCommitId);
            }
            model.TrainingGitModel.Url = gitLogic.GetTreeUiUrl(tempalteVersion.TrainingRepositoryGitId, tempalteVersion.TrainingRepositoryName, tempalteVersion.TrainingRepositoryOwner, tempalteVersion.TrainingRepositoryCommitId);
            if (tempalteVersion.EvaluationRepositoryGitId != null)
            {
                model.EvaluationGitModel.Url = gitLogic.GetTreeUiUrl(tempalteVersion.EvaluationRepositoryGitId.Value, tempalteVersion.EvaluationRepositoryName, tempalteVersion.EvaluationRepositoryOwner, tempalteVersion.EvaluationRepositoryCommitId);
            }

            return JsonOK(model);
        }

        /// <summary>
        /// テンプレートを編集する
        /// </summary>
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
    }
}
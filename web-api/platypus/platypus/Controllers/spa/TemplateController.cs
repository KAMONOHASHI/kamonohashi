using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    /// テンプレート管理を扱うためのAPI集
    /// </summary>
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}")]
    public class TemplateController : PlatypusApiControllerBase
    {
        private readonly ITemplateRepository templateRepository;
        private readonly ITemplate2Repository template2Repository;
        private readonly ITenantRepository tenantRepository;
        private readonly IExperimentHistoryRepository experimentHistoryRepository;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGitLogic gitLogic;
        private readonly IMultiTenancyLogic multiTenancyLogic;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TemplateController(
            IMultiTenancyLogic multiTenancyLogic,
            IExperimentHistoryRepository experimentHistoryRepository,
            ITemplateRepository templateRepository,
            ITemplate2Repository template2Repository,
            ITenantRepository tenantRepository,
            IGitLogic gitLogic,
            IUnitOfWork unitOfWork,
            IClusterManagementLogic clusterManagementLogic,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.multiTenancyLogic = multiTenancyLogic;
            this.experimentHistoryRepository = experimentHistoryRepository;
            this.templateRepository = templateRepository;
            this.template2Repository = template2Repository;
            this.tenantRepository = tenantRepository;
            this.unitOfWork = unitOfWork;
            this.clusterManagementLogic = clusterManagementLogic;
            this.gitLogic = gitLogic;
        }
        #region old
        /// <summary>
        /// 全テンプレート一覧を取得
        /// </summary>
        [HttpGet("admin/templates")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllTemplatesForAdmin([FromQuery] int? perPage, [FromQuery] int page = 1, bool withTotal = false)
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
        /// 接続中のテナントに有効なテンプレート一覧を取得
        /// </summary>
        [HttpGet("tenant/templates")]
        [PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetTemplatesforTenant(bool withTotal = false)
        {
            var templates = templateRepository.GetAccessibleTemplates(CurrentUserInfo.SelectedTenant.Id);
            templates = templates.OrderByDescending(t => t.Id);
            if (withTotal)
            {
                //テンプレートの場合は件数が少ない想定なので、別のSQLを投げずにカウントしてしまう
                SetTotalCountToHeader(templates.Count());
            }

            return JsonOK(templates.Select(t => new IndexOutputModel(t)));
        }
        /*
                /// <summary>
                /// テンプレートアクセスレベルの一覧を取得する
                /// </summary>
                [HttpGet("admin/template-access-levels")]
                [PermissionFilter(MenuCode.Template)]
                [ProducesResponseType(typeof(IEnumerable<EnumInfo>), (int)HttpStatusCode.OK)]
                public IActionResult GetAllTypes()
                {
                    var accessLevels = Enum.GetValues(typeof(TemplateAccessLevel)) as TemplateAccessLevel[];

                    return JsonOK(accessLevels.Select(n => new EnumInfo() { Id = (int)n, Name = n.ToString() }));
                }
        */
        /// <summary>
        /// 指定されたIDのテンプレート情報を取得。
        /// </summary>
        /// <param name="id">テンプレートID</param>
        /// <param name="templateTenantMapRepository">DI用</param>
        [HttpGet("admin/templates/{id}")]
        [PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long? id, [FromServices] ITemplateTenantMapRepository templateTenantMapRepository)
        {
            if (id == null)
            {
                return JsonBadRequest("Template ID is required.");
            }
            var template = await templateRepository.GetIncludeAllAsync(id.Value);
            if (template == null)
            {
                return JsonNotFound($"Template Id {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(template);
            if (model.AccessLevel == TemplateAccessLevel.Private)
            {
                //プライベートモードの時に限り、アクセス可能なテナントを探索する
                var tenants = templateRepository.GetAssignedTenants(template.Id);
                model.AssignedTenants = templateRepository.GetAssignedTenants(template.Id).Select(t => new DetailsOutputModel.AssignedTenant()
                {
                    Id = t.Id,
                    Name = t.Name,
                    DisplayName = t.DisplayName
                });
            }
            // Gitの表示用URLを作る
            if (template.PreprocessRepositoryGitId != null)
            {
                model.PreprocessGitModel.Url = gitLogic.GetTreeUiUrl(template.PreprocessRepositoryGitId.Value, template.PreprocessRepositoryName, template.PreprocessRepositoryOwner, template.PreprocessRepositoryCommitId);
            }
            if (template.TrainingRepositoryGitId != null)
            {
                model.TrainingGitModel.Url = gitLogic.GetTreeUiUrl(template.TrainingRepositoryGitId.Value, template.TrainingRepositoryName, template.TrainingRepositoryOwner, template.TrainingRepositoryCommitId);
            }

            return JsonOK(model);
        }



        /// <summary>
        /// 新規にテンプレートを登録する
        /// </summary>
        [HttpPost("admin/templates")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] CreateInputModel model,
            [FromServices] ITenantRepository tenantRepository)
        {
            //TODO:データの入力チェック
            //前処理コンテナは空白でも許す
            //if (!ModelState.IsValid)
            //{
            //    return JsonBadRequest("Invalid inputs.");
            //}

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                // 名前に空文字は許可しない
                return JsonBadRequest($"A name of Preprocessing is NOT allowed to set empty string.");
            }
            if (model.PreprocessGitModel != null && model.PreprocessGitModel.IsValid() == false)
            {
                return JsonBadRequest($"The input about Git is not valid.");
            }
            if (model.TrainingGitModel != null && model.TrainingGitModel.IsValid() == false)
            {
                return JsonBadRequest($"The input about Git is not valid.");
            }



            ModelTemplate template = new ModelTemplate();
            var errorResult = await SetTemplateDetailsAsync(template, model);
            if (errorResult != null)
            {
                return errorResult;
            }
            template.Version = 1;


            if (template.AccessLevel == TemplateAccessLevel.Private)
            {
                //現在のテナントをアサイン   
                model.AssignedTenantId = CurrentUserInfo.SelectedTenant.Id;
                templateRepository.AssignTenant(template, (long)model.AssignedTenantId, true);

            }

            templateRepository.Add(template);
            unitOfWork.Commit();
            return JsonCreated(new IndexOutputModel(template));
        }

        /*
                /// <summary>
                /// バージョンを変更するテンプレート情報の編集
                /// </summary>
                /// <remarks>
                /// バージョンを上げる編集では前処理コンテナ・学習コンテナの編集を扱う
                /// </remarks>
                /// <param name="id">変更対象のテンプレートID</param>
                /// <param name="model">変更内容</param>
                [HttpPost("admin/templates/{id}")]
                [PermissionFilter(MenuCode.Template)]
                [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
                public async Task<IActionResult> PostEdit(long? id, [FromBody] CreateInputModel model)
                {
                    // データの入力チェック
                    if (!ModelState.IsValid || !id.HasValue)
                    {
                        return JsonBadRequest("Invalid inputs.");
                    }
                    // データの存在チェック
                    var template
                        = await templateRepository.GetByIdAsync(id.Value);
                    if (template == null)
                    {
                        return JsonNotFound($"Template ID {id.Value} is not found.");
                    }

                    var errorResult = await SetTemplateDetailsAsync(template, model);
                    if (errorResult != null)
                    {
                        return errorResult;
                    }
                    unitOfWork.Commit();

                    return JsonOK(new IndexOutputModel(template));
                }


                /// <summary>
                /// バージョンを変更しないテンプレート情報の編集
                /// </summary>
                /// <remarks>
                /// 同バージョンの編集ではテンプレートが実行済みの場合でも編集可能な項目のみ扱う
                /// </remarks>
                /// <param name="id">変更対象のテンプレートID</param>
                /// <param name="model">変更内容</param>
                [HttpPatch("admin/templates/{id}")]
                [PermissionFilter(MenuCode.Template)]
                [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
                public async Task<IActionResult> Edit(long? id, [FromBody] EditInputModel model)
                {
                    // データの入力チェック
                    if (!ModelState.IsValid || !id.HasValue)
                    {
                        return JsonBadRequest("Invalid inputs.");
                    }
                    // データの存在チェック
                    var template
                        = await templateRepository.GetByIdAsync(id.Value);
                    if (template == null)
                    {
                        return JsonNotFound($"Template ID {id.Value} is not found.");
                    }

                    if (model.Name != null)
                    {
                        if (string.IsNullOrWhiteSpace(model.Name))
                        {
                            // 名前に空文字は許可しない
                            return JsonBadRequest($"A name of Template is NOT allowed to set empty string.");
                        }
                        template.Name = model.Name;
                    }
                    template.Memo = EditColumn(model.Memo, template.Memo);

                    if (template.AccessLevel != TemplateAccessLevel.Disabled)
                    {
                        // まずは全てのアサイン情報を削除する
                        templateRepository.ResetAssinedTenants((long)id);
                        if (template.AccessLevel == TemplateAccessLevel.Private)
                        {
                            //現在のテナントをアサイン   
                            model.AssignedTenantId = CurrentUserInfo.SelectedTenant.Id;
                            templateRepository.AssignTenant(template, (long)model.AssignedTenantId, true);

                        }


                    }

                    unitOfWork.Commit();

                    return JsonOK(new IndexOutputModel(template));
                }
        */

        /// <summary>
        /// テンプレートを削除する。
        /// </summary>
        /// <remarks>
        /// 一度でもテンプレートが実行されていた場合、削除不可
        /// </remarks>
        /// <param name="id">テンプレートID</param>
        [HttpDelete("admin/templates/{id}")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id)
        {
            // データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            // データの存在チェック
            var template = await templateRepository.GetByIdAsync(id.Value);
            if (template == null)
            {
                return JsonNotFound($"Template ID {id.Value} is not found.");
            }

            //テンプレート実行履歴の有無をチェック
            var history = experimentHistoryRepository.Find(t => t.TemplateId == id.Value);
            if (history != null)
            {
                // 過去にテンプレートを実行済みなので、削除できない
                return JsonConflict($"Template ID {id.Value} is already executed. Experiment History ID = {history.Id} ");
            }

            templateRepository.Delete(template);
            unitOfWork.Commit();

            return JsonNoContent();
        }
        /// <summary>
        /// 引数のテンプレートインスタンスに、入力モデルの値を詰め込む。
        /// 成功時はnullを返す。エラーが発生したらエラー内容を返す。
        /// 事前に<see cref="CreateInputModel.PreprocessGitModel"/>の入力チェックを行っておくこと。
        /// </summary>
        /// <param name="template">テンプレート</param>
        /// <param name="model">入力内容</param>
        private async Task<IActionResult> SetTemplateDetailsAsync(ModelTemplate template, CreateInputModel model)
        {
            // 各リソースの超過チェック
            Tenant tenant = tenantRepository.Get(CurrentUserInfo.SelectedTenant.Id);
            string preprocessErrorMessage = clusterManagementLogic.CheckQuota(tenant, model.PreprocessCpu, model.PreprocessMemory, model.PreprocessGpu);
            if (preprocessErrorMessage != null)
            {
                return JsonBadRequest(preprocessErrorMessage);
            }
            string trainingErrorMessage = clusterManagementLogic.CheckQuota(tenant, model.TrainingCpu, model.TrainingMemory, model.TrainingGpu);
            if (trainingErrorMessage != null)
            {
                return JsonBadRequest(trainingErrorMessage);
            }

            long? preprocessGitId = null;
            string preprocessRepository = null;
            string preprocessOwner = null;
            string preprocessBranch = null;
            string preprocessCommitId = null;
            long? trainingGitId = null;
            string trainingRepository = null;
            string trainingOwner = null;
            string trainingBranch = null;
            string trainingCommitId = null;

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
                trainingGitId = model.TrainingGitModel.GitId ?? CurrentUserInfo.SelectedTenant.DefaultGit?.Id;
                trainingRepository = model.TrainingGitModel.Repository;
                trainingOwner = model.TrainingGitModel.Owner;
                trainingBranch = model.TrainingGitModel.Branch;
                trainingCommitId = model.TrainingGitModel.CommitId;
                // コミットIDが指定されていなければ、ブランチのHEADからコミットIDを取得する
                if (string.IsNullOrEmpty(trainingCommitId))
                {
                    trainingCommitId = await gitLogic.GetCommitIdAsync(trainingGitId.Value, model.TrainingGitModel.Repository, model.TrainingGitModel.Owner, model.TrainingGitModel.Branch);
                    if (string.IsNullOrEmpty(trainingCommitId))
                    {
                        // コミットIDが特定できなかったらエラー
                        return JsonNotFound($"The branch {trainingBranch} for {trainingGitId.Value}/{model.TrainingGitModel.Owner}/{ model.TrainingGitModel.Repository} is not found.");
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
            long? trainingRegistryId = null;
            string trainingImage = null;
            string trainingTag = null;
            if (model.TrainingContainerImage != null)
            {
                trainingRegistryId = model.TrainingContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id;
                trainingImage = model.TrainingContainerImage.Image;
                trainingTag = model.TrainingContainerImage.Tag;
            }

            template.Name = model.Name;
            template.Memo = model.Memo;
            template.Version += 1;
            template.GroupId = model.GroupId;
            template.AccessLevel = (TemplateAccessLevel)model.AccessLevel;
            template.PreprocessEntryPoint = model.PreprocessEntryPoint;
            template.PreprocessContainerRegistryId = preprocessRegistryId;
            template.PreprocessContainerImage = preprocessImage;
            template.PreprocessContainerTag = preprocessTag; // latestは運用上使用されていないハズなので、そのまま直接代入
            template.PreprocessRepositoryGitId = preprocessGitId;
            template.PreprocessRepositoryName = preprocessRepository;
            template.PreprocessRepositoryOwner = preprocessOwner;
            template.PreprocessRepositoryBranch = preprocessBranch;
            template.PreprocessRepositoryCommitId = preprocessCommitId;
            template.PreprocessCpu = model.PreprocessCpu;
            template.PreprocessMemory = model.PreprocessMemory;
            template.PreprocessGpu = model.PreprocessGpu;

            template.TrainingEntryPoint = model.TrainingEntryPoint;
            template.TrainingContainerRegistryId = trainingRegistryId;
            template.TrainingContainerImage = trainingImage;
            template.TrainingContainerTag = trainingTag; // latestは運用上使用されていないハズなので、そのまま直接代入
            template.TrainingRepositoryGitId = trainingGitId;
            template.TrainingRepositoryName = trainingRepository;
            template.TrainingRepositoryOwner = trainingOwner;
            template.TrainingRepositoryBranch = trainingBranch;
            template.TrainingRepositoryCommitId = trainingCommitId;
            template.TrainingCpu = model.TrainingCpu;
            template.TrainingMemory = model.TrainingMemory;
            template.TrainingGpu = model.TrainingGpu;

            return null;
        }
        #endregion
        #region new
        private async Task<IActionResult> SetTemplateDetails2(TemplateVersion templateVersion, Template template, VersionCreateInputModel model)
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
        [HttpGet("admin/templates2")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel2>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllTemplatesForAdmin2([FromQuery] int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            var templates = template2Repository.GetAll();
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

            return JsonOK(templates.Select(t => new IndexOutputModel2(t)));
        }

        /// <summary>
        /// 接続中のテナントに有効なテンプレート一覧を取得する
        /// </summary>
        [HttpGet("tenant/templates2")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel2>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllTemplates2(bool withTotal = false)
        {
            var templates = template2Repository.GetAll()
                .Where(x => (x.AccessLevel == TemplateAccessLevel.Private && x.CreaterTenantId == multiTenancyLogic.TenantId)
                || (x.AccessLevel == TemplateAccessLevel.Public));
            templates = templates.OrderByDescending(t => t.Id);
            if (withTotal)
            {
                //テンプレートの場合は件数が少ない想定なので、別のSQLを投げずにカウントしてしまう
                SetTotalCountToHeader(templates.Count());
            }
            return JsonOK(templates.Select(t => new IndexOutputModel2(t)));
        }

        /// <summary>
        /// テンプレートを作成する
        /// </summary>
        [HttpPost("admin/templates2")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType(typeof(IndexOutputModel2), (int)HttpStatusCode.Created)]
        public IActionResult Create2([FromBody] CreateInputModel2 model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            var userInfo = multiTenancyLogic.CurrentUserInfo;
            if (userInfo == null)
            {
                return JsonBadRequest("Couldn't get activated user informations");
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
                CreaterUserId = userInfo.Id,
                CreaterTenantId = userInfo.SelectedTenant.Id,
            };
            template2Repository.Add(template);
            unitOfWork.Commit();
            return JsonCreated(new IndexOutputModel2(template));
        }

        /// <summary>
        /// テンプレートを取得する
        /// </summary>
        /// <param name="id">テンプレートID</param>
        [HttpGet("admin/templates2/{id}")]
        [PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IndexOutputModel2), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTemplateDetail(long id)
        {
            var template = await template2Repository.GetByIdAsync(id);
            if (template == null)
            {
                return JsonNotFound($"Template ID {id} is not found.");
            }

            var result = new IndexOutputModel2(template);
            return JsonOK(result);
        }

        /// <summary>
        /// テンプレートバージョンを作成する
        /// </summary>
        [HttpPost("admin/templates2/{id}/versions")]
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

            var template = await template2Repository.GetByIdAsync(id);
            if (template == null)
            {
                return JsonNotFound($"Template ID {id} is not found.");
            }
            template.LatestVersion += 1;
            template2Repository.Update(template);

            var templateVersion = new TemplateVersion();
            var errorResult = await SetTemplateDetails2(templateVersion, template, model);
            if (errorResult != null)
            {
                return errorResult;
            }

            template2Repository.Add(templateVersion);
            unitOfWork.Commit();
            return JsonCreated(new VersionIndexOutputModel(templateVersion));
        }

        /// <summary>
        /// テンプレートバージョン一覧を取得する
        /// </summary>
        /// <param name="id">取得するテンプレートのID</param>
        [HttpGet("admin/templates2/{id}/versions")]
        [ProducesResponseType(typeof(IEnumerable<VersionIndexOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTemplateVersionList(long id)
        {
            var template = await template2Repository.GetTemplateWithVersionsAsync(id);
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
        [HttpGet("admin/templates2/{id}/versions/{versionId}")]
        [PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(VersionDetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTemplateVersion(long id, long versionId)
        {
            var tempalteVersion = await template2Repository.GetTemplateVersionAsync(id, versionId);
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
        [HttpPut("admin/templates2/{id}")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditTemplate(long id, [FromBody] EditInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            var template = await template2Repository.GetByIdAsync(id);
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

            return JsonOK(new IndexOutputModel2(template));
        }

        #endregion
    }
}
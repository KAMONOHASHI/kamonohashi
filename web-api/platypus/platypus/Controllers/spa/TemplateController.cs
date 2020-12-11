using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nssol.Platypus.ApiModels.TemplateApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System;
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
        private readonly ITenantRepository tenantRepository;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGitLogic gitLogic;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TemplateController(
            ITemplateRepository templateRepository,
            ITenantRepository tenantRepository,
            IGitLogic gitLogic,
            IUnitOfWork unitOfWork,
            IClusterManagementLogic clusterManagementLogic,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.templateRepository = templateRepository;
            this.tenantRepository = tenantRepository;
            this.unitOfWork = unitOfWork;
            this.clusterManagementLogic = clusterManagementLogic;
            this.gitLogic = gitLogic;
        }

        /// <summary>
        /// 全テンプレート一覧を取得
        /// </summary>
        [HttpGet("admin/templates")]
        [PermissionFilter(MenuCode.Template)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllTemplatesForAdmin([FromQuery] int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            var templates = templateRepository.GetAll();

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
        //[HttpGet("tenant/templates")]
        //[PermissionFilter(MenuCode.Template)]
        //[ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        //public IActionResult GetTemplatesByGroupId([FromQuery] int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        //{
        //    var templates = templateRepository.GetAccessibleTemplates(CurrentUserInfo.SelectedTenant.Id);

        //    if (withTotal)
        //    {
        //        //テンプレートの場合は件数が少ない想定なので、別のSQLを投げずにカウントしてしまう
        //        SetTotalCountToHeader(templates.Count());
        //    }

        //    //if (perPage.HasValue)
        //    //{
        //    //    templates = templates.Paging(page, perPage.Value);
        //    //}
        //    return JsonOK(templates.Select(t => new IndexOutputModel(t)));
        //}

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

        /// <summary>
        /// 指定されたIDのテンプレート情報を取得。
        /// </summary>
        /// <param name="id">テンプレートID</param>
        /// <param name="templateTenantMapRepository">DI用</param>
        [HttpGet("admin/templates/{id}")]
        [PermissionFilter(MenuCode.Template)]
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
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

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


            if (template.AccessLevel != TemplateAccessLevel.Disabled)
            {
                if (template.AccessLevel == TemplateAccessLevel.Private)
                {
                    //テナントをアサイン
                    if (model.AssignedTenantIds != null)
                    {
                        foreach (long tenantId in model.AssignedTenantIds)
                        {
                            Tenant tenant = tenantRepository.Get(tenantId);
                            if (tenant == null)
                            {
                                return JsonNotFound($"Tenant ID {tenantId} is not found.");
                            }
                            await clusterManagementLogic.UpdateTenantEnabledLabelAsync(template.Name, tenant.Name, true);
                        }
                        templateRepository.AssignTenants(template, model.AssignedTenantIds, true);
                    }
                }
                else
                {
                    // アクセスレベルが "Public" の場合、全てのテナントをアサイン
                    var tenants = tenantRepository.GetAllTenants();
                    foreach (Tenant tenant in tenants)
                    {
                        await clusterManagementLogic.UpdateTenantEnabledLabelAsync(template.Name, tenant.Name, true);
                    }
                }
            }
            templateRepository.Add(template);
            unitOfWork.Commit();
            return JsonCreated(new IndexOutputModel(template));
        }

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
                if (template.AccessLevel == TemplateAccessLevel.Private)
                {
                    //テナントをアサイン
                    if (model.AssignedTenantIds != null)
                    {
                        foreach (long tenantId in model.AssignedTenantIds)
                        {
                            Tenant tenant = tenantRepository.Get(tenantId);
                            if (tenant == null)
                            {
                                return JsonNotFound($"Tenant ID {tenantId} is not found.");
                            }
                            await clusterManagementLogic.UpdateTenantEnabledLabelAsync(template.Name, tenant.Name, true);
                        }
                        templateRepository.AssignTenants(template, model.AssignedTenantIds, true);
                    }
                }
                else
                {
                    // アクセスレベルが "Public" の場合、全てのテナントをアサイン
                    var tenants = tenantRepository.GetAllTenants();
                    foreach (Tenant tenant in tenants)
                    {
                        await clusterManagementLogic.UpdateTenantEnabledLabelAsync(template.Name, tenant.Name, true);
                    }
                }
            }

            unitOfWork.Commit();

            return JsonOK(new IndexOutputModel(template));
        }

        /// <summary>
        /// テンプレートを削除する。
        /// </summary>
        /// <remarks>
        /// 一度でも前処理が実行されていた場合、削除不可
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

            //TODO テンプレート実行履歴の有無をチェック
            //var history = templateHistoryRepository.Find(p => p.PreprocessId == id.Value);
            //if (history != null)
            //{
            //    // 過去にテンプレートを実行済みなので、削除できない
            //    return JsonConflict($"Template ID {id.Value} is already executed. History ID = {history.Id} ");
            //}

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

    }
}

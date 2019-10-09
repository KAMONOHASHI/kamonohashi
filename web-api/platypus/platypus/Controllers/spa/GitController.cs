using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.GitApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    [Route("api/v1/git")]
    public class GitController : PlatypusApiControllerBase
    {
        private readonly IGitLogic gitLogic;
        private readonly ITenantRepository tenantRepository;
        private readonly IGitRepository gitRepository;
        private readonly IUnitOfWork unitOfWork;

        public GitController(
            IGitLogic gitLogic,
            ITenantRepository tenantRepository,
            IGitRepository gitRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.gitLogic = gitLogic;
            this.tenantRepository = tenantRepository;
            this.gitRepository = gitRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Gitエンドポイント登録
        /// <summary>
        /// 登録済みのGitエンドポイント一覧を取得
        /// </summary>
        [HttpGet("/api/v1/admin/git/endpoints")]
        [Filters.PermissionFilter(MenuCode.Git, MenuCode.Tenant)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var gitEndpoints = gitRepository.GetGitAll();

            return JsonOK(gitEndpoints.Select(g => new IndexOutputModel(g)));
        }

        /// <summary>
        /// Git種別一覧を取得
        /// </summary>
        [HttpGet("/api/v1/admin/git/types")]
        [Filters.PermissionFilter(MenuCode.Git)]
        [ProducesResponseType(typeof(IEnumerable<EnumInfo>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllTypes()
        {
            var gitTypes = Enum.GetValues(typeof(GitServiceType)) as GitServiceType[];
            //Noneは除外して返却
            return JsonOK(gitTypes.Where(r => r != GitServiceType.None).Select(g => new EnumInfo() { Id = (int)g, Name = g.ToString() }));
        }

        /// <summary>
        /// 指定されたIDのGitエンドポイント情報を取得。
        /// </summary>
        /// <param name="id">GitエンドポイントID</param>
        [HttpGet("/api/v1/admin/git/endpoints/{id}")]
        [Filters.PermissionFilter(MenuCode.Git)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetails(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Git ID is required.");
            }

            Git git = await gitRepository.GetByIdAsync(id.Value);
            if (git == null)
            {
                return JsonNotFound($"Git Id {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(git);

            return JsonOK(model);
        }

        /// <summary>
        /// 新規にGitエンドポイントを登録する
        /// </summary>
        [HttpPost("/api/v1/admin/git/endpoints")]
        [Filters.PermissionFilter(MenuCode.Git)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public IActionResult Create([FromBody]CreateInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            Git git = new Git()
            {
                Name = model.Name,
                ServiceType = model.ServiceType.Value,
                ApiUrl = model.ApiUrl,
                RepositoryUrl = model.RepositoryUrl,
            };

            gitRepository.Add(git);
            unitOfWork.Commit();

            var result = new IndexOutputModel(git);

            return JsonOK(result);
        }

        /// <summary>
        /// Gitエンドポイント情報の編集
        /// </summary>
        [HttpPut("/api/v1/admin/git/endpoints/{id}")]
        [Filters.PermissionFilter(MenuCode.Git)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody]CreateInputModel model) //EditとCreateで項目が同じなので、入力モデルを使いまわし
        {
            //データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var git = await gitRepository.GetByIdAsync(id.Value);
            if (git == null)
            {
                return JsonNotFound($"Git ID {id.Value} is not found.");
            }
            //データの編集可否チェック
            if (git.IsNotEditable)
            {
                return JsonBadRequest($"Git ID {id.Value} is not allowed to edit.");
            }

            git.Name = model.Name;
            git.ServiceType = model.ServiceType.Value;
            git.ApiUrl = model.ApiUrl;
            git.RepositoryUrl = model.RepositoryUrl;

            gitRepository.Update(git);
            unitOfWork.Commit();

            return JsonOK(new IndexOutputModel(git));
        }

        /// <summary>
        /// Gitエンドポイント情報の削除
        /// </summary>
        [HttpDelete("/api/v1/admin/git/endpoints/{id}")]
        [Filters.PermissionFilter(MenuCode.Git)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id,
            [FromServices] DataAccess.Repositories.Interfaces.TenantRepositories.ITrainingHistoryRepository trainingHistoryRepository)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var git = await gitRepository.GetByIdAsync(id.Value);
            if (git == null)
            {
                return JsonNotFound($"Git ID {id.Value} is not found.");
            }
            //データの編集可否チェック
            if (git.IsNotEditable)
            {
                return JsonBadRequest($"Git ID {id.Value} is not allowed to delete.");
            }

            //このGitを登録しているテナントがいた場合、削除はできない
            var tenant = gitRepository.GetTenant(git.Id);
            if (tenant != null)
            {
                return JsonConflict($"Git {git.Id}:{git.Name} is used at Tenant {tenant.Id}:{tenant.Name}.");
            }
            //このGitを使った履歴がある場合、削除はできない
            var training = trainingHistoryRepository.Find(t => t.ModelGitId == git.Id);
            if (training != null)
            {
                return JsonConflict($"Git {git.Id}:{git.Name} is used at training {training.Id} in Tenant {training.TenantId}.");
            }

            gitRepository.Delete(git);
            unitOfWork.Commit();

            return JsonNoContent();
        }
        #endregion

        #region Gitリポジトリアクセス
        /// <summary>
        /// 全てのリポジトリを取得する
        /// </summary>
        [HttpGet("{gitId}/repos")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<ServiceModels.Git.RepositoryModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllRepository([FromRoute] long gitId)
        {
            long? selectedGitId = gitId == -1 ? CurrentUserInfo.SelectedTenant.DefaultGitId : gitId;
            if (selectedGitId == null)
            {
                return JsonNotFound($"There is no git server you can use. Please contact a user administrator.");
            }

            var result = await gitLogic.GetAllRepositoriesAsync(selectedGitId.Value);
            if(result.IsSuccess == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to access a git service: {result.Error}");
            }
            return JsonOK(result.Value);
        }

        /// <summary>
        /// ブランチ一覧を取得する
        /// </summary>
        /// <param name="gitId">GitId</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        [HttpGet("{gitId}/repos/{owner}/{repositoryName}/branches")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<ServiceModels.Git.BranchModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllBranchAsync([FromRoute] long gitId, [FromRoute] string owner, [FromRoute] string repositoryName)
        {
            long? selectedGitId = gitId == -1 ? CurrentUserInfo.SelectedTenant.DefaultGitId : gitId;
            if (selectedGitId == null)
            {
                return JsonNotFound($"There is no git server you can use. Please contact a user administrator.");
            }
            var result = await gitLogic.GetAllBranchesAsync(selectedGitId.Value, repositoryName, owner);
            if (result.IsSuccess == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to access a git service: {result.Error}");
            }
            if(result.Value == null)
            {
                return JsonNotFound($"Repository {owner}/{repositoryName} is not found.");
            }
            return JsonOK(result.Value);
        }

        /// <summary>
        /// コミット一覧を取得する
        /// </summary>
        /// <param name="gitId">GitId</param>
        /// <param name="repositoryName">リポジトリ名</param>
        /// <param name="owner">オーナー名</param>
        /// <param name="branch">ブランチ名</param>
        [HttpGet("{gitId}/repos/{owner}/{repositoryName}/commits")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<ServiceModels.Git.CommitModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCommitAsync([FromRoute] long gitId, [FromRoute] string owner, [FromRoute] string repositoryName, string branch)
        {
            long? selectedGitId = gitId == -1 ? CurrentUserInfo.SelectedTenant.DefaultGitId : gitId;
            if (selectedGitId == null)
            {
                return JsonNotFound($"There is no git server you can use. Please contact a user administrator.");
            }
            var result = await gitLogic.GetAllCommitsAsync(selectedGitId.Value, repositoryName, owner, branch);
            if (result.IsSuccess == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to access a git service: {result.Error}");
            }
            if (result.Value == null)
            {
                return JsonNotFound($"Repository {owner}/{repositoryName} is not found.");
            }
            return JsonOK(result.Value);
        }

        /// <summary>
        /// 階層化されたURLを吸収するためのダミーAPI。
        /// 製品版のSwaggerからは削除する。
        /// </summary>
        [HttpGet("{gitId}/repos/{*segments}")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Preprocess, MenuCode.Inference, MenuCode.Notebook)]
        public async Task<IActionResult> AllocatieRoute([FromRoute] long gitId, [FromRoute] string segments, [FromQuery] string branch)
        {
            string[] segmentsArray = segments.Split('/');

            if(segmentsArray.Length < 3)
            {
                return JsonNotFound();
            }

            //最後の一つがリソース
            string resource = segmentsArray[segmentsArray.Length - 1];
            //その一つ前がリポジトリ
            string repository = segmentsArray[segmentsArray.Length - 2];
            //それ以外がオーナー
            string owner = string.Join("/", segmentsArray.Take(segmentsArray.Length - 2));

            switch(resource)
            {
                case "branches":
                    return await GetAllBranchAsync(gitId, owner, repository);
                case "commits":
                    return await GetAllCommitAsync(gitId, owner, repository, branch);
                default:
                    return JsonNotFound();
            }
        }

        #endregion
    }
}

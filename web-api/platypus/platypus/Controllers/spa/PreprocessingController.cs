using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.PreprocessingApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// 前処理管理を扱うためのAPI集
    /// </summary>
    [ApiController]
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/preprocessings")]
    public class PreprocessingController : PlatypusApiControllerBase
    {
        private readonly IPreprocessRepository preprocessRepository;
        private readonly IPreprocessHistoryRepository preprocessHistoryRepository;
        private readonly ITenantRepository tenantRepository;
        private readonly IDataRepository dataRepository;
        private readonly IPreprocessLogic preprocessLogic;
        private readonly ITagLogic tagLogic;
        private readonly IGitLogic gitLogic;
        private readonly IStorageLogic storageLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PreprocessingController(
            IPreprocessRepository preprocessRepository,
            IPreprocessHistoryRepository preprocessHistoryRepository,
            ITenantRepository tenantRepository,
            IDataRepository dataRepository,
            IPreprocessLogic preprocessLogic,
            ITagLogic tagLogic,
            IGitLogic gitLogic,
            IStorageLogic storageLogic,
            IClusterManagementLogic clusterManagementLogic,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.preprocessRepository = preprocessRepository;
            this.preprocessHistoryRepository = preprocessHistoryRepository;
            this.tenantRepository = tenantRepository;
            this.dataRepository = dataRepository;
            this.preprocessLogic = preprocessLogic;
            this.tagLogic = tagLogic;
            this.gitLogic = gitLogic;
            this.storageLogic = storageLogic;
            this.clusterManagementLogic = clusterManagementLogic;
            this.unitOfWork = unitOfWork;
        }

        #region 前処理

        /// <summary>
        /// 指定された条件でページングされた状態で、全前処理を取得
        /// </summary>
        /// <param name="page">ページ番号。デフォルトは1。</param>
        /// <param name="perPage">表示件数。指定がない場合は全件。</param>
        /// <param name="filter">検索条件</param>
        /// <param name="withTotal">合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。</param>
        [HttpGet]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll([FromQuery]SearchInputModel filter, [FromQuery]int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            var preprocessings = preprocessRepository.GetAllWithOrderby(p => p.Id, false);
            preprocessings = Search(preprocessings, filter);

            // 未指定、あるいは1000件以上であれば、1000件に指定
            int pageCount = (perPage.HasValue && perPage.Value < 1000) ? perPage.Value : 1000;
            preprocessings = preprocessings.Paging(page, pageCount);

            if (withTotal)
            {
                int total = GetTotalCount(filter);
                SetTotalCountToHeader(total);
            }

            return JsonOK(preprocessings.Select(p => new IndexOutputModel(p)));
        }

        /// <summary>
        /// データ件数を取得する
        /// </summary>
        /// <param name="filter">検索条件</param>
        private int GetTotalCount(SearchInputModel filter)
        {
            IQueryable<Preprocess> preprocessings = preprocessRepository.GetAll();
            preprocessings = Search(preprocessings, filter);
            return preprocessings.Count();
        }

        /// <summary>
        /// 検索条件の追加
        /// </summary>
        /// <param name="sourceData">加工前の検索結果</param>
        /// <param name="filter">検索条件</param>
        private static IQueryable<Preprocess> Search(IQueryable<Preprocess> sourceData, SearchInputModel filter)
        {
            IQueryable<Preprocess> data = sourceData;
            data = data
                .SearchLong(d => d.Id, filter.Id)
                .SearchString(d => d.Name, filter.Name)
                .SearchTime(d => d.CreatedAt, filter.CreatedAt)
                .SearchString(d => d.Memo, filter.Memo);
            return data;
        }

        /// <summary>
        /// 指定されたIDの前処理の詳細情報を取得。
        /// </summary>
        /// <param name="id">前処理ID</param>
        [HttpGet("{id}")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Preprocessing ID is required.");
            }
            var preprocessing = await preprocessRepository.GetIncludeAllAsync(id.Value);
            if (preprocessing == null)
            {
                return JsonNotFound($"Preprocessing Id {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(preprocessing)
            {
                IsLocked = await preprocessHistoryRepository.ExistsAsync(p => p.PreprocessId == id.Value)
            };

            // Gitの表示用URLを作る
            if (preprocessing.RepositoryGitId != null)
            {
                model.GitModel.Url = gitLogic.GetTreeUiUrl(preprocessing.RepositoryGitId.Value, preprocessing.RepositoryName, preprocessing.RepositoryOwner, preprocessing.RepositoryCommitId);
            }

            return JsonOK(model);
        }

        /// <summary>
        /// 新規に前処理を登録する
        /// </summary>
        /// <param name="model">新規作成内容</param>
        [HttpPost]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody]CreateInputModel model)
        {
            // データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                // 名前に空文字は許可しない
                return JsonBadRequest($"A name of Preprocessing is NOT allowed to set empty string.");
            }
            if (model.GitModel != null && model.GitModel.IsValid() == false)
            {
                return JsonBadRequest($"The input about Git is not valid.");
            }
            // 各リソースの超過チェック
            Tenant tenant = tenantRepository.Get(CurrentUserInfo.SelectedTenant.Id);
            string errorMessage = clusterManagementLogic.CheckQuota(tenant, model.Cpu, model.Memory, model.Gpu);
            if (errorMessage != null)
            {
                return JsonBadRequest(errorMessage);
            }

            Preprocess preprocessing = new Preprocess();
            var errorResult = await SetPreprocessDetailsAsync(preprocessing, model);
            if(errorResult != null)
            {
                return errorResult;
            }

            preprocessRepository.Add(preprocessing);
            unitOfWork.Commit();

            return JsonCreated(new IndexOutputModel(preprocessing));
        }

        /// <summary>
        /// 前処理の編集
        /// </summary>
        /// <remarks>
        /// 前処理が実行済みの場合でも編集可能な項目のみ扱う
        /// </remarks>
        /// <param name="id">変更対象の前処理ID</param>
        /// <param name="model">変更内容</param>
        [HttpPatch("{id}")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody]EditInputModel model)
        {
            // データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            // データの存在チェック
            var preprocessing = await preprocessRepository.GetByIdAsync(id.Value);
            if (preprocessing == null)
            {
                return JsonNotFound($"Preprocessing ID {id.Value} is not found.");
            }

            if (model.Name != null)
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    // 名前に空文字は許可しない
                    return JsonBadRequest($"A name of Preprocessing is NOT allowed to set empty string.");
                }
                preprocessing.Name = model.Name;
            }
            preprocessing.Memo = EditColumn(model.Memo, preprocessing.Memo);
            preprocessing.Cpu = EditColumn(model.Cpu, preprocessing.Cpu);
            preprocessing.Memory = EditColumn(model.Memory, preprocessing.Memory);
            preprocessing.Gpu = EditColumn(model.Gpu, preprocessing.Gpu);

            // 各リソースの超過チェック
            // 変更がない場合、変更内容モデルの項目にはnullが設定されているので、DB設定値を用いてここでチェックをする。
            Tenant tenant = tenantRepository.Get(CurrentUserInfo.SelectedTenant.Id);
            string errorMessage = clusterManagementLogic.CheckQuota(tenant, preprocessing.Cpu, preprocessing.Memory, preprocessing.Gpu);
            if (errorMessage != null)
            {
                return JsonBadRequest(errorMessage);
            }

            unitOfWork.Commit();

            return JsonOK(new IndexOutputModel(preprocessing));
        }

        /// <summary>
        /// 前処理の詳細情報編集
        /// </summary>
        /// <remarks>
        /// 全ての項目が対象だが、一度でも前処理が実行されていた場合、編集不可
        /// </remarks>
        /// <param name="id">変更対象の前処理ID</param>
        /// <param name="model">変更内容</param>
        [HttpPut("{id}")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditDetails(long? id, [FromBody]CreateInputModel model)
        {
            // データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            if (model.GitModel != null && model.GitModel.IsValid() == false)
            {
                return JsonBadRequest($"The input about Git is not valid.");
            }

            // データの存在チェック
            var preprocessing = await preprocessRepository.GetIncludeAllAsync(id.Value);
            if (preprocessing == null)
            {
                return JsonNotFound($"Preprocessing ID {id.Value} is not found.");
            }

            var history = preprocessHistoryRepository.Find(p => p.PreprocessId == id.Value);
            if (history != null)
            {
                // 過去に前処理を実行済みなので、編集できない
                return JsonConflict($"Preprocessing ID {id.Value} is already executed. History ID = {history.Id} ");
            }

            var errorResult = await SetPreprocessDetailsAsync(preprocessing, model);
            if (errorResult != null)
            {
                return errorResult;
            }
            unitOfWork.Commit();

            return JsonOK(new IndexOutputModel(preprocessing));
        }

        /// <summary>
        /// 前処理を削除する。
        /// </summary>
        /// <remarks>
        /// 一度でも前処理が実行されていた場合、削除不可
        /// </remarks>
        /// <param name="id">前処理ID</param>
        [HttpDelete("{id}")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id)
        {
            // データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            // データの存在チェック
            var preprocessing = await preprocessRepository.GetByIdAsync(id.Value);
            if (preprocessing == null)
            {
                return JsonNotFound($"Preprocessing ID {id.Value} is not found.");
            }

            var history = preprocessHistoryRepository.Find(p => p.PreprocessId == id.Value);
            if (history != null)
            {
                // 過去に前処理を実行済みなので、削除できない
                return JsonConflict($"Preprocessing ID {id.Value} is already executed. History ID = {history.Id} ");
            }

            preprocessRepository.Delete(preprocessing);
            unitOfWork.Commit();

            return JsonNoContent();
        }

        /// <summary>
        /// 引数の前処理インスタンスに、入力モデルの値を詰め込む。
        /// 成功時はnullを返す。エラーが発生したらエラー内容を返す。
        /// 事前に<see cref="CreateInputModel.GitModel"/>の入力チェックを行っておくこと。
        /// </summary>
        /// <param name="preprocessing">前処理</param>
        /// <param name="model">入力内容</param>
        private async Task<IActionResult> SetPreprocessDetailsAsync(Preprocess preprocessing, CreateInputModel model)
        {
            // 各リソースの超過チェック
            Tenant tenant = tenantRepository.Get(CurrentUserInfo.SelectedTenant.Id);
            string errorMessage = clusterManagementLogic.CheckQuota(tenant, model.Cpu, model.Memory, model.Gpu);
            if (errorMessage != null)
            {
                return JsonBadRequest(errorMessage);
            }

            long? gitId = null;
            string repository = null;
            string owner = null;
            string branch = null;
            string commitId = null;
            if (model.GitModel != null)
            {
                gitId = model.GitModel.GitId ?? CurrentUserInfo.SelectedTenant.DefaultGit?.Id;
                repository = model.GitModel.Repository;
                owner = model.GitModel.Owner;
                branch = model.GitModel.Branch;
                commitId = model.GitModel.CommitId;
                // コミットIDが指定されていなければ、ブランチのHEADからコミットIDを取得する
                if (string.IsNullOrEmpty(commitId))
                {
                    commitId = await gitLogic.GetCommitIdAsync(gitId.Value, model.GitModel.Repository, model.GitModel.Owner, model.GitModel.Branch);
                    if (string.IsNullOrEmpty(commitId))
                    {
                        // コミットIDが特定できなかったらエラー
                        return JsonNotFound($"The branch {branch} for {gitId.Value}/{model.GitModel.Owner}/{model.GitModel.Repository} is not found.");
                    }
                }
            }

            long? registryId = null;
            string image = null;
            string tag = null;
            if (model.ContainerImage != null)
            {
                registryId = model.ContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id;
                image = model.ContainerImage.Image;
                tag = model.ContainerImage.Tag;
            }

            preprocessing.Name = model.Name;
            preprocessing.EntryPoint = model.EntryPoint;
            preprocessing.ContainerRegistryId = registryId;
            preprocessing.ContainerImage = image;
            preprocessing.ContainerTag = tag; // latestは運用上使用されていないハズなので、そのまま直接代入
            preprocessing.RepositoryGitId = gitId;
            preprocessing.RepositoryName = repository;
            preprocessing.RepositoryOwner = owner;
            preprocessing.RepositoryBranch = branch;
            preprocessing.RepositoryCommitId = commitId;
            preprocessing.Memo = model.Memo;
            preprocessing.Cpu = model.Cpu;
            preprocessing.Memory = model.Memory;
            preprocessing.Gpu = model.Gpu;

            return null;
        }

        #endregion

        #region 前処理履歴

        /// <summary>
        /// 指定した前処理の履歴情報を取得する。
        /// </summary>
        /// <param name="id">前処理ID</param>
        [HttpGet("{id}/histories")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(IEnumerable<HistoriesOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHistories([FromRoute] long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Preprocessing ID is required.");
            }

            var preprocessingHistories = await preprocessHistoryRepository.GetPreprocessAllIncludeDataAndPreprocessAsync(id.Value);

            return JsonOK(preprocessingHistories.ToList().Select(ph => GetUpdatedIndexOutputModelAsync(ph, new HistoriesOutputModel(ph)).Result));
        }

        /// <summary>
        /// ステータスを更新して、出力モデルに変換する
        /// </summary>
        /// <param name="history">前処理履歴</param>
        /// <param name="model">出力モデル</param>
        private async Task<HistoriesOutputModel> GetUpdatedIndexOutputModelAsync(PreprocessHistory history, HistoriesOutputModel model)
        {
            var status = ContainerStatus.Convert(history.Status);
            model.StatusType = status.StatusType;
            if (status.Exist())
            {
                // コンテナがまだ存在している場合、情報を更新する
                var newStatus = await clusterManagementLogic.GetContainerStatusAsync(history.Name, CurrentUserInfo.SelectedTenant.Name, false);

                if (status.Key != newStatus.Key)
                {
                    // 更新があったので、変更処理
                    history.Status = newStatus.Key;
                    unitOfWork.Commit();

                    model.Status = newStatus.Name;
                    model.StatusType = newStatus.StatusType;
                }
            }
            return model;
        }

        /// <summary>
        /// 指定されたデータに対する前処理の履歴を取得。
        /// </summary>
        /// <param name="id">前処理ID</param>
        /// <param name="dataId">データID</param>
        [HttpGet("{id}/histories/{dataId}")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(HistoryDetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetailHistory([FromRoute] long id, [FromRoute] long? dataId)
        {
            if (dataId == null)
            {
                return JsonBadRequest("Data ID is required.");
            }

            var history = await preprocessHistoryRepository.GetPreprocessIncludeDataAndPreprocessAsync(id, dataId.Value);
            if (history == null)
            {
                return JsonNotFound($"Preprocessing History about Preprocess {id} to Data {dataId} is not found.");
            }

            var result = new HistoryDetailsOutputModel(history);
            result = await GetUpdatedIndexOutputModelAsync(history, result) as HistoryDetailsOutputModel;

            result.OutputDataIds = preprocessHistoryRepository.GetPreprocessOutputs(history.Id);
            return JsonOK(result);
        }


        /// <summary>
        /// 前処理履歴のイベントを取得する
        /// </summary>
        /// <param name="id">前処理ID</param>
        /// <param name="dataId">入力データID</param>
        [HttpGet("{id}/histories/{dataId}/events")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(HistoriesOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadPreprocessImage([FromRoute]long id, [FromRoute]long dataId)
        {
            var history = await preprocessHistoryRepository.GetPreprocessIncludeDataAndPreprocessAsync(id, dataId);
            if (history == null)
            {
                return JsonNotFound($"Preprocessing History about Preprocess {id} to Data {dataId} is not found.");
            }

            var status = ContainerStatus.Convert(history.Status);
            if (status.Exist() == false)
            {
                return JsonBadRequest($"A container for the preprocessing does not exist.");
            }

            var events = await clusterManagementLogic.GetEventsAsync(CurrentUserInfo.SelectedTenant, history.Name, false, true);

            if (events.IsSuccess == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to get container events: {events.Error}");
            }
            else
            {
                return JsonOK(events.Value);
            }
        }

        /// <summary>
        /// 前処理履歴を作成する。
        /// </summary>
        /// <remarks>
        /// 前処理実行用のコンテナなどは起動しない。ローカル環境など、KAMONOHASHI外で作成した前処理結果をアップロードする際に用いる。
        /// 作成された前処理履歴は実行中のステータスとなり、前処理結果の追加が可能な状態になる。
        /// </remarks>
        /// <param name="id">前処理ID</param>
        /// <param name="dataId">入力データID</param>
        [HttpPost("{id}/histories/{dataId}")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(HistoriesOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreatePreprocessHistory([FromRoute] long id, [FromRoute] long? dataId)
        {
            var result = await ValidateCreatePreprocessHistoryInputModelAsync(id, dataId);
            if (result.IsSuccess == false)
            {
                return result.Error;
            }
            PreprocessHistory preprocessHistory = result.Value;
            // ステータスはRunningではなくOpenedにする。
            preprocessHistory.Status = ContainerStatus.Opened.Key;

            preprocessHistoryRepository.Add(preprocessHistory);
            unitOfWork.Commit();

            return JsonCreated(new HistoriesOutputModel(preprocessHistory));
        }


        /// <summary>
        /// 前処理を実行し、履歴を作成する。
        /// </summary>
        /// <remarks>
        /// 前処理実行用のコンテナを起動する。
        /// 作成された前処理履歴は実行中のステータスとなり、前処理結果の追加が可能な状態になる。
        /// </remarks>
        /// <param name="id">前処理ID</param>
        /// <param name="model">実行設定</param>
        [HttpPost("{id}/run")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(HistoriesOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RunPreprocessHistory([FromRoute]long id, [FromBody] RunPreprocessHistoryInputModel model)
        {
            // 環境変数名のチェック
            if (model.Options != null && model.Options.Count > 0)
            {
                foreach (var env in model.Options)
                {
                    if (!string.IsNullOrEmpty(env.Key))
                    {
                        // フォーマットチェック
                        if (!Regex.IsMatch(env.Key, "^[-._a-zA-Z][-._a-zA-Z0-9]*$"))
                        {
                            return JsonNotFound($"Invalid envName. Please match the format of '^[-._a-zA-Z][-._a-zA-Z0-9]*$'.");
                        }
                    }
                }
            }

            // 各リソースの超過チェック
            Tenant tenant = tenantRepository.Get(CurrentUserInfo.SelectedTenant.Id);
            string errorMessage = clusterManagementLogic.CheckQuota(tenant, model.Cpu.Value, model.Memory.Value, model.Gpu.Value);
            if (errorMessage != null)
            {
                return JsonBadRequest(errorMessage);
            }

            var validateResult = await ValidateCreatePreprocessHistoryInputModelAsync(id, model.DataId);
            if (validateResult.IsSuccess == false)
            {
                return validateResult.Error;
            }
            PreprocessHistory preprocessHistory = validateResult.Value;
            preprocessHistory.Cpu = model.Cpu;
            preprocessHistory.Memory = model.Memory;
            preprocessHistory.Gpu = model.Gpu;
            preprocessHistory.Partition = model.Partition;
            preprocessHistory.OptionDic = model.Options ?? new Dictionary<string, string>(); // オプションはnullの可能性があるので、その時は初期化
            if (preprocessHistory.OptionDic.ContainsKey("")) // 空文字は除外する
            {
                preprocessHistory.OptionDic.Remove("");
            }

            preprocessHistoryRepository.Add(preprocessHistory);
            unitOfWork.Commit();

            // 入力データの詳細情報がないので、取得
            preprocessHistory.InputData = await dataRepository.GetDataIncludeAllAsync(preprocessHistory.InputDataId);

            var result = await clusterManagementLogic.RunPreprocessingContainerAsync(preprocessHistory);
            if (result.IsSuccess == false)
            {
                // コンテナの起動に失敗した状態。エラーを出力して、保存した学習履歴も削除する。
                preprocessHistoryRepository.Delete(preprocessHistory);

                return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to run preprocessing. The message bellow may be help to resolve: " + result.Error);
            }

            // 結果に従い、学習結果を更新する。
            // 実行には時間がかかりうるので、DBから最新の情報を取ってくる
            preprocessHistory = await preprocessHistoryRepository.GetByIdAsync(preprocessHistory.Id);
            preprocessHistory.Status = result.Value.Status.Key;
            unitOfWork.Commit();

            if (result.Value.Status.Succeed())
            {
                return JsonCreated(new HistoriesOutputModel(preprocessHistory));
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run preprocessing. Status={result.Value.Status.Name}. Please contact your server administrator.");
            }
        }

        /// <summary>
        /// 前処理履歴作成の入力モデルのチェックを行う。
        /// </summary>
        /// <param name="preprocessId">前処理ID</param>
        /// <param name="inputDataId">入力データID</param>
        private async Task<Result<PreprocessHistory, IActionResult>> ValidateCreatePreprocessHistoryInputModelAsync(long preprocessId, long? inputDataId)
        {
            if (inputDataId == null)
            {
                return Result<PreprocessHistory, IActionResult>.CreateErrorResult(JsonBadRequest("Data ID is requried."));
            }

            if (!ModelState.IsValid)
            {
                return Result<PreprocessHistory, IActionResult>.CreateErrorResult(JsonBadRequest("Invalid Input"));
            }

            // データIDの存在確認
            var data = await dataRepository.GetByIdAsync(inputDataId.Value);
            if (data == null)
            {
                return Result<PreprocessHistory, IActionResult>.CreateErrorResult(JsonNotFound($"Data ID {inputDataId} is not found."));
            }

            // 前処理の存在確認
            var preprocess = await preprocessRepository.GetByIdAsync(preprocessId);
            if (preprocess == null)
            {
                return Result<PreprocessHistory, IActionResult>.CreateErrorResult(JsonNotFound($"Preprocessing ID {preprocessId} is not found."));
            }

            // 実行コマンドが指定されていない前処理は起動不能(空白だけでもアウト)
            if (string.IsNullOrWhiteSpace(preprocess.EntryPoint))
            {
                return Result<PreprocessHistory, IActionResult>.CreateErrorResult(JsonNotFound($"Preprocessing {preprocess.Name} can not be run because of lack of an execution command."));
            }
            // イメージが指定されていない前処理は起動不能
            if (string.IsNullOrEmpty(preprocess.ContainerImage) || string.IsNullOrEmpty(preprocess.ContainerTag))
            {
                return Result<PreprocessHistory, IActionResult>.CreateErrorResult(JsonNotFound($"Preprocessing {preprocess.Name} can not be run because a container image has not been selected properly yet."));
            }

            // 前処理が既に実行中か確認する
            var preprocessHistory = preprocessHistoryRepository.Find(pph => pph.InputDataId == inputDataId && pph.PreprocessId == preprocess.Id);
            if (preprocessHistory != null)
            {
                string status = ContainerStatus.Convert(preprocessHistory.Status).Name;
                return Result<PreprocessHistory, IActionResult>.CreateErrorResult(JsonNotFound($"Data {data.Id}:{data.Name} has already been processed by {preprocess.Id}:{preprocess.Name}. Status:{status}"));
            }

            preprocessHistory = new PreprocessHistory()
            {
                InputDataId = data.Id,
                PreprocessId = preprocess.Id,
                Preprocess = preprocess,
                Status = ContainerStatus.Running.Key
            };

            return Result<PreprocessHistory, IActionResult>.CreateResult(preprocessHistory);
        }

        /// <summary>
        /// 前処理履歴に出力データを追加する。
        /// 追加する対象の前処理履歴は実行中のステータスのみ許可される。
        /// </summary>
        /// <param name="id">前処理ID</param>
        /// <param name="dataId">入力データID</param>
        /// <param name="model">データ情報</param>
        [HttpPost("{id}/histories/{dataId}/data")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(HistoriesOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadPreprocessImage([FromRoute]long id, [FromRoute]long dataId,
            [FromBody] AddOutputDataInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            // データの存在チェック
            var preprocessHistory = await preprocessHistoryRepository.GetPreprocessIncludeDataAndPreprocessAsync(id, dataId);
            if (preprocessHistory == null)
            {
                return JsonNotFound($"Preprocessing History about Preprocess {id} to Data {dataId} is not found.");
            }
            var status = ContainerStatus.Convert(preprocessHistory.Status);
            if (status.IsOpened() == false)
            {
                // 追加できるのは開放中のコンテナだけ（ローカルの結果を追加することがあるので、Runningとは限らない）
                return JsonBadRequest($"Preprocessing History {preprocessHistory.Id} is not opened.");
            }

            // データを追加する
            Data newData = new Data()
            {
                // データ名が未指定であれば、デフォルトの値を入れる
                Name = string.IsNullOrEmpty(model.Name) ? $"{preprocessHistory.InputData.Name}_{preprocessHistory.Preprocess.Name}" : model.Name,
                Memo = model.Memo,
                ParentDataId = preprocessHistory.InputDataId
            };
            dataRepository.Add(newData);

            foreach (var file in model.Files)
            {
                dataRepository.AddFile(newData, file.FileName, file.StoredPath);
            }

            // タグの登録
            if (model.Tags != null && model.Tags.Count() > 0)
            {
                tagLogic.CreateDataTags(newData, model.Tags);
            }
            else
            {
                // タグが未指定であれば、前処理名を付ける
                List<string> tags = new List<string>() { preprocessHistory.Preprocess.Name };
                tagLogic.CreateDataTags(newData, tags);
            }

            preprocessHistoryRepository.AddOutputData(preprocessHistory.Id, newData);

            unitOfWork.Commit();

            return JsonOK(new HistoriesOutputModel(preprocessHistory));
        }

        /// <summary>
        /// 前処理履歴の登録を異常終了させる。
        /// 前処理履歴はエラーのステータスとなり、前処理結果の追加が不可能な状態になる。
        /// </summary>
        /// <param name="id">前処理ID</param>
        /// <param name="dataId">入力データID</param>
        [HttpPost("{id}/histories/{dataId}/halt")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(HistoriesOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Halt([FromRoute] long id, [FromRoute]long dataId)
        {
            return await ExitAsync(id, dataId, ContainerStatus.Killed);
        }

        /// <summary>
        /// 前処理履歴の登録を完了する。
        /// 前処理履歴は完了のステータスとなり、前処理結果の追加が不可能な状態になる。
        /// </summary>
        /// <param name="id">前処理ID</param>
        /// <param name="dataId">入力データID</param>
        [HttpPost("{id}/histories/{dataId}/complete")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(HistoriesOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Complete([FromRoute] long id, [FromRoute]long dataId)
        {
            return await ExitAsync(id, dataId, ContainerStatus.Completed);
        }

        /// <summary>
        /// 前処理実行を終了させる。
        /// </summary>
        /// <param name="id">前処理ID</param>
        /// <param name="dataId">入力データID</param>
        /// <param name="newStatus">変更後のステータス</param>
        private async Task<IActionResult> ExitAsync(long id, long dataId, ContainerStatus newStatus)
        {
            // データの存在チェック
            var preprocessHistory = await preprocessHistoryRepository.GetPreprocessIncludeDataAndPreprocessAsync(id, dataId);
            if (preprocessHistory == null)
            {
                return JsonNotFound($"Preprocessing History about Preprocess {id} to Data {dataId} is not found.");
            }
            var status = ContainerStatus.Convert(preprocessHistory.Status);
            if (status.IsOpened() == false)
            {
                // 終了できるのは開放中のコンテナだけ（ローカルの結果を追加することがあるので、Runningとは限らない）
                return JsonBadRequest($"Preprocessing History {preprocessHistory.Id} is not opened.");
            }
            if (status.Exist())
            {
                // ジョブ実行履歴追加
                var tenant = CurrentUserInfo.SelectedTenant;
                var info = await clusterManagementLogic.GetContainerDetailsInfoAsync(preprocessHistory.Name, tenant.Name, false);
                var node = info.NodeName != null
                    ? (await clusterManagementLogic.GetAllNodesAsync()).FirstOrDefault(x => x.Name == info.NodeName)
                    : null;
                preprocessLogic.AddJobHistory(preprocessHistory, node, tenant, info, newStatus.Key);

                // コンテナが動いていれば、停止する
                await clusterManagementLogic.DeleteContainerAsync(
                    ContainerType.Preprocessing, preprocessHistory.Name, tenant.Name, false);
            }

            preprocessHistory.CompletedAt = DateTime.Now;
            preprocessHistory.Status = newStatus.Key;
            unitOfWork.Commit();

            return JsonOK(new HistoriesOutputModel(preprocessHistory));
        }

        /// <summary>
        /// 前処理履歴添付ファイルの一覧を取得する。
        /// </summary>
        /// <param name="id">対象の前処理履歴ID</param>
        /// <param name="dataId">入力データID</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        [HttpGet("{id}/histories/{dataId}/files")]
        [Filters.PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType(typeof(PreprocessAttachedFileOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAttachedFiles(long id, long dataId, bool withUrl)
        {
            // データの存在チェック
            var preprocessHistory = await preprocessHistoryRepository.GetPreprocessIncludeDataAndPreprocessAsync(id, dataId);
            if (preprocessHistory == null)
            {
                return JsonNotFound($"Preprocessing History {preprocessHistory.Id} is not opened.");
            }

            string fileName = $"preproc_stdout_stderr_{id}_{dataId}.log";

            var result = new PreprocessAttachedFileOutputModel(preprocessHistory.Id, fileName, -1)
            {
                Url = withUrl? storageLogic.GetPreSignedUriForGet(ResourceType.PreprocContainerAttachedFiles, $"{preprocessHistory.Id}/{fileName}", fileName, true).ToString() : null,
                IsLocked = true
            };
            return JsonOK(result);
        }

        /// <summary>
        /// 前処理履歴を削除する。生成された前処理済みデータもまとめて削除する。
        /// </summary>
        /// <param name="id">前処理ID</param>
        /// <param name="dataId">入力データID</param>
        [HttpDelete("{id}/histories/{dataId}")]
        [PermissionFilter(MenuCode.Preprocess)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteHistory([FromRoute] long id, [FromRoute] long? dataId)
        {
            if (dataId == null)
            {
                return JsonBadRequest("Data ID is requried.");
            }

            // データの存在チェック
            var preprocessHistory = await preprocessHistoryRepository.GetPreprocessIncludeDataAndPreprocessAsync(id, dataId.Value);
            if (preprocessHistory == null)
            {
                return JsonNotFound($"Preprocessing History about Preprocess {id} to Data {dataId} is not found.");
            }

            // 出力データを削除できるか、確認
            var lockedOutput = preprocessHistoryRepository.GetLockedOutput(preprocessHistory.Id);
            if(lockedOutput != null)
            {
                return JsonConflict($"Preprocessing History about Preprocess {id} to Data {dataId} can NOT delete. The output data {lockedOutput.Id} is locked.");
            }

            // 前処理履歴の削除
            bool result = await preprocessLogic.DeleteAsync(preprocessHistory, false);
            if (result)
            {
                return JsonNoContent();
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to delete Preprocessing History about Preprocess {id} to Data {dataId}. Please contact your server administrator.");
            }
        }

        #endregion
    }
}
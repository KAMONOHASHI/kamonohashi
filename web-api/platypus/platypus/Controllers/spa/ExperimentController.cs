using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.ApiModels.ExperimentApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Options;
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
    /// Experimentを扱うためのAPI集
    /// </summary>
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/experiment")]
    public class ExperimentController : PlatypusApiControllerBase
    {
        private readonly IExperimentHistoryRepository experimentHistoryRepository;
        private readonly IExperimentPreprocessHistoryRepository experimentPreprocessHistoryRepository;
        private readonly IInferenceHistoryRepository inferenceHistoryRepository;
        private readonly IExperimentTensorBoardContainerRepository tensorBoardContainerRepository;
        private readonly IAquariumDataSetRepository aquariumDataSetRepository;
        private readonly IDataSetRepository dataSetRepository;
        private readonly ITemplateRepository templateRepository;
        private readonly ITagRepository tagRepository;
        private readonly ITenantRepository tenantRepository;
        private readonly ITagLogic tagLogic;
        private readonly IDataRepository dataRepository;
        private readonly IDataTypeRepository dataTypeRepository;
        private readonly INodeRepository nodeRepository;
        private readonly IExperimentLogic experimentLogic;
        private readonly IStorageLogic storageLogic;
        private readonly IGitLogic gitLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly ContainerManageOptions containerOptions;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExperimentController(
            IExperimentHistoryRepository experimentHistoryRepository,
            IExperimentPreprocessHistoryRepository experimentPreprocessHistoryRepository,
            IInferenceHistoryRepository inferenceHistoryRepository,
            IExperimentTensorBoardContainerRepository tensorBoardContainerRepository,
            DataAccess.Repositories.Interfaces.TenantRepositories.IAquariumDataSetRepository aquariumDataSetRepository,
            IDataSetRepository dataSetRepository,
            IDataLogic dataLogic,
            IDataTypeRepository dataTypeRepository,
            ITemplateRepository templateRepository,
            ITenantRepository tenantRepository,
            IDataRepository dataRepository,
            INodeRepository nodeRepository,
            ITagLogic tagLogic,
            IExperimentLogic experimentLogic,
            IStorageLogic storageLogic,
            IGitLogic gitLogic,
            IClusterManagementLogic clusterManagementLogic,
            IOptions<ContainerManageOptions> containerOptions,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.experimentHistoryRepository = experimentHistoryRepository;
            this.experimentPreprocessHistoryRepository = experimentPreprocessHistoryRepository;
            this.inferenceHistoryRepository = inferenceHistoryRepository;
            this.tensorBoardContainerRepository = tensorBoardContainerRepository;
            this.dataRepository = dataRepository;
            this.dataTypeRepository = dataTypeRepository;
            this.aquariumDataSetRepository = aquariumDataSetRepository;
            this.dataSetRepository = dataSetRepository;
            this.templateRepository = templateRepository;
            this.tenantRepository = tenantRepository;
            this.nodeRepository = nodeRepository;
            this.experimentLogic = experimentLogic;
            this.tagLogic = tagLogic;
            this.storageLogic = storageLogic;            this.gitLogic = gitLogic;
            this.clusterManagementLogic = clusterManagementLogic;
            this.containerOptions = containerOptions.Value;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 全学習履歴のIDと名前を取得
        /// </summary>
        [HttpGet("simple")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(IEnumerable<SimpleOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var histories = await experimentHistoryRepository.GetAllNameAsync();
            return JsonOK(histories.Select(history => new SimpleOutputModel(history)));
        }

        /// <summary>
        /// 指定された条件でページングされた状態で、全実験履歴を取得
        /// </summary>
        /// <param name="page">ページ番号。デフォルトは1。</param>
        /// <param name="perPage">表示件数。指定がない場合は上限(1000件)。</param>
        /// <param name="filter">検索条件</param>
        /// <param name="withTotal">合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。</param>
        [HttpGet]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll([FromQuery] SearchInputModel filter, [FromQuery] int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            var data = experimentHistoryRepository.GetAllIncludeDataSetAndParentWithOrdering();
            data = Search(data, filter);

            //未指定、あるいは1000件以上であれば、1000件に指定
            int pageCount = (perPage.HasValue && perPage.Value < 1000) ? perPage.Value : 1000;
            data = data.Paging(page, pageCount);

            if (withTotal)
            {
                int total = GetTotalCount(filter);
                SetTotalCountToHeader(total);
            }

            //SQLが多重実行されることを防ぐため、ToListで即時発行させたうえで、結果を生成
            return JsonOK(data.ToList().Select(history => GetUpdatedIndexOutputModelAsync(history).Result));
        }

        /// <summary>
        /// ステータスを更新して、出力モデルに変換する
        /// </summary>
        /// <param name="history">実験履歴</param>
        private async Task<IndexOutputModel> GetUpdatedIndexOutputModelAsync(ExperimentHistory history)
        {
            var model = new IndexOutputModel(history);

            var status = history.GetStatus();
            if (status.Exist())
            {
                //実験がまだ進行中の場合、情報を更新する
                var newStatus = await clusterManagementLogic.GetContainerStatusAsync(history.Key, CurrentUserInfo.SelectedTenant.Name, false);

                if (status.Key != newStatus.Key)
                {
                    //更新があったので、変更処理
                    await experimentHistoryRepository.UpdateStatusAsync(history.Id, newStatus, false);
                    unitOfWork.Commit();

                    model.Status = newStatus.Name;
                }
            }
            // storageへの出力値があれば取得し、modelに格納
            var outputFileName = "confusion_matrix.csv";   //値を読み込むファイル名
            var outputPath = history.Id + "/" + outputFileName;
            var content = await storageLogic.GetFileContentAsync(ResourceType.ExperimentContainerOutputFiles, outputPath, outputFileName, true);
            if (content != null)
            {
                model.OutputValue = content;
            }
            return model;
        }

        /// <summary>
        /// データ件数を取得する
        /// </summary>
        /// <param name="filter">検索条件</param>
        private int GetTotalCount(SearchInputModel filter)
        {
            IQueryable<ExperimentHistory> histories;
            histories = experimentHistoryRepository.GetAll();
            histories = Search(histories, filter);
            return histories.Count();
        }

        /// <summary>
        /// 検索条件の追加
        /// </summary>
        /// <param name="sourceData">加工前の検索結果</param>
        /// <param name="filter">検索条件</param>
        private static IQueryable<ExperimentHistory> Search(IQueryable<ExperimentHistory> sourceData, SearchInputModel filter)
        {
            IQueryable<ExperimentHistory> data = sourceData;
            data = data
                .SearchLong(d => d.Id, filter.Id)
                .SearchString(d => d.Name, filter.Name)
                .SearchTime(d => d.CreatedAt, filter.StartedAt)
                .SearchString(d => d.GetStatus().ToString(), filter.Status);

            return data;
        }


        /// <summary>
        /// 指定されたIDの実験履歴の詳細情報を取得。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        [HttpGet("{id}")]
        [Filters.PermissionFilter(MenuCode.Experiment, MenuCode.Preprocess, MenuCode.Training, MenuCode.Inference)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Experiment ID is required.");
            }
            var history = await experimentHistoryRepository.GetIncludeAllAsync(id.Value);
            if (history == null)
            {
                return JsonNotFound($"Experiment ID {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(history);

            var status = history.GetStatus();
            model.StatusType = status.StatusType;
            if (status.Exist())
            {
                //コンテナがまだ存在している場合、情報を更新する
                var details = await clusterManagementLogic.GetContainerDetailsInfoAsync(history.Key, CurrentUserInfo.SelectedTenant.Name, false);
                model.Status = details.Status.Name;
                model.StatusType = details.Status.StatusType;

                //ステータスを更新
                history.Status = details.Status.Key;
                unitOfWork.Commit();

                model.ConditionNote = details.ConditionNote;
            }

            //TODO:必要に応じて前処理Git、学習・推論Gitの表示用URLを作る
            //model.GitModel.Url = gitLogic.GetTreeUiUrl(history.ModelGitId, history.ModelRepository, history.ModelRepositoryOwner, history.ModelCommitId);
            return JsonOK(model);
        }

        /// <summary>
        /// 指定された実験履歴のエラーイベントを取得します。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <returns>ログファイル</returns>
        [HttpGet("{id}/events")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(ContainerEventInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetErrorEventAsync(long id)
        {
            var history = await experimentHistoryRepository.GetByIdAsync(id);
            if (history == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }
            if (history.GetStatus().Exist() == false)
            {
                return JsonBadRequest($"A container for Experiment ID {id} does not exist.");
            }

            var events = await clusterManagementLogic.GetEventsAsync(CurrentUserInfo.SelectedTenant, history.Key, false, true);

            if (events.IsSuccess == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to get container events: {events.Error}");
            }
            else
            {
                return JsonOK(events.Value);
            }
        }

        #region 実験実行

        /// <summary>
        /// 新規に実験を開始する
        /// </summary>
        /// <param name="model">新規実験実行内容</param>
        [HttpPost("run")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] CreateInputModel model)
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

            // テンプレートの存在確認
            var template = await templateRepository.GetByIdAsync(model.TemplateId);
            if (template == null)
            {
                return JsonNotFound($"Template ID {template.Id} is not found.");
            }

            // イメージが指定されていない学習は起動不能(前処理は必須ではないのでチェックしない。)
            if (string.IsNullOrEmpty(template.TrainingContainerImage) || string.IsNullOrEmpty(template.TrainingContainerTag))
            {
                return JsonNotFound($"Training of Template {template.Name} can not be run because a container image has not been selected properly yet.");
            }

            // 学習コンテナの各リソースの超過チェック
            Tenant tenant = tenantRepository.Get(CurrentUserInfo.SelectedTenant.Id);
            string trainingErrorMessage = clusterManagementLogic.CheckQuota(tenant, template.TrainingCpu, template.TrainingMemory, template.TrainingGpu);
            if (trainingErrorMessage != null)
            {
                return JsonBadRequest(trainingErrorMessage);
            }
            
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


            var options = model.Options ?? new Dictionary<string, string>(); // オプションはnullの可能性があるので、その時は初期化
            if (options.ContainsKey("")) // 空文字は除外する
            {
                options.Remove("");
            }


            //コンテナを起動する
            //指定したテンプレート内に前処理がない場合、テンプレート実行の学習コンテナのみ起動する
            //指定したテンプレート内に前処理がある場合ではじめて実行されるデータの場合、テンプレート実行の前処理コンテナ起動後、学習コンテナを起動する
            if (string.IsNullOrWhiteSpace(template.PreprocessContainerImage))
            {

                //コンテナの実行前に、学習履歴を作成する（コンテナの実行に失敗した場合、そのステータスをユーザに表示するため）
                var experimentHistory = new ExperimentHistory()
                {
                    Name = model.Name,
                    DataSetId = dataSet.Id,
                    DataSetVersionId = dataSetVersion.Id,
                    InputDataSetId = dataSetVersion.DataSetId,
                    TemplateId = template.Id,
                    OptionDic = options,
                    Status = ContainerStatus.None.Key
                };
                experimentHistoryRepository.Add(experimentHistory);
                unitOfWork.Commit();

                var trainingResult = await clusterManagementLogic.RunExperimentTrainContainerAsync(experimentHistory);
                if (trainingResult.IsSuccess == false)
                {
                    //コンテナの起動に失敗した状態。エラーを出力して、保存した学習履歴も削除する。
                    experimentHistoryRepository.Delete(experimentHistory);
                    unitOfWork.Commit();

                    return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to run Experiment. The message bellow may be help to resolve: " + trainingResult.Error);
                }

                //結果に従い、学習結果を更新する。
                //実行には時間がかかりうるので、DBから最新の情報を取ってくる
                experimentHistory = await experimentHistoryRepository.GetByIdAsync(experimentHistory.Id);
                experimentHistory.Status = trainingResult.Value.Status.Key;
                unitOfWork.Commit();

                if (trainingResult.Value.Status.Succeed())
                {
                    return JsonCreated(new SimpleOutputModel(experimentHistory));
                }
                else
                {
                    return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run Experiment. Status={trainingResult.Value.Status.Name}. Please contact your server administrator.");
                }
            }
            else
            {
                var experimentPreprocess = experimentPreprocessHistoryRepository
                    .Find(x => x.TemplateId == model.TemplateId
                    && x.DataSetId == model.DataSetId
                    && x.DataSetVersionId == dataSetVersion.Id
                    && x.OutputDataSetId.HasValue);
                if (experimentPreprocess != null)
                {
                    var experimentHistory = new ExperimentHistory()
                    {
                        Name = model.Name,
                        DataSetId = dataSet.Id,
                        DataSetVersionId = dataSetVersion.Id,
                        InputDataSetId = experimentPreprocess.OutputDataSetId,
                        TemplateId = template.Id,
                        OptionDic = options,
                        Status = ContainerStatus.None.Key,
                        ExperimentPreprocessHistory = experimentPreprocess,
                    };
                    experimentHistoryRepository.Add(experimentHistory);
                    unitOfWork.Commit();

                    // テンプレート前処理で生成されたデータを入力にする学習コンテナを起動
                    var trainingResult = await clusterManagementLogic.RunExperimentTrainAfterPreprocessingContainerAsync(experimentHistory);
                    if (trainingResult.IsSuccess == false)
                    {
                        //コンテナの起動に失敗した状態。エラーを出力して、保存した学習履歴も削除する。
                        experimentHistoryRepository.Delete(experimentHistory);
                        unitOfWork.Commit();

                        return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to run Experiment. The message bellow may be help to resolve: " + trainingResult.Error);
                    }

                    //結果に従い、学習結果を更新する。
                    //実行には時間がかかりうるので、DBから最新の情報を取ってくる
                    experimentHistory = await experimentHistoryRepository.GetByIdAsync(experimentHistory.Id);
                    experimentHistory.Status = trainingResult.Value.Status.Key;
                    unitOfWork.Commit();

                    if (trainingResult.Value.Status.Succeed())
                    {
                        return JsonCreated(new SimpleOutputModel(experimentHistory));
                    }
                    else
                    {
                        return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run Experiment. Status={trainingResult.Value.Status.Name}. Please contact your server administrator.");
                    }
                }
                else
                {
                    // 前処理

                    // 各リソースの超過チェック
                    string preprocessErrorMessage = clusterManagementLogic.CheckQuota(tenant, template.PreprocessCpu, template.PreprocessMemory, template.PreprocessGpu);
                    if (preprocessErrorMessage != null)
                    {
                        return JsonBadRequest(preprocessErrorMessage);
                    }

                    // イメージが指定されていない前処理テンプレートは起動不能
                    if (string.IsNullOrEmpty(template.PreprocessContainerImage) || string.IsNullOrEmpty(template.PreprocessContainerTag))
                    {
                        return JsonNotFound($"Preprocessing {template.Name} can not be run because a container image has not been selected properly yet.");
                    }


                    var experimentPreprocessHistory = new ExperimentPreprocessHistory()
                    {
                        DataSetId = model.DataSetId,
                        DataSetVersionId = dataSetVersion.Id,
                        TemplateId = template.Id,
                        Status = ContainerStatus.None.Key,
                        OptionDic = options,
                    };
                    var experimentHistory = new ExperimentHistory()
                    {
                        Name = model.Name,
                        DataSetId = model.DataSetId,
                        DataSetVersionId = dataSetVersion.Id,
                        TemplateId = template.Id,
                        Status = ContainerStatus.None.Key,
                        OptionDic = options,
                        ExperimentPreprocessHistory = experimentPreprocessHistory,
                    };

                    experimentPreprocessHistoryRepository.Add(experimentPreprocessHistory);
                    experimentHistoryRepository.Add(experimentHistory);

                    unitOfWork.Commit();

                    var result = await clusterManagementLogic.RunExperimentPreprocessContainerAsync(experimentHistory, experimentPreprocessHistory);
                    if (result.IsSuccess == false)
                    {
                        // コンテナの起動に失敗した状態。エラーを出力して、保存した学習履歴も削除する。
                        experimentPreprocessHistoryRepository.Delete(experimentPreprocessHistory);

                        return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to run experiment preprocessing. The message bellow may be help to resolve: " + result.Error);
                    }


                    // 結果に従い、前処理実験結果を更新する。
                    // 実行には時間がかかりうるので、DBから最新の情報を取ってくる
                    experimentPreprocessHistory = await experimentPreprocessHistoryRepository.GetByIdAsync(experimentPreprocessHistory.Id);
                    experimentPreprocessHistory.Status = result.Value.Status.Key;
                    unitOfWork.Commit();

                    if (result.Value.Status.Succeed())
                    {
                        return JsonCreated(new SimpleOutputModel(experimentHistory));
                    }
                    else
                    {
                        return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run experiment-preprocessing. Status={result.Value.Status.Name}. Please contact your server administrator.");
                    }
                }
            }
        }
/*
        /// <summary>
        /// 新規に実験の前処理を実行し、履歴を作成する。
        /// </summary>
        /// <remarks>
        /// 実験の前処理実行用のコンテナを起動する。
        /// 作成された前処理履歴は実行中のステータスとなり、前処理結果の追加が可能な状態になる。
        /// </remarks>
        /// <param name="id">テンプレートID</param>
        /// <param name="model">実行設定</param>
        [HttpPost("{id}/preprocessing/run")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(PreprocessHistoriesOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RunPreprocessHistory([FromRoute] long id, [FromBody]CreateInputModel model)
        {
            // テンプレートの存在確認
            var template = await templateRepository.GetByIdAsync(model.TemplateId);
            if (template == null)
            {
                return JsonNotFound($"Template ID {template.Id} is not found.");
            }
            // 各リソースの超過チェック
            Tenant tenant = tenantRepository.Get(CurrentUserInfo.SelectedTenant.Id);
            string preprocessErrorMessage = clusterManagementLogic.CheckQuota(tenant, template.PreprocessCpu, template.PreprocessMemory, template.PreprocessGpu);
            if (preprocessErrorMessage != null)
            {
                return JsonBadRequest(preprocessErrorMessage);
            }
            var validateResult = await ValidateCreatePreprocessHistoryInputModelAsync(id, model.DataSetId, model.DataSetVersion);
            if (validateResult.IsSuccess == false)
            {
                return validateResult.Error;
            }

            var experimentPreprocessHistory = validateResult.Value;
            experimentPreprocessHistory.OptionDic = model.Options ?? new Dictionary<string, string>(); // オプションはnullの可能性があるので、その時は初期化
            if (experimentPreprocessHistory.OptionDic.ContainsKey("")) // 空文字は除外する
            {
                experimentPreprocessHistory.OptionDic.Remove("");
            }

            experimentPreprocessHistoryRepository.Add(experimentPreprocessHistory);

            var experimentHistory = new ExperimentHistory()
            {
                Name = model.Name,
                DataSetId = model.DataSetId,
                DataSetVersionId = experimentPreprocessHistory.DataSetVersionId,
                TemplateId = template.Id,
                Status = ContainerStatus.None.Key,
                ExperimentPreprocessHistory = experimentPreprocessHistory,
            };

            experimentHistory.OptionDic = model.Options ?? new Dictionary<string, string>(); // オプションはnullの可能性があるので、その時は初期化
            if (experimentHistory.OptionDic.ContainsKey("")) // 空文字は除外する
            {
                experimentHistory.OptionDic.Remove("");
            }

            experimentHistoryRepository.Add(experimentHistory);


            unitOfWork.Commit();


            var result = await clusterManagementLogic.RunExperimentPreprocessContainerAsync(experimentHistory, experimentPreprocessHistory);
            if (result.IsSuccess == false)
            {
                // コンテナの起動に失敗した状態。エラーを出力して、保存した学習履歴も削除する。
                experimentPreprocessHistoryRepository.Delete(experimentPreprocessHistory);

                return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to run experiment preprocessing. The message bellow may be help to resolve: " + result.Error);
            }


            // 結果に従い、前処理実験結果を更新する。
            // 実行には時間がかかりうるので、DBから最新の情報を取ってくる
            experimentPreprocessHistory = await experimentPreprocessHistoryRepository.GetByIdAsync(experimentPreprocessHistory.Id);
            experimentPreprocessHistory.Status = result.Value.Status.Key;
            unitOfWork.Commit();

            if (result.Value.Status.Succeed())
            {
                return JsonCreated(new PreprocessHistoriesOutputModel(experimentPreprocessHistory));
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run experiment-preprocessing. Status={result.Value.Status.Name}. Please contact your server administrator.");
            }

        }

        /// <summary>
        /// 前処理履歴作成の入力モデルのチェックを行う。
        /// </summary>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="aquariumDataSetId">入力アクアリウムデータセットID</param>
        private async Task<Result<ExperimentPreprocessHistory, IActionResult>> ValidateCreatePreprocessHistoryInputModelAsync(long? templateId, long? aquariumDataSetId, long? aquariumDataSetVersionNo)
        {
            if (aquariumDataSetId == null)
            {
                return Result<ExperimentPreprocessHistory, IActionResult>.CreateErrorResult(JsonBadRequest("AquariumDataSet ID is requried."));
            }

            if (!ModelState.IsValid)
            {
                return Result<ExperimentPreprocessHistory, IActionResult>.CreateErrorResult(JsonBadRequest("Invalid Input"));
            }

            // データIDの存在確認
            var aquariumDataSet = await aquariumDataSetRepository.GetDataSetWithVersionsAsync(aquariumDataSetId.Value);
            if (aquariumDataSet == null)
            {
                return Result<ExperimentPreprocessHistory, IActionResult>.CreateErrorResult(JsonNotFound($"AquariumDataSet ID {aquariumDataSetId} is not found."));
            }

            // データセットバージョンの存在確認
            var aquariumDataSetVersion = aquariumDataSet.DataSetVersions.FirstOrDefault(x => x.Version == aquariumDataSetVersionNo.Value);
            if (aquariumDataSetVersion == null)
            {
                return Result<ExperimentPreprocessHistory, IActionResult>.CreateErrorResult(JsonNotFound($"versionno {aquariumDataSetVersionNo.Value} is not found."));
            }

            // テンプレートの存在確認
            var template = await templateRepository.GetByIdAsync(templateId.Value);
            if (template == null)
            {
                return Result<ExperimentPreprocessHistory, IActionResult>.CreateErrorResult(JsonNotFound($"Template ID {template.Id} is not found."));
            }
            // イメージが指定されていない前処理テンプレートは起動不能
            if (string.IsNullOrEmpty(template.PreprocessContainerImage) || string.IsNullOrEmpty(template.PreprocessContainerTag))
            {
                return Result<ExperimentPreprocessHistory, IActionResult>.CreateErrorResult(JsonNotFound($"Preprocessing {template.Name} can not be run because a container image has not been selected properly yet."));
            }
            // 前処理が既に実行中か確認する
            //var experimentPreprocessHistory = experimentPreprocessHistoryRepository
            //    .Find(pph => pph.DataSetId == aquariumDataSetId && pph.TemplateId == template.Id && pph.DataSetVersion.Version == aquariumDataSetVersionNo);
            //if (experimentPreprocessHistory != null)
            //{
            //    string status = ContainerStatus.Convert(experimentPreprocessHistory.Status).Name;
            //    return Result<ExperimentPreprocessHistory, IActionResult>.CreateErrorResult(JsonNotFound($"DataSet {aquariumDataSet.Id}:{aquariumDataSet.Name} has already been processed by {template.Id}:{template.Name}. Status:{status}"));
            //}

            var experimentPreprocessHistory = new ExperimentPreprocessHistory()
            {
                DataSetId = aquariumDataSet.Id,
                DataSetVersionId = aquariumDataSetVersion.Id,
                TemplateId = template.Id,
                Status = ContainerStatus.Running.Key
            };

            return Result<ExperimentPreprocessHistory, IActionResult>.CreateResult(experimentPreprocessHistory);

        }
*/
        /// <summary>
        /// 前処理履歴に出力データを追加する。
        /// 追加する対象の前処理履歴は実行中のステータスのみ許可される。
        /// </summary>
        /// <param name="id">実験ID</param>
        /// <param name="model">データ情報</param>
        [HttpPost("{id}/preprocessing/data")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(PreprocessHistoryIndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadPreprocessImage([FromRoute] long id, [FromBody] AddOutputDataInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            //データの存在チェック
            var experimentHistory = await experimentHistoryRepository.GetByIdAsync(id);
            if (experimentHistory == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }

            if (experimentHistory.ExperimentPreprocessHistoryId == null)
            {
                return JsonBadRequest($"Experiment ID {id} preproc not exist.");
            }

            var preprocessHistory = await experimentPreprocessHistoryRepository.GetByIdAsync(experimentHistory.ExperimentPreprocessHistoryId.Value);
            if (preprocessHistory == null)
            {
                return JsonBadRequest($"Experiment ID {id} preproc2 not exist.");
            }

            var status = ContainerStatus.Convert(preprocessHistory.Status);
            if (status.IsOpened() == false)
            {
                // 追加できるのは開放中のコンテナだけ（ローカルの結果を追加することがあるので、Runningとは限らない）
                return JsonBadRequest($"Experiment Preprocessing History {id} is not opened.");
            }

            // データを追加する
            Data newData = new Data()
            {
                Name = model.Name,
                Memo = model.Memo
            };
            dataRepository.Add(newData);
            foreach (var file in model.Files)
            {
                dataRepository.AddFile(newData, file.FileName, file.StoredPath);
            }
            // タグの登録
            List<string> tags = new List<string>() { experimentHistory.Name };
            tagLogic.CreateDataTags(newData, tags);

            experimentPreprocessHistoryRepository.AddOutputData(preprocessHistory.Id, newData);

            unitOfWork.Commit();

            return JsonOK(new PreprocessHistoryIndexOutputModel(preprocessHistory));
        }
/*
        /// <summary>
        /// 実験の前処理のステータスを更新して、出力モデルに変換する
        /// </summary>
        /// <param name="history">前処理履歴</param>
        /// <param name="model">出力モデル</param>
        private async Task<PreprocessHistoryIndexOutputModel> GetUpdatedPreproccessIndexOutputModelAsync(ExperimentPreprocessHistory history, PreprocessHistoryIndexOutputModel model)
        {
            var status = ContainerStatus.Convert(history.Status);
            model.StatusType = status.StatusType;
            if (status.Exist())
            {
                // コンテナがまだ存在している場合、情報を更新する
                var newStatus = await clusterManagementLogic.GetContainerStatusAsync(history.Key, CurrentUserInfo.SelectedTenant.Name, false);

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
*/
        ///// <summary>
        ///// 指定されたアクアリウムデータセットに対するテンプレート前処理実行の履歴を取得。
        ///// </summary>
        ///// <param name="id">テンプレートID</param>
        ///// <param name="dataSetId">アクアリウムデータセットID</param>
        //[HttpGet("{id}/preprocessing/histories/{dataSetId}")]
        //[Filters.PermissionFilter(MenuCode.Experiment)]
        //[ProducesResponseType(typeof(PreprocessHistoryDetailsOutputModel), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetDetailHistory([FromRoute] long id, [FromRoute] long? dataSetId)
        //{
        //    if (dataSetId == null)
        //    {
        //        return JsonBadRequest("Aquarium DataSet ID is required.");
        //    }

        //    var history = await experimentPreprocessHistoryRepository.GetPreprocessIncludeDataSetAndTemplateAsync(id, dataSetId.Value);
        //    if (history == null)
        //    {
        //        return JsonNotFound($"Preprocessing History about Template {id} to Aquarium DataSet {dataSetId} is not found.");
        //    }

        //    var result = new PreprocessHistoryDetailsOutputModel(history);
        //    result = await GetUpdatedPreproccessIndexOutputModelAsync(history, result) as PreprocessHistoryDetailsOutputModel;

        //    result.OutputDataIds = experimentPreprocessHistoryRepository.GetExperimentPreprocessOutputs(history.Id);
        //    return JsonOK(result);
        //}
        #endregion

        #region コンテナ出力・添付ファイル


        /// <summary>
        /// コンテナの出力ファイルの一覧を取得する。
        /// </summary>
        /// <remarks> 
        /// コンテナの/output/配下から指定ディレクトリパスの直下を検索する
        /// 検索対象ディレクトリが見つからない場合もファイル・ディレクトリが空の結果を返す
        /// </remarks>
        /// <param name="id">対象の学習履歴ID</param>
        /// <param name="path">検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        [HttpGet("{id}/container-files")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(StorageListResultInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUnderDir(long id, [FromQuery] string path = "/", [FromQuery] bool withUrl = false)
        {
            //データの存在チェック
            var experimentHistory = await experimentHistoryRepository.GetByIdAsync(id);
            if (experimentHistory == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }

            // 検索path文字列の先頭・末尾が/でない場合はつける
            if (!path.StartsWith("/", StringComparison.CurrentCulture))
            {
                path = "/" + path;
            }
            if (!path.EndsWith("/", StringComparison.CurrentCulture))
            {
                path = path + "/";
            }

            // windowsから実行された場合、区切り文字が"\\"として送られてくるので"/"に置換する
            path = path.Replace("\\", "/");

            var rootDir = $"{id}" + path;

            var result = await storageLogic.GetUnderDirAsync(ResourceType.ExperimentContainerOutputFiles, rootDir);

            if (withUrl)
            {
                result.Value.Files.ForEach(x => x.Url = storageLogic.GetPreSignedUriForGetFromKey(x.Key, x.FileName, true).ToString());
            }


            return JsonOK(result.Value);
        }

        /// <summary>
        /// 実験履歴添付ファイルの一覧を取得する。
        /// </summary>
        /// <param name = "id" > 対象の学習履歴ID </ param >
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
         [HttpGet("{id}/files")]
         [Filters.PermissionFilter(MenuCode.Experiment)]
         [ProducesResponseType(typeof(IEnumerable<AttachedFileOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAttachedFiles(long id, [FromQuery] bool withUrl = false)
        {
            //データの存在チェック
            var experimentHistory = await experimentHistoryRepository.GetByIdAsync(id);
            if (experimentHistory == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }

            var underDir = await storageLogic.GetUnderDirAsync(ResourceType.ExperimentContainerAttachedFiles, $"{id}/");
            if (underDir.IsSuccess == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to access the storage service. Please contact to system administrators.");
            }

            var containerAttachedFiles = new List<AttachedFileOutputModel>();
            containerAttachedFiles.AddRange(underDir.Value.Files.Select(
                f => new AttachedFileOutputModel(experimentHistory.Id, f.FileName, -1)
                {
                    Url = withUrl ? storageLogic.GetPreSignedUriForGetFromKey(f.Key, f.FileName, true).ToString() : null,
                    IsLocked = true
                }
                ));

            var userAttachedFiles = new List<AttachedFileOutputModel>();

            var filesOnDB = await experimentHistoryRepository.GetAllAttachedFilesAsync(id);
            userAttachedFiles.AddRange(filesOnDB.Select(
                f => new AttachedFileOutputModel(experimentHistory.Id, f.FileName, f.Id)
                {
                    Url = withUrl ? storageLogic.GetPreSignedUriForGet(ResourceType.ExperimentHistoryAttachedFiles, f.StoredPath, f.FileName, true).ToString() : null,
                    IsLocked = false
                }
                ));

            var result = containerAttachedFiles.Concat(userAttachedFiles);
            return JsonOK(result);
        }

        /// <summary>
        /// 学習履歴添付ファイルを削除する
        /// </summary>
        /// <param name="id">対象の学習履歴ID</param>
        /// <param name="fileId">削除するファイルのID</param>
        [HttpDelete("{id}/files/{fileId}")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAttachedFile(long id, long? fileId)
        {
            if (fileId == null)
            {
                return JsonBadRequest("File ID is required.");
            }
            if (fileId.Value < 0)
            {
                return JsonBadRequest("The file is locked. You can NOT delete the file.");
            }
            //存在チェック
            //子の添付ファイルが存在すれば親の学習履歴は必ず存在するはずなので、そっちはチェックしない
            ExperimentHistoryAttachedFile file = await experimentHistoryRepository.GetAttachedFileAsync(fileId.Value);
            if (file == null)
            {
                return JsonNotFound($"File ID {fileId.Value} is not found.");
            }

            experimentHistoryRepository.DeleteAttachedFile(file);
            await storageLogic.DeleteFileAsync(ResourceType.ExperimentHistoryAttachedFiles, file.StoredPath);
            unitOfWork.Commit();

            return JsonNoContent();
        }

        #endregion

        #region TensorBoard
        /// <summary>
        /// 指定したTensorBoardコンテナ情報を取得する
        /// </summary>
        /// <param name="id">対象の学習履歴ID</param>
        [HttpGet("{id}/tensorboard")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(TensorBoardOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTensorBoardStatus(long id)
        {
            //データの存在チェック
            var experimentHistory = await experimentHistoryRepository.GetByIdAsync(id);
            if (experimentHistory == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }

            //実験履歴IDとテナントIDが等しく、Disable状態じゃないコンテナをDBから取得する
            ExperimentTensorBoardContainer container = tensorBoardContainerRepository.GetAvailableContainer(id);

            //以下、Jsonで返却するパラメータ
            ContainerStatus status = ContainerStatus.None; //コンテナのステータス

            // 対象コンテナ情報が存在する場合、その結果を返す
            if (container != null)
            {
                //コンテナのステータスを最新にする
                status = await clusterManagementLogic.SyncExperimentContainerStatusAsync(container, false);
            }

            return JsonOK(new TensorBoardOutputModel(container, status, containerOptions.WebEndPoint));
        }



        /// <summary>
        /// 指定した実験のTensor Boardを立てる
        /// </summary>
        /// <param name="id">対象の実験履歴ID</param>
        /// <param name="model">起動モデル</param>
        [HttpPut("{id}/tensorboard")] //TensorBoardはIDをユーザに通知するわけではないので、POSTではなくPUTで扱う
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(TensorBoardOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RunTensorBoard(long id, [FromBody] TensorBoardInputModel model)
        {
            //データの存在チェック
            var experimentHistory = await experimentHistoryRepository.GetByIdAsync(id);
            if (experimentHistory == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }

            //実験履歴IDとテナントIDが等しく、Disable状態じゃないコンテナを取得する
            ExperimentTensorBoardContainer container = tensorBoardContainerRepository.GetAvailableContainer(id);
            // 当該レコードが存在する場合、時間差で他の人が立てたとみなし、何もしない
            if (container != null)
            {
                var status = ContainerStatus.Convert(container.Status);
                return JsonOK(new TensorBoardOutputModel(container, status, containerOptions.WebEndPoint));
            }

            // コンテナ生存期間
            int expiresIn = 0; // 無期限だが、DeleteTensorBoardContainerTimerの動作タイミングで削除する
            if (model.ExpiresIn.HasValue)
            {
                // 値が存在する場合、その期間起動させる
                expiresIn = model.ExpiresIn.Value;
            }

            //新規にTensorBoardコンテナを起動する。
            var result = await clusterManagementLogic.RunExperimentTensorBoardContainerAsync(experimentHistory, expiresIn);
            if (result == null || result.Status.Succeed() == false)
            {
                //起動に失敗した場合、ステータス Failed で返す。
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run tensorboard container: {result.Status}");
            }

            container = new ExperimentTensorBoardContainer()
            {
                Name = result.Name,
                StartedAt = DateTime.Now,
                Status = result.Status.Name,
                TenantId = CurrentUserInfo.SelectedTenant.Id,
                ExperimentHistoryId = id,
                Host = result.Host,
                PortNo = result.Port,
                ExpiresIn = expiresIn
            };

            // コンテナテーブルにInsertする
            tensorBoardContainerRepository.Add(container);

            unitOfWork.Commit();

            return JsonOK(new TensorBoardOutputModel(container, result.Status, containerOptions.WebEndPoint));
        }

        /// <summary>
        /// 指定した実験のTensor Boardを削除する
        /// </summary>
        /// <param name="id">対象の学習履歴ID</param>
        [HttpDelete("{id}/tensorboard")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteTensorBoard(long id)
        {
            //実験履歴IDとテナントIDが等しく、Disable状態じゃないコンテナを取得する
            ExperimentTensorBoardContainer container = tensorBoardContainerRepository.GetAvailableContainer(id);
            // 当該レコードが存在しない場合、404エラー
            if (container == null)
            {
                return JsonNotFound($"TensorBoard container for ExperimentHistory ID {id} is not found.");
            }

            await experimentLogic.DeleteTensorBoardAsync(container, false);

            return JsonNoContent();
        }

        #endregion

        #region 停止・削除

        /// <summary>
        /// 実験を途中で強制終了させる。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpPost("{id}/halt")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Halt(long? id)
        {
            await ExitPreprocessAsync(id, ContainerStatus.Killed);
            return await ExitAsync(id, ContainerStatus.Killed);
        }

        /// <summary>
        /// 実験を途中で強制終了させる。
        /// ユーザ自身がジョブを停止させた場合。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        [HttpPost("{id}/user-cancel")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UserCancel(long? id)
        {
            await ExitPreprocessAsync(id, ContainerStatus.UserCanceled);
            return await ExitAsync(id, ContainerStatus.UserCanceled);
        }

        /// <summary>
        /// 実験を正常終了させる。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpPost("{id}/complete")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Complete(long? id)
        {
            await ExitPreprocessAsync(id, ContainerStatus.Completed);
            return await ExitAsync(id, ContainerStatus.Completed);
        }

        /// <summary>
        /// 実験を終了させる。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        /// <param name="status">変更後のステータス</param>
        /// <returns></returns>
        private async Task<IActionResult> ExitAsync(long? id, ContainerStatus status)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var experimentHistory = await experimentHistoryRepository.GetByIdAsync(id.Value);
            if (experimentHistory == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }
            //if (experimentHistory.GetStatus().Exist() == false)
            //{
            //    //終了できるのはRunningのコンテナだけ
            //    return JsonBadRequest($"Experiment {experimentHistory.Name} does not exist.");
            //}

            await experimentLogic.ExitAsync(experimentHistory, status, false);

            return JsonOK(new SimpleOutputModel(experimentHistory));
        }

        /// <summary>
        /// 前処理を途中で強制終了させる。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpPost("{id}/preprocessing/halt")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(PreprocessHistoryIndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> HaltPreprocess(long id)
        {
            var experimentHistory = await experimentHistoryRepository.GetIncludePreprocessdDataAsync(id);
            if (experimentHistory == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }
            if (experimentHistory.ExperimentPreprocessHistory != null)
            {
                foreach (var entry in experimentHistory.ExperimentPreprocessHistory.ExperimentPreprocessHistoryOutputs)
                {
                    var data = await dataRepository.GetDataIncludeAllAsync(entry.OutputDataId);
                    dataRepository.DeleteData(data);
                }
                await ExitPreprocessAsync(id, ContainerStatus.Killed);
                return JsonOK(new PreprocessHistoryIndexOutputModel(experimentHistory.ExperimentPreprocessHistory));
            }
            return JsonNoContent();
        }
         
        /// <summary>
        /// 前処理を正常終了させる。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpPost("{id}/preprocessing/complete")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CompletePreprocess(long id)
        {
            var experimentHistory = await experimentHistoryRepository.GetIncludePreprocessdDataAsync(id);
            if (experimentHistory == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }

            DataSet dataSet = new DataSet()
            {
                Name = $"aquarium-{experimentHistory.Template.Name}",
                IsLocked = false,
                IsFlat = true,
            };
            dataSetRepository.Add(dataSet);

            //データ種別の指定が名前のため、名前からIDを引けるようにキャッシュしておく
            var dataTypes = new Dictionary<string, DataType>();
            foreach (var dataType in dataTypeRepository.GetAllWithOrderby(d => d.SortOrder, true))
            {
                dataTypes.Add(dataType.Name, dataType);
            }
            var training = dataTypes["training"];


            foreach (var entry in experimentHistory.ExperimentPreprocessHistory.ExperimentPreprocessHistoryOutputs)
            {
                //Dataがなかった場合の処理
                if (await dataRepository.ExistsAsync(d => d.Id == entry.OutputDataId) == false)
                {
                    return JsonNotFound($"Data ID {entry.OutputDataId} is not found.");
                }

                dataSetRepository.AddEntry(dataSet, training.Id, entry.OutputDataId, true);
            }

            experimentHistory.ExperimentPreprocessHistory.OutPutDataSet = dataSet;
            experimentHistory.InputDataSet = dataSet;
            var result = await ExitPreprocessAsync(id, ContainerStatus.Completed);

            // テンプレート前処理で生成されたデータを入力にする学習コンテナを起動
            var trainingResult = await clusterManagementLogic.RunExperimentTrainAfterPreprocessingContainerAsync(experimentHistory);
            if (trainingResult.IsSuccess == false)
            {
                //コンテナの起動に失敗した状態。エラーを出力して、保存した学習履歴も削除する。
                experimentHistoryRepository.Delete(experimentHistory);
                unitOfWork.Commit();

                return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to run Experiment. The message bellow may be help to resolve: " + trainingResult.Error);
            }

            //結果に従い、学習結果を更新する。
            //実行には時間がかかりうるので、DBから最新の情報を取ってくる
            experimentHistory = await experimentHistoryRepository.GetByIdAsync(experimentHistory.Id);
            experimentHistory.Status = trainingResult.Value.Status.Key;
            unitOfWork.Commit();

            if (trainingResult.Value.Status.Succeed())
            {
                return JsonCreated(new SimpleOutputModel(experimentHistory));
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run Experiment. Status={trainingResult.Value.Status.Name}. Please contact your server administrator.");
            }
        }

        private async Task<IActionResult> ExitPreprocessAsync(long? id, ContainerStatus status)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var experimentHistory = await experimentHistoryRepository.GetByIdAsync(id.Value);
            if (experimentHistory == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }

            if (experimentHistory.ExperimentPreprocessHistoryId == null)
            {
                return JsonBadRequest($"Experiment ID {id} preproc not exist.");
            }

            var preprocessHistory = await experimentPreprocessHistoryRepository.GetByIdAsync(experimentHistory.ExperimentPreprocessHistoryId.Value);
            if (preprocessHistory == null)
            {
                return JsonBadRequest($"Experiment ID {id} preproc2 not exist.");
            }

            //if (preprocessHistory.GetStatus().Exist() == false)
            //{
            //    //終了できるのはRunningのコンテナだけ
            //    return JsonBadRequest($"Experiment ID {id} preproc does not running.");
            //}

            await experimentLogic.ExitPreprocessAsync(preprocessHistory, status, false);

            return JsonNoContent();
        }


        /// <summary>
        /// 実験履歴を削除する。
        /// </summary>
        /// <param name="id">実験履歴ID</param>
        [HttpDelete("{id}")]
        [Filters.PermissionFilter(MenuCode.Experiment)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var experimentHistory = await experimentHistoryRepository.GetByIdAsync(id.Value);
            if (experimentHistory == null)
            {
                return JsonNotFound($"Experiment ID {id} is not found.");
            }

            //ステータスを確認

            var status = experimentHistory.GetStatus();
            if (status.Exist())
            {
                //学習がまだ進行中の場合、情報を更新する
                status = await clusterManagementLogic.GetContainerStatusAsync(experimentHistory.Key, CurrentUserInfo.SelectedTenant.Name, false);
            }

           
            if (status.Exist())
            {
                //実行中であれば、コンテナを削除
                await clusterManagementLogic.DeleteContainerAsync(
                    ContainerType.Experiment, experimentHistory.Key, CurrentUserInfo.SelectedTenant.Name, false);
            }

            //TensorBoardを起動中だった場合は、そっちも消す
            ExperimentTensorBoardContainer container = tensorBoardContainerRepository.GetAvailableContainer(experimentHistory.Id);
            if (container != null)
            {
                await clusterManagementLogic.DeleteContainerAsync(
                    ContainerType.TensorBoard, container.Name, CurrentUserInfo.SelectedTenant.Name, false);
                tensorBoardContainerRepository.Delete(container, true);
            }

            // 添付ファイルがあったらまとめて消す
            var files = await experimentHistoryRepository.GetAllAttachedFilesAsync(experimentHistory.Id);
            foreach (var file in files)
            {
                experimentHistoryRepository.DeleteAttachedFile(file);
                await storageLogic.DeleteFileAsync(ResourceType.ExperimentHistoryAttachedFiles, file.StoredPath);
            }



            experimentHistoryRepository.Delete(experimentHistory);
            unitOfWork.Commit();



            // DBへタグ削除結果のコミット
            unitOfWork.Commit();

            // ストレージ内の実験データを削除する
            await storageLogic.DeleteResultsAsync(ResourceType.ExperimentContainerAttachedFiles, experimentHistory.Id);
            await storageLogic.DeleteResultsAsync(ResourceType.ExperimentContainerOutputFiles, experimentHistory.Id);

            return JsonNoContent();
        }
        #endregion

    }
}

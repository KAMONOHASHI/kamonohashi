using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.ApiModels.TrainingApiModels;
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
    /// Trainを扱うためのAPI集
    /// </summary>
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/training")]
    public class TrainingController : PlatypusApiControllerBase
    {
        private readonly ITrainingHistoryRepository trainingHistoryRepository;
        private readonly IInferenceHistoryRepository inferenceHistoryRepository;
        private readonly ITensorBoardContainerRepository tensorBoardContainerRepository;
        private readonly IDataSetRepository dataSetRepository;
        private readonly ITagRepository tagRepository;
        private readonly ITenantRepository tenantRepository;
        private readonly INodeRepository nodeRepository;
        private readonly ITagLogic tagLogic;
        private readonly ITrainingLogic trainingLogic;
        private readonly IStorageLogic storageLogic;
        private readonly IGitLogic gitLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly ContainerManageOptions containerOptions;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrainingController(
            ITrainingHistoryRepository trainingHistoryRepository,
            IInferenceHistoryRepository inferenceHistoryRepository,
            ITensorBoardContainerRepository tensorBoardContainerRepository,
            IDataSetRepository dataSetRepository,
            ITagRepository tagRepository,
            ITenantRepository tenantRepository,
            INodeRepository nodeRepository,
            ITagLogic tagLogic,
            ITrainingLogic trainingLogic,
            IStorageLogic storageLogic,
            IGitLogic gitLogic,
            IClusterManagementLogic clusterManagementLogic,
            IOptions<ContainerManageOptions> containerOptions,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.trainingHistoryRepository = trainingHistoryRepository;
            this.inferenceHistoryRepository = inferenceHistoryRepository;
            this.tensorBoardContainerRepository = tensorBoardContainerRepository;
            this.dataSetRepository = dataSetRepository;
            this.tagRepository = tagRepository;
            this.tenantRepository = tenantRepository;
            this.nodeRepository = nodeRepository;
            this.tagLogic = tagLogic;
            this.trainingLogic = trainingLogic;
            this.storageLogic = storageLogic;
            this.gitLogic = gitLogic;
            this.clusterManagementLogic = clusterManagementLogic;
            this.containerOptions = containerOptions.Value;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 全学習履歴のIDと名前を取得
        /// </summary>
        [HttpGet("simple")]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(IEnumerable<SimpleOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var histories = await trainingHistoryRepository.GetAllNameAsync();
            return JsonOK(histories.Select(history => new SimpleOutputModel(history)));
        }

        /// <summary>
        /// 指定された条件でページングされた状態で、全学習履歴を取得
        /// </summary>
        /// <param name="page">ページ番号。デフォルトは1。</param>
        /// <param name="perPage">表示件数。指定がない場合は上限(1000件)。</param>
        /// <param name="filter">検索条件</param>
        /// <param name="withTotal">合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。</param>
        [HttpGet]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll([FromQuery] SearchInputModel filter, [FromQuery] int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            var data = trainingHistoryRepository.GetAllIncludeDataSetAndParentWithOrdering();
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
        /// <param name="history">学習履歴</param>
        private async Task<IndexOutputModel> GetUpdatedIndexOutputModelAsync(TrainingHistory history)
        {
            var status = await DoGetUpdatedIndexOutputModelAsync(history, clusterManagementLogic, CurrentUserInfo, trainingHistoryRepository, unitOfWork);
            var model = new IndexOutputModel(history);
            model.Status = status.Name;
            return model;
        }

        public static async Task<ContainerStatus> DoGetUpdatedIndexOutputModelAsync(TrainingHistory history,
            IClusterManagementLogic clusterManagementLogic, UserInfo currentUserInfo, ITrainingHistoryRepository trainingHistoryRepository,
            IUnitOfWork unitOfWork)
        {
            var status = history.GetStatus();
            if (status.Exist())
            {
                //学習がまだ進行中の場合、情報を更新する
                var newStatus = await clusterManagementLogic.GetContainerStatusAsync(history.Key, currentUserInfo.SelectedTenant.Name, false);

                if (status.Key != newStatus.Key)
                {
                    //更新があったので、変更処理
                    await trainingHistoryRepository.UpdateStatusAsync(history.Id, newStatus, false);
                    unitOfWork.Commit();
                    return newStatus;
                }
            }
            return status;
        }


        /// <summary>
        /// データ件数を取得する
        /// </summary>
        /// <param name="filter">検索条件</param>
        private int GetTotalCount(SearchInputModel filter)
        {
            IQueryable<TrainingHistory> histories;
            if (string.IsNullOrEmpty(filter.DataSet))
            {
                histories = trainingHistoryRepository.GetAll();
            }
            else
            {
                //データセット名のフィルターがかかっている場合、データセットも併せて取得しないといけない
                histories = trainingHistoryRepository.GetAllIncludeDataSet();
            }

            histories = Search(histories, filter);
            return histories.Count();
        }

        /// <summary>
        /// 検索条件の追加
        /// </summary>
        /// <param name="sourceData">加工前の検索結果</param>
        /// <param name="filter">検索条件</param>
        private static IQueryable<TrainingHistory> Search(IQueryable<TrainingHistory> sourceData, SearchInputModel filter)
        {
            IQueryable<TrainingHistory> data = sourceData;
            data = data
                .SearchLong(d => d.Id, filter.Id)
                .SearchString(d => d.Name, filter.Name)
                .SearchTime(d => d.CreatedAt, filter.StartedAt)
                .SearchString(d => d.CreatedBy, filter.StartedBy)
                .SearchString(d => d.Memo, filter.Memo)
                .SearchString(d => d.EntryPoint, filter.EntryPoint)
                .SearchString(d => d.GetStatus().ToString(), filter.Status);

            // データセット名の検索
            if (string.IsNullOrEmpty(filter.DataSet) == false)
            {
                if (filter.DataSet.StartsWith("!", StringComparison.CurrentCulture))
                {
                    data = data.Where(d => d.DataSet != null && d.DataSet.Name != null && d.DataSet.Name.Contains(filter.DataSet.Substring(1), StringComparison.CurrentCulture) == false);
                }
                else
                {
                    data = data.Where(d => d.DataSet != null && d.DataSet.Name != null && d.DataSet.Name.Contains(filter.DataSet, StringComparison.CurrentCulture));
                }
            }

            // 親学習IDの検索
            if (string.IsNullOrEmpty(filter.ParentId) == false)
            {
                if (filter.ParentId.StartsWith(">=", StringComparison.CurrentCulture))
                {
                    if (long.TryParse(filter.ParentId.Substring(2), out long target))
                    {
                        data = data.Where(d => d.ParentMaps != null && d.ParentMaps.Any(m => m.ParentId >= target));
                    }
                }
                else if (filter.ParentId.StartsWith(">", StringComparison.CurrentCulture))
                {
                    if (long.TryParse(filter.ParentId.Substring(1), out long target))
                    {
                        data = data.Where(d => d.ParentMaps != null && d.ParentMaps.Any(m => m.ParentId > target));
                    }
                }
                else if (filter.ParentId.StartsWith("<=", StringComparison.CurrentCulture))
                {
                    if (long.TryParse(filter.ParentId.Substring(2), out long target))
                    {
                        data = data.Where(d => d.ParentMaps != null && d.ParentMaps.Any(m => m.ParentId <= target));
                    }
                }
                else if (filter.ParentId.StartsWith("<", StringComparison.CurrentCulture))
                {
                    if (long.TryParse(filter.ParentId.Substring(1), out long target))
                    {
                        data = data.Where(d => d.ParentMaps != null && d.ParentMaps.Any(m => m.ParentId < target));
                    }
                }
                else if (filter.ParentId.StartsWith("=", StringComparison.CurrentCulture))
                {
                    if (long.TryParse(filter.ParentId.Substring(1), out long target))
                    {
                        data = data.Where(d => d.ParentMaps != null && d.ParentMaps.Any(m => m.ParentId == target));
                    }
                }
                else
                {
                    if (long.TryParse(filter.ParentId, out long target))
                    {
                        data = data.Where(d => d.ParentMaps != null && d.ParentMaps.Any(m => m.ParentId == target));
                    }
                }
            }

            // 親学習名の検索
            if (string.IsNullOrEmpty(filter.ParentName) == false)
            {
                if (filter.ParentName.StartsWith("!", StringComparison.CurrentCulture))
                {
                    data = data.Where(d => d.ParentMaps == null || d.ParentMaps.Count == 0 || d.ParentMaps.All(m => m.Parent.Name.Contains(filter.ParentName.Substring(1), StringComparison.CurrentCulture) == false));
                }
                else
                {
                    data = data.Where(d => d.ParentMaps != null && d.ParentMaps.Any(m => m.Parent.Name.Contains(filter.ParentName, StringComparison.CurrentCulture)));
                }
            }

            // タグの検索
            if (filter.Tags != null)
            {
                foreach (var tag in filter.Tags)
                {
                    if (string.IsNullOrEmpty(tag) == false)
                    {
                        if (tag.StartsWith("!", StringComparison.CurrentCulture))
                        {
                            data = data.Where(d => d.TagMaps == null || d.TagMaps.Count == 0 || d.TagMaps.All(m => m.Tag.Name.Contains(tag.Substring(1), StringComparison.CurrentCulture) == false));
                        }
                        else
                        {
                            data = data.Where(d => d.TagMaps != null && d.TagMaps.Any(m => m.Tag.Name.Contains(tag, StringComparison.CurrentCulture)));
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// マウントする学習履歴を取得
        /// </summary>
        /// <param name="filter">検索条件</param>
        [HttpGet("mount")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetTrainingToMount(MountInputModel filter)
        {
            var data = trainingHistoryRepository.GetAllIncludeDataSetAndParentWithOrdering();

            // ステータスを限定する
            if (filter.Status != null)
            {
                data = data.Where(t => filter.Status.Contains(t.GetStatus().ToString()));
            }

            // SQLが多重実行されることを防ぐため、ToListで即時発行させたうえで、結果を生成
            return JsonOK(data.ToList().Select(history => GetUpdatedIndexOutputModelAsync(history).Result));
        }
        /// <summary>
        /// 指定されたIDの学習履歴の詳細情報を取得。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpGet("{id}")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Inference)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Training ID is required.");
            }
            var history = await trainingHistoryRepository.GetIncludeAllAsync(id.Value);
            if (history == null)
            {
                return JsonNotFound($"Training ID {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(history);
            model.Tags = tagLogic.GetAllTrainingHistoryTag(history.Id).Select(t => t.Name);

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
                if (details.Status.IsRunning())
                {
                    // 開放ポート未指定時には行わない
                    if (model.Ports != null && model.Ports.Count > 0)
                    {
                        var endpointInfo = await clusterManagementLogic.GetContainerEndpointInfoAsync(history.Key, CurrentUserInfo.SelectedTenant.Name, false);
                        foreach (var endpoint in endpointInfo.EndPoints ?? new List<EndPointInfo>())
                        {
                            //ノードポート番号を返す
                            model.NodePorts.Add(new KeyValuePair<string, string>(endpoint.Key, endpoint.Port.ToString()));
                        }
                    }
                }
            }

            //Gitの表示用URLを作る
            model.GitModel.Url = gitLogic.GetTreeUiUrl(history.ModelGitId, history.ModelRepository, history.ModelRepositoryOwner, history.ModelCommitId);
            return JsonOK(model);
        }

        /// <summary>
        /// 指定された学習履歴のエラーイベントを取得します。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        /// <returns>ログファイル</returns>
        [HttpGet("{id}/events")]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(ContainerEventInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetErrorEventAsync(long id)
        {
            var history = await trainingHistoryRepository.GetByIdAsync(id);
            if (history == null)
            {
                return JsonNotFound($"Training ID {id} is not found.");
            }
            if (history.GetStatus().Exist() == false)
            {
                return JsonBadRequest($"A container for Training ID {id} does not exist.");
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

        public static async Task<(TrainingHistory, IActionResult)> DoCreate(CreateInputModel model,
            IDataSetRepository dataSetRepository,
            INodeRepository nodeRepository,
            ITenantRepository tenantRepository,
            ITrainingHistoryRepository trainingHistoryRepository,
            IClusterManagementLogic clusterManagementLogic,
            IGitLogic gitLogic,
            ITagLogic tagLogic,
            IUnitOfWork unitOfWork,
            UserInfo currentUserInfo,
            ModelStateDictionary modelState,
            string requestUrl,
            string scriptType
            )
        {
            //データの入力チェック
            if (!modelState.IsValid)
            {
                return (null,
                    DoJsonBadRequest(typeof(TrainingController), requestUrl, modelState, "Invalid inputs."));
            }
            //データの存在チェック
            var dataSet = await dataSetRepository.GetByIdAsync(model.DataSetId.Value);
            if (dataSet == null)
            {
                return (null,
                    DoJsonNotFound(typeof(TrainingController), requestUrl, modelState,
                    $"DataSet ID {model.DataSetId} is not found."));
            }
            if (string.IsNullOrEmpty(model.Partition) == false)
            {
                bool existPartition = await nodeRepository.IsEnablePartitionAsync(model.Partition, true);
                if (existPartition == false)
                {
                    return (null,
                        DoJsonNotFound(typeof(TrainingController), requestUrl, modelState,
                        $"There are no enable nodes with Partition {model.Partition}."));
                }
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
                            return (null,
                                DoJsonNotFound(typeof(TrainingController), requestUrl, modelState, $"Invalid envName. Please match the format of '^[-._a-zA-Z][-._a-zA-Z0-9]*$'."));
                        }
                    }
                }
            }

            long? gitId = model.GitModel.GitId ?? currentUserInfo.SelectedTenant.DefaultGit?.Id;
            string branch = model.GitModel.Branch ?? "master";
            string commitId = model.GitModel.CommitId;
            //コミットIDが指定されていなければ、ブランチのHEADからコミットIDを取得する
            if (string.IsNullOrEmpty(commitId))
            {
                commitId = await gitLogic.GetCommitIdAsync(gitId.Value, model.GitModel.Repository, model.GitModel.Owner, branch);
                if (string.IsNullOrEmpty(commitId))
                {
                    //コミットIDが特定できなかったらエラー
                    return (null,
                        DoJsonNotFound(typeof(TrainingController), requestUrl, modelState,
                        $"The branch {branch} for {gitId.Value}/{model.GitModel.Owner}/{model.GitModel.Repository} is not found."));
                }
            }

            // 各リソースの超過チェック
            Tenant tenant = tenantRepository.Get(currentUserInfo.SelectedTenant.Id);
            string errorMessage = clusterManagementLogic.CheckQuota(tenant, model.Cpu.Value, model.Memory.Value, model.Gpu.Value);
            if (errorMessage != null)
            {
                return (null, DoJsonBadRequest(typeof(TrainingController), requestUrl, modelState, errorMessage));
            }

            //コンテナの実行前に、学習履歴を作成する（コンテナの実行に失敗した場合、そのステータスをユーザに表示するため）
            var trainingHistory = new TrainingHistory()
            {
                Name = model.Name,
                DisplayId = -1,
                ContainerRegistryId = model.ContainerImage.RegistryId ?? currentUserInfo.SelectedTenant.DefaultRegistry?.Id,
                ContainerImage = model.ContainerImage.Image,
                ContainerTag = model.ContainerImage.Tag, //latestは運用上使用されていないハズなので、そのまま直接代入
                DataSetId = model.DataSetId.Value,
                EntryPoint = model.EntryPoint,
                ModelGitId = gitId.Value,
                ModelRepository = model.GitModel.Repository,
                ModelRepositoryOwner = model.GitModel.Owner,
                ModelBranch = branch,
                ModelCommitId = commitId,
                OptionDic = model.Options ?? new Dictionary<string, string>(), //オプションはnullの可能性があるので、その時は初期化
                Memo = model.Memo,
                Cpu = model.Cpu.Value,
                Memory = model.Memory.Value,
                Gpu = model.Gpu.Value,
                Partition = model.Partition,
                PortList = model.Ports,
                Status = ContainerStatus.Running.Key,
                Zip = model.Zip,
                LocalDataSet = model.LocalDataSet,
            };
            if (trainingHistory.OptionDic.ContainsKey("")) //空文字は除外する
            {
                trainingHistory.OptionDic.Remove("");
            }
            // 親学習が指定されていれば存在チェック
            if (model.ParentIds != null)
            {
                var maps = new List<TrainingHistoryParentMap>();

                foreach (var parentId in model.ParentIds)
                {
                    var parent = await trainingHistoryRepository.GetByIdAsync(parentId);
                    if (parent == null)
                    {
                        return (null, DoJsonNotFound(typeof(TrainingController), requestUrl, modelState, $"Training ID {parentId} is not found."));
                    }
                    // 学習履歴に親学習を紐づける
                    var map = trainingHistoryRepository.AttachParentAsync(trainingHistory, parent);
                    if (map != null)
                    {
                        maps.Add(map);
                    }
                }

                trainingHistory.ParentMaps = maps;
            }
            //タグの登録
            if (model.Tags != null && model.Tags.Count() > 0)
            {
                tagLogic.CreateTrainingHistoryTags(trainingHistory, model.Tags);
            }

            trainingHistoryRepository.Add(trainingHistory);
            if (dataSet.IsLocked == false)
            {
                dataSet.IsLocked = true;
            }
            unitOfWork.Commit();

            var result = await clusterManagementLogic.RunTrainContainerAsync(trainingHistory, scriptType);
            if (result.IsSuccess == false)
            {
                //コンテナの起動に失敗した状態。エラーを出力して、保存した学習履歴も削除する。
                trainingHistoryRepository.Delete(trainingHistory);
                unitOfWork.Commit();

                return (null, DoJsonError(HttpStatusCode.ServiceUnavailable, "Failed to run training. The message bellow may be help to resolve: " + result.Error,
                    typeof(TrainingController), requestUrl, modelState));
            }

            //結果に従い、学習結果を更新する。
            //実行には時間がかかりうるので、DBから最新の情報を取ってくる
            trainingHistory = await trainingHistoryRepository.GetByIdAsync(trainingHistory.Id);
            trainingHistory.Configuration = result.Value.Configuration;
            trainingHistory.Status = result.Value.Status.Key;
            unitOfWork.Commit();

            if (result.Value.Status.Succeed())
            {
                return (trainingHistory,
                    JsonCreated(new SimpleOutputModel(trainingHistory)));
            }
            else
            {
                return (trainingHistory,
                    DoJsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run training. Status={result.Value.Status.Name}. Please contact your server administrator.",
                    typeof(TrainingController), requestUrl, modelState));
            }
        }

        /// <summary>
        /// 新規に学習を開始する
        /// </summary>
        /// <param name="model">新規学習実行内容</param>
        [HttpPost("run")]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] CreateInputModel model)
        {
            (var _, var result) = await DoCreate(model,
                dataSetRepository, nodeRepository, tenantRepository, trainingHistoryRepository,
                clusterManagementLogic, gitLogic, tagLogic,
                unitOfWork, CurrentUserInfo, ModelState, RequestUrl, "training");
            return result;
        }

        /// <summary>
        /// 学習履歴の編集
        /// </summary>
        /// <param name="id">変更対象の学習履歴ID</param>
        /// <param name="model">変更内容</param>
        [HttpPut("{id}")]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody]EditInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid || ! id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var history = await trainingHistoryRepository.GetByIdAsync(id.Value);
            if (history == null)
            {
                return JsonNotFound($"Training ID {id.Value} is not found.");
            }

            history.Name = EditColumnNotEmpty(model.Name, history.Name);
            history.Memo = EditColumn(model.Memo, history.Memo);
            history.Favorite = EditColumn(model.Favorite, history.Favorite);
            
            //タグの編集。指定がない場合は変更なしと見なして何もしない。
            if (model.Tags != null)
            {
                if (model.Tags.Count() > 0)
                {
                    //タグが一つでも指定されていたら、全部上書き
                    await tagLogic.EditTrainingHistoryTagsAsync(history.Id, model.Tags);
                }
                else
                {
                    //タグがゼロなら全削除
                    tagLogic.DeleteTrainingHistoryTags(history.Id);
                }
            }

            unitOfWork.Commit();

            // 未使用タグ削除
            tagRepository.DeleteUnUsedTrainingHistoryTags();

            // DBへタグ削除結果のコミット
            unitOfWork.Commit();

            return JsonOK(new SimpleOutputModel(history));
        }

        /// <summary>
        /// 学習履歴添付ファイルを登録する。
        /// </summary>
        /// <param name="id">対象の学習履歴ID</param>
        /// <param name="model">追加するファイル情報</param>
        [HttpPost("{id}/files")]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(AttachedFileOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegistAttachedFile(long id, [FromBody]AddFileInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            //データの存在チェック
            var trainingHistory = await trainingHistoryRepository.GetByIdAsync(id);
            if (trainingHistory == null)
            {
                return JsonNotFound($"Training ID {id} is not found.");
            }

            //同じ名前のファイルは登録できない
            if(await trainingHistoryRepository.ExistsAttachedFileAsync(id, model.FileName))
            {
                return JsonConflict($"Training {id} has already a file named {model.FileName}.");
            }

            var attachedFile = new TrainingHistoryAttachedFile
            {
                TrainingHistoryId = id,
                FileName = model.FileName,
                Key = ResourceType.TrainingHistoryAttachedFiles.ToString(), //model.Key ?? ResourceType.TrainingHistoryAttachedFiles.ToString();
                StoredPath = model.StoredPath
            };
            trainingHistoryRepository.AddAttachedFile(attachedFile);
            unitOfWork.Commit();

            return JsonOK(new AttachedFileOutputModel(id, model.FileName, attachedFile.Id));
        }

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
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(StorageListResultInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUnderDir(long id, [FromQuery] string path = "/", [FromQuery] bool withUrl = false)
        {
            //データの存在チェック
            var trainingHistory = await trainingHistoryRepository.GetByIdAsync(id);
            if (trainingHistory == null)
            {
                return JsonNotFound($"Training ID {id} is not found.");
            }

            // 検索path文字列の先頭・末尾が/でない場合はつける
            if (!path.StartsWith("/", StringComparison.CurrentCulture)) {
                path = "/" + path;
            }
            if (!path.EndsWith("/", StringComparison.CurrentCulture)) {
                path = path + "/";
            }

            // windowsから実行された場合、区切り文字が"\\"として送られてくるので"/"に置換する
            path = path.Replace("\\", "/");

            var rootDir = $"{id}" + path;

            var result = await storageLogic.GetUnderDirAsync(ResourceType.TrainingContainerOutputFiles, rootDir);

            if (withUrl)
            {
                result.Value.Files.ForEach(x => x.Url = storageLogic.GetPreSignedUriForGetFromKey(x.Key, x.FileName, true).ToString());
            }
             

            return JsonOK(result.Value);
        }

        /// <summary>
        /// 学習履歴添付ファイルの一覧を取得する。
        /// </summary>
        /// <param name="id">対象の学習履歴ID</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        [HttpGet("{id}/files")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Experiment)]
        [ProducesResponseType(typeof(IEnumerable<AttachedFileOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAttachedFiles(long id, [FromQuery] bool withUrl = false)
        {
            //データの存在チェック
            var trainingHistory = await trainingHistoryRepository.GetByIdAsync(id);
            if (trainingHistory == null)
            {
                return JsonNotFound($"Training ID {id} is not found.");
            }

            var underDir = await storageLogic.GetUnderDirAsync(ResourceType.TrainingContainerAttachedFiles, $"{id}/");
            if(underDir.IsSuccess == false) {
                return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to access the storage service. Please contact to system administrators.");
            }

            var containerAttachedFiles = new List<AttachedFileOutputModel>();
            containerAttachedFiles.AddRange(underDir.Value.Files.Select(
                f => new AttachedFileOutputModel(trainingHistory.Id, f.FileName, -1)
                {
                    Url = withUrl ? storageLogic.GetPreSignedUriForGetFromKey(f.Key, f.FileName, true).ToString() : null,
                    IsLocked = true
                }
                ));

            var userAttachedFiles = new List<AttachedFileOutputModel>();

            var filesOnDB = await trainingHistoryRepository.GetAllAttachedFilesAsync(id);
            userAttachedFiles.AddRange(filesOnDB.Select(
                f => new AttachedFileOutputModel(trainingHistory.Id, f.FileName, f.Id)
                {
                    Url = withUrl ? storageLogic.GetPreSignedUriForGet(ResourceType.TrainingHistoryAttachedFiles, f.StoredPath, f.FileName, true).ToString() : null,
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
        [Filters.PermissionFilter(MenuCode.Training)]
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
            TrainingHistoryAttachedFile file = await trainingHistoryRepository.GetAttachedFileAsync(fileId.Value);
            if (file == null)
            {
                return JsonNotFound($"File ID {fileId.Value} is not found.");
            }

            trainingHistoryRepository.DeleteAttachedFile(file);
            await storageLogic.DeleteFileAsync(ResourceType.TrainingHistoryAttachedFiles, file.StoredPath);
            unitOfWork.Commit();

            return JsonNoContent();
        }

        /// <summary>
        /// 指定したTensorBoardコンテナ情報を取得する
        /// </summary>
        /// <param name="id">対象の学習履歴ID</param>
        [HttpGet("{id}/tensorboard")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Experiment)]
        [ProducesResponseType(typeof(TensorBoardOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTensorBoardStatus(long id)
        {
            //データの存在チェック
            var trainingHistory = await trainingHistoryRepository.GetByIdAsync(id);
            if (trainingHistory == null)
            {
                return JsonNotFound($"Training ID {id} is not found.");
            }

            //学習履歴IDとテナントIDが等しく、Disable状態じゃないコンテナをDBから取得する
            TensorBoardContainer container = tensorBoardContainerRepository.GetAvailableContainer(id);

            //以下、Jsonで返却するパラメータ
            ContainerStatus status = ContainerStatus.None; //コンテナのステータス

            // 対象コンテナ情報が存在する場合、その結果を返す
            if (container != null)
            {
                //コンテナのステータスを最新にする
                status = await clusterManagementLogic.SyncContainerStatusAsync(container, false);
            }

            return JsonOK(new TensorBoardOutputModel(container, status, containerOptions.WebEndPoint));
        }

        #region TensorBoard

        /// <summary>
        /// 指定した学習のTensor Boardを立てる
        /// </summary>
        /// <param name="id">対象の学習履歴ID</param>
        /// <param name="model">起動モデル</param>
        [HttpPut("{id}/tensorboard")] //TensorBoardはIDをユーザに通知するわけではないので、POSTではなくPUTで扱う
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Experiment)]
        [ProducesResponseType(typeof(TensorBoardOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RunTensorBoard(long id, [FromBody]TensorBoardInputModel model)
        {
            //データの存在チェック
            var trainingHistory = await trainingHistoryRepository.GetByIdAsync(id);
            if (trainingHistory == null)
            {
                return JsonNotFound($"Training ID {id} is not found.");
            }

            //学習履歴IDとテナントIDが等しく、Disable状態じゃないコンテナを取得する
            TensorBoardContainer container = tensorBoardContainerRepository.GetAvailableContainer(id);
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
            var result = await clusterManagementLogic.RunTensorBoardContainerAsync(trainingHistory, expiresIn, model.selectedHistoryIds);
            if (result == null || result.Status.Succeed() == false)
            {
                //起動に失敗した場合、ステータス Failed で返す。
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run tensorboard container: {result.Status}");
            }

            container = new TensorBoardContainer()
            {
                Name = result.Name,
                StartedAt = DateTime.Now,
                Status = result.Status.Name,
                TenantId = CurrentUserInfo.SelectedTenant.Id,
                TrainingHistoryId = id,
                Host = result.Host,
                PortNo = result.Port,
                ExpiresIn = expiresIn,
                MountedTrainingHistoryIdList = model.selectedHistoryIds
            };

            // コンテナテーブルにInsertする
            tensorBoardContainerRepository.Add(container);
            
            unitOfWork.Commit();

            return JsonOK(new TensorBoardOutputModel(container, result.Status, containerOptions.WebEndPoint));
        }

        /// <summary>
        /// 指定した学習のTensor Boardを削除する
        /// </summary>
        /// <param name="id">対象の学習履歴ID</param>
        [HttpDelete("{id}/tensorboard")]
        [Filters.PermissionFilter(MenuCode.Training, MenuCode.Experiment)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteTensorBoard(long id)
        {
            //学習履歴IDとテナントIDが等しく、Disable状態じゃないコンテナを取得する
            TensorBoardContainer container = tensorBoardContainerRepository.GetAvailableContainer(id);
            // 当該レコードが存在しない場合、404エラー
            if (container == null)
            {
                return JsonNotFound($"TensorBoard container for TrainingHistory ID {id} is not found.");
            }

            await trainingLogic.DeleteTensorBoardAsync(container, false);

            return JsonNoContent();
        }

        # endregion

        /// <summary>
        /// 学習を途中で強制終了させる。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpPost("{id}/halt")]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Halt(long? id)
        {
            return await ExitAsync(id, ContainerStatus.Killed);
        }

        /// <summary>
        /// 学習を途中で強制終了させる。
        /// ユーザ自身がジョブを停止させた場合。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpPost("{id}/user-cancel")]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UserCancel(long? id)
        {
            return await ExitAsync(id, ContainerStatus.UserCanceled);
        }

        /// <summary>
        /// 学習を正常終了させる。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpPost("{id}/complete")]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Complete(long? id)
        {
            return await ExitAsync(id, ContainerStatus.Completed);
        }

        /// <summary>
        /// 学習を終了させる。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
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
            var trainingHistory = await trainingHistoryRepository.GetByIdAsync(id.Value);
            if (trainingHistory == null)
            {
                return JsonNotFound($"Training ID {id} is not found.");
            }
            if (trainingHistory.GetStatus().Exist() == false)
            {
                //終了できるのはRunningのコンテナだけ
                return JsonBadRequest($"Training {trainingHistory.Name} does not exist.");
            }

            await trainingLogic.ExitAsync(trainingHistory, status, false);

            return JsonOK(new SimpleOutputModel(trainingHistory));
        }

        /// <summary>
        /// 学習履歴を削除する。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpDelete("{id}")]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var trainingHistory = await trainingHistoryRepository.GetByIdAsync(id.Value);
            if (trainingHistory == null)
            {
                return JsonNotFound($"Training ID {id} is not found.");
            }

            //ステータスを確認

            var status = trainingHistory.GetStatus();
            if (status.Exist())
            {
                //学習がまだ進行中の場合、情報を更新する
                status = await clusterManagementLogic.GetContainerStatusAsync(trainingHistory.Key, CurrentUserInfo.SelectedTenant.Name, false);
            }

            //派生した学習履歴があったら消せない
            var child = (await trainingHistoryRepository.GetChildrenAsync(trainingHistory.Id)).FirstOrDefault();
            if (child != null)
            {
                return JsonConflict($"There is another training which is derived from training {trainingHistory.Id}.");
            }

            //学習結果を利用した推論ジョブがあったら消せない
            var inferenceHistory = (await inferenceHistoryRepository.GetMountedTrainingAsync(trainingHistory.Id)).FirstOrDefault();
            if (inferenceHistory != null)
            {
                return JsonConflict($"Training {trainingHistory.Id} has been used by inference.");
            }

            if (status.Exist())
            {
                //実行中であれば、コンテナを削除
                await clusterManagementLogic.DeleteContainerAsync(
                    ContainerType.Training, trainingHistory.Key, CurrentUserInfo.SelectedTenant.Name, false);
            }
            
            //TensorBoardを起動中だった場合は、そっちも消す
            TensorBoardContainer container = tensorBoardContainerRepository.GetAvailableContainer(trainingHistory.Id);
            if (container != null)
            {
                await clusterManagementLogic.DeleteContainerAsync(
                    ContainerType.TensorBoard, container.Name, CurrentUserInfo.SelectedTenant.Name, false);
                tensorBoardContainerRepository.Delete(container, true);
            }

            //添付ファイルがあったらまとめて消す
            var files = await trainingHistoryRepository.GetAllAttachedFilesAsync(trainingHistory.Id);
            foreach(var file in files)
            {
                trainingHistoryRepository.DeleteAttachedFile(file);
                await storageLogic.DeleteFileAsync(ResourceType.TrainingHistoryAttachedFiles, file.StoredPath);
            }

            // タグマップを削除
            tagLogic.DeleteTrainingHistoryTags(trainingHistory.Id);

            trainingHistoryRepository.Delete(trainingHistory);
            unitOfWork.Commit();

            // 未使用タグ削除
            tagRepository.DeleteUnUsedTrainingHistoryTags();

            // DBへタグ削除結果のコミット
            unitOfWork.Commit();

            // ストレージ内の学習データを削除する
            await storageLogic.DeleteResultsAsync(ResourceType.TrainingContainerAttachedFiles, trainingHistory.Id);
            await storageLogic.DeleteResultsAsync(ResourceType.TrainingContainerOutputFiles, trainingHistory.Id);

            return JsonNoContent();
        }

        /// <summary>
        /// 選択中のテナントに登録されている学習管理で使用するタグを表示する
        /// </summary>
        [HttpGet("tags")]
        [Filters.PermissionFilter(MenuCode.Training)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public IActionResult GetTags()
        {
            // タグ種別が学習のものに限定する
            var tags = tagLogic.GetAllTags().Where(t => t.Type == TagType.Training);
            return JsonOK(tags.Select(t => t.Name));
        }
    }
}

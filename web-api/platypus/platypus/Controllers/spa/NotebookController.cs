using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.NotebookApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
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
    /// Notebookを扱うためのAPI集
    /// </summary>
    [ApiVersion("1"), ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/notebook")]
    public class NotebookController : PlatypusApiControllerBase
    {
        private readonly INotebookHistoryRepository notebookHistoryRepository;
        private readonly ITrainingHistoryRepository trainingHistoryRepository;
        private readonly IInferenceHistoryRepository inferenceHistoryRepository;
        private readonly IDataSetRepository dataSetRepository;
        private readonly ITenantRepository tenantRepository;
        private readonly INodeRepository nodeRepository;
        private readonly INotebookLogic notebookLogic;
        private readonly IStorageLogic storageLogic;
        private readonly IGitLogic gitLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NotebookController(
            INotebookHistoryRepository notebookHistoryRepository,
            ITrainingHistoryRepository trainingHistoryRepository,
            IInferenceHistoryRepository inferenceHistoryRepository,
            IDataSetRepository dataSetRepository,
            ITenantRepository tenantRepository,
            INodeRepository nodeRepository,
            INotebookLogic notebookLogic,
            IStorageLogic storageLogic,
            IGitLogic gitLogic,
            IClusterManagementLogic clusterManagementLogic,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.notebookHistoryRepository = notebookHistoryRepository;
            this.trainingHistoryRepository = trainingHistoryRepository;
            this.inferenceHistoryRepository = inferenceHistoryRepository;
            this.dataSetRepository = dataSetRepository;
            this.tenantRepository = tenantRepository;
            this.nodeRepository = nodeRepository;
            this.notebookLogic = notebookLogic;
            this.storageLogic = storageLogic;
            this.gitLogic = gitLogic;
            this.clusterManagementLogic = clusterManagementLogic;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 全ノートブック履歴のIDと名前を取得
        /// </summary>
        [HttpGet("simple")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<SimpleOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var histories = notebookHistoryRepository.GetAll();
            return JsonOK(histories.Select(history => new SimpleOutputModel(history)));
        }

        /// <summary>
        /// 指定された条件でページングされた状態で、全ノートブック履歴を取得
        /// </summary>
        /// <param name="page">ページ番号。デフォルトは1。</param>
        /// <param name="perPage">表示件数。指定がない場合は上限(1000件)。</param>
        /// <param name="filter">検索条件</param>
        /// <param name="withTotal">合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。</param>
        [HttpGet]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll([FromQuery]SearchInputModel filter, [FromQuery]int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            var data = notebookHistoryRepository.GetAllWithOrdering();
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
        /// <param name="history">ノートブック履歴</param>
        private async Task<IndexOutputModel> GetUpdatedIndexOutputModelAsync(NotebookHistory history)
        {
            var model = new IndexOutputModel(history);

            var status = history.GetStatus();
            if (status.Exist())
            {
                //ノートブックコンテナがまだ進行中の場合、情報を更新する
                var newStatus = await clusterManagementLogic.GetContainerStatusAsync(history.Key, CurrentUserInfo.SelectedTenant.Name, false);

                if (status.Key != newStatus.Key)
                {
                    //更新があったので、変更処理
                    await notebookHistoryRepository.UpdateStatusAsync(history.Id, newStatus, false);
                    unitOfWork.Commit();

                    model.Status = newStatus.Name;
                }
            }
            return model;
        }

        /// <summary>
        /// データ件数を取得する
        /// </summary>
        /// <param name="filter">検索条件</param>
        private int GetTotalCount(SearchInputModel filter)
        {
            IQueryable<NotebookHistory> histories;
            histories = notebookHistoryRepository.GetAll();
            histories = Search(histories, filter);
            return histories.Count();
        }

        /// <summary>
        /// 検索条件の追加
        /// </summary>
        /// <param name="sourceData">加工前の検索結果</param>
        /// <param name="filter">検索条件</param>
        private static IQueryable<NotebookHistory> Search(IQueryable<NotebookHistory> sourceData, SearchInputModel filter)
        {
            IQueryable<NotebookHistory> data = sourceData;
            data = data
                .SearchLong(d => d.Id, filter.Id)
                .SearchString(d => d.Name, filter.Name)
                .SearchTime(d => d.CreatedAt, filter.CreatedAt)
                .SearchString(d => d.CreatedBy, filter.CreatedBy)
                .SearchString(d => d.Memo, filter.Memo)
                .SearchString(d => d.GetStatus().ToString(), filter.Status);
            return data;
        }

        /// <summary>
        /// 指定されたIDのノートブック履歴の詳細情報を取得。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        [HttpGet("{id}")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Notebook ID is required.");
            }
            var notebookHistory = await notebookHistoryRepository.GetIncludeAllAsync(id.Value);
            if (notebookHistory == null)
            {
                return JsonNotFound($"Notebook ID {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(notebookHistory);

            var status = notebookHistory.GetStatus();
            model.StatusType = status.StatusType;
            if (status.Exist())
            {
                //コンテナがまだ存在している場合、情報を更新する
                var details = await clusterManagementLogic.GetContainerEndpointInfoAsync(notebookHistory.Key, CurrentUserInfo.SelectedTenant.Name, false);
                model.Status = details.Status.Name;
                model.StatusType = details.Status.StatusType;

                //ステータスを更新
                notebookHistory.Status = details.Status.Key;
                if (notebookHistory.StartedAt == null)
                {
                    notebookHistory.StartedAt = details.StartedAt;
                    notebookHistory.Node = details.Node; //設計上ノードが切り替わることはない
                }
                unitOfWork.Commit();

                model.ConditionNote = details.ConditionNote;

                //コンテナが正常動作している場合、notebookのエンドポイントを取得
                if (details.Status.IsRunning())
                {
                    model.NotebookNodePort = GetNotebookNodePort(details.EndPoints);
                    model.NotebookToken = await GetNotebookTokenAsync(notebookHistory.Id);
                }
            }

            if (notebookHistory.ModelGitId != null)
            {
                //Gitの表示用URLを作る
                model.GitModel.Url = gitLogic.GetTreeUiUrl(notebookHistory.ModelGitId.Value, notebookHistory.ModelRepository, notebookHistory.ModelRepositoryOwner, notebookHistory.ModelCommitId);
            }
            return JsonOK(model);
        }

        /// <summary>
        /// ノートブックのノードポート番号を取得する
        /// </summary>
        /// <param name="endPoints">エンドポイント</param>
        /// <returns>ノードポート番号</returns>
        private string GetNotebookNodePort(IEnumerable<EndPointInfo> endPoints)
        {
            // ノードのendpoint情報を取得し、tokenと繋ぎ合わせてmodelに設定
            var endPoint = endPoints.Where(name => name.Key == "notebook").FirstOrDefault();
            if (endPoint != null)
            {
                //ノードポート番号+tokenを返す
                return endPoint.Port.ToString();
            }
            return "";
        }

        /// <summary>
        /// ノートブックのトークンを取得する
        /// </summary>
        /// <param name="historyId">ノートブック履歴ID</param>
        /// <returns></returns>
        private async Task<string> GetNotebookTokenAsync(long historyId)
        {
            //notebook起動時のログをストレージから取得し、token情報を抜き出す。
            var outputFileName = ".notebook.log";   //値を読み込むファイル名
            var outputPath = historyId + "/" + outputFileName;
            var content = await storageLogic.GetFileContentAsync(ResourceType.NotebookContainerAttachedFiles, outputPath, outputFileName, true);
            if (content != null)
            {
                // ?token=...という文字列を抜き出す
                var token = Regex.Match(content, @"\?token=.*").Value;
                return token;
            }
            return "";
        }

        /// <summary>
        /// ノートブック履歴の編集
        /// </summary>
        /// <param name="id">変更対象のノートブック履歴ID</param>
        /// <param name="model">変更内容</param>
        [HttpPut("{id}")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody]EditInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var notebookHistory = await notebookHistoryRepository.GetByIdAsync(id.Value);
            if (notebookHistory == null)
            {
                return JsonNotFound($"Notebook ID {id.Value} is not found.");
            }

            notebookHistory.Name = EditColumnNotEmpty(model.Name, notebookHistory.Name);
            notebookHistory.Memo = EditColumn(model.Memo, notebookHistory.Memo);
            notebookHistory.Favorite = EditColumn(model.Favorite, notebookHistory.Favorite);
            unitOfWork.Commit();

            return JsonOK(new SimpleOutputModel(notebookHistory));
        }

        /// <summary>
        /// ノートブック履歴を削除する。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        [HttpDelete("{id}")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            //データの存在チェック
            var notebookHistory = await notebookHistoryRepository.GetByIdAsync(id.Value);
            if (notebookHistory == null)
            {
                return JsonNotFound($"Notebook ID {id} is not found.");
            }

            //ステータスを確認
            var status = notebookHistory.GetStatus();
            if (status.Exist())
            {
                //Notebookコンテナが起動中の場合、情報を更新する
                status = await clusterManagementLogic.GetContainerStatusAsync(notebookHistory.Key, CurrentUserInfo.SelectedTenant.Name, false);
            }

            if (status.Exist())
            {
                //実行中であれば、コンテナを削除
                await clusterManagementLogic.DeleteContainerAsync(
                    ContainerType.Notebook, notebookHistory.Key, CurrentUserInfo.SelectedTenant.Name, false);
            }

            notebookHistoryRepository.Delete(notebookHistory);
            unitOfWork.Commit();

            // ストレージ内のノートブックデータを削除する
            await storageLogic.DeleteResultsAsync(ResourceType.NotebookContainerAttachedFiles, notebookHistory.Id);
            await storageLogic.DeleteResultsAsync(ResourceType.NotebookContainerOutputFiles, notebookHistory.Id);

            return JsonNoContent();
        }

        /// <summary>
        /// 指定されたノートブック履歴のエラーイベントを取得します。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        /// <returns>ログファイル</returns>
        [HttpGet("{id}/events")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(ContainerEventInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetErrorEventAsync(long id)
        {
            var history = await notebookHistoryRepository.GetByIdAsync(id);
            if (history == null)
            {
                return JsonNotFound($"Notebook ID {id} is not found.");
            }
            if (history.GetStatus().Exist() == false)
            {
                return JsonBadRequest($"A container for Notebook ID {id} does not exist.");
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

        /// <summary>
        /// 指定されたノートブック履歴のエンドポイントを取得します。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        /// <returns>ノートブックURL</returns>
        [HttpGet("{id}/endpoint")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(EndPointOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetEndpointAsync(long id)
        {
            //データの存在チェック
            var notebookHistory = await notebookHistoryRepository.GetByIdAsync(id);
            if (notebookHistory == null)
            {
                return JsonNotFound($"Notebook ID {id} is not found.");
            }

            //ノードポート番号およびアクセストークンを取得する
            string nodePort = "";
            string token = "";
            var status = notebookHistory.GetStatus();
            //ステータスを確認
            if (status.Exist())
            {
                //コンテナがまだ存在している場合、情報を更新する
                var details = await clusterManagementLogic.GetContainerEndpointInfoAsync(notebookHistory.Key, CurrentUserInfo.SelectedTenant.Name, false);
                //ステータスを更新
                notebookHistory.Status = details.Status.Key;
                if (notebookHistory.StartedAt == null)
                {
                    notebookHistory.StartedAt = details.StartedAt;
                    notebookHistory.Node = details.Node; //設計上ノードが切り替わることはない
                }
                unitOfWork.Commit();

                //コンテナが正常動作している場合、notebookのエンドポイントを取得
                if (details.Status.IsRunning())
                {
                    nodePort = GetNotebookNodePort(details.EndPoints);
                    token = await GetNotebookTokenAsync(notebookHistory.Id);
                }
            }
            else
            {
                return JsonBadRequest($"A container for Notebook ID {id} does not exist.");
            }

            return JsonOK(new EndPointOutputModel(nodePort, token));
        }

        /// <summary>
        /// 新規にノートブックコンテナを開始する
        /// </summary>
        /// <param name="model">新規実行内容</param>
        [HttpPost("run")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody]CreateInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            //データセットが指定されていれば存在チェック
            if (model.DataSetId.HasValue)
            {
                var dataSet = await dataSetRepository.GetByIdAsync(model.DataSetId.Value);
                if (dataSet == null)
                {
                    return JsonNotFound($"DataSet ID {model.DataSetId} is not found.");
                }
            }

            // ノートブック無期限フラグのチェック
            Tenant tenant = tenantRepository.Get(CurrentUserInfo.SelectedTenant.Id);
            if (tenant.AvailableInfiniteTimeNotebook == false && model.ExpiresIn == 0)
            {
                return JsonBadRequest($"Tenant [{tenant.DisplayName}] is not allowed to run infinite time.");
            }

            if (string.IsNullOrEmpty(model.Partition) == false)
            {
                bool existPartition = await nodeRepository.IsEnablePartitionAsync(model.Partition, true);
                if (existPartition == false)
                {
                    return JsonNotFound($"There are no enable nodes with Partition {model.Partition}.");
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
                            return JsonNotFound($"Invalid envName. Please match the format of '^[-._a-zA-Z][-._a-zA-Z0-9]*$'.");
                        }
                    }
                }
            }

            // 各リソースの超過チェック
            string errorMessage = clusterManagementLogic.CheckQuota(tenant, model.Cpu.Value, model.Memory.Value, model.Gpu.Value);
            if (errorMessage != null)
            {
                return JsonBadRequest(errorMessage);
            }

            //コンテナの実行前に、ノートブック履歴を作成する（コンテナの実行に失敗した場合、そのステータスをユーザに表示するため）
            var notebookHistory = new NotebookHistory()
            {
                DisplayId = -1,
                Name = model.Name,
                DataSetId = model.DataSetId,
                OptionDic = model.Options ?? new Dictionary<string, string>(), //オプションはnullの可能性があるので、その時は初期化
                Cpu = model.Cpu.Value,
                Memory = model.Memory.Value,
                Gpu = model.Gpu.Value,
                Partition = model.Partition,
                Memo = model.Memo,
                Status = ContainerStatus.Running.Key,
                StartedAt = DateTime.Now,
                ExpiresIn = model.ExpiresIn,
                LocalDataSet = model.LocalDataSet,
                EntryPoint = model.EntryPoint
            };

            //コンテナが指定されているかチェック
            if (model.ContainerImage != null)
            {
                notebookHistory.ContainerRegistryId = model.ContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id;
                notebookHistory.ContainerImage = model.ContainerImage.Image;
                notebookHistory.ContainerTag = model.ContainerImage.Tag; //latestは運用上使用されていないハズなので、そのまま直接代入
            }
            else
            {
                //コンテナイメージの設定がない場合デフォルトのイメージを設定
                notebookHistory.ContainerRegistryId = null;
                notebookHistory.ContainerImage = "tensorflow/tensorflow";
                notebookHistory.ContainerTag = "1.13.1-gpu-py3";
            }

            //gitが指定されているかチェック
            if (model.GitModel != null)
            {
                //gitリポジトリ名が指定されていれば、ブランチ、コミットIDを設定。指定されていなければnull
                long? gitId = model.GitModel.GitId ?? CurrentUserInfo.SelectedTenant.DefaultGit?.Id;
                string branch = null;
                string commitId = null;
                if (!string.IsNullOrEmpty(model.GitModel.Repository))
                {
                    branch = model.GitModel.Branch ?? "master";
                    commitId = model.GitModel.CommitId;
                    //コミットIDが指定されていなければ、ブランチのHEADからコミットIDを取得する
                    if (string.IsNullOrEmpty(commitId))
                    {
                        commitId = await gitLogic.GetCommitIdAsync(gitId.Value, model.GitModel.Repository, model.GitModel.Owner, branch);
                        if (string.IsNullOrEmpty(commitId))
                        {
                            //コミットIDが特定できなかったらエラー
                            return JsonNotFound($"The branch {branch} for {gitId.Value}/{model.GitModel.Owner}/{model.GitModel.Repository} is not found.");
                        }
                    }
                }
                //git情報を設定
                notebookHistory.ModelGitId = gitId.Value;
                notebookHistory.ModelRepository = model.GitModel.Repository;
                notebookHistory.ModelRepositoryOwner = model.GitModel.Owner;
                notebookHistory.ModelBranch = branch;
                notebookHistory.ModelCommitId = commitId;
            }

            if (notebookHistory.OptionDic.ContainsKey("")) //空文字は除外する
            {
                notebookHistory.OptionDic.Remove("");
            }
            // 親学習が指定されていれば存在チェック
            if (model.ParentIds != null)
            {
                var maps = new List<NotebookHistoryParentTrainingMap>();

                foreach (var parentId in model.ParentIds)
                {
                    var parent = await trainingHistoryRepository.GetByIdAsync(parentId);
                    if (parent == null)
                    {
                        return JsonNotFound($"Training ID {parentId} is not found.");
                    }
                    // ノートブック履歴に親学習を紐づける
                    var map = notebookHistoryRepository.AttachParentToNotebookAsync(notebookHistory, parent);
                    if (map != null)
                    {
                        maps.Add(map);
                    }
                }
                notebookHistory.ParentTrainingMaps = maps;
            }

            // 親推論が指定されていれば存在チェック
            if (model.InferenceIds != null)
            {
                var maps = new List<NotebookHistoryParentInferenceMap>();

                foreach (var parentId in model.InferenceIds)
                {
                    var parenInferencet = await inferenceHistoryRepository.GetByIdAsync(parentId);
                    if (parenInferencet == null)
                    {
                        return JsonNotFound($"Inference ID {parentId} is not found.");
                    }
                    // ノートブック履歴に親学習を紐づける
                    var map = notebookHistoryRepository.AttachParentInferenceToNotebookAsync(notebookHistory, parenInferencet);
                    if (map != null)
                    {
                        maps.Add(map);
                    }
                }
                notebookHistory.ParentInferenceMaps = maps;
            }
            
            notebookHistoryRepository.Add(notebookHistory);
            unitOfWork.Commit();

            var result = await clusterManagementLogic.RunNotebookContainerAsync(notebookHistory);
            if (result.IsSuccess == false)
            {
                //コンテナの起動に失敗した状態。エラーを出力して、保存したノートブック履歴も削除する。
                notebookHistoryRepository.Delete(notebookHistory);
                unitOfWork.Commit();

                return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to run notebook. The message bellow may be help to resolve: " + result.Error);
            }

            //結果に従い、ノートブック結果を更新する。
            //実行には時間がかかりうるので、DBから最新の情報を取ってくる
            notebookHistory = await notebookHistoryRepository.GetByIdAsync(notebookHistory.Id);
            notebookHistory.Configuration = result.Value.Configuration;
            notebookHistory.Status = result.Value.Status.Key;
            unitOfWork.Commit();

            if (result.Value.Status.Succeed())
            {
                return JsonCreated(new SimpleOutputModel(notebookHistory));
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run notebook. Status={result.Value.Status.Name}. Please contact your server administrator.");
            }
        }

        /// <summary>
        /// コンテナの出力ファイルの一覧を取得する。
        /// </summary>
        /// <remarks> 
        /// コンテナの/output/配下から指定ディレクトリパスの直下を検索する
        /// 検索対象ディレクトリが見つからない場合もファイル・ディレクトリが空の結果を返す
        /// </remarks>
        /// <param name="id">対象のノートブック履歴ID</param>
        /// <param name="path">検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        [HttpGet("{id}/container-files")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(StorageListResultInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUnderDir(long id, [FromQuery] string path = "/", [FromQuery] bool withUrl = false)
        {
            //データの存在チェック
            var notebookHistory = await notebookHistoryRepository.GetByIdAsync(id);
            if (notebookHistory == null)
            {
                return JsonNotFound($"Notebook ID {id} is not found.");
            }

            if (!Regex.IsMatch(path, "[-_1-9a-zA-Z/]+"))
            {
                return JsonBadRequest("Invalid path. allowed characters are -_1-9a-zA-Z/");
            }

            // 検索path文字列の先頭・末尾が/でない場合はつける
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            if (!path.EndsWith("/"))
            {
                path = path + "/";
            }
            var rootDir = $"{id}" + path;

            var result = await storageLogic.GetUnderDirAsync(ResourceType.NotebookContainerOutputFiles, rootDir);

            if (withUrl)
            {
                result.Value.Files.ForEach(x => x.Url = storageLogic.GetPreSignedUriForGetFromKey(x.Key, x.FileName, true).ToString());
            }

            return JsonOK(result.Value);
        }

        /// <summary>
        /// ノートブックコンテナを途中で強制終了させる。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        [HttpPost("{id}/halt")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Halt(long? id)
        {
            return await ExitAsync(id, ContainerStatus.Killed);
        }

        /// <summary>
        /// ノートブックを終了させる。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
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
            var notebookHistory = await notebookHistoryRepository.GetByIdAsync(id.Value);
            if (notebookHistory == null)
            {
                return JsonNotFound($"Notebook ID {id} is not found.");
            }
            if (notebookHistory.GetStatus().Exist() == false)
            {
                //終了できるのはRunningのコンテナだけ
                return JsonBadRequest($"Notebook {notebookHistory.Name} does not exist.");
            }

            await notebookLogic.ExitAsync(notebookHistory, status, false);

            return JsonOK(new SimpleOutputModel(notebookHistory));
        }

        /// <summary>
        /// 指定されたノートブック履歴のコンテナを再起動する
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        /// <param name="model">再起動内容</param>
        [HttpPost("{id}/rerun")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Rerun(long? id, [FromBody]RerunInputModel model)
        {
            // データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            // データの存在チェック
            var notebookHistory = await notebookHistoryRepository.GetIncludeAllAsync(id.Value);
            if (notebookHistory == null)
            {
                return JsonNotFound($"Notebook ID {id} is not found.");
            }
            // ステータスのチェック
            if (notebookHistory.GetStatus().Exist())
            {
                return JsonNotFound($"Notebook ID {id} is already running.");
            }

            // データセットが指定されていれば存在チェック
            if (model.DataSetId.HasValue)
            {
                var dataSet = await dataSetRepository.GetByIdAsync(model.DataSetId.Value);
                if (dataSet == null)
                {
                    return JsonNotFound($"DataSet ID {model.DataSetId} is not found.");
                }
            }

            // ノートブック無期限フラグのチェック
            Tenant tenant = tenantRepository.Get(CurrentUserInfo.SelectedTenant.Id);
            if (tenant.AvailableInfiniteTimeNotebook == false && model.ExpiresIn == 0)
            {
                return JsonBadRequest($"Tenant [{tenant.DisplayName}] is not allowed to run infinite time.");
            }

            if (string.IsNullOrEmpty(notebookHistory.Partition) == false)
            {
                bool existPartition = await nodeRepository.IsEnablePartitionAsync(notebookHistory.Partition, true);
                if (existPartition == false)
                {
                    return JsonNotFound($"There are no enable nodes with Partition {notebookHistory.Partition}.");
                }
            }

            // コンテナが指定されているかチェック
            if (model.ContainerImage != null)
            {
                notebookHistory.ContainerRegistryId = model.ContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id;
                notebookHistory.ContainerImage = model.ContainerImage.Image;
                notebookHistory.ContainerTag = model.ContainerImage.Tag; // latestは運用上使用されていないハズなので、そのまま直接代入
            }
            else
            {
                // コンテナイメージの設定がない場合デフォルトのイメージを設定
                notebookHistory.ContainerRegistryId = null;
                notebookHistory.ContainerImage = "tensorflow/tensorflow";
                notebookHistory.ContainerTag = "1.13.1-gpu-py3";
            }
            // gitが指定されているかチェック
            if (model.GitModel != null)
            {
                // gitリポジトリ名が指定されていれば、ブランチ、コミットIDを設定。指定されていなければnull
                long? gitId = model.GitModel.GitId ?? CurrentUserInfo.SelectedTenant.DefaultGit?.Id;
                string branch = null;
                string commitId = null;
                if (!string.IsNullOrEmpty(model.GitModel.Repository))
                {
                    branch = model.GitModel.Branch ?? "master";
                    commitId = model.GitModel.CommitId;
                    // コミットIDが指定されていなければ、ブランチのHEADからコミットIDを取得する
                    if (string.IsNullOrEmpty(commitId))
                    {
                        commitId = await gitLogic.GetCommitIdAsync(gitId.Value, model.GitModel.Repository, model.GitModel.Owner, branch);
                        if (string.IsNullOrEmpty(commitId))
                        {
                            // コミットIDが特定できなかったらエラー
                            return JsonNotFound($"The branch {branch} for {gitId.Value}/{model.GitModel.Owner}/{model.GitModel.Repository} is not found.");
                        }
                    }
                }
                // git情報を設定
                notebookHistory.ModelGitId = gitId.Value;
                notebookHistory.ModelRepository = model.GitModel.Repository;
                notebookHistory.ModelRepositoryOwner = model.GitModel.Owner;
                notebookHistory.ModelBranch = branch;
                notebookHistory.ModelCommitId = commitId;
            }
            else
            {
                // gitが未指定の場合は各項目にnullを設定する
                notebookHistory.ModelGitId = null;
                notebookHistory.ModelRepository = null;
                notebookHistory.ModelRepositoryOwner = null;
                notebookHistory.ModelBranch = null;
                notebookHistory.ModelCommitId = null;
            }

            // 各リソースの超過チェック
            string errorMessage = clusterManagementLogic.CheckQuota(tenant, model.Cpu.Value, model.Memory.Value, model.Gpu.Value);
            if (errorMessage != null)
            {
                return JsonBadRequest(errorMessage);
            }

            // 現状のノートブック履歴IDに紐づいている親学習をすべて外す。
            notebookHistoryRepository.DetachParentToNotebookAsync(notebookHistory);

            // 親学習が指定されていれば存在チェック
            if (model.ParentIds != null)
            {
                var maps = new List<NotebookHistoryParentTrainingMap>();

                foreach (var parentId in model.ParentIds)
                {
                    var parent = await trainingHistoryRepository.GetByIdAsync(parentId);
                    if (parent == null)
                    {
                        return JsonNotFound($"Training ID {parentId} is not found.");
                    }
                    // ノートブック履歴に親学習を紐づける
                    var map = notebookHistoryRepository.AttachParentToNotebookAsync(notebookHistory, parent);
                    if (map != null)
                    {
                        maps.Add(map);
                    }
                }
                notebookHistory.ParentTrainingMaps = maps;
            }

            // 現状のノートブック履歴IDに紐づいている親推論をすべて外す。
            notebookHistoryRepository.DetachParentInferenceToNotebookAsync(notebookHistory);

            // 親推論が指定されていれば存在チェック
            if (model.InferenceIds != null)
            {
                var maps = new List<NotebookHistoryParentInferenceMap>();

                foreach (var parentId in model.InferenceIds)
                {
                    var parenInferencet = await inferenceHistoryRepository.GetByIdAsync(parentId);
                    if (parenInferencet == null)
                    {
                        return JsonNotFound($"Inference ID {parentId} is not found.");
                    }
                    // ノートブック履歴に親学習を紐づける
                    var map = notebookHistoryRepository.AttachParentInferenceToNotebookAsync(notebookHistory, parenInferencet);
                    if (map != null)
                    {
                        maps.Add(map);
                    }
                }
                notebookHistory.ParentInferenceMaps = maps;
            }

            // コンテナの実行前に、ノートブック履歴を更新する（コンテナの実行に失敗した場合、そのステータスをユーザに表示するため）
            notebookHistory.DataSetId = model.DataSetId;
            notebookHistory.Cpu = model.Cpu.Value;
            notebookHistory.Memory = model.Memory.Value;
            notebookHistory.Gpu = model.Gpu.Value;
            notebookHistory.Status = ContainerStatus.Running.Key;
            notebookHistory.StartedAt = DateTime.Now;
            notebookHistory.CompletedAt = null;
            notebookHistory.ExpiresIn = model.ExpiresIn;
            notebookHistory.LocalDataSet = model.LocalDataSet;
            notebookHistory.EntryPoint = model.EntryPoint;

            notebookHistoryRepository.Update(notebookHistory);
            unitOfWork.Commit();

            var result = await clusterManagementLogic.RunNotebookContainerAsync(notebookHistory);
            if (result.IsSuccess == false)
            {
                // コンテナの起動に失敗した状態。エラーを出力して、保存したノートブック履歴も削除する。
                notebookHistory.Status = ContainerStatus.Killed.Key;
                notebookHistory.CompletedAt = DateTime.Now;
                notebookHistoryRepository.Update(notebookHistory);
                unitOfWork.Commit();

                return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to run notebook. The message bellow may be help to resolve: " + result.Error);
            }

            // 結果に従い、ノートブック結果を更新する。
            // 実行には時間がかかりうるので、DBから最新の情報を取ってくる
            notebookHistory = await notebookHistoryRepository.GetByIdAsync(notebookHistory.Id);
            notebookHistory.Configuration = result.Value.Configuration;
            notebookHistory.Status = result.Value.Status.Key;
            unitOfWork.Commit();

            if (result.Value.Status.Succeed())
            {
                return JsonCreated(new SimpleOutputModel(notebookHistory));
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run notebook. Status={result.Value.Status.Name}. Please contact your server administrator.");
            }
        }

        /// <summary>
        /// 選択中のテナントのノートブック無期限利用可否フラグを取得する
        /// </summary>
        [HttpGet("available-infinite-time")]
        [Filters.PermissionFilter(MenuCode.Notebook)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public IActionResult GetAvailableInfiniteTime()
        {
            Tenant tenant = tenantRepository.Get(CurrentUserInfo.SelectedTenant.Id);
            if (tenant == null)
            {
                return JsonNotFound($"Tenant Id {CurrentUserInfo.SelectedTenant.Id} is not found.");
            }
            bool availableinfinitetime = tenant.AvailableInfiniteTimeNotebook;

            return JsonOK(availableinfinitetime);
        }
    }
}

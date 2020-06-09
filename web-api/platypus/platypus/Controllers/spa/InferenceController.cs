using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.ApiModels.InferenceApiModels;
using Nssol.Platypus.ApiModels.TrainingApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
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
    /// Inferenceを扱うためのAPI集
    /// </summary>
    [Route("api/v1/inferences")]
    public class InferenceController : PlatypusApiControllerBase
    {
        private readonly ITrainingHistoryRepository trainingHistoryRepository;
        private readonly IInferenceHistoryRepository inferenceHistoryRepository;
        private readonly IDataSetRepository dataSetRepository;
        private readonly IInferenceLogic inferenceLogic;
        private readonly IStorageLogic storageLogic;
        private readonly IGitLogic gitLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InferenceController(
          ITrainingHistoryRepository trainingHistoryRepository,
          IInferenceHistoryRepository inferenceHistoryRepository,
          IDataSetRepository dataSetRepository,
          IInferenceLogic inferenceLogic,
          IStorageLogic storageLogic,
          IGitLogic gitLogic,
          IClusterManagementLogic clusterManagementLogic,
          IUnitOfWork unitOfWork,
          IHttpContextAccessor accessor): base(accessor)
        {
            this.trainingHistoryRepository = trainingHistoryRepository;
            this.inferenceHistoryRepository = inferenceHistoryRepository;
            this.dataSetRepository = dataSetRepository;
            this.inferenceLogic = inferenceLogic;
            this.storageLogic = storageLogic;
            this.gitLogic = gitLogic;
            this.clusterManagementLogic = clusterManagementLogic;
            this.unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 全推論履歴のIDと名前を取得
        /// </summary>
        [HttpGet("simple")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(IEnumerable<InferenceSimpleOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var histories = await inferenceHistoryRepository.GetAllNameAsync();
            return JsonOK(histories.Select(history => new InferenceSimpleOutputModel(history)));
        }

        /// <summary>
        /// 指定された条件でページングされた状態で、全推論履歴を取得
        /// </summary>
        /// <param name="page">ページ番号。デフォルトは1。</param>
        /// <param name="perPage">表示件数。指定がない場合は上限(1000件)。</param>
        /// <param name="filter">検索条件</param>
        /// <param name="withTotal">合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。</param>
        [HttpGet]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(IEnumerable<InferenceIndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll([FromQuery]InferenceSearchInputModel filter, [FromQuery]int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            var data = inferenceHistoryRepository.GetAllIncludeDataSetAndParentWithOrdering();
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
        /// <param name="history">推論履歴</param>
        private async Task<InferenceIndexOutputModel> GetUpdatedIndexOutputModelAsync(InferenceHistory history)
        {
            var model = new InferenceIndexOutputModel(history);

            var status = history.GetStatus();
            if (status.Exist())
            {
                //推論がまだ進行中の場合、情報を更新する
                var newStatus = await clusterManagementLogic.GetContainerStatusAsync(history.Key, CurrentUserInfo.SelectedTenant.Name, false);

                if (status.Key != newStatus.Key)
                {
                    //更新があったので、変更処理
                    await inferenceHistoryRepository.UpdateStatusAsync(history.Id, newStatus, false);
                    unitOfWork.Commit();

                    model.Status = newStatus.Name;
                }
            }

            // storageへの出力値があれば取得し、modelに格納
            var outputFileName = "value.txt";   //値を読み込むファイル名
            var outputPath = history.Id + "/" + outputFileName;
            var content = await storageLogic.GetFileContentAsync(ResourceType.InferenceContainerOutputFiles, outputPath, outputFileName, true);
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
        private int GetTotalCount(InferenceSearchInputModel filter)
        {
            IQueryable<InferenceHistory> histories;
            if (string.IsNullOrEmpty(filter.DataSet))
            {
                histories = inferenceHistoryRepository.GetAll();
            }
            else
            {
                //データセット名のフィルターがかかっている場合、データセットも併せて取得しないといけない
                histories = inferenceHistoryRepository.GetAllIncludeDataSet();
            }

            histories = Search(histories, filter);
            return histories.Count();
        }

        /// <summary>
        /// 検索条件の追加
        /// </summary>
        /// <param name="sourceData">加工前の検索結果</param>
        /// <param name="filter">検索条件</param>
        private static IQueryable<InferenceHistory> Search(IQueryable<InferenceHistory> sourceData, InferenceSearchInputModel filter)
        {
            IQueryable<InferenceHistory> data = sourceData;
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

            // マウントした学習IDの検索
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

            // マウントした学習名の検索
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
            return data;
        }

        /// <summary>
        /// 指定されたIDの推論履歴の詳細情報を取得
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        [HttpGet("{id}")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(InferenceDetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Inference ID is required.");
            }
            var history = await inferenceHistoryRepository.GetIncludeAllAsync(id.Value);
            if (history == null)
            {
                return JsonNotFound($"Inference ID {id.Value} is not found.");
            }

            var model = new InferenceDetailsOutputModel(history);

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

            //Gitの表示用URLを作る
            model.GitModel.Url = gitLogic.GetTreeUiUrl(history.ModelGitId.Value, history.ModelRepository, history.ModelRepositoryOwner, history.ModelCommitId);
            return JsonOK(model);
        }

        /// <summary>
        /// 指定された推論履歴のエラーイベントを取得
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        /// <returns>ログファイル</returns>
        [HttpGet("{id}/events")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(ContainerEventInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetErrorEventAsync(long id)
        {
            var history = await inferenceHistoryRepository.GetByIdAsync(id);
            if (history == null)
            {
                return JsonNotFound($"Inference Id {id} is not found.");
            }
            if (history.GetStatus().Exist() == false)
            {
                return JsonBadRequest($"A container for Inference Id {id} does not exist.");
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
        /// 新規に推論を開始
        /// </summary>
        /// <param name="model">新規推論実行内容</param>
        /// <param name="nodeRepository">DI用</param>
        [HttpPost("run")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(InferenceSimpleOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody]CreateInputModel model, [FromServices]INodeRepository nodeRepository)
        {

            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var dataSet = await dataSetRepository.GetByIdAsync(model.DataSetId.Value);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet ID {model.DataSetId} is not found.");
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

            long? gitId = model.GitModel.GitId ?? CurrentUserInfo.SelectedTenant.DefaultGit?.Id;
            string branch = model.GitModel.Branch ?? "master";
            string commitId = model.GitModel.CommitId;
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

            //コンテナの実行前に、推論履歴を作成する（コンテナの実行に失敗した場合、そのステータスをユーザに表示するため）
            var inferenceHistory = new InferenceHistory()
            {
                Name = model.Name,
                DisplayId = -1,
                ContainerRegistryId = model.ContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id,
                ContainerImage = model.ContainerImage.Image,
                ContainerTag = model.ContainerImage.Tag, //latestは運用上使用されていないハズなので、そのまま直接代入
                DataSetId = model.DataSetId.Value,
                EntryPoint = model.EntryPoint,
                ModelGitId = gitId,
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
                Status = ContainerStatus.Running.Key,
                Zip = model.Zip,
                LocalDataSet = model.LocalDataSet,
            };

            if (inferenceHistory.OptionDic.ContainsKey("")) //空文字は除外する
            {
                inferenceHistory.OptionDic.Remove("");
            }
            // 親学習が指定されていれば存在チェック
            if (model.ParentIds != null)
            {
                var maps = new List<InferenceHistoryParentMap>();

                foreach (var parentId in model.ParentIds)
                {
                    var parent = await trainingHistoryRepository.GetByIdAsync(parentId);
                    if (parent == null)
                    {
                        return JsonNotFound($"Training ID {parentId} is not found.");
                    }
                    // 推論履歴に親学習を紐づける
                    var map = inferenceHistoryRepository.AttachParentAsync(inferenceHistory, parent);
                    if (map != null)
                    {
                        maps.Add(map);
                    }
                }

                inferenceHistory.ParentMaps = maps;
            }
            inferenceHistoryRepository.Add(inferenceHistory);
            if (dataSet.IsLocked == false)
            {
                dataSet.IsLocked = true;
            }
            unitOfWork.Commit();


            var result = await clusterManagementLogic.RunInferenceContainerAsync(inferenceHistory);
            if (result.IsSuccess == false)
            {
                //コンテナの起動に失敗した状態。エラーを出力して、保存した推論履歴も削除する。
                inferenceHistoryRepository.Delete(inferenceHistory);
                unitOfWork.Commit();

                return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to run training. The message bellow may be help to resolve: " + result.Error);
            }

            //結果に従い、推論結果を更新する。
            //実行には時間がかかりうるので、DBから最新の情報を取ってくる
            inferenceHistory = await inferenceHistoryRepository.GetByIdAsync(inferenceHistory.Id);
            inferenceHistory.Configuration = result.Value.Configuration;
            inferenceHistory.Status = result.Value.Status.Key;
            unitOfWork.Commit();

            if (result.Value.Status.Succeed())
            {
                return JsonCreated(new InferenceSimpleOutputModel(inferenceHistory));
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run training. Status={result.Value.Status.Name}. Please contact your server administrator.");
            }
        }
        /// <summary>
        /// 推論履歴の編集
        /// </summary>
        /// <param name="id">変更対象の推論履歴ID</param>
        /// <param name="model">変更内容</param>
        [HttpPut("{id}")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(InferenceSimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody]EditInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var history = await inferenceHistoryRepository.GetByIdAsync(id.Value);
            if (history == null)
            {
                return JsonNotFound($"Inference ID {id.Value} is not found.");
            }

            history.Name = EditColumnNotEmpty(model.Name, history.Name);
            history.Memo = EditColumn(model.Memo, history.Memo);
            history.Favorite = EditColumn(model.Favorite, history.Favorite);
            unitOfWork.Commit();

            return JsonOK(new InferenceSimpleOutputModel(history));
        }


        /// <summary>
        /// 推論履歴添付ファイルを登録
        /// </summary>
        /// <param name="id">対象の推論履歴ID</param>
        /// <param name="model">追加するファイル情報</param>
        [HttpPost("{id}/files")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(AttachedFileOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegistAttachedFile(long id, [FromBody]AddFileInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            //データの存在チェック
            var inferenceHistory = await inferenceHistoryRepository.GetByIdAsync(id);
            if (inferenceHistory == null)
            {
                return JsonNotFound($"Inference ID {id} is not found.");
            }

            //同じ名前のファイルは登録できない
            if (await inferenceHistoryRepository.ExistsAttachedFileAsync(id, model.FileName))
            {
                return JsonConflict($"Inference {id} has already a file named {model.FileName}.");
            }

            var attachedFile = new InferenceHistoryAttachedFile
            {
                InferenceHistoryId = id,
                FileName = model.FileName,
                Key = ResourceType.InferenceHistoryAttachedFiles.ToString(), //model.Key ?? ResourceType.InferenceHistoryAttachedFiles.ToString();
                StoredPath = model.StoredPath
            };
            inferenceHistoryRepository.AddAttachedFile(attachedFile);
            unitOfWork.Commit();

            return JsonOK(new AttachedFileOutputModel(id, model.FileName, attachedFile.Id));
        }

        /// <summary>
        /// コンテナの出力ファイルの一覧を取得
        /// </summary>
        /// <remarks> 
        /// コンテナの/output/配下から指定ディレクトリパスの直下を検索する
        /// 検索対象ディレクトリが見つからない場合もファイル・ディレクトリが空の結果を返す
        /// </remarks>
        /// <param name="id">対象の推論履歴ID</param>
        /// <param name="path">検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        [HttpGet("{id}/container-files")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(StorageListResultInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUnderDir(long id, [FromQuery] string path = "/", [FromQuery] bool withUrl = false)
        {
            //データの存在チェック
            var inferenceHistory = await inferenceHistoryRepository.GetByIdAsync(id);
            if (inferenceHistory == null)
            {
                return JsonNotFound($"Inference ID {id} is not found.");
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

            // windowsから実行された場合、区切り文字が"\\"として送られてくるので"/"に置換する
            path = path.Replace("\\", "/");

            var rootDir = $"{id}" + path;

            var result = await storageLogic.GetUnderDirAsync(ResourceType.InferenceContainerOutputFiles, rootDir);

            if (withUrl)
            {
                result.Value.Files.ForEach(x => x.Url = storageLogic.GetPreSignedUriForGetFromKey(x.Key, x.FileName, true).ToString());
            }


            return JsonOK(result.Value);
        }

        /// <summary>
        /// 推論履歴添付ファイルの一覧を取得
        /// </summary>
        /// <param name="id">対象の推論履歴ID</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        [HttpGet("{id}/files")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(IEnumerable<AttachedFileOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAttachedFiles(long id, [FromQuery] bool withUrl = false)
        {
            //データの存在チェック
            var inferenceHistory = await inferenceHistoryRepository.GetByIdAsync(id);
            if (inferenceHistory == null)
            {
                return JsonNotFound($"Inference ID {id} is not found.");
            }

            var underDir = await storageLogic.GetUnderDirAsync(ResourceType.InferenceContainerAttachedFiles, $"{id}/");
            if (underDir.IsSuccess == false)
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to access the storage service. Please contact to system administrators.");
            }

            var containerAttachedFiles = new List<AttachedFileOutputModel>();
            containerAttachedFiles.AddRange(underDir.Value.Files.Select(
                f => new AttachedFileOutputModel(inferenceHistory.Id, f.FileName, -1)
                {
                    Url = withUrl ? storageLogic.GetPreSignedUriForGetFromKey(f.Key, f.FileName, true).ToString() : null,
                    IsLocked = true
                }
                ));

            var userAttachedFiles = new List<AttachedFileOutputModel>();

            var filesOnDB = await inferenceHistoryRepository.GetAllAttachedFilesAsync(id);
            userAttachedFiles.AddRange(filesOnDB.Select(
                f => new AttachedFileOutputModel(inferenceHistory.Id, f.FileName, f.Id)
                {
                    Url = withUrl ? storageLogic.GetPreSignedUriForGet(ResourceType.InferenceHistoryAttachedFiles, f.StoredPath, f.FileName, true).ToString() : null,
                    IsLocked = false
                }
                ));

            var result = containerAttachedFiles.Concat(userAttachedFiles);
            return JsonOK(result);
        }

        /// <summary>
        /// 推論履歴添付ファイルを削除
        /// </summary>
        /// <param name="id">対象の推論履歴ID</param>
        /// <param name="fileId">削除するファイルのID</param>
        [HttpDelete("{id}/files/{fileId}")]
        [Filters.PermissionFilter(MenuCode.Inference)]
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
            //子の添付ファイルが存在すれば親の推論履歴は必ず存在するはずなので、そっちはチェックしない
            InferenceHistoryAttachedFile file = await inferenceHistoryRepository.GetAttachedFileAsync(fileId.Value);
            if (file == null)
            {
                return JsonNotFound($"File ID {fileId.Value} is not found.");
            }

            inferenceHistoryRepository.DeleteAttachedFile(file);
            await storageLogic.DeleteFileAsync(ResourceType.InferenceHistoryAttachedFiles, file.StoredPath);
            unitOfWork.Commit();

            return JsonNoContent();
        }

        /// <summary>
        /// 推論を途中で強制終了
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        [HttpPost("{id}/halt")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(InferenceSimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Halt(long? id)
        {
            return await ExitAsync(id, ContainerStatus.Killed);
        }

        /// <summary>
        /// 推論を途中で強制終了させる。
        /// ユーザ自身がジョブを停止させた場合。
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        [HttpPost("{id}/user-cancel")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(InferenceSimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UserCancel(long? id)
        {
            return await ExitAsync(id, ContainerStatus.UserCanceled);
        }

        /// <summary>
        /// 推論を正常終了
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        [HttpPost("{id}/complete")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType(typeof(InferenceSimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Complete(long? id)
        {
            return await ExitAsync(id, ContainerStatus.Completed);
        }

        /// <summary>
        /// 推論を終了
        /// </summary>
        /// <param name="id">推論履歴ID</param>
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
            var inferenceHistory = await inferenceHistoryRepository.GetByIdAsync(id.Value);
            if (inferenceHistory == null)
            {
                return JsonNotFound($"Inference ID {id} is not found.");
            }
            if (inferenceHistory.GetStatus().Exist() == false)
            {
                //終了できるのはRunningのコンテナだけ
                return JsonBadRequest($"Inference {inferenceHistory.Name} does not exist.");
            }

            await inferenceLogic.ExitAsync(inferenceHistory, status, false);

            return JsonOK(new InferenceSimpleOutputModel(inferenceHistory));
        }

        /// <summary>
        /// 推論履歴を削除
        /// </summary>
        /// <param name="id">推論履歴ID</param>
        [HttpDelete("{id}")]
        [Filters.PermissionFilter(MenuCode.Inference)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long? id)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var inferenceHistory = await inferenceHistoryRepository.GetByIdAsync(id.Value);
            if (inferenceHistory == null)
            {
                return JsonNotFound($"Inference ID {id} is not found.");
            }

            //ステータスを確認

            var status = inferenceHistory.GetStatus();
            if (status.Exist())
            {
                //推論がまだ進行中の場合、情報を更新する
                status = await clusterManagementLogic.GetContainerStatusAsync(inferenceHistory.Key, CurrentUserInfo.SelectedTenant.Name, false);
            }

            if (status.Exist())
            {
                //実行中であれば、コンテナを削除
                await clusterManagementLogic.DeleteContainerAsync(
                    ContainerType.Training, inferenceHistory.Key, CurrentUserInfo.SelectedTenant.Name, false);
            }

            //添付ファイルがあったらまとめて消す
            var files = await inferenceHistoryRepository.GetAllAttachedFilesAsync(inferenceHistory.Id);
            foreach (var file in files)
            {
                inferenceHistoryRepository.DeleteAttachedFile(file);
                await storageLogic.DeleteFileAsync(ResourceType.InferenceHistoryAttachedFiles, file.StoredPath);
            }

            inferenceHistoryRepository.Delete(inferenceHistory);
            unitOfWork.Commit();

            // ストレージ内の推論データを削除する
            await storageLogic.DeleteResultsAsync(ResourceType.InferenceContainerAttachedFiles, inferenceHistory.Id);
            await storageLogic.DeleteResultsAsync(ResourceType.InferenceContainerOutputFiles, inferenceHistory.Id);

            return JsonNoContent();
        }

    }
}
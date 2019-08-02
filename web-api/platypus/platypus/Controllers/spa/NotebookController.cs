using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.ApiModels.NotebookApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Options;
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
    [Route("api/v1/notebook")]
    public class NotebookController : PlatypusApiControllerBase
    {
        private readonly INotebookHistoryRepository notebookHistoryRepository;
        private readonly IDataSetRepository dataSetRepository;
        private readonly INotebookLogic notebookLogic;
        private readonly IStorageLogic storageLogic;
        private readonly IGitLogic gitLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private readonly IUnitOfWork unitOfWork;
        public NotebookController(
            INotebookHistoryRepository notebookHistoryRepository,
            IDataSetRepository dataSetRepository,
            INotebookLogic notebookLogic,
            IStorageLogic storageLogic,
            IGitLogic gitLogic,
            IClusterManagementLogic clusterManagementLogic,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.notebookHistoryRepository = notebookHistoryRepository;
            this.dataSetRepository = dataSetRepository;
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
        [Filters.PermissionFilter(MenuCode.Training)] // TODO MenuCode.Notebookに変更
        [ProducesResponseType(typeof(IEnumerable<SimpleOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
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
        [Filters.PermissionFilter(MenuCode.Training)] // TODO MenuCode.Notebookに変更
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
        private async Task<IndexOutputModel> GetUpdatedIndexOutputModelAsync(NotebookHistory history)
        {
            var model = new IndexOutputModel(history);

            var status = history.GetStatus();
            if (status.Exist())
            {
                //学習がまだ進行中の場合、情報を更新する
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
        private static IQueryable<NotebookHistory> Search(IQueryable<NotebookHistory> sourceData, SearchInputModel filter)
        {
            IQueryable<NotebookHistory> data = sourceData;
            data = data
                .SearchLong(d => d.Id, filter.Id)
                .SearchString(d => d.Name, filter.Name)
                .SearchString(d => d.Memo, filter.Memo)
                .SearchString(d => d.GetStatus().ToString(), filter.Status);
            return data;
        }

        /// <summary>
        /// 指定されたIDのノートブック履歴の詳細情報を取得。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        [HttpGet("{id}")]
        [Filters.PermissionFilter(MenuCode.Training)] // TODO MenuCode.Notebookに変更
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Notebook ID is required.");
            }
            var history = await notebookHistoryRepository.GetIncludeAllAsync(id.Value);
            if (history == null)
            {
                return JsonNotFound($"Notebook ID {id.Value} is not found.");
            }

            var model = new DetailsOutputModel(history);

            var status = history.GetStatus();
            model.StatusType = status.StatusType;
            if (status.Exist())
            {
                //コンテナがまだ存在している場合、情報を更新する
                var details = await clusterManagementLogic.GetContainerEndpointInfoAsync(history.Key, CurrentUserInfo.SelectedTenant.Name, false);
                model.Status = details.Status.Name;
                model.StatusType = details.Status.StatusType;

                //ステータスを更新
                history.Status = details.Status.Key;
                if (history.StartedAt == null)
                {
                    history.StartedAt = details.StartedAt;
                    history.Node = details.Node; //設計上ノードが切り替わることはない
                }
                unitOfWork.Commit();

                model.ConditionNote = details.ConditionNote;
                if (details.Status.IsRunning())
                {
                    //コンテナが正常に動いているので、エンドポイントを表示する
                    // TODO: Tokenを取得してJupyterのエンドポイントを作成する(string)
                    // details.EndpointsからNotebookのものを取得→Tokenを取得→つなぎ合わせてNotebookEndpointに設定
                    //model.NotebookEndpoint = details.EndPoints;
                }
            }

            //Gitの表示用URLを作る
            model.GitModel.Url = gitLogic.GetTreeUiUrl(history.ModelGitId.Value, history.ModelRepository, history.ModelRepositoryOwner, history.ModelCommitId);
            return JsonOK(model);
        }

        /// <summary>
        /// ノートブック履歴の編集
        /// </summary>
        /// <param name="id">変更対象のノートブック履歴ID</param>
        /// <param name="model">変更内容</param>
        [HttpPut("{id}")]
        [Filters.PermissionFilter(MenuCode.Training)] // TODO MenuCode.Notebookに変更
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long? id, [FromBody]EditInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var history = await notebookHistoryRepository.GetByIdAsync(id.Value);
            if (history == null)
            {
                return JsonNotFound($"Notebook ID {id.Value} is not found.");
            }

            history.Name = EditColumnNotEmpty(model.Name, history.Name);
            history.Memo = EditColumn(model.Memo, history.Memo);
            history.Favorite = EditColumn(model.Favorite, history.Favorite);
            unitOfWork.Commit();

            return JsonOK(new SimpleOutputModel(history));
        }

        /// <summary>
        /// ノートブック履歴を削除する。
        /// </summary>
        /// <param name="id">学習履歴ID</param>
        [HttpDelete("{id}")]
        [Filters.PermissionFilter(MenuCode.Training)] // TODO MenuCode.Notebookに変更
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

            return JsonNoContent();
        }

        /// <summary>
        /// 指定されたノートブック履歴のエラーイベントを取得します。
        /// </summary>
        /// <param name="id">ノートブック履歴ID</param>
        /// <returns>ログファイル</returns>
        [HttpGet("{id}/events")]
        [Filters.PermissionFilter(MenuCode.Training)] // TODO MenuCode.Notebookに変更
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
        /// 新規に学習を開始する
        /// </summary>
        [HttpPost("run")]
        [Filters.PermissionFilter(MenuCode.Training)] // TODO MenuCode.Notebookに変更
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody]CreateInputModel model, [FromServices]INodeRepository nodeRepository)
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
            if (string.IsNullOrEmpty(model.Partition) == false)
            {
                bool existPartition = await nodeRepository.IsEnablePartitionAsync(model.Partition, true);
                if (existPartition == false)
                {
                    return JsonNotFound($"There are no enable nodes with Partition {model.Partition}.");
                }
            }


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


            //コンテナの実行前に、ノートブック履歴を作成する（コンテナの実行に失敗した場合、そのステータスをユーザに表示するため）
            var notebookHistory = new NotebookHistory()
            {
                DisplayId = -1,
                Name = model.Name,
                DataSetId = model.DataSetId.Value,
                ModelGitId = gitId.Value,
                ModelRepository = model.GitModel.Repository,
                ModelRepositoryOwner = model.GitModel.Owner,
                ModelBranch = branch,
                ModelCommitId = commitId,
                ContainerRegistryId = model.ContainerImage.RegistryId ?? CurrentUserInfo.SelectedTenant.DefaultRegistry?.Id,
                ContainerImage = model.ContainerImage.Image,
                ContainerTag = model.ContainerImage.Tag, //latestは運用上使用されていないハズなので、そのまま直接代入
                OptionDic = model.Options ?? new Dictionary<string, string>(), //オプションはnullの可能性があるので、その時は初期化
                Cpu = model.Cpu.Value,
                Memory = model.Memory.Value,
                Gpu = model.Gpu.Value,
                Partition = model.Partition,
                Memo = model.Memo,
                Status = ContainerStatus.Running.Key,
                ExpiresIn = model.Expiresln
            };
            if (notebookHistory.OptionDic.ContainsKey("")) //空文字は除外する
            {
                notebookHistory.OptionDic.Remove("");
            }
            notebookHistoryRepository.Add(notebookHistory);
            unitOfWork.Commit();

            // TODO: RunNotebookContainerAsyncの実装
            //var result = await clusterManagementLogic.RunNotebookContainerAsync(notebookHistory);
            //if (result.IsSuccess == false)
            //{
            //    //コンテナの起動に失敗した状態。エラーを出力して、保存した学習履歴も削除する。
            //    notebookHistoryRepository.Delete(notebookHistory);
            //    unitOfWork.Commit();

            //    return JsonError(HttpStatusCode.ServiceUnavailable, "Failed to run notebook. The message bellow may be help to resolve: " + result.Error);
            //}

            ////結果に従い、ノートブック結果を更新する。
            ////実行には時間がかかりうるので、DBから最新の情報を取ってくる
            //notebookHistory = await notebookHistoryRepository.GetByIdAsync(notebookHistory.Id);
            //notebookHistory.Configuration = result.Value.Configuration;
            //notebookHistory.Status = result.Value.Status.Key;
            //unitOfWork.Commit();

            //if (result.Value.Status.Succeed())
            //{
            //    return JsonCreated(new SimpleOutputModel(notebookHistory));
            //}
            //else
            //{
            //    return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to run notebook. Status={result.Value.Status.Name}. Please contact your server administrator.");
            //}
            return JsonError(HttpStatusCode.InternalServerError, "tmp");
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
        [Filters.PermissionFilter(MenuCode.Training)] // TODO MenuCode.Notebookに変更
        [ProducesResponseType(typeof(StorageListResultInfo), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUnderDir(long id, [FromQuery] string path = "/", [FromQuery] bool withUrl = false)
        {
            //データの存在チェック
            var trainingHistory = await notebookHistoryRepository.GetByIdAsync(id);
            if (trainingHistory == null)
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
        /// <param name="id">学習履歴ID</param>
        [HttpPost("{id}/halt")]
        [Filters.PermissionFilter(MenuCode.Training)] // TODO MenuCode.Notebookに変更
        [ProducesResponseType(typeof(SimpleOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Halt(long? id)
        {
            return await ExitAsync(id, ContainerStatus.Killed);
        }

        /// <summary>
        /// ノートブックを終了させる。
        /// </summary>
        /// <param name="id">Notebook ID</param>
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
                return JsonNotFound($"Training ID {id} is not found.");
            }
            if (notebookHistory.GetStatus().Exist() == false)
            {
                //終了できるのはRunningのコンテナだけ
                return JsonBadRequest($"Notebook {notebookHistory.Name} does not exist.");
            }

            await notebookLogic.ExitAsync(notebookHistory, status, false);

            return JsonOK(new SimpleOutputModel(notebookHistory));
        }
    }

}

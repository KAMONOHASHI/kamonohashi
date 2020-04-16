using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.ApiModels.DataApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    [Route("api/v1/data")]
    public class DataController : PlatypusApiControllerBase
    {
        private readonly IDataRepository dataRepository;
        private readonly IPreprocessHistoryRepository preprocessHistoryRepository;
        private readonly IDataLogic dataLogic;
        private readonly ITagLogic tagLogic;
        private readonly IStorageLogic storageLogic;
        private readonly IUnitOfWork unitOfWork;

        public DataController(
            IDataRepository dataRepository,
            IPreprocessHistoryRepository preprocessHistoryRepository,
            IDataLogic dataLogic,
            ITagLogic tagLogic,
            IStorageLogic storageLogic,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.dataRepository = dataRepository;
            this.preprocessHistoryRepository = preprocessHistoryRepository;
            this.dataLogic = dataLogic;
            this.tagLogic = tagLogic;
            this.storageLogic = storageLogic;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 指定された条件でページングされた状態で、データ一覧を取得。
        /// タグ情報が含まれる。
        /// </summary>
        /// <param name="page">ページ番号。デフォルトは1。</param>
        /// <param name="perPage">表示件数。指定がない場合は上限(1000件)。</param>
        /// <param name="filter">検索条件</param>
        /// <param name="withTotal">合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。</param>
        [HttpGet]
        [Filters.PermissionFilter(MenuCode.Data, MenuCode.DataSet, MenuCode.Preprocess)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll([FromQuery]SearchInputModel filter, [FromQuery]int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            //タグ付きで取得
            var data = dataRepository.GetDataIndex();

            data = data
                .SearchLong(d => d.Id, filter.Id)
                .SearchString(d => d.Name, filter.Name)
                .SearchString(d => d.CreatedBy, filter.CreatedBy)
                .SearchString(d => d.Memo, filter.Memo)
                .SearchTime(d => d.CreatedAt, filter.CreatedAt);
            if (filter.Tags != null)
            {
                foreach (var tag in filter.Tags)
                {
                    data = data.SearchString(d => d.Tag, tag);
                }
            }

            data = data.OrderByDescending(d => d.Id);

            //未指定、あるいは1000件以上であれば、1000件に指定
            int pageCount = (perPage.HasValue && perPage.Value < 1000) ? perPage.Value : 1000;
            data = data.Paging(page, pageCount);

            if (withTotal)
            {
                int total = GetTotalCount(filter);
                SetTotalCountToHeader(total);
            }

            return JsonOK(data.Select(_ => new IndexOutputModel(_)));
        }

        /// <summary>
        /// データ件数を取得する
        /// </summary>
        private int GetTotalCount(SearchInputModel filter)
        {
            //タグによるフィルタの有無で、処理方法を大きく変える
            if (filter.Tags != null && filter.Tags.Count() > 0)
            {
                var data = dataRepository.GetDataIndex();

                data = data
                    .SearchLong(d => d.Id, filter.Id)
                    .SearchString(d => d.Name, filter.Name)
                    .SearchString(d => d.CreatedBy, filter.CreatedBy)
                    .SearchString(d => d.Memo, filter.Memo)
                    .SearchTime(d => d.CreatedAt, filter.CreatedAt);
                if (filter.Tags != null)
                {
                    foreach (var tag in filter.Tags)
                    {
                        data = data.SearchString(d => d.Tag, tag);
                    }
                }
                return data.Count();
            }
            else
            {
                //タグによるフィルタがないので、データ情報だけ取得して件数を数える
                IQueryable<Data> data = dataRepository.GetAll()
                    .SearchLong(d => d.Id, filter.Id)
                    .SearchString(d => d.Name, filter.Name)
                    .SearchString(d => d.CreatedBy, filter.CreatedBy)
                    .SearchString(d => d.Memo, filter.Memo)
                    .SearchTime(d => d.CreatedAt, filter.CreatedAt);
                return data.Count();
            }
        }

        /// <summary>
        /// 指定したIDのデータ詳細情報を取得する。
        /// </summary>
        /// <param name="id">データID</param>
        [HttpGet("{id}")]
        [Filters.PermissionFilter(MenuCode.Data, MenuCode.Preprocess)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("Data ID is required.");
            }
            var data = await dataRepository.GetDataIncludeAllAsync(id.Value);
            if (data == null)
            {
                return JsonNotFound($"Data Id {id.Value} is not found.");
            }
            var model = new DetailsOutputModel(data);
            model.Tags = tagLogic.GetAllDataTag(data.Id).Select(t => t.Name);
            var parent = await preprocessHistoryRepository.GetInputDataAsync(data.Id);
            if (parent != null)
            {
                model.Parent = new IndexOutputModel(parent);
            }
            var children = preprocessHistoryRepository.GetPreprocessIncludePreprocessByInputDataId(data.Id);
            if (children != null & children.Count() > 0)
            {
                model.Children = children.Select(p => new PreprocessHistoryOutputModel(p));
            }
            return JsonOK(model);
        }

        /// <summary>
        /// データの新規作成
        /// </summary>
        [HttpPost]
        [Filters.PermissionFilter(MenuCode.Data)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public IActionResult Create([FromBody]CreateInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                //名前に空文字は許可しない
                return JsonBadRequest($"A name of Data is NOT allowed to set empty string.");
            }

            // データの登録
            Data newData = new Data()
            {
                // 名前の前後の空白文字を除去して設定する。
                Name = model.Name.Trim(),
                Memo = model.Memo,
            };

            //タグの登録
            if (model.Tags != null && model.Tags.Count() > 0)
            {
                tagLogic.CreateDataTags(newData, model.Tags);
            }

            dataRepository.Add(newData);
            // DBへのコミット
            unitOfWork.Commit();

            return JsonCreated(new IndexOutputModel(newData));
        }

        /// <summary>
        /// データ編集メソッド。
        /// ファイルの追加は別のメソッドで行う。
        /// </summary>
        /// <param name="id">変更対象のデータID</param>
        /// <param name="model">変更内容</param>
        /// <param name="tagRepository">Di用</param>
        [HttpPut("{id}")]
        [Filters.PermissionFilter(MenuCode.Data)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditData(long? id, [FromBody]EditInputModel model, [FromServices] ITagRepository tagRepository)
        {
            //データの入力チェック
            if (!ModelState.IsValid || !id.HasValue)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var data = await dataRepository.GetByIdAsync(id.Value);
            if (data == null)
            {
                return JsonNotFound($"Data ID {id.Value} is not found.");
            }

            if (model.Name != null)
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    //名前に空文字は許可しない
                    return JsonBadRequest($"A name of Data is NOT allowed to set empty string.");
                }
                // 名前の前後の空白文字を除去して設定する。
                data.Name = model.Name.Trim();
            }
            data.Memo = EditColumn(model.Memo, data.Memo);

            //タグの編集。指定がない場合は変更なしと見なして何もしない。
            if (model.Tags != null)
            {
                if (model.Tags.Count() > 0)
                {
                    //タグが一つでも指定されていたら、全部上書き
                    await tagLogic.EditDataTagsAsync(data.Id, model.Tags);
                }
                else
                {
                    //タグがゼロなら全削除
                    tagLogic.DeleteDataTags(data.Id);
                }
            }

            // DBへの編集内容を一旦確定させるためコミット
            unitOfWork.Commit();

            // 未使用タグ削除
            tagRepository.DeleteUnUsedDataTags();

            // DBへタグ削除結果のコミット
            unitOfWork.Commit();

            return JsonOK(new IndexOutputModel(data));
        }

        /// <summary>
        /// 指定したデータに対してタグを追加する。
        /// </summary>
        [HttpPut("{id}/tags/{tag}")]
        [Filters.PermissionFilter(MenuCode.Data)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AttachTag(long id, string tag, [FromServices] ITagRepository tagRepository)
        {
            //データの存在チェック
            var data = await dataRepository.GetByIdAsync(id);
            if (data == null)
            {
                return JsonNotFound($"Data ID {id} is not found.");
            }

            await tagRepository.AddDataTagAsync(data.Id, tag);

            //結果を取り直す
            var result = await dataRepository.GetDataIncludeAllAsync(data.Id);

            return JsonOK(new DetailsOutputModel(result));
        }

        /// <summary>
        /// 指定したデータからタグを削除する。
        /// </summary>
        [HttpDelete("{id}/tags/{tag}")]
        [Filters.PermissionFilter(MenuCode.Data)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DetachTag(long id, string tag, [FromServices] ITagRepository tagRepository)
        {
            //データの存在チェック
            var data = await dataRepository.GetByIdAsync(id);
            if (data == null)
            {
                return JsonNotFound($"Data ID {id} is not found.");
            }

            tagRepository.DeleteDataTag(data.Id, tag);

            return JsonNoContent();
        }

        /// <summary>
        /// ファイルのダウンロードURLを取得する
        /// </summary>
        /// <param name="id">対象データID</param>
        /// <param name="name">対象ファイル名</param>
        [HttpGet("{id}/files/{name}")]
        [Filters.PermissionFilter(MenuCode.Data)]
        [ProducesResponseType(typeof(DataFileOutputModel), (int)HttpStatusCode.OK)]
        public IActionResult GetFileUrl(long id, string name)
        {
            //データの存在チェック
            var property = dataRepository.GetDataProperty(id, name);
            if (property == null || property.DataFile == null)
            {
                return JsonNotFound($"Data ID {id} or file name {name} is not found.");
            }

            string url = storageLogic.GetPreSignedUriForGet(ResourceType.Data, property.DataFile.StoredPath, property.DataFile.FileName, true).ToString();
            return JsonOK(new DataFileOutputModel { Id = id, Url = url, Key = name, FileId = property.Id, FileName = property.DataFile.FileName });
        }

        /// <summary>
        /// 指定したデータのファイル情報を全て取得する
        /// </summary>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        /// <param name="id">対象データID</param>
        [HttpGet("{id}/files")]
        [Filters.PermissionFilter(MenuCode.Data, MenuCode.Preprocess)]
        [ProducesResponseType(typeof(IEnumerable<DataFileOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFiles([FromRoute] long id, [FromQuery] bool withUrl)
        {
            //データの存在チェック
            var exist = await dataRepository.ExistsAsync(d => d.Id == id);
            if (exist == false)
            {
                return JsonNotFound($"Data ID {id} is not found.");
            }

            return JsonOK(dataLogic.GetDataFiles(id, withUrl));
        }

        /// <summary>
        /// ファイルを追加する。
        /// </summary>
        /// <param name="id">変更対象のデータID</param>
        /// <param name="model">追加するファイル情報</param>
        /// <param name="dataSetRepository">DI用</param>
        [HttpPost("{id}/files")]
        [Filters.PermissionFilter(MenuCode.Data)]
        [ProducesResponseType(typeof(DataFileOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddFile(long id, [FromBody]AddFileInputModel model, [FromServices] IDataSetRepository dataSetRepository)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            //データの存在チェック
            var data = await dataRepository.GetDataIncludeAllAsync(id);
            if (data == null)
            {
                return JsonNotFound($"Data ID {id} is not found.");
            }

            var checkResult = await CheckDataIsLocked(data, dataSetRepository);
            if (checkResult != null)
            {
                return checkResult;
            }

            //同じファイル名は登録できない
            var file = data.DataProperties.FirstOrDefault(d => d.Key == model.FileName);
            if (file != null)
            {
                return JsonConflict($"Data {data.Name} has already a file named {model.FileName}.");
            }

            // データファイルの登録
            var property = dataRepository.AddFile(data, model.FileName, model.StoredPath);
            unitOfWork.Commit();

            return JsonCreated(new DataFileOutputModel { Id = id, Key = property.Key, FileId = property.Id, FileName = model.FileName });
        }

        /// <summary>
        /// 指定されたIDのファイルを削除する
        /// </summary>
        /// <param name="id">対象のデータID</param>
        /// <param name="fileId">削除するファイルのID</param>
        /// <param name="dataSetRepository">DI用</param>
        [HttpDelete("{id}/files/{fileId}")]
        [Filters.PermissionFilter(MenuCode.Data)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteFile(long? id, long? fileId, [FromServices] IDataSetRepository dataSetRepository)
        {
            if (id == null)
            {
                //IDが指定されていなければエラー
                return JsonBadRequest("Invalid inputs.");
            }
            if (fileId == null)
            {
                return JsonBadRequest("File ID is required.");
            }
            if (fileId.Value < 0)
            {
                return JsonBadRequest("The file is locked. You can NOT delete the file.");
            }
            //Dataとそれに紐づくSectionimageをDBから取得
            var data = await dataRepository.GetDataIncludeAllAsync(id.Value);
            if (data == null)
            {
                return JsonNotFound($"Data ID {id.Value} is not found.");
            }

            var checkResult = await CheckDataIsLocked(data, dataSetRepository);
            if (checkResult != null)
            {
                return checkResult;
            }

            // ファイルの存在チェック
            var file= data.DataProperties.FirstOrDefault(d => d.Id == fileId);

            if (file == null)
            {
                return JsonNotFound($"File ID {fileId.Value} is not found.");
            }

            // 削除処理
            dataRepository.DeleteFile(data, fileId.Value);
            await storageLogic.DeleteFileAsync(ResourceType.Data, file.DataFile.StoredPath);

            // 結果に関わらずコミット
            unitOfWork.Commit();

            return JsonNoContent();
        }

        /// <summary>
        /// 指定したデータを削除する
        /// </summary>
        /// <param name="id">データID</param>
        /// <param name="dataSetRepository">Di用</param>
        /// <param name="tagRepository">Di用</param>
        [HttpDelete("{id}")]
        [Filters.PermissionFilter(MenuCode.Data)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteDataAsync(long? id, [FromServices] IDataSetRepository dataSetRepository, [FromServices] ITagRepository tagRepository)
        {
            if (id == null)
            {
                //IDが指定されていなければエラー
                return JsonBadRequest("Invalid inputs.");
            }
            //Dataとそれに紐づくSectionimageをDBから取得
            var data = await dataRepository.GetDataIncludeAllAsync(id.Value);
            if (data == null)
            {
                return JsonNotFound($"Data ID {id.Value} is not found.");
            }

            var checkResult = await CheckDataIsLocked(data, dataSetRepository);
            if(checkResult != null)
            {
                return checkResult;
            }

            // 削除処理
            bool result = await dataLogic.DeleteDataAsync(id.Value);

            // DBへの編集内容を一旦確定させるためコミット
            unitOfWork.Commit();

            // 未使用タグ削除
            tagRepository.DeleteUnUsedDataTags();

            // DBへタグ削除結果のコミット
            unitOfWork.Commit();

            if (result)
            {
                return JsonNoContent();
            }
            else
            {
                return JsonError(HttpStatusCode.ServiceUnavailable, $"Failed to delete data {data.Name}. Please contact your server administrator.");
            }
        }

        /// <summary>
        /// 指定したデータが編集可能か判定する。
        /// 編集可能な場合はnullを返す。
        /// </summary>
        private async Task<IActionResult> CheckDataIsLocked(Data data, IDataSetRepository dataSetRepository)
        {
            //このデータが含まれるデータセットが編集可能か
            var lockedDataSet = dataSetRepository.GetLockedDataSetByData(data.Id);
            if (lockedDataSet != null)
            {
                return JsonConflict($"Data {data.Name} is in the locked DataSet {lockedDataSet.Name}.");
            }

            // このデータを元にした加工履歴履歴がある場合は削除禁止
            if (await preprocessHistoryRepository.ExistsByInputDataIdAsync(data.Id))
            {
                return JsonConflict($"Data {data.Name} has been preprocessed.");
            }
            // このデータが何かしらのデータから派生した物であれば、削除禁止
            var sourceData = await preprocessHistoryRepository.GetInputDataAsync(data.Id);
            if (sourceData != null)
            {
                return JsonConflict($"Data {data.Name} was generated from {sourceData.Id}:{sourceData.Name}.");
            }
            return null;
        }

        /// <summary>
        /// 選択中のテナントに登録されているデータ管理で使用できるタグを表示する
        /// </summary>
        [HttpGet("datatags")]
        [Filters.PermissionFilter(MenuCode.Data)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public IActionResult GetTags()
        {
            // タグ種別がデータのものに限定する
            var tags = tagLogic.GetAllTags().Where(t => t.Type == TagType.Data);
            return JsonOK(tags.Select(t => t.Name));
        }

        [HttpDelete("datatags")]
        //[Filters.PermissionFilter(MenuCode.System)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public IActionResult DeleteUnUsedTags([FromServices] ITagRepository tagRepository)
        {
            tagRepository.DeleteUnUsedDataTags();
            unitOfWork.Commit();
            return JsonNoContent();
        }
    }
}
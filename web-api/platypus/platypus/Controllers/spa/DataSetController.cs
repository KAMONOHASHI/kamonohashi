using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.DataSetApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// データセット管理を扱うためのAPI集
    /// </summary>
    [Route("api/v1/datasets")]
    public class DataSetController : PlatypusApiControllerBase
    {
        private readonly IDataRepository dataRepository;
        private readonly IDataSetRepository dataSetRepository;
        private readonly IDataTypeRepository dataTypeRepository;
        private readonly IDataLogic dataLogic;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataSetController(
            IDataRepository dataRepository,
            IDataSetRepository dataSetRepository,
            IDataTypeRepository dataTypeRepository,
            IDataLogic dataLogic,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.dataRepository = dataRepository;
            this.dataSetRepository = dataSetRepository;
            this.dataTypeRepository = dataTypeRepository;
            this.dataLogic = dataLogic;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 指定された条件でページングされた状態で、データセット一覧を取得
        /// </summary>
        /// <param name="page">ページ番号。デフォルトは1。</param>
        /// <param name="perPage">表示件数。指定がない場合は上限(1000件)。</param>
        /// <param name="filter">検索条件</param>
        /// <param name="withTotal">合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。</param>
        [HttpGet]
        [Filters.PermissionFilter(MenuCode.DataSet, MenuCode.Training, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll([FromQuery]SearchInputModel filter, [FromQuery]int? perPage, [FromQuery] int page = 1, bool withTotal = false)
        {
            var dataSet = dataSetRepository.GetAll();
            dataSet = Search(dataSet, filter).OrderByDescending(d => d.Id);

            //未指定、あるいは1000件以上であれば、1000件に指定
            int pageCount = (perPage.HasValue && perPage.Value < 1000) ? perPage.Value : 1000;
            dataSet = dataSet.Paging(page, pageCount);

            if (withTotal)
            {
                int total = GetTotalCount(filter);
                SetTotalCountToHeader(total);
            }

            return JsonOK(dataSet.Select(_ => new IndexOutputModel(_)));
        }

        /// <summary>
        /// データセット件数を取得する
        /// </summary>
        /// <param name="filter">検索条件</param>
        private int GetTotalCount(SearchInputModel filter)
        {
            var dataSet = dataSetRepository.GetAll();
            dataSet = Search(dataSet, filter);
            return dataSet.Count();
        }

        /// <summary>
        /// 検索条件の追加
        /// </summary>
        /// <param name="sourceData">加工前の検索結果</param>
        /// <param name="query">検索条件</param>
        private IQueryable<DataSet> Search(IQueryable<DataSet> sourceData, SearchInputModel query)
        {
            IQueryable<DataSet> data = sourceData;
            return data
                .SearchLong(d => d.Id, query.Id)
                .SearchString(d => d.Name, query.Name)
                .SearchString(d => d.Memo, query.Memo)
                .SearchTime(d => d.CreatedAt, query.CreatedAt);
        }

        /// <summary>
        /// 指定したIDのデータセット詳細情報を取得する。
        /// </summary>
        /// <param name="id">データセットID</param>
        [HttpGet("{id}")]
        [Filters.PermissionFilter(MenuCode.DataSet, MenuCode.Training, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(DetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetail(long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("DataSet ID is required.");
            }
            var dataSet = await dataSetRepository.GetDataSetIncludeDataSetEntryAsync(id.Value);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet Id {id.Value} is not found.");
            }
            var model = new DetailsOutputModel(dataSet);

            if (dataSet.DataSetEntries != null)
            {
                //エントリの作成開始
                var entities = new Dictionary<string, List<ApiModels.DataApiModels.IndexOutputModel>>();

                //空のデータ種別も表示したい＆順番を統一したいので、先に初期化しておく
                foreach (var dataType in dataTypeRepository.GetAllWithOrderby(d => d.SortOrder, true))
                {
                    entities.Add(dataType.Name, new List<ApiModels.DataApiModels.IndexOutputModel>());
                }

                //エントリを一つずつ突っ込んでいく。件数次第では遅いかも。
                foreach (var entry in dataSet.DataSetEntries)
                {
                    string key = entry.DataType.Name;
                    var dataFile = new ApiModels.DataApiModels.IndexOutputModel(entry.Data);
                    entities[key].Add(dataFile);
                }

                model.Entries = entities;
            }

            model.IsLocked = dataSet.IsLocked;

            return JsonOK(model);
        }

        /// <summary>
        /// 指定したIDのデータセットに含まれるデータファイル情報を取得する。
        /// </summary>
        /// <param name="id">データセットID</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        [HttpGet("{id}/files")]
        [Filters.PermissionFilter(MenuCode.DataSet, MenuCode.Training, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(DataFileOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDataFiles([FromRoute] long? id, [FromQuery] bool withUrl)
        {
            if (id == null)
            {
                return JsonBadRequest("DataSet ID is required.");
            }
            var dataSet = await dataSetRepository.GetDataSetIncludeDataSetEntryAndDataAsync(id.Value);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet Id {id.Value} is not found.");
            }
            var model = new DataFileOutputModel(dataSet);

            if (dataSet.DataSetEntries != null)
            {
                //エントリの作成開始。最初はDictionary形式で
                var entities = new Dictionary<string, List<ApiModels.DataApiModels.DataFileOutputModel>>();

                //空のデータ種別も表示したい＆順番を統一したいので、先に初期化しておく
                foreach (var dataType in dataTypeRepository.GetAllWithOrderby(d => d.SortOrder, true))
                {
                    entities.Add(dataType.Name, new List<ApiModels.DataApiModels.DataFileOutputModel>());
                }

                //エントリを並列で取得する
                dataSet.DataSetEntries.AsParallel().ForAll(entry =>
                {
                    string key = entry.DataType.Name;
                    var dataFiles = dataLogic.GetDataFiles(entry.Data, withUrl);
                    lock (entities)
                    {
                        entities[key].AddRange(dataFiles);
                    }
                });

                model.SetEntries(entities);
            }

            return JsonOK(model);
        }

        /// <summary>
        /// 指定したIDのデータセットに含まれるデータとNFS上のデータ名のペア情報を取得する。
        /// </summary>
        /// <param name="id">データセットID</param>
        [HttpGet("{id}/pathpairs")]
        [Filters.PermissionFilter(MenuCode.DataSet, MenuCode.Training, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(IEnumerable<PathPairOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFilePaths([FromRoute] long? id)
        {
            if (id == null)
            {
                return JsonBadRequest("DataSet ID is required.");
            }
            var dataSet = await dataSetRepository.GetDataSetIncludeDataSetEntryAndDataAsync(id.Value);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet Id {id.Value} is not found.");
            }

            if (dataSet.DataSetEntries == null)
            {
                return JsonNoContent();
            }

            // training, testingといったデータタイプの種類を取得する
            Dictionary<long, string> dataTypes = new Dictionary<long, string>();
            foreach (var dataType in dataTypeRepository.GetAllWithOrderby(d => d.SortOrder, true))
            {
                dataTypes[dataType.Id] = dataType.Name;
            }

            //エントリを取得し、データのパスとデータ名のペアを作る
            List<PathPairOutputModel> pathPairs = new List<PathPairOutputModel>();
            foreach(var entry in dataSet.DataSetEntries)
            {
                string dataTypeName = dataTypes[entry.DataTypeId];
                foreach (var data in entry.Data.DataProperties)
                {
                    lock (pathPairs)
                    {
                        pathPairs.Add(new PathPairOutputModel($"{dataTypeName}/{entry.DataId}/{data.Key}", data.DataFile.StoredPath));
                    }
                }
            }
            return JsonOK(pathPairs);
        }

        /// <summary>
        /// データセットを新規作成する
        /// </summary>
        /// <param name="model">新規作成内容</param>
        [HttpPost]
        [Filters.PermissionFilter(MenuCode.DataSet)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateDataSet([FromBody]CreateInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            DataSet dataSet = new DataSet()
            {
                Name = model.Name,
                Memo = model.Memo,
                IsLocked = false
            };
            dataSetRepository.Add(dataSet);

            if (model.Entries == null)
            {
                unitOfWork.Commit();
                return JsonOK(new IndexOutputModel(dataSet));
            }

            return await InsertDataSetEntryAsync(dataSet, model.Entries, true);
        }

        /// <summary>
        /// データセットの付属情報（メモなど、任意のタイミングで変更できるもの）を変更する。
        /// </summary>
        /// <param name="id">データセットID</param>
        /// <param name="model">変更内容</param>
        [HttpPatch("{id}")]
        [Filters.PermissionFilter(MenuCode.DataSet)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit([FromRoute] long? id, [FromBody] EditInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid || id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var dataset = await dataSetRepository.GetByIdAsync(id.Value);
            if (dataset == null)
            {
                return JsonNotFound($"DataSet Id {id.Value} is not found.");
            }

            EditDataSet(dataset, model);

            unitOfWork.Commit();

            return JsonOK(new IndexOutputModel(dataset));
        }

        /// <summary>
        /// データセット編集
        /// </summary>
        /// <param name="dataset">編集前のデータセット</param>
        /// <param name="model">編集内容</param>
        private void EditDataSet(DataSet dataset, EditInputModel model)
        {
            //Nameは空文字禁止なので、空文字は変更なしと見なす
            if (string.IsNullOrEmpty(model.Name) == false)
            {
                dataset.Name = model.Name == ValueOfEmptyString ? dataset.Name : model.Name;
            }
            dataset.Memo = EditColumn(model.Memo, dataset.Memo);
        }
        
        /// <summary>
        /// データセットのエントリ内容（学習で使用後は編集不可）を変更する
        /// </summary>
        /// <param name="id">データセットID</param>
        /// <param name="model">変更内容</param>
        [HttpPut("{id}")]
        [Filters.PermissionFilter(MenuCode.DataSet)]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditEntries([FromRoute] long? id, [FromBody] EditEntriesInputModel model)
        {
            //データの入力チェック
            if (!ModelState.IsValid || id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var dataSet = await dataSetRepository.GetByIdAsync(id.Value);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet Id {id.Value} is not found.");
            }

            //学習に使われていたら更新できない
            if (dataSet.IsLocked)
            {
                return JsonConflict($"DataSet {dataSet.Name} has been used by training.");
            }
            
            //メタデータ編集
            EditDataSet(dataSet, model);

            //何も考えず、全Delete&Insertする。

            //まずは全削除
            dataSetRepository.DeleteAllEntries(dataSet.Id);

            //値があれば登録
            if (model.Entries == null)
            {
                unitOfWork.Commit();
                return JsonOK(new IndexOutputModel(dataSet));
            }

            return await InsertDataSetEntryAsync(dataSet, model.Entries, false);
        }

        /// <summary>
        /// データセットに指定したエントリを紐づける。
        /// エントリが空になっている前提。
        /// </summary>
        /// <param name="dataSet">データセット</param>
        /// <param name="entries">エントリ</param>
        /// <param name="isCreated">新規作成か否か</param>
        private async Task<IActionResult> InsertDataSetEntryAsync(DataSet dataSet, Dictionary<string, IEnumerable<CreateInputModel.Entry>> entries, bool isCreated)
        {
            //データ種別の指定が名前のため、名前からIDを引けるようにキャッシュしておく
            var dataTypes = new Dictionary<string, DataType>();
            foreach (var dataType in dataTypeRepository.GetAllWithOrderby(d => d.SortOrder, true))
            {
                dataTypes.Add(dataType.Name, dataType);
            }

            foreach (var dataLists in entries)
            {
                //DataTypeがなかった場合の処理
                if (dataTypes.ContainsKey(dataLists.Key) == false)
                {
                    return JsonNotFound($"DataType {dataLists.Key} is not found.");
                }

                var dataType = dataTypes[dataLists.Key];
                foreach (var entry in dataLists.Value)
                {
                    //Dataがなかった場合の処理
                    if (await dataRepository.ExistsAsync(d => d.Id == entry.Id) == false)
                    {
                        return JsonNotFound($"Data ID {entry.Id} is not found.");
                    }

                    dataSetRepository.AddEntry(dataSet, dataType.Id, entry.Id, isCreated);
                }
            }

            unitOfWork.Commit();

            if (isCreated)
            {
                return JsonCreated(new IndexOutputModel(dataSet));
            }
            else
            {
                return JsonOK(new IndexOutputModel(dataSet));
            }
        }

        /// <summary>
        /// データセットを削除する
        /// </summary>
        /// <param name="id">データセットID</param>
        [HttpDelete("{id}")]
        [Filters.PermissionFilter(MenuCode.DataSet)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteDataSet([FromRoute] long? id)
        {
            //データの入力チェック
            if (id == null)
            {
                return JsonBadRequest("Invalid inputs.");
            }
            //データの存在チェック
            var dataSet = await dataSetRepository.GetByIdAsync(id.Value);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet Id {id.Value} is not found.");
            }

            //学習に使われていたら削除できない
            if (dataSet.IsLocked)
            {
                return JsonConflict($"DataSet {dataSet.Name} has been used by training.");
            }

            dataSetRepository.Delete(dataSet);
            unitOfWork.Commit();

            return JsonNoContent();
        }

        /// <summary>
        /// 全データ種別を取得する
        /// </summary>
        [HttpGet("/api/v1/datatypes")]
        [Filters.PermissionFilter(MenuCode.DataSet)]
        [ProducesResponseType(typeof(IEnumerable<DataTypeOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetDataTypes()
        {
            var dataTypes = dataTypeRepository.GetAllWithOrderby(d => d.SortOrder, true).Select(d => new DataTypeOutputModel(d));

            return JsonOK(dataTypes);
        }
    }
}
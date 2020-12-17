using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.Aquarium.DataSetApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models.TenantModels.Aquarium;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.spa
{
    /// <summary>
    /// アクアリウムデータセットAPI
    /// </summary>
    [ApiVersion("2")]
    [Route("api/v{api-version:apiVersion}/aquarium/datasets")]
    public class AquariumDataSetController : PlatypusApiControllerBase
    {
        private readonly IDataRepository dataRepository;
        private readonly DataAccess.Repositories.Interfaces.TenantRepositories.Aquarium.IDataSetRepository dataSetRepository;
        private readonly IDataLogic dataLogic;
        private readonly IUnitOfWork unitOfWork;

        public AquariumDataSetController(
            IDataRepository dataRepository,
            DataAccess.Repositories.Interfaces.TenantRepositories.Aquarium.IDataSetRepository dataSetRepository,
            IDataLogic dataLogic,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.dataRepository = dataRepository;
            this.dataSetRepository = dataSetRepository;
            this.dataLogic = dataLogic;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// アクアリウムデータセットを作成する
        /// </summary>
        /// <param name="model">作成するアクアリウムデータセット</param>
        [HttpPost]
        [ProducesResponseType(typeof(IndexOutputModel), (int)HttpStatusCode.Created)]
        public IActionResult CreateDataSet([FromBody]CreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            var dataSet = new DataSet
            {
                Name = model.Name,
                LatestVersion = 0,
            };
            dataSetRepository.Add(dataSet);

            unitOfWork.Commit();
            return JsonCreated(new IndexOutputModel(dataSet));
        }

        /// <summary>
        /// アクアリウムデータセットバージョンを作成する
        /// </summary>
        /// <param name="id">作成先のアクアリウムデータセットID</param>
        /// <param name="model">作成するアクアリウムデータセットバージョン</param>
        [HttpPost("{id}/versions")]
        [ProducesResponseType(typeof(VersionIndexOutputModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateDataSetVersion(long id, [FromBody]VersionCreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonBadRequest("Invalid inputs.");
            }

            var dataSet = await dataSetRepository.GetByIdAsync(id);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet ID {id} is not found.");
            }

            dataSet.LatestVersion += 1;
            dataSetRepository.Update(dataSet);

            var dataSetVersion = new DataSetVersion
            {
                DataSetId = id,
                Version = dataSet.LatestVersion,
            };
            dataSetRepository.Add(dataSetVersion);

            foreach (var x in model.Entries)
            {
                if (!await dataRepository.ExistsAsync(y => y.Id == x.Id))
                {
                    return JsonNotFound($"Data ID {x.Id} is not found.");
                }
                dataSetRepository.Add(new DataSetVersionEntry
                {
                    DataId = x.Id,
                    DataSetVersion = dataSetVersion,
                });
            }

            unitOfWork.Commit();
            return JsonCreated(new VersionIndexOutputModel(dataSetVersion));
        }

        /// <summary>
        /// アクアリウムデータセットを削除する
        /// </summary>
        /// <param name="id">削除するアクアリウムデータセットID</param>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteDataSet(long id)
        {
            var dataSet = await dataSetRepository.GetByIdAsync(id);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet ID {id} is not found.");
            }
            dataSetRepository.Delete(dataSet);

            unitOfWork.Commit();
            return JsonNoContent();
        }

        /// <summary>
        /// 指定された条件でページングされた状態で、アクアリウムデータセット一覧を取得する
        /// </summary>
        /// <param name="page">ページ番号。デフォルトは1。</param>
        /// <param name="perPage">表示件数。指定がない場合は上限(1000件)。</param>
        /// <param name="filter">検索条件</param>
        /// <param name="withTotal">合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。</param>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IndexOutputModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetDataSetList([FromQuery]SearchInputModel filter, [FromQuery]int? perPage,
            [FromQuery]int page = 1, bool withTotal = false)
        {
            var dataSet = dataSetRepository.GetAll()
                .SearchLong(d => d.Id, filter.Id)
                .SearchString(d => d.Name, filter.Name)
                .SearchString(d => d.CreatedBy, filter.CreatedBy)
                .SearchTime(d => d.CreatedAt, filter.CreatedAt)
                .SearchString(d => d.ModifiedBy, filter.ModifiedBy)
                .SearchTime(d => d.ModifiedAt, filter.ModifiedAt);
            dataSet = dataSet.OrderByDescending(d => d.Id);

            if (withTotal)
            {
                SetTotalCountToHeader(dataSet.Count());
            }

            // 未指定、あるいは1000件以上であれば、1000件に指定
            var pageCount = (perPage.HasValue && perPage.Value < 1000) ? perPage.Value : 1000;
            dataSet = dataSet.Paging(page, pageCount);

            return JsonOK(dataSet.Select(x => new IndexOutputModel(x)));
        }

        /// <summary>
        /// アクアリウムデータセットバージョン一覧を取得する
        /// </summary>
        /// <param name="id">取得するアクアリウムデータセットのID</param>
        [HttpGet("{id}/versions")]
        [ProducesResponseType(typeof(IEnumerable<VersionIndexOutputModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDataSetVersionList(long id)
        {
            var dataSet = await dataSetRepository.GetDataSetWithVersionsAsync(id);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet Id {id} is not found.");
            }
            return JsonOK(dataSet.DataSetVersions
                .OrderByDescending(x => x.Version)
                .Select(x => new VersionIndexOutputModel(x)));
        }

        /// <summary>
        /// アクアリウムデータセットバージョンを取得する
        /// </summary>
        /// <param name="id">取得するアクアリウムデータセットID</param>
        /// <param name="versionId">取得するアクアリウムデータセットバージョンID</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        [HttpGet("{id}/versions/{versionId}")]
        [Filters.PermissionFilter(MenuCode.DataSet, MenuCode.Training, MenuCode.Inference, MenuCode.Notebook)]
        [ProducesResponseType(typeof(VersionDetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDataSetVersion(long id, long versionId, [FromQuery]bool withUrl)
        {
            var dataSetVersion = await dataSetRepository.GetDataSetVersionWithFilesAsync(id, versionId);
            if (dataSetVersion == null)
            {
                return JsonNotFound($"DataSetVersion (DataSetId {id} and VersionId {versionId}) is not found.");
            }

            var result = new VersionDetailsOutputModel(dataSetVersion);
            if (dataSetVersion.DataSetVersionEntries != null)
            {
                result.Entries = dataSetVersion.DataSetVersionEntries
                    .AsParallel()
                    .Select(x => new VersionDetailsOutputModel.Entry
                    {
                        Data = new ApiModels.DataApiModels.IndexOutputModel(x.Data),
                        Files = dataLogic.GetDataFiles(x.Data, withUrl)
                    });
            }
            return JsonOK(result);
        }
    }
}

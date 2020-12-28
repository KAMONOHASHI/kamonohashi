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
        private readonly IAquariumDataSetRepository aquariumDataSetRepository;
        private readonly IDataSetRepository dataSetRepository;
        private readonly IDataTypeRepository dataTypeRepository;
        private readonly IDataLogic dataLogic;
        private readonly IUnitOfWork unitOfWork;

        public AquariumDataSetController(
            IDataRepository dataRepository,
            IAquariumDataSetRepository aquariumDataSetRepository,
            IDataSetRepository dataSetRepository,
            IDataTypeRepository dataTypeRepository,
            IDataLogic dataLogic,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.dataRepository = dataRepository;
            this.aquariumDataSetRepository = aquariumDataSetRepository;
            this.dataSetRepository = dataSetRepository;
            this.dataTypeRepository = dataTypeRepository;
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
            aquariumDataSetRepository.Add(dataSet);

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

            var dataSet = await dataSetRepository.GetByIdAsync(model.DataSetId);
            if (dataSet == null)
            {
                return JsonNotFound($"DataSet ID {model.DataSetId} is not found.");
            }

            var aquariumDataSet = await aquariumDataSetRepository.GetByIdAsync(id);
            if (aquariumDataSet == null)
            {
                return JsonNotFound($"AquariumDataSet ID {id} is not found.");
            }

            aquariumDataSet.LatestVersion += 1;
            aquariumDataSetRepository.Update(aquariumDataSet);

            var dataSetVersion = new DataSetVersion
            {
                AquariumDataSetId = id,
                Version = aquariumDataSet.LatestVersion,
                DataSetId = model.DataSetId,
            };
            aquariumDataSetRepository.Add(dataSetVersion);

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
            var aquariumDataSet = await aquariumDataSetRepository.GetByIdAsync(id);
            if (aquariumDataSet == null)
            {
                return JsonNotFound($"AquariumDataSet ID {id} is not found.");
            }
            aquariumDataSetRepository.Delete(aquariumDataSet);

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
            var dataSet = aquariumDataSetRepository.GetAll()
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
            var aquariumDataSet = await aquariumDataSetRepository.GetDataSetWithVersionsAsync(id);
            if (aquariumDataSet == null)
            {
                return JsonNotFound($"AquariumDataSet Id {id} is not found.");
            }
            return JsonOK(aquariumDataSet.DataSetVersions
                .OrderByDescending(x => x.Version)
                .Select(x => new VersionIndexOutputModel(x)));
        }

        /// <summary>
        /// アクアリウムデータセットバージョンを取得する
        /// </summary>
        /// <param name="id">取得するアクアリウムデータセットID</param>
        /// <param name="versionId">取得するアクアリウムデータセットバージョンID</param>
        [HttpGet("{id}/versions/{versionId}")]
        [ProducesResponseType(typeof(VersionDetailsOutputModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDataSetVersion(long id, long versionId)
        {
            var dataSetVersion = await aquariumDataSetRepository.GetDataSetVersionWithDataAsync(id, versionId);
            if (dataSetVersion == null)
            {
                return JsonNotFound($"DataSetVersion (AquariumDataSetId {id} and VersionId {versionId}) is not found.");
            }

            var result = new VersionDetailsOutputModel(dataSetVersion);
            var dataSet = dataSetVersion.DataSet;

            if (dataSet.DataSetEntries != null)
            {
                result.Entries = new Dictionary<string, List<ApiModels.DataApiModels.IndexOutputModel>>();
                result.FlatEntries = new List<ApiModels.DataApiModels.IndexOutputModel>();

                foreach (var dataType in dataTypeRepository.GetAllWithOrderby(d => d.SortOrder, true))
                {
                    result.Entries.Add(dataType.Name, new List<ApiModels.DataApiModels.IndexOutputModel>());
                }

                if (dataSet.IsFlat)
                {
                    result.FlatEntries = dataSet.DataSetEntries
                        .OrderByDescending(x => x.Data.Id)
                        .Select(x => new ApiModels.DataApiModels.IndexOutputModel(x.Data));
                }
                else
                {
                    foreach (var x in dataSet.DataSetEntries.OrderByDescending(x => x.Data.Id))
                    {
                        result.Entries[x.DataType.Name].Add(new ApiModels.DataApiModels.IndexOutputModel(x.Data));
                    }
                }
            }
            return JsonOK(result);
        }
    }
}

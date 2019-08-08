using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.ApiModels.DataApiModels;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    public class DataLogic : PlatypusLogicBase, IDataLogic
    {
        private readonly IDataRepository dataRepository;
        private readonly ITagLogic tagLogic;
        private readonly IStorageLogic storageLogic;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataLogic(
            IDataRepository dataRepository,
            ITagLogic tagLogic,
            IStorageLogic storageLogic,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.dataRepository = dataRepository;
            this.tagLogic = tagLogic;
            this.storageLogic = storageLogic;
        }

        /// <summary>
        /// 指定したIDの全ファイル情報を取得する。
        /// </summary>
        /// <param name="dataId">データID</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        public IEnumerable<DataFileOutputModel> GetDataFiles(long dataId, bool withUrl)
        {
            var result = new List<DataFileOutputModel>();
            var properties = dataRepository.GetAllDataProperty(dataId);
            foreach(var property in properties)
            {
                var model = new DataFileOutputModel { Id = dataId, Key = property.Key, FileId = property.Id, FileName = property.DataFile.FileName };

                if (withUrl)
                {
                    model.Url = storageLogic.GetPreSignedUriForGet(ResourceType.Data, property.DataFile.StoredPath, property.DataFile.FileName, true).ToString();
                }
                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 指定したIDの全ファイル情報を取得する。
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        public IEnumerable<DataFileOutputModel> GetDataFiles(Data data, bool withUrl)
        {
            var result = new List<DataFileOutputModel>();
            
            foreach (var property in data.DataProperties)
            {
                var model = new DataFileOutputModel { Id = data.Id, Key = property.Key, FileId = property.Id, FileName = property.DataFile.FileName };

                if (withUrl)
                {
                    model.Url = storageLogic.GetPreSignedUriForGet(ResourceType.Data, property.DataFile.StoredPath, property.DataFile.FileName, true).ToString();
                }
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 指定されたIDのデータを削除する。
        /// まずDBのレコードを削除して、その後オブジェクトストレージから上記のデータを削除する。
        /// コミットは行わないので、呼び出し側で実行すること。（基本的には結果に関わらずコミットする）
        /// </summary>
        /// <param name="dataId">データID</param>
        public async Task<bool> DeleteDataAsync(long dataId)
        {
            var dataSetRepository = CommonDiLogic.DynamicDi<IDataSetRepository>();
            // データセットエントリを削除
            dataSetRepository.RemoveDataFromDataSet(dataId);

            // データとファイルを削除
            var data = await dataRepository.GetDataIncludeAllAsync(dataId);

            dataRepository.DeleteData(data);

            foreach (var file in data.DataProperties)
            {
                if (file.DataFile != null)
                {
                    await storageLogic.AddFileToDeleteListAsync(ResourceType.Data, file.DataFile.StoredPath);
                }
            }

            // タグマップを削除
            tagLogic.Delete(dataId);

            // オブジェクトストレージからファイルを削除
            return await storageLogic.DeleteFilesInDeleteListAsync();
        }
    }
}

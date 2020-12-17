using Amazon.S3.Model;
using Nssol.Platypus.LogicModels.StorageLogicModels;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// オブジェクトストレージとDBにまたがったデータの管理を行うロジック。
    /// 主に整合性の担保を目的とする。
    /// </summary>
    public class StorageLogic : PlatypusLogicBase, IStorageLogic
    {
        private IObjectStorageService objectStorageService;

        private StorageConfigModel config;

        /// <summary>
        /// 一括削除に使う削除対象リスト。
        /// </summary>
        private List<string> deleteList = new List<string>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StorageLogic(
            ICommonDiLogic commonDiLogic,
            IObjectStorageService objectStorageService) : base(commonDiLogic)
        {
            this.objectStorageService = objectStorageService;

            //DBのテナント情報からオブジェクトストレージの設定を初期化する。
            Tenant tenant = CurrentUserInfo?.SelectedTenant;
            if (tenant != null && tenant.Storage != null)
            {
                config = ConvertToStorageConfig(tenant, tenant.Storage);
                this.objectStorageService.Initialize(config);
            }
        }

        /// <summary>
        /// <see cref="Tenant"/>を<see cref="StorageConfigModel"/>に変換する
        /// </summary>
        private StorageConfigModel ConvertToStorageConfig(Tenant tenant, Storage storage)
        {
            return new StorageConfigModel()
            {
                AccessKey = storage.AccessKey,
                SecretKey = storage.SecretKey,
                StorageServer = storage.ServerAddress,
                Bucket = tenant.StorageBucket
            };
        }

        /// <summary>
        /// 指定したテナントに対応するバケットを新規作成する。
        /// </summary>
        /// <remarks>新規作成時は<see cref="Tenant.Storage"/>が未セットの場合があるため、それぞれ引数で与えている</remarks>
        /// <returns>バケットが存在せず新規作成なら true、既にバケットが存在していたなら false を返却</returns>
        /// <exception>バケット生成の失敗</exception>
        public async Task<bool> CreateBucketAsync(Tenant tenant, Storage storage)
        {
            config = ConvertToStorageConfig(tenant, storage);
            return await objectStorageService.CreateBucketAsync(config);
        }

        /// <summary>
        /// 指定したテナントに対応するバケットを削除する。
        /// </summary>
        /// <remarks>削除用の StorageConfigModel を生成するために、Tenant と Storage オブジェクトを引数に取る</remarks>
        /// <returns>バケットが存在して削除したなら true、既にバケットが存在していなかったなら false を返却</returns>
        /// <exception>バケット削除失敗</exception>
        public async Task<bool> DeleteBucketAsync(Tenant tenant, Storage storage)
        {
            StorageConfigModel configForDelete = ConvertToStorageConfig(tenant, storage);
            return await objectStorageService.DeleteBucketAsync(configForDelete);
        }

        /// <summary>
        /// 指定したリソース種別と履歴データIDに対応するフォルダ階層以下の結果データを削除する。
        /// </summary>
        /// <remarks> type/historyId で指定するフォルダ階層以下のオブジェクトを全て削除する</remarks>
        /// <param name="type">リソース種別</param>
        /// <param name="historyId">履歴データID</param>
        public async Task DeleteResultsAsync(ResourceType type, long historyId)
        {
            var prefix = type.ToString() + "/" + historyId.ToString();

            await objectStorageService.DeleteObjectsAsync(config, prefix);
        }

        /// <summary>
        /// 指定したファイルを削除する。
        /// </summary>
        public async Task DeleteFileAsync(ResourceType type, string fileName)
        {
            await AddFileToDeleteListAsync(type, fileName);
            await DeleteFilesInDeleteListAsync();
        }


        /// <summary>
        /// 指定したファイルを削除対象リストに登録する。
        /// 一括削除の上限である1000件を超えそうであれば、削除自体も行う。
        /// 確実にすべて削除するためには <see cref="DeleteFilesInDeleteListAsync"/> を実行する必要がある。
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <param name="fileName">オブジェクトストレージ上の対象ファイル名（拡張子付き）</param>
        public async Task AddFileToDeleteListAsync(ResourceType type, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) == false)
            {
                deleteList.Add(type.ToString() + "/" + fileName);

                //1000件になったら削除実施
                if (deleteList.Count == 1000)
                {
                    await DeleteFilesInDeleteListAsync();
                }
            }
        }

        /// <summary>
        /// オブジェクトストレージから、削除対象リストに登録された全てのデータを一括削除する。
        /// </summary>
        public async Task<bool> DeleteFilesInDeleteListAsync()
        {
            if (deleteList.Count() == 0)
            {
                //削除対象件数が0であれば、成功と見なす
                return true;
            }

            bool result = await objectStorageService.DeleteAsync(deleteList);

            //削除に失敗した場合も、削除対象リストからは除去する（＝システム的なリトライはしない）
            deleteList.Clear();

            return result;
        }

        /// <summary>
        /// 指定されたデータファイルへアクセスする設定情報を返す。
        /// </summary>
        public StorageConfigModel GetConfig()
        {
            return config;
        }

        /// <summary>
        /// 指定されたデータファイルの署名付URI(GET)を返す。
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <param name="fileStoredPath">ストレージ側ファイル名</param>
        /// <param name="fileName">ユーザーにダウンロードさせるファイル名</param>
        /// <param name="useHttp">通信でHTTPを使用するか</param>
        public Uri GetPreSignedUriForGet(ResourceType type, string fileStoredPath, string fileName, bool useHttp)
        {
            string key = CreateKey(type, fileStoredPath);
            return objectStorageService.GetPreSignedUriForGet(key, fileName, useHttp);
        }

        /// <summary>
        /// 指定されたKeyの署名付URI(GET)を返す。
        /// </summary>
        /// <param name="key">S3のファイルアクセスパス（バケット直下からのパス）</param>
        /// <param name="fileName">ユーザーにダウンロードさせるファイル名</param>
        /// <param name="useHttp">通信でHTTPを使用するか</param>
        public Uri GetPreSignedUriForGetFromKey(string key, string fileName, bool useHttp)
        {
            return objectStorageService.GetPreSignedUriForGet(key, fileName, useHttp);
        }

        /// <summary>
        /// 指定されたデータファイルの内容を返す。
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <param name="fileStoredPath">ストレージ側ファイル名</param>
        /// <param name="fileName">ユーザーにダウンロードさせるファイル名</param>
        /// <param name="useHttp">通信でHTTPを使用するか</param>
        public async Task<string> GetFileContentAsync(ResourceType type, string fileStoredPath, string fileName, bool useHttp)
        {
            string key = CreateKey(type, fileStoredPath);
            var fileList = await objectStorageService.GetUnderDirAsync(key);
            // 該当ファイルが存在する場合は内容を取得
            if(fileList.IsSuccess && fileList.Value.Files.Count == 1)
            {
                Uri uri = objectStorageService.GetPreSignedUriForGet(key, fileName, useHttp);
                var content = await objectStorageService.GetFileContentAsync(uri);
                return content == null ? null : content;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 指定されたファイルのファイルサイズを返す。
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <param name="fileStoredPath">ファイルのPath</param>
        /// <returns>ファイルサイズ</returns>
        public long GetFileSize(ResourceType type, string fileStoredPath)
        {
            string key = CreateKey(type, fileStoredPath);
            return objectStorageService.GetFileSize(key);
        }

        /// <summary>
        /// ファイル種別とファイル名からオブジェクトストレージのキーを作成する。
        /// </summary>
        private string CreateKey(ResourceType type, string fileName)
        {
            return type.ToString() + "/" + fileName;
        }

        /// <summary>
        /// 分割アップロード用URLの取得
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileName"></param>
        /// <param name="numPart"></param>
        /// <returns></returns>
        public async Task<MultiPartUploadModel> GetPartUploadPreSignedUrlAsync(ResourceType type, string fileName, int numPart)
        {
            // 拡張子つきのパス生成
            var extension = Path.GetExtension(fileName).Replace(".", "");
            var generatedName = Path.ChangeExtension(Guid.NewGuid().ToString(), extension);

            var key = CreateKey(type, generatedName);
            var uploadId = await objectStorageService.InitiateMultiPartUploadAsync(key);

            var presignedUrls = new List<Uri>();
            for (int i = 0; i < numPart;)
            {
                presignedUrls.Add(objectStorageService.GetMultiPartSignatureUrl(key, uploadId, ++i));
            }

            var model = new MultiPartUploadModel
            {
                Uris = presignedUrls,
                PartsSum = numPart,
                UploadId = uploadId,
                Key = key,
                FileName = fileName,
                StoredPath = generatedName,
            };

            return model;
        }

        /// <summary>
        /// 分割アップロードの完了処理
        /// </summary>
        /// <param name="completeInfo"></param>
        /// <returns></returns>
        public async Task CompletPartUploadAsync(CompleteMultiplePartUploadInputModel completeInfo)
        {
            var etags = completeInfo.PartETags.Select(t =>
            {
                var sep = t.Split('+');
                return new PartETag
                {
                    PartNumber = int.Parse(sep[0]),
                    ETag = sep[1].Trim('"'),
                };
            });
            await objectStorageService.CompleteMultiPartUploadAsync(completeInfo.Key, completeInfo.UploadId, etags);
        }
        /// <summary>
        /// minio NFSに書き込まれた学習結果を一覧で取得する
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <param name="searchDirPath">検索対象ディレクトリ</param>
        public async Task<Result<StorageListResultInfo, string>> GetUnderDirAsync(ResourceType type, string searchDirPath) {

           searchDirPath = CreateKey(type, searchDirPath);
           return await objectStorageService.GetUnderDirAsync("/" + searchDirPath);
        }
    }
}

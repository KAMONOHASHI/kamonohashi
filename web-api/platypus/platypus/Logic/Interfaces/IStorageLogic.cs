using Amazon.S3.Model;
using Nssol.Platypus.LogicModels.StorageLogicModels;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// オブジェクトストレージとDBにまたがったデータの管理を行うロジック。
    /// 主に整合性の担保を目的とする。
    /// </summary>
    public interface IStorageLogic
    {

        /// <summary>
        /// 指定したテナントに対応するバケットを新規作成する。
        /// </summary>
        /// <remarks>新規作成時は<see cref="Tenant.Storage"/>が未セットの場合があるため、それぞれ引数で与えている</remarks>
        /// <returns>バケットが存在せず新規作成なら true、既にバケットが存在していたなら false を返却</returns>
        /// <exception>バケット生成の失敗</exception>
        Task<bool> CreateBucketAsync(Tenant tenant, Storage storage);

        /// <summary>
        /// 指定したテナントに対応するバケットを削除する。
        /// </summary>
        /// <remarks>削除用の StorageConfigModel を生成するために、Tenant と Storage オブジェクトを引数に取る</remarks>
        /// <returns>バケットが存在して削除したなら true、既にバケットが存在していなかったなら false を返却</returns>
        /// <exception>バケット削除失敗</exception>
        Task<bool> DeleteBucketAsync(Tenant tenant, Storage storage);

        /// <summary>
        /// 指定したリソース種別と履歴データIDに対応するフォルダ階層以下の結果データを削除する。
        /// </summary>
        /// <remarks> type/historyId で指定するフォルダ階層以下のオブジェクトを全て削除する</remarks>
        /// <param name="type">リソース種別</param>
        /// <param name="historyId">履歴データID</param>
        Task DeleteResultsAsync(ResourceType type, long historyId);

        /// <summary>
        /// 指定したファイルを削除する。
        /// </summary>
        Task DeleteFileAsync(ResourceType type, string fileName);

        /// <summary>
        /// 指定したファイルを削除対象リストに登録する。
        /// 一括削除の上限である1000件を超えそうであれば、削除自体も行う。
        /// 確実にすべて削除するためには <see cref="DeleteFilesInDeleteListAsync"/> を実行する必要がある。
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <param name="fileName">オブジェクトストレージ上の対象ファイル名（拡張子付き）</param>
        Task AddFileToDeleteListAsync(ResourceType type, string fileName);

        /// <summary>
        /// オブジェクトストレージから、削除対象リストに登録された全てのデータを一括削除する。
        /// </summary>
        Task<bool> DeleteFilesInDeleteListAsync();

        /// <summary>
        /// 指定されたデータファイルの署名付URI(GET)を返す。
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <param name="fileStoredPath">ストレージ側ファイル名</param>
        /// <param name="fileName">ユーザーにダウンロードさせるファイル名</param>
        /// <param name="useHttp">通信でHTTPを使用するか</param>
        Uri GetPreSignedUriForGet(ResourceType type, string fileStoredPath, string fileName, bool useHttp);

        /// <summary>
        /// 指定されたデータファイルのファイルサイズを返す。
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <returns>ファイルサイズ</returns>
        long GetFileSize(ResourceType type, string fileStoredPath);

        /// <summary>
        /// 指定されたデータファイルの内容を返す。
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <param name="fileStoredPath">ストレージ側ファイル名</param>
        /// <param name="fileName">ユーザーにダウンロードさせるファイル名</param>
        /// <param name="useHttp">通信でHTTPを使用するか</param>
        Task<string> GetFileContentAsync(ResourceType type, string fileStoredPath, string fileName, bool useHttp);

        /// <summary>
        /// 指定されたKeyの署名付URI(GET)を返す。
        /// </summary>
        /// <param name="key">S3のファイルアクセスパス（バケット直下からのパス）</param>
        /// <param name="fileName">ユーザーにダウンロードさせるファイル名</param>
        /// <param name="useHttp">通信でHTTPを使用するか</param>
        Uri GetPreSignedUriForGetFromKey(string key, string fileName, bool useHttp);

        /// <summary>
        /// 指定されたデータファイルへアクセスする設定情報を返す。
        /// </summary>
        StorageConfigModel GetConfig();

        /// <summary>
        /// 分割アップロード用URLの取得
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileStoredPath"></param>
        /// <param name="numPart"></param>
        /// <returns></returns>
        Task<MultiPartUploadModel> GetPartUploadPreSignedUrlAsync(ResourceType type, string fileStoredPath, int numPart);

        /// <summary>
        /// 分割アップロードの完了処理
        /// </summary>
        /// <param name="completeInfo"></param>
        /// <returns></returns>
        Task CompletPartUploadAsync(CompleteMultiplePartUploadInputModel completeInfo);

        /// <summary>
        /// minio NFSに書き込まれた学習結果を一覧で取得する
        /// </summary>
        /// <param name="type">リソース種別</param>
        /// <param name="searchDirPath">検索対象ディレクトリ</param>
        Task<Result<StorageListResultInfo, string>> GetUnderDirAsync(ResourceType type, string searchDirPath);
    }
}

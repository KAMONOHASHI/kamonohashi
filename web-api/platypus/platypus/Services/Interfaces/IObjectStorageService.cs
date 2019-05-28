using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.ServiceModels;
using System.Collections.Specialized;
using Amazon.S3;
using Amazon.S3.Model;

namespace Nssol.Platypus.Services.Interfaces
{
    /// <summary>
    /// オブジェクトストレージを使ってデータをCRUDするサービスクラスのインターフェース。
    /// </summary>
    public interface IObjectStorageService
    {
        /// <summary>
        /// オブジェクトストレージに接続するための設定情報を初期化する。
        /// </summary>
        void Initialize(StorageConfigModel storageConfig);

        /// <summary>
        /// 新規にバケットを作成する。
        /// 接続情報は<see cref="Initialize(StorageConfigModel)"/>で指定したものではなく、引数で指定したものを使う。
        /// </summary>
        /// <returns>バケットが存在せず新規作成なら true、既にバケットが存在していたなら false を返却</returns>
        /// <exception cref="AmazonS3Exception">バケット生成の失敗</exception>
        Task<bool> CreateBucketAsync(StorageConfigModel storageConfig);

        /// <summary>
        /// バケットを削除する。
        /// 接続情報は<see cref="Initialize(StorageConfigModel)"/>で指定したものではなく、引数で指定したものを使う。
        /// </summary>
        Task<bool> DeleteBucketAsync(StorageConfigModel storageConfig);

        /// <summary>
        /// 指定した接続情報でCORS設定を取得する。
        /// </summary>
        Task<string> GetCorsConfigurationAsync(StorageConfigModel storageConfig);

        /// <summary>
        /// 指定した接続情報でCORS設定を変更する。
        /// </summary>
        Task<bool> SetCorsConfigurationAsync(StorageConfigModel storageConfig, string corsConfigStr);

        /// <summary>
        /// 指定した接続情報でCORS設定を削除する。
        /// </summary>
        Task<bool> DeleteCorsConfigurationAsync(StorageConfigModel storageConfig);

        /// <summary>
        /// オブジェクトストレージから、引数で与えられた全てのデータを一括削除する。
        /// </summary>
        Task<bool> DeleteAsync(IEnumerable<string> keys);

        /// <summary>
        /// 指定されたデータファイルの署名付URI(GET)を返す。
        /// </summary>
        /// <param name="key">オブジェクトのキー</param>
        /// <param name="fileName">ダウンロード時に変換するファイル名</param>
        /// <param name="useHttp">通信でHTTPを使用するか</param>
        Uri GetPreSignedUriForGet(string key, string fileName, bool useHttp);

        /// <summary>
        /// 指定されたデータファイルの内容を返す。
        /// </summary>
        /// <param name="uri">データファイルのURI</param>
        Task<string> GetFileContentAsync(Uri uri);

        /// <summary>
        /// 分割アップロードの初期化
        /// </summary>
        /// <param name="key">Bucket中のオブジェクトを特定するためのキー</param>
        /// <returns>ObjectStorageで生成されたUploadId</returns>
        Task<string> InitiateMultiPartUploadAsync(string key);

        /// <summary>
        /// 分割アップロードの完了処理
        /// </summary>
        /// <param name="key">Bucket中のオブジェクトを特定するためのキー</param>
        /// <param name="uploadId">一連のアップロードを識別するためのキー</param>
        /// <param name="partETags">個別のアップロードを特定するためのシークエンス</param>
        Task CompleteMultiPartUploadAsync(string key, string uploadId, IEnumerable<PartETag> partETags);

        /// <summary>
        /// 分割アップロード用のURL取得
        /// </summary>
        /// <param name="key">Bucket中のオブジェクトを特定するためのキー</param>
        /// <param name="uploadId">一連のアップロードを識別するためのキー</param>
        /// <param name="partNumber">同一アップロード中のシークエンス番号</param>
        /// <returns>URL</returns>
        Uri GetMultiPartSignatureUrl(string key, string uploadId, int partNumber);

        /// <summary>
        /// バケットにあるファイルの一覧取得。minio NFSに書き込まれた学習結果を一覧で取得するために使用。
        /// 1000件までしか取得しない
        /// </summary>
        /// <param name="searchDirPath"> 検索対象ディレクトリ </param>
        /// <returns> StorageListResultInfo  </returns>
        Task<Result<StorageListResultInfo, string>> GetUnderDirAsync(string searchDirPath);
    }
}

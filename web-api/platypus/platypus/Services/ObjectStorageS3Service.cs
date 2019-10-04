using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.ServiceModels;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Nssol.Platypus.Services
{
    /// <summary>
    /// S3互換のオブジェクトストレージを使って
    /// データをCRUDするためのロジッククラス。
    /// </summary>
    public class ObjectStorageS3Service : PlatypusServiceBase, IObjectStorageService
    {
        /// <summary>
        /// オブジェクトストレージに接続するためのクライアント。
        /// リクエストごとに一つ生成し、同一リクエスト内では使いまわす。
        /// </summary>
        private AmazonS3Client client;

        /// <summary>
        /// バケット名
        /// </summary>
        private string bucket;

        /// <summary>
        /// 一時URLを発行するために必要なシークレットキー
        /// </summary>
        private string secretKey;

        /// <summary>
        /// アクセスキー
        /// </summary>
        private string accessKey;

        private ObjectStorageOptions options;

        /// <summary>
        /// スタティックイニシャライザ
        /// </summary>
        static ObjectStorageS3Service()
        {
            // 以下を有効にすると Presigned URL が v4 で発行されるようになる
            // 指定しないと、v2 形式
            // なお、現在シス研の Ceph は Jewel であり、クエリパラメータ形式の
            // Presigned URL を受け付けない模様
            // https://stackoverflow.com/questions/38640824/ceph-s3-radosgw-403
            // AWSConfigsS3.UseSignatureVersion4 = true;
        }

        /// <summary>
        /// S3互換オブジェクトストレージのクライアントの実装
        /// </summary>
        public ObjectStorageS3Service(
            Logic.Interfaces.ICommonDiLogic commonDiLogic,
            IOptions<ObjectStorageOptions> options) : base(commonDiLogic)
        {
            this.options = options.Value;
        }

        #region 分割署名付URLの生成

        /// <summary>
        /// 分割アップロードの初期化
        /// </summary>
        /// <param name="key">Bucket中のオブジェクトを特定するためのキー</param>
        /// <returns>ObjectStorageで生成されたUploadId</returns>
        public async Task<string> InitiateMultiPartUploadAsync(string key)
        {
            var initRequest = new InitiateMultipartUploadRequest
            {
                BucketName = bucket,
                Key = key
            };
            var initResponse = await client.InitiateMultipartUploadAsync(initRequest);
            return initResponse.UploadId;
        }

        /// <summary>
        /// 分割アップロードの完了処理
        /// </summary>
        /// <param name="key">Bucket中のオブジェクトを特定するためのキー</param>
        /// <param name="uploadId">一連のアップロードを識別するためのキー</param>
        /// <param name="partETags">個別のアップロードを特定するためのシークエンス</param>
        public async Task CompleteMultiPartUploadAsync(string key, string uploadId, IEnumerable<PartETag> partETags)
        {
            var compRequest = new CompleteMultipartUploadRequest
            {
                BucketName = bucket,
                Key = key,
                UploadId = uploadId,
                PartETags = partETags.ToList(),
            };
            var compResponse = await client.CompleteMultipartUploadAsync(compRequest);
        }

        /// <summary>
        /// 分割アップロード用のURL取得
        /// </summary>
        /// <param name="key">Bucket中のオブジェクトを特定するためのキー</param>
        /// <param name="uploadId">一連のアップロードを識別するためのキー</param>
        /// <param name="partNumber">同一アップロード中のシークエンス番号</param>
        /// <returns>URL</returns>
        public Uri GetMultiPartSignatureUrl(string key, string uploadId, int partNumber)
        {
            // 今はここは v2 でやっている。
            // v4 対応する場合、クライアントからリクエストボディのハッシュ値を計算
            // するなどしないといけない。
            // --------
            // v2 でやるにしてもクライアントライブラリをつかって
            // 本当はこんな感じでやりたいが、できないらしい。
            // JS とかだとできるっぽいけど。
            // https://github.com/aws/aws-sdk-js/issues/468
            // https://stackoverflow.com/questions/20847196/amazon-s3-multipart-upload-using-query-string-authentication-and-net-sdk
            // 
            // var presignedRequest = new GetPreSignedUrlRequest()
            // {
            //     Protocol = Protocol.HTTP,
            //     BucketName = bucket,
            //     Key = key,
            //     ContentType = "application/x-www-form-urlencoded",
            //     Verb = HttpVerb.PUT,
            //     Expires = DateTime.Now.AddSeconds(options.PreSignedUrlExpirationSec)
            // };
            // presignedRequest.Parameters.Add("uploadId", WebUtility.UrlEncode(uploadId));
            // presignedRequest.Parameters.Add("partNumber", WebUtility.UrlEncode(Convert.ToString(partNumber)));
            // var presignedResponse = client.GetPreSignedURL(presignedRequest);
            // return new Uri(presignedResponse);
            // --
            // 仕方がないので、三橋氏がWebから拾ってきた以下のソースコードを改変して使っている
            // http://gauravmantri.com/2014/01/06/create-pre-signed-url-using-c-for-uploading-large-files-in-amazon-s3/

            DateTime expiryDate = DateTime.UtcNow.AddSeconds(this.options.PreSignedUrlExpirationSec);
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = new TimeSpan(expiryDate.Ticks - Jan1st1970.Ticks);
            var expiry = Convert.ToInt64(ts.TotalSeconds);
            var requestUrl = $"{this.client.Config.ServiceURL}{bucket}/{key}";
            var partUploadUrl = new Uri($"{requestUrl}?uploadId={WebUtility.UrlEncode(uploadId)}&partNumber={partNumber}");
            var content = "application/x-www-form-urlencoded";
            var stringToSign = GetStringToSign(partUploadUrl, "PUT", string.Empty, content, expiry, null);
            var partUploadSignature = CreateSignature(secretKey, stringToSign);
            var partUploadPreSignedUrl = new Uri(
                $"{requestUrl}?uploadId={WebUtility.UrlEncode(uploadId)}&partNumber={partNumber}&" +
                $"AWSAccessKeyId={this.accessKey}&Signature={WebUtility.UrlEncode(partUploadSignature)}&Expires={expiry}");
            return partUploadPreSignedUrl;
        }

        private string GetStringToSign(Uri requestUri, string httpVerb, string contentMD5, string contentType, long secondsSince1stJan1970, NameValueCollection requestHeaders)
        {
            var canonicalizedResourceString = GetCanonicalizedResourceString(requestUri);
            var canonicalizedAmzHeadersString = GetCanonicalizedAmzHeadersString(requestHeaders);
            var stringToSign = $"{httpVerb}\n{contentMD5}\n{contentType}\n{secondsSince1stJan1970}\n{canonicalizedAmzHeadersString}{canonicalizedResourceString}";
            return stringToSign;
        }

        private string GetCanonicalizedResourceString(Uri requestUri)
        {
            string[] subResourcesToConsider = new string[] {
                "acl", "lifecycle", "location", "logging", "notification", "partNumber", "policy",
                "requestPayment", "torrent", "uploadId", "uploads", "versionId", "versioning", "versions", "website"
            };
            string[] overrideResponseHeadersToConsider = new string[] {
                "response-content-type", "response-content-language", "response-expires",
                "response-cache-control", "response-content-disposition", "response-content-encoding"
            };

            var host = requestUri.DnsSafeHost;
            var hostElementsArray = host.Split('.');
            var bucketName = "";
            var subResourcesList = subResourcesToConsider.ToList();
            var overrideResponseHeadersList = overrideResponseHeadersToConsider.ToList();
            StringBuilder canonicalizedResourceStringBuilder = new StringBuilder();
            canonicalizedResourceStringBuilder.Append(bucketName);
            canonicalizedResourceStringBuilder.Append(requestUri.AbsolutePath);
            var queryVariables = QueryHelpers.ParseQuery(requestUri.Query);
            SortedDictionary<string, string> queryVariablesToConsider = new SortedDictionary<string, string>();
            SortedDictionary<string, string> overrideResponseHeaders = new SortedDictionary<string, string>();
            if (queryVariables != null && queryVariables.Count > 0)
            {
                foreach (var item in queryVariables)
                {
                    var key = item.Key;
                    var value = item.Value;
                    if (subResourcesList.Contains(key))
                    {
                        if (queryVariablesToConsider.ContainsKey(key))
                        {
                            var val = queryVariablesToConsider[key];
                            queryVariablesToConsider[key] = $"{value},{val}";
                        }
                        else
                        {
                            queryVariablesToConsider.Add(key, value);
                        }
                    }
                    if (overrideResponseHeadersList.Contains(key))
                    {
                        overrideResponseHeaders.Add(key, WebUtility.UrlDecode(value));
                    }
                }
            }
            if (queryVariablesToConsider.Count > 0 || overrideResponseHeaders.Count > 0)
            {
                StringBuilder queryStringInCanonicalizedResourceString = new StringBuilder();
                queryStringInCanonicalizedResourceString.Append("?");
                for (int i = 0; i < queryVariablesToConsider.Count; i++)
                {
                    var key = queryVariablesToConsider.Keys.ElementAt(i);
                    var value = queryVariablesToConsider.Values.ElementAt(i);
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        queryStringInCanonicalizedResourceString.Append($"{key}={value}&");
                    }
                    else
                    {
                        queryStringInCanonicalizedResourceString.Append($"{key}&");
                    }
                }
                for (int i = 0; i < overrideResponseHeaders.Count; i++)
                {
                    var key = overrideResponseHeaders.Keys.ElementAt(i);
                    var value = overrideResponseHeaders.Values.ElementAt(i);
                    queryStringInCanonicalizedResourceString.Append($"{key}={value}&");
                }
                var str = queryStringInCanonicalizedResourceString.ToString();
                if (str.EndsWith("&"))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                canonicalizedResourceStringBuilder.Append(str);
            }
            return canonicalizedResourceStringBuilder.ToString();
        }

        private string GetCanonicalizedAmzHeadersString(NameValueCollection requestHeaders)
        {
            var canonicalizedAmzHeadersString = string.Empty;
            if (requestHeaders != null && requestHeaders.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                SortedDictionary<string, string> sortedRequestHeaders = new SortedDictionary<string, string>();
                var requestHeadersCount = requestHeaders.Count;
                for (int i = 0; i < requestHeadersCount; i++)
                {
                    var key = requestHeaders.Keys.Get(i);
                    var value = requestHeaders[key].Trim();
                    key = key.ToLowerInvariant();
                    if (key.StartsWith("x-amz-", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (sortedRequestHeaders.ContainsKey(key))
                        {
                            var val = sortedRequestHeaders[key];
                            sortedRequestHeaders[key] = $"{val},{value}";
                        }
                        else
                        {
                            sortedRequestHeaders.Add(key, value);
                        }
                    }
                }
                if (sortedRequestHeaders.Count > 0)
                {
                    foreach (var item in sortedRequestHeaders)
                    {
                        sb.Append($"{item.Key}:{item.Value}\n");
                    }
                    canonicalizedAmzHeadersString = sb.ToString();
                }
            }
            return canonicalizedAmzHeadersString;
        }

        private string CreateSignature(string secretKey, string stringToSign)
        {
            byte[] dataToSign = Encoding.UTF8.GetBytes(stringToSign);
            using (HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey)))
            {
                return Convert.ToBase64String(hmacsha1.ComputeHash(dataToSign));
            }
        }

        #endregion

        /// <summary>
        /// オブジェクトストレージに接続するための設定情報を初期化する。
        /// </summary>
        public void Initialize(StorageConfigModel storageConfig)
        {
            bucket = storageConfig.Bucket;
            secretKey = storageConfig.SecretKey;
            accessKey = storageConfig.AccessKey;
            client = GenerateConfig(storageConfig);
        }

        /// <summary>
        /// Configから接続クライアントインスタンスを生成する
        /// </summary>
        private AmazonS3Client GenerateConfig(StorageConfigModel storageConfig)
        {
            var amazonS3Config = new AmazonS3Config()
            {
                //アプリからの接続はすべてHTTPで繋いでいる
                ServiceURL = $"http://{storageConfig.StorageServer}/",
                UseHttp = true,
                ForcePathStyle = true,
                SignatureVersion = "4"
            };
            return new AmazonS3Client(storageConfig.AccessKey, storageConfig.SecretKey, amazonS3Config);
        }

        /// <summary>
        /// 新規にバケットを作成する。
        /// 接続情報は<see cref="Initialize(StorageConfigModel)"/>で指定したものではなく、引数で指定したものを使う。
        /// </summary>
        /// <returns>バケットが存在せず新規作成なら true、既にバケットが存在していたなら false を返却</returns>
        /// <exception cref="AmazonS3Exception">バケット生成の失敗</exception>
        public async Task<bool> CreateBucketAsync(StorageConfigModel storageConfig)
        {
            var createClient = GenerateConfig(storageConfig);

            bool isContainsBucket = await ContainsBucketAsync(createClient, storageConfig.Bucket);
            if (isContainsBucket)
            {
                // バケットの存在は確認できたので、直下にディレクトリが存在するか
                await CreateDirUnderBucketAsync(createClient, storageConfig.Bucket);

                // 既にバケットが存在しているので false を返却する
                LogError($"bucket already exists : {storageConfig.Bucket}");
                return false;
            }

            //バケットを作成
            try
            {
                await createClient.PutBucketAsync(storageConfig.Bucket);
                LogDebug($"created new bucket : {storageConfig.Bucket}");
            }
            catch (AmazonS3Exception e)
            {
                // 例外を投げる or 値を返す : 現状は例外を投げるので呼び出し側で catch すること
                LogError($"AmazonS3Client#PutBucketAsync() throws exception: {e.Message}");
                throw;
            }

            // ディレクトリとdummyファイルの作成
            await CreateDirAsync(storageConfig.Bucket, createClient, ResourceType.Data.ToString());
            await CreateDirAsync(storageConfig.Bucket, createClient, ResourceType.TrainingContainerOutputFiles.ToString());
            await CreateDirAsync(storageConfig.Bucket, createClient, ResourceType.TrainingContainerAttachedFiles.ToString());
            await CreateDirAsync(storageConfig.Bucket, createClient, ResourceType.TrainingHistoryAttachedFiles.ToString());
            await CreateDirAsync(storageConfig.Bucket, createClient, ResourceType.PreprocContainerAttachedFiles.ToString());
            await CreateDirAsync(storageConfig.Bucket, createClient, ResourceType.InferenceHistoryAttachedFiles.ToString());
            await CreateDirAsync(storageConfig.Bucket, createClient, ResourceType.InferenceContainerAttachedFiles.ToString());
            await CreateDirAsync(storageConfig.Bucket, createClient, ResourceType.InferenceContainerOutputFiles.ToString());
            await CreateDirAsync(storageConfig.Bucket, createClient, ResourceType.NotebookContainerAttachedFiles.ToString());
            await CreateDirAsync(storageConfig.Bucket, createClient, ResourceType.NotebookContainerOutputFiles.ToString());
            return true;
        }

        /// <summary>
        /// バケットが存在するなら true を返却する。
        /// </summary>
        private static async Task<bool> ContainsBucketAsync(AmazonS3Client aClient, String bucketName)
        {
            ListBucketsResponse responce = await aClient.ListBucketsAsync();
            return responce.Buckets.Find(b => b.BucketName == bucketName) != null;
        }

        /// <summary>
        /// バケットを削除する。
        /// 接続情報は<see cref="Initialize(StorageConfigModel)"/>で指定したものではなく、引数で指定したものを使う。
        /// </summary>
        /// <remarks>
        /// バケット内に空のサブフォルダがある場合、削除できずに例外が発生する。
        /// </remarks>
        public async Task<bool> DeleteBucketAsync(StorageConfigModel storageConfig)
        {
            var deleteClient = GenerateConfig(storageConfig);
            bool isContainsBucket = await ContainsBucketAsync(deleteClient, storageConfig.Bucket);
            if (!isContainsBucket)
            {
                // 指定するバケットは既に存在しない
                LogDebug($"bucket already deleted : {storageConfig.Bucket}");
                return false;
            }
            // 全てのファイルとバケットの削除
            LogDebug($"deleting bucket : {storageConfig.Bucket}");
            try
            {
                // バケットの削除は全てのオブジェクトを削除して実施
                await deleteObjectsAsync(deleteClient, storageConfig.Bucket, "");
                await deleteClient.DeleteBucketAsync(storageConfig.Bucket);
            }
            catch (AmazonS3Exception e)
            {
                LogWarning($"AmazonS3Client#DeleteBucketAsync() throws exception: {e.Message}");
                throw;
            }
            LogDebug($"deleted bucket : {storageConfig.Bucket}");
            return true;
        }

        /// <summary>
        /// prefix で指定するフォルダ階層以下のオブジェクトを全て削除する。トップなら "" を prefix として指定する。
        /// </summary>
        private async Task deleteObjectsAsync(AmazonS3Client aClient, string bucketName, string prefix)
        {
            ListObjectsRequest req = new ListObjectsRequest()
            {
                BucketName = bucketName,
                Prefix = prefix,
                Delimiter = "/"
            };
            ListObjectsResponse res = await aClient.ListObjectsAsync(req);
            // フォルダは再帰的に処理
            foreach (string dirName in res.CommonPrefixes)
            {
                await deleteObjectsAsync(aClient, bucketName, dirName);
            }
            // オブジェクト(ファイル)は削除
            foreach (S3Object file in res.S3Objects)
            {
                await aClient.DeleteObjectAsync(bucketName, file.Key);
            }
        }

        /// <summary>
        /// バケットにディレクトリを作成する
        /// </summary>
        /// <param name="bucketName">バケット名</param>
        /// <param name="client">使用するS3Client</param>
        /// <param name="dirName">ディレクトリ名</param>
        private async Task CreateDirAsync(string bucketName, AmazonS3Client client, string dirName)
        {
            // 末尾が/でないとディレクトリにならない
            var key = dirName.EndsWith("/") ? dirName : dirName + "/";
            var putObjectRequestDir = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = key,
                CannedACL = S3CannedACL.Private
            };
            await client.PutObjectAsync(putObjectRequestDir);

            // ディレクトリが空になるとディレクトリごと削除されるため、ダミーファイルを配置
            var putObjectRequestDummyFile = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = key + "dummyFile",
                CannedACL = S3CannedACL.Private
            };
            await client.PutObjectAsync(putObjectRequestDummyFile);

            LogInformation($"create dir: {key}");
        }

        /// <summary>
        /// バケット直下にディレクトリが存在しなければ、ディレクトリを作成する
        /// </summary>
        /// <param name="client">使用するS3Client</param>
        /// <param name="bucketName">バケット名</param>
        private async Task CreateDirUnderBucketAsync(AmazonS3Client client, string bucketName)
        {
            ListObjectsRequest request = new ListObjectsRequest
            {
                BucketName = bucketName,
                MaxKeys = 1000,
                Prefix = "",
                Delimiter = "/" // https://dev.classmethod.jp/cloud/aws/amazon-s3-folders/
            };

            try
            {
                LogDebug($"start querying objects under bucket");
                // ディレクトリ・ファイルの一覧を取得
                ListObjectsResponse response = await client.ListObjectsAsync(request);
                if (response.IsTruncated)
                {
                    LogWarning("too many output files(should be less than 1000). exceeded files are ignored.");
                }
                LogDebug($"storeage response : {response.ToString()}");

                // ディレクトリの存在チェックと作成
                await CheckAndCreateDir(bucketName, client, response.CommonPrefixes, ResourceType.Data.ToString());
                await CheckAndCreateDir(bucketName, client, response.CommonPrefixes, ResourceType.TrainingContainerOutputFiles.ToString());
                await CheckAndCreateDir(bucketName, client, response.CommonPrefixes, ResourceType.TrainingContainerAttachedFiles.ToString());
                await CheckAndCreateDir(bucketName, client, response.CommonPrefixes, ResourceType.TrainingHistoryAttachedFiles.ToString());
                await CheckAndCreateDir(bucketName, client, response.CommonPrefixes, ResourceType.PreprocContainerAttachedFiles.ToString());
                await CheckAndCreateDir(bucketName, client, response.CommonPrefixes, ResourceType.InferenceHistoryAttachedFiles.ToString());
                await CheckAndCreateDir(bucketName, client, response.CommonPrefixes, ResourceType.InferenceContainerAttachedFiles.ToString());
                await CheckAndCreateDir(bucketName, client, response.CommonPrefixes, ResourceType.InferenceContainerOutputFiles.ToString());
                await CheckAndCreateDir(bucketName, client, response.CommonPrefixes, ResourceType.NotebookContainerAttachedFiles.ToString());
                await CheckAndCreateDir(bucketName, client, response.CommonPrefixes, ResourceType.NotebookContainerOutputFiles.ToString());
            }
            catch (Exception e)
            {
                LogDebug($"CreateDirUnderBucketAsync error : {e.ToString()}");
            }
        }

        /// <summary>
        /// ディレクトリ一覧に対象ディレクトリが含まれないかチェックし、
        /// 含まれない場合、ディレクトリを作成する
        /// </summary>
        /// <param name="bucketName">バケット名</param>
        /// <param name="client">使用するS3Client</param>
        /// <param name="dirNameList">ディレクトリ一覧</param>
        /// <param name="dirName">対象ディレクトリ名</param>
        private async Task CheckAndCreateDir(string bucketName, AmazonS3Client client, List<string> dirNameList, string dirName)
        {
            // ディレクトリの存在チェック
            if (!dirNameList.Contains(dirName + "/"))
            {
                // ディレクトリとdummyファイルの作成
                await CreateDirAsync(bucketName, client, dirName);
            }
        }

        /// <summary>
        /// 指定した接続情報でCORS設定を取得する。
        /// </summary>
        public async Task<string> GetCorsConfigurationAsync(StorageConfigModel storageConfig)
        {
            GetCORSConfigurationRequest request = new GetCORSConfigurationRequest()
            {
                BucketName = storageConfig.Bucket
            };

            var s3Client = GenerateConfig(storageConfig);
            GetCORSConfigurationResponse response;
            try
            {
                response = await s3Client.GetCORSConfigurationAsync(request);
            }
            catch (HttpRequestException e)
            {
                LogError(e.Message, e);
                return "Unable to connect storage server. Please contact system administrators. NOTE: If you edit the tenant information, you may loose settings for the storage service." + Environment.NewLine + e.Message;
            }
            string result;
            if (response.Configuration == null)
            {
                result = "";
            }
            else
            {
                //結果を文字列に変換
                var serializer = new XmlSerializer(typeof(CORSConfiguration));
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, response.Configuration);
                    result = writer.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// 指定した接続情報でCORS設定を変更する。
        /// </summary>
        public async Task<bool> SetCorsConfigurationAsync(StorageConfigModel storageConfig, string corsConfigStr)
        {
            //CORS設定の更新

            PutCORSConfigurationRequest request = new PutCORSConfigurationRequest()
            {
                BucketName = storageConfig.Bucket
            };

            using (var reader = new StringReader(corsConfigStr))
            {
                var deserializer = new XmlSerializer(typeof(CORSConfiguration));
                var corsConfig = deserializer.Deserialize(reader) as CORSConfiguration;
                request.Configuration = corsConfig;
            }

            AmazonWebServiceResponse response;
            try
            {
                var s3Client = GenerateConfig(storageConfig);
                response = await s3Client.PutCORSConfigurationAsync(request);
            }
            catch (AmazonS3Exception e)
            {
                //XMLに不備があると、例外になってしまう。
                //接続エラー（System.Net.Http.HttpRequestException）との区別はつくので、他のAmazonS3Exceptionエラーとは区別できなくなるが、まとめてcatch
                LogWarning($"CORS設定に失敗しました。Message = {e.Message}");
                return false;
            }

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                LogWarning($"CORS設定に失敗しました。HttpStatusCode = {response.HttpStatusCode}. {response.ResponseMetadata.Metadata}");
                return false;
            }
        }

        /// <summary>
        /// 指定した接続情報でCORS設定を削除する。
        /// </summary>
        public async Task<bool> DeleteCorsConfigurationAsync(StorageConfigModel storageConfig)
        {
            //CORS設定の削除

            DeleteCORSConfigurationRequest request = new DeleteCORSConfigurationRequest()
            {
                BucketName = storageConfig.Bucket
            };
            var s3Client = GenerateConfig(storageConfig);
            AmazonWebServiceResponse response = await s3Client.DeleteCORSConfigurationAsync(request);

            //削除の際はNoContentが返ってくるのが正常
            if (response.HttpStatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                LogWarning($"CORS設定の削除に失敗しました。HttpStatusCode = {response.HttpStatusCode}. {response.ResponseMetadata.Metadata}");
                return false;
            }
        }

        /// <summary>
        /// オブジェクトストレージから、削除対象リストに登録された全てのデータを一括削除する。
        /// keysは1件以上、1000件以下であること。
        /// </summary>
        public async Task<bool> DeleteAsync(IEnumerable<string> keys)
        {
            var deleteObjectsRequest = new DeleteObjectsRequest() { BucketName = bucket };

            foreach (var key in keys)
            {
                deleteObjectsRequest.AddKey(key);
                LogInformation($"deleted file: {key}");
            }
            try
            {
                await client.DeleteObjectsAsync(deleteObjectsRequest);
            }
            catch (DeleteObjectsException ex)
            {
                LogError("failed to delete files", ex);
                DeleteObjectsResponse errorResponse = ex.Response;
                foreach (var item in errorResponse.DeletedObjects)
                {
                    LogInformation($"deleted file: {item.Key}");
                }
                LogWarning("Printing error data...");
                foreach (DeleteError deleteError in errorResponse.DeleteErrors)
                {
                    LogWarning("Object Key: {0}\t{1}\t{2}", deleteError.Key, deleteError.Code, deleteError.Message);
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// ディレクトリ直下のディレクトリ・オブジェクト一覧取得。
        /// minio NFSに書き込まれた学習結果を一覧で取得するために使用。
        /// </summary>
        /// <param name="searchDirPath"> 検索対象ディレクトリ </param>
        /// <returns>StorageListResultInfo (直下のディレクトリ一覧, 直下のオブジェクト一覧)  </returns>
        public async Task<Result<StorageListResultInfo, string>> GetUnderDirAsync(string searchDirPath)
        {

            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = bucket,
                MaxKeys = 1000,
                Prefix = searchDirPath,
                Delimiter = "/" // https://dev.classmethod.jp/cloud/aws/amazon-s3-folders/
            };

            try
            {
                List<StorageDirInfo> dirs = new List<StorageDirInfo>();
                List<StorageFileInfo> files = new List<StorageFileInfo>();
                bool exceeded = false;

                LogDebug($"start querying objects under bucket");

                const int MaxRequestCount = 10; // 1ディレクトリに含まれるファイル数の上限は、10*1,000=10,000件とする
                var requestCount = 0;
                ListObjectsV2Response response = new ListObjectsV2Response();
                do
                {
                    // ディレクトリ直下のディレクトリ・オブジェクト一覧を取得する
                    response = await client.ListObjectsV2Async(request);

                    if (response.IsTruncated)
                    {
                        LogWarning("more than 1000 output files exist. get continuously.");
                        exceeded = response.IsTruncated;
                    }
                    LogDebug($"storeage response : {response.ToString()}");

                    dirs.AddRange(response.CommonPrefixes.Select(x => new StorageDirInfo(x)).ToList());
                    files.AddRange(response.S3Objects.Select(x => new StorageFileInfo(x.Key, x.LastModified, x.Size)).ToList());
                    
                    // 次の一覧取得の開始位置であるオブジェクトを設定する
                    request.ContinuationToken = response.NextContinuationToken;
                    requestCount++;
                }
                while (response.IsTruncated && requestCount < MaxRequestCount); // 一覧に続きがある場合再度問い合わせる

                var result = new StorageListResultInfo(dirs, files, exceeded);

                return Result<StorageListResultInfo, string>.CreateResult(result);
            }
            catch (Exception e)
            {
                LogDebug($"GetUnderDirAsync error : {e.ToString()}");
                return Result<StorageListResultInfo, string>.CreateErrorResult(e.ToString());
            }
        }

        /// <summary>
        /// 指定されたデータファイルの署名付URI(GET)を返す。
        /// </summary>
        /// <param name="key">オブジェクトのキー</param>
        /// <param name="fileName">ダウンロード時に変換するファイル名</param>
        /// <param name="useHttp">通信でHTTPを使用するか</param>
        public Uri GetPreSignedUriForGet(string key, string fileName, bool useHttp)
        {
            return CreatePreSignedUri(key, HttpVerb.GET, useHttp, fileName);
        }

        /// <summary>
        /// 指定されたデータファイルの署名付URIを返す。
        /// </summary>
        /// <param name="key">オブジェクトのキー</param>
        /// <param name="verb">HTTPメソッド</param>
        /// <param name="useHttp">通信でHTTPを使用するか</param>
        /// <param name="fileName">ダウンロード時に変換するファイル名</param>
        /// <returns>署名付URI</returns>
        private Uri CreatePreSignedUri(string key, HttpVerb verb, bool useHttp, string fileName = null)
        {
            // 現状 v2 形式で発行している
            // v4 で発行するにはスタティックイニシャライザを有効にする必要がある
            // その場合、使えるメソッドは GET のみ。
            // その他のプロトコルに使おうとする場合、ペイロード（リクエストボディ）から計算されるSHA256値
            // を署名の計算に含める必要があるのでそもそも動かない（はず）
            var getPreSignedURLRequest = new GetPreSignedUrlRequest()
            {
                Protocol = Protocol.HTTP,
                //Protocol = useHttp ? Protocol.HTTP : Protocol.HTTPS,
                BucketName = bucket,
                Key = key,
                Verb = verb,
                Expires = DateTime.Now.AddSeconds(options.PreSignedUrlExpirationSec)
            };

            if (fileName != null)
            {
                var encodedFileName = Uri.EscapeDataString(fileName);
                // ファイル名変更
                // https://tech.innovator.jp.net/entry/s3-content-disposition#f-668451e2
                getPreSignedURLRequest.ResponseHeaderOverrides.ContentDisposition = "attachment;filename*=UTF-8''" + encodedFileName;
            }
            Uri result;
            var getPreSignedURLResponse = client.GetPreSignedURL(getPreSignedURLRequest);

            // useHttpに関わらずHTTPで接続するようにしている。
            result = new Uri(getPreSignedURLResponse);
            LogDebug("Create pre-signed url: " + result);
            return result;
        }

        /// <summary>
        /// 指定されたデータファイルの内容を返す。
        /// </summary>
        /// <param name="uri">データファイルのURI</param>
        async public Task<string> GetFileContentAsync(Uri uri)
        {
            var response = await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = uri.Scheme + "://" + uri.Authority,
                ApiPath = uri.PathAndQuery
            });
            if (response.IsSuccess)
            {
                return response.Value;
            }
            else
            {
                return null;
            }
        }
    }
}
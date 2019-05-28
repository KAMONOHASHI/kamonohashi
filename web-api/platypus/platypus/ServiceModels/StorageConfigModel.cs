using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels
{
    /// <summary>
    /// オブジェクトストレージにアクセスする際の設定情報をまとめたモデルクラス
    /// </summary>
    public class StorageConfigModel
    {
        /// <summary>
        /// オブジェクトストレージサーバのホスト名＆ポート番号。
        /// e.g. storage.kamonohashi.ai:7480
        /// </summary>
        public string StorageServer { get; set; }

        /// <summary>
        /// オブジェクトストレージのアクセスキー
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// オブジェクトストレージのシークレットキー
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// オブジェクトストレージのバケット
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// オブジェクトストレージのオブジェクトキー
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 署名サービスのURL
        /// </summary>
        public string SignerUrl { get; set; }
    }
}

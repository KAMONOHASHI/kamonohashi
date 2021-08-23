using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nssol.Platypus.Infrastructure.Options
{
    /// <summary>
    /// コンテナ管理に使用する情報を保持するクラス。
    /// コンテナ内で使用する情報もこの中で管理。
    /// </summary>
    public class ContainerManageOptions
    {
        /// <summary>
        /// コンテナ管理サービス(e.g. k8s)のベースURL
        /// </summary>
        public string ContainerServiceBaseUrl
        {
            get {
                    return $"https://{ KubernetesHostName }:{ KubernetesPort }";
            }
        }
        /// <summary>
        /// k8sのホスト名
        /// </summary>
        public string KubernetesHostName { get; set; }
        /// <summary>
        /// k8sのポート番号
        /// </summary>
        public string KubernetesPort {
            get {
                return Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_PORT");
            }
        }
        /// <summary>
        /// コンテナのホスト名ラベル
        /// </summary>
        public string ContainerLabelHostName { get; set; }
        /// <summary>
        /// コンテナのパーティション用ラベル
        /// </summary>
        public string ContainerLabelPartition { get; set; }
        /// <summary>
        /// コンテナのTensorBoard実行可否設定用ラベル
        /// </summary>
        public string ContainerLabelTensorBoardEnabled { get; set; }
        /// <summary>
        /// コンテナのNotebook実行可否設定用ラベル
        /// </summary>
        public string ContainerLabelNotebookEnabled { get; set; }
        /// <summary>
        /// クラスタのリソース管理サービス(e.g. k8s)のアクセスキー
        /// </summary>
        public string ResourceManageKey {
            get {
                // 開発環境は環境変数からtoken取得。本番はk8sの用意するファイルから読む
                string k8sApiKeyOfEnv = Environment.GetEnvironmentVariable("ContainerManageOptions__ResourceManageKey");
                if (string.IsNullOrEmpty(k8sApiKeyOfEnv))
                {
                    return File.ReadAllText(@"/var/run/secrets/kubernetes.io/serviceaccount/token");
                }
                else
                {
                    return k8sApiKeyOfEnv;
                }
            }
        }
        /// <summary>
        /// TensorBoardイメージ名
        /// </summary>
        public string TensorBoardImage { get; set; }

        /// <summary>
        /// Kubernetesシステム用のNamespaceプレフィックス
        /// </summary>
        public string KubernetesNamespacePrefix { get; set; }

        /// <summary>
        /// KQI管理外で無視すべきnamespaceのリスト
        /// ユーザーが指定した,区切りの値
        /// </summary>
        public string IgnoreNamespaces { get; set; }

        /// <summary>
        /// KQI管理外で無視すべきnamespaceのリスト
        /// </summary>
        public List<string> IgnoreNamespacesList { 
           get
            {
                return IgnoreNamespaces.Split(",").ToList();
            }
        }

        /// <summary>
        /// Kqiシステム用のNamespaceプレフィックス
        /// </summary>
        public string KqiNamespacePrefix { get; set; }

        /// <summary>
        /// Kqiシステム管理者用のNamespace
        /// </summary>
        public string KqiAdminNamespace { get; set; }

        /// <summary>
        /// データを取得するためのREST APIのベースURL。
        /// TODO: 互換性用。<see cref="WebEndPoint"/>か<see cref="WebServerUrl"/>を使用すること。
        /// </summary>
        [Obsolete]
        public string DataSetServerUrl { get; set; }

        /// <summary>
        /// 外部からWebアプリケーションにアクセスする際に使用されるホスト名(IPアドレス,FQDN)。
        /// コンテナからのREST APIアクセス先や、TensorBoardへのアクセス先に利用される。
        /// </summary>
        /// <remarks>
        /// 外から見たときのアクセスURLが中から確認した場合と異なりうるので、別途記載してもらう。
        /// </remarks>
        public string WebEndPoint { get; set; }

        /// <summary>
        /// プロキシ指定。なければ空とする
        /// </summary>
        public string Proxy
        {
            get {
                // linuxのプロキシ環境変数を用いる
                return Environment.GetEnvironmentVariable("http_proxy");
            }
        }

        /// <summary>
        /// プロキシ除外指定。なければ空とする
        /// </summary>
        public string NoProxy
        {
            get
            {
                // linuxのプロキシ環境変数を用いる
                return Environment.GetEnvironmentVariable("no_proxy");
            }
        }

        /// <summary>
        /// ターミナルの列数(1行の文字数)
        /// </summary>
        public string ShellColumns { get; set; }

        /// <summary>
        /// データを取得するためのREST APIのベースURL。
        /// </summary>
        public string WebServerUrl
        {
            get
            {
                if (string.IsNullOrEmpty(DataSetServerUrl))
                {
                    return "http://" + WebEndPoint;
                }
                return DataSetServerUrl;
            }
        }

    }
}

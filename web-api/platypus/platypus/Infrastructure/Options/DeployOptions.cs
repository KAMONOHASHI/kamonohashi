using Nssol.Platypus.Logic.HostedService;

namespace Nssol.Platypus.Infrastructure.Options
{
    public class DeployOptions : TimerScheduleOptionsBase
    {
        /// <summary>
        /// 開発・本番環境の指定を行います
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// DB接続・KQIの管理者アカウントに使用するパスワード
        /// 管理者アカウントには初回のみアプリ起動後のセットアップに使用
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// GPUノードの指定。初回のみアプリ起動後のセットアップに使用
        /// </summary>
        public string GpuNodes { get; set; }
        /// <summary>
        /// ストレージの指定。初回のみアプリ起動後のセットアップに使用
        /// </summary>
        public string ObjectStorageNode { get; set; }
        /// <summary>
        /// ストレージの指定。初回のみアプリ起動後のセットアップに使用
        /// </summary>
        public string ObjectStoragePort { get; set; }
        /// <summary>
        /// ストレージの指定。初回のみアプリ起動後のセットアップに使用
        /// </summary>
        public string ObjectStorageAccessKey { get; set; }
        /// <summary>
        /// ストレージの指定。初回のみアプリ起動後のセットアップに使用
        /// </summary>
        public string ObjectStorageSecretKey { get; set; }
        /// <summary>
        /// ストレージの指定。初回のみアプリ起動後のセットアップに使用
        /// </summary>
        public string NfsStorage { get; set; }
        /// <summary>
        /// ストレージの指定。初回のみアプリ起動後のセットアップに使用
        /// </summary>
        public string NfsPath { get; set; }
    }
}

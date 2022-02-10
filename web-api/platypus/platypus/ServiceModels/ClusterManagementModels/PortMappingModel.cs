namespace Nssol.Platypus.ServiceModels.ClusterManagementModels
{
    /// <summary>
    /// ポートマッピング情報
    /// </summary>
    /// <remarks>
    /// 本来的にはサービス層の特定仕様に依存すべきではないんだけど、今はこのクラスのメンバをそのままyaml内で流用してしまっている。
    /// なので、プロパティ名が変わると動かなくなってしまう。注意。
    /// </remarks>
    public class PortMappingModel
    {
        /// <summary>
        /// 識別名。
        /// 一回のコンテナ設定内で一意である必要がある。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// プロトコル
        /// </summary>
        public string Protocol { get; set; }
        /// <summary>
        /// コンテナでのポート
        /// </summary>
        public int TargetPort { get; set; }
        /// <summary>
        /// ホストでのポート
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// ノードに直接アクセスする際のアクセス先ポート。
        /// 未指定の場合は動的に決定される。
        /// </summary>
        public int NodePort { get; set; }
    }
}

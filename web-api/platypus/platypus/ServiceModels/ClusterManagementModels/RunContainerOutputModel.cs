using Nssol.Platypus.Infrastructure;
using System.Collections.Generic;

namespace Nssol.Platypus.ServiceModels.ClusterManagementModels
{
    /// <summary>
    /// クラスタ管理サービスで新規にコンテナを起動するための出力モデルクラス
    /// </summary>
    public class RunContainerOutputModel
    {
        /// <summary>
        /// 作成したコンテナの識別名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// コンテナのステータス
        /// </summary>
        public ContainerStatus Status { get; set; }
        /// <summary>
        /// コンテナが立ったホスト
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// コンテナの設定値詳細
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// ホストのポートとコンテナのポートのマッピング情報。
        /// </summary>
        public List<PortMappingModel> PortMappings { get; set; }
    }
}

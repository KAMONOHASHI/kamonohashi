using Nssol.Platypus.ServiceModels.ClusterManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure.Infos
{
    /// <summary>
    /// 複数サービスコンテナの詳細情報
    /// 単一サービスコンテナ（エンドポイントが複数あるコンテナ）は<see cref="Infos.ContainerEndpointInfo"/>を使う。
    /// </summary>
    public class ContainerEndpointInfo
    {
        /// <summary>
        /// コンテナ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// コンテナのステータス
        /// </summary>
        public ContainerStatus Status { get; set; }

        /// <summary>
        /// コンテナの状況を表すメッセージ。異常系の場合に参照される。
        /// </summary>
        public string ConditionNote { get; set; }

        /// <summary>
        /// エンドポイント
        /// </summary>
        public IEnumerable<EndPointInfo> EndPoints { get; set; }

        /// <summary>
        /// 開始時刻
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// コンテナが実行されているノード名
        /// </summary>
        public string Node { get; set; }
    }
}

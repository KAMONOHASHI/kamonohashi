using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure.Infos
{
    /// <summary>
    /// 単一サービスコンテナの詳細情報
    /// 複数サービスコンテナ（エンドポイントが複数あるコンテナ）は<see cref="Infos.ContainerEndpointInfo"/>を使う。
    /// </summary>
    public class ContainerInfo
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
        /// コンテナへのアクセス先ホスト
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// コンテナへのアクセス先ポート。
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// コンテナの設定詳細。
        /// ログとして残すだけで、ユーザに見せることは想定しない。
        /// </summary>
        public string Configuration { get; set; }
    }
}

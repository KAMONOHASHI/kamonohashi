using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels.ClusterManagementModels;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services.Interfaces
{
    /// <summary>
    /// コンテナ管理サービスの管理クラスのインターフェース
    /// </summary>
    /// <remarks>
    /// コンテナ管理とリソース管理でクラスは分けない。
    /// 基本的には同じサービスを使う（違っても入り混じる）想定のため、参照する際にどっちのクラスを見ればいいか迷うくらいなら、まとめてしまう。
    /// </remarks>
    public interface IClusterManagementService
    {
        #region コンテナ管理
        /// <summary>
        /// 指定したテナントに、新規にコンテナを作成する。
        /// 失敗した場合はnullが返る。
        /// </summary>
        Task<Result<RunContainerOutputModel, string>> RunContainerAsync(RunContainerInputModel inModel, string baseUrl);

        /// <summary>
        /// 全コンテナ情報を取得する
        /// </summary>
        Task<Result<IEnumerable<ContainerDetailsInfo>, ContainerStatus>> GetAllContainerDetailsInfosAsync(string token, string tenantName = null);

        /// <summary>
        /// 指定したコンテナのステータスを取得する。
        /// </summary>
        Task<ContainerStatus> GetContainerStatusAsync(string containerName, string tenantName, string token);

        /// <summary>
        /// 指定したコンテナの詳細情報を取得する。
        /// </summary>
        Task<ContainerDetailsInfo> GetContainerDetailsInfoAsync(string jobName, string tenantName, string token);

        /// <summary>
        /// 指定したコンテナの情報をエンドポイント付きで取得する。
        /// </summary>
        Task<ContainerEndpointInfo> GetContainerEndpointInfoAsync(string containerName, string tenantName, string token);

        /// <summary>
        /// コンテナを削除する。
        /// 対象コンテナが存在しない場合はエラーになる。
        /// </summary>
        Task<bool> DeleteContainerAsync(ContainerType type, string containerName, string tenantName, string token);

        /// <summary>
        /// 指定したコンテナのログを取得する
        /// </summary>
        Task<Result<System.IO.Stream, ContainerStatus>> DownloadLogAsync(string containerName, string tenantName, string token);

        /// <summary>
        /// 指定したテナントのイベントを取得する
        /// </summary>
        Task<Result<IEnumerable<ContainerEventInfo>, ContainerStatus>> GetEventsAsync(Tenant tenant, string token);

        /// <summary>
        /// 指定したテナントの appName に対応する Pod 名を取得する
        /// </summary>
        Task<Result<string, ContainerStatus>> GetPodNameAsync(string tenantName, string appName, int limit, string token);

        /// <summary>
        /// 指定したテナントの Pod 上でコマンドを実行する。
        /// 実行コンテナ名と、コマンド実行終了を確認する最大回数 maxLoopCount やその間隔 intervalMillisec も指定する。
        /// </summary>
        Task<bool> ExecBashCommandAsync(string tenantName, string podName, string command, string container, string token, int intervalMillisec, int maxLoopCount);
        #endregion

        #region クラスタ管理
        /// <summary>
        /// ノード単位で、指定したキーのラべルリストを取得する
        /// 当該ラベルがないノードは結果に含まれない。
        /// </summary>
        Task<Result<Dictionary<string, string>, string>> GetNodeLabelMapAsync(string labelKey, List<string> registeredNodeNames);


        /// <summary>
        /// 全ノード情報を取得する。
        /// 取得失敗した場合はnullが返る。
        /// </summary>
        Task<IEnumerable<NodeInfo>> GetAllNodesAsync(List<string> registeredNodeNames);

        ///// <summary>
        ///// 管理イベントを取得する
        ///// </summary>
        //void GetEvents();

        /// <summary>
        /// 指定されたノードラベルを設定する。
        /// 既にラベルが存在すれば上書きし、存在しない場合は作成する。
        /// 空文字列を指定した場合は、削除する。
        /// </summary>
        /// <param name="nodeName">対象ノード名</param>
        /// <param name="label">ラベル名</param>
        /// <param name="value">設定値</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        Task<bool> SetNodeLabelAsync(string nodeName, string label, string value);

        /// <summary>
        /// 指定されたテナントにクォータを設定する。
        /// 既にクォータ設定が存在すれば上書きし、存在しない場合は作成する。
        /// 0が指定された場合、無制限となる。
        /// </summary>
        /// <param name="cpu">CPUコア数</param>
        /// <param name="memory">メモリ容量（GB）</param>
        /// <param name="gpu">GPU数</param>
        /// <param name="tenantName">テナント名</param>
        Task<bool> SetQuotaAsync(string tenantName, int cpu, int memory, int gpu);
        #endregion

        #region 権限管理

        /// <summary>
        /// クラスタ管理サービスにコンテナレジストリを登録する。
        /// idempotentを担保。
        /// </summary>
        Task<bool> RegistRegistryTokenyAsync(RegistRegistryTokenInputModel model);


        /// <summary>
        /// クラスタ管理サービスに新規テナントを登録する。
        /// </summary>
        Task<bool> RegistTenantAsync(string tenantName);

        /// <summary>
        /// クラスタ管理サービスに新規ユーザを追加する。
        /// 既に追加済みの場合、その認証トークンを取得する。
        /// 取得に失敗した場合、nullが返る。
        /// </summary>
        /// <remarks>
        /// 名前空間とロールは作成済みの前提。存在確認は行わない。
        /// </remarks>
        Task<string> RegistUserAsync(string tenantName, string userName, KubernetesEndpointModel kubernetes = null);

        /// <summary>
        /// クラスタ管理サービスよりテナントを抹消(削除)する。
        /// </summary>
        Task<bool> EraseTenantAsync(string tenantName);

        #endregion

        #region WebSocket通信
        /// <summary>
        /// kubernetesとのwebsocket接続を確立
        /// </summary>
        Task<Result<ClientWebSocket, ContainerStatus>> ConnectWebSocketAsync(string jobName, string tenantName, string token);
        #endregion
    }
}

using Microsoft.AspNetCore.Http;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// クラスタ管理・コンテナ管理を行うロジックのインターフェース
    /// </summary>

    public interface IClusterManagementLogic
    {
        #region コンテナ管理

        /// <summary>
        /// 新規に前処理用コンテナを作成する。
        /// </summary>
        /// <param name="preprocessHistory">対象の前処理履歴</param>
        /// <returns>作成したコンテナのステータス</returns>
        Task<Result<ContainerInfo, string>> RunPreprocessingContainerAsync(PreprocessHistory preprocessHistory);

        /// <summary>
        /// 新規にTensorBoard表示用のコンテナを作成する。
        /// </summary>
        /// <param name="trainingHistory">対象の学習履歴</param>
        /// <param name="expiresIn">生存期間(秒)</param>
        /// <returns>作成したコンテナのステータス</returns>
        Task<ContainerInfo> RunTensorBoardContainerAsync(TrainingHistory trainingHistory, int expiresIn);

        /// <summary>
        /// 新規に画像認識の訓練用コンテナを作成する。
        /// </summary>
        /// <param name="trainHistory">対象の学習履歴</param>
        /// <returns>作成したコンテナのステータス</returns>
        Task<Result<ContainerInfo, string>> RunTrainContainerAsync(TrainingHistory trainHistory);

        /// <summary>
        /// 新規に画像認識の推論用コンテナを作成する。
        /// </summary>
        /// <param name="inferenceHistory">対象の推論履歴</param>
        /// <returns>作成したコンテナのステータス</returns>
        Task<Result<ContainerInfo, string>> RunInferenceContainerAsync(InferenceHistory inferenceHistory);

        /// <summary>
        /// 新規にノートブック用コンテナを作成する。
        /// </summary>
        /// <param name="notebookHistory">対象のノートブック履歴</param>
        /// <returns>作成したコンテナのステータス</returns>
        Task<Result<ContainerInfo, string>> RunNotebookContainerAsync(NotebookHistory notebookHistory);

        /// <summary>
        /// 新規にバケット(テナントデータ)削除用のコンテナを作成する。
        /// </summary>
        /// <param name="tenant">対象のテナント</param>
        /// <returns>作成したコンテナのステータス</returns>
        Task<ContainerInfo> RunDeletingTenantDataContainerAsync(Tenant tenant);

        /// <summary>
        /// 全コンテナの情報を取得する
        /// </summary>
        Task<Result<IEnumerable<ContainerDetailsInfo>, ContainerStatus>> GetAllContainerDetailsInfosAsync();

        /// <summary>
        /// 特定のテナントに紐づいた全コンテナの情報を取得する
        /// </summary>
        Task<Result<IEnumerable<ContainerDetailsInfo>, ContainerStatus>> GetAllContainerDetailsInfosAsync(string tenantName);

        /// <summary>
        /// 指定したコンテナのエンドポイント付きの情報をクラスタ管理サービスに問い合わせる。
        /// </summary>
        Task<ContainerEndpointInfo> GetContainerEndpointInfoAsync(string containerName, string tenantName, bool force);

        /// <summary>
        /// 指定したコンテナの詳細情報をクラスタ管理サービスに問い合わせる。
        /// </summary>
        Task<ContainerDetailsInfo> GetContainerDetailsInfoAsync(string containerName, string tenantName, bool force);

        /// <summary>
        /// 指定したコンテナのステータスをクラスタ管理サービスに問い合わせる。
        /// </summary>
        Task<ContainerStatus> GetContainerStatusAsync(string containerName, string tenantName, bool force);

        /// <summary>
        /// 指定したTensorBoardコンテナのステータスをクラスタ管理サービスに問い合わせ、結果でDBを更新する。
        /// </summary>
        Task<ContainerStatus> SyncContainerStatusAsync(TensorBoardContainer container, bool force);

        /// <summary>
        /// 指定したコンテナを削除する。
        /// 対象コンテナが存在しない場合はエラーになる。
        /// </summary>
        Task<bool> DeleteContainerAsync(ContainerType type, string containerName, string tenantName, bool force);

        /// <summary>
        /// 指定したコンテナのログを取得する
        /// </summary>
        Task<Result<System.IO.Stream, ContainerStatus>> DownloadLogAsync(string containerName, string tenantName, bool force);

        /// <summary>
        /// 指定したテナントのイベントを取得する
        /// </summary>
        Task<Result<IEnumerable<ContainerEventInfo>, ContainerStatus>> GetEventsAsync(Tenant tenant, bool force);

        /// <summary>
        /// 指定したコンテナのイベントを取得する
        /// </summary>
        Task<Result<IEnumerable<ContainerEventInfo>, ContainerStatus>> GetEventsAsync(Tenant tenant, string containerName, bool force, bool errorOnly);
        #endregion

        #region クラスタ管理

        /// <summary>
        /// 全ノード情報を取得する。
        /// 取得失敗した場合はnullが返る。
        /// </summary>
        Task<IEnumerable<NodeInfo>> GetAllNodesAsync();

        ///// <summary>
        ///// 現在のアクセスユーザが利用可能なノード一覧を取得する
        ///// </summary>
        //void GetAccessibleNods();

        ///// <summary>
        ///// 指定したテナントに紐づく管理イベントを取得する
        ///// </summary>
        //void GetEvents(string tenantName);

        /// <summary>
        /// ノード単位のパーティションリストを取得する
        /// </summary>
        Task<Result<Dictionary<string, string>, string>> GetNodePartitionMapAsync();

        /// <summary>
        /// パーティションを更新する
        /// </summary>
        /// <param name="nodeName">ノード名</param>
        /// <param name="labelValue">ノード値</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        Task<bool> UpdatePartitionLabelAsync(string nodeName, string labelValue);

        /// <summary>
        /// TensorBoardの実行可否設定を更新する
        /// </summary>
        /// <param name="nodeName">ノード名</param>
        /// <param name="enabled">実行可否</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        Task<bool> UpdateTensorBoardEnabledLabelAsync(string nodeName, bool enabled);

        /// <summary>
        /// Notebookの実行可否設定を更新する
        /// </summary>
        /// <param name="nodeName">ノード名</param>
        /// <param name="enabled">実行可否</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        Task<bool> UpdateNotebookEnabledLabelAsync(string nodeName, bool enabled);

        /// <summary>
        /// テナントの実行可否設定を更新する
        /// </summary>
        /// <param name="nodeName">ノード名</param>
        /// <param name="tenantName">テナント名</param>
        /// <param name="enabled">実行可否</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        Task<bool> UpdateTenantEnabledLabelAsync(string nodeName, string tenantName, bool enabled);


        /// <summary>
        /// 指定されたテナントのクォータ設定をクラスタに反映させる。
        /// </summary>
        /// <returns>更新結果、更新できた場合、true</returns>
        Task<bool> SetQuotaAsync(Tenant tenant);
        #endregion

        #region 権限管理

        /// <summary>
        /// クラスタ管理サービス上で、指定したユーザ＆テナントにコンテナレジストリを登録する。
        /// idempotentを担保。
        /// </summary>
        Task<bool> RegistRegistryToTenantAsync(string selectedTenantName, UserTenantRegistryMap userRegistryMap);

        /// <summary>
        /// 指定したテナントを作成する。
        /// 既にある場合は何もしない。
        /// </summary>
        Task<bool> RegistTenantAsync(string tenantName);

        /// <summary>
        /// ログイン中のユーザ＆テナントに対する、クラスタ管理サービスにアクセスするためのトークンを取得する。
        /// 存在しない場合、新規に作成する。
        /// </summary>
        Task<string> GetUserAccessTokenAsync();

        /// <summary>
        /// 指定したテナントを抹消(削除)する。
        /// </summary>
        Task<bool> EraseTenantAsync(string tenantName);

        #endregion

        #region WebSocket通信
        /// <summary>
        /// ブラウザとのWebSocket接続および、KubernetesとのWebSocket接続を確立する。
        /// そしてブラウザからのメッセージを待機し、メッセージを受信した際にはその内容をKubernetesに送信する。
        /// </summary>
        Task ConnectKubernetesWebSocketAsync(HttpContext context);
        #endregion
    }
}

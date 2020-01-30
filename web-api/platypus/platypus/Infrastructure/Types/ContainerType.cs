namespace Nssol.Platypus.Infrastructure.Types
{
    /// <summary>
    /// コンテナ種別
    /// </summary>
    public enum ContainerType
    {
        /// <summary>
        /// DBに存在しないコンテナ
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 学習実行用
        /// </summary>
        Training = 1,
        /// <summary>
        /// TensorBoard表示用
        /// </summary>
        TensorBoard = 2,
        /// <summary>
        /// 前処理用
        /// </summary>
        Preprocessing = 3,
        /// <summary>
        /// 推論実行用
        /// </summary>
        Inferencing = 4,
        /// <summary>
        /// ノートブック実行用
        /// </summary>
        Notebook = 5,
        /// <summary>
        /// テナント削除実行用
        /// </summary>
        DeleteTenant = 6
    }
}

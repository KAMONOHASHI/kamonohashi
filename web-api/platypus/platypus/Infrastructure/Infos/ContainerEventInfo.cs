namespace Nssol.Platypus.Infrastructure.Infos
{
    public class ContainerEventInfo
    {
        /// <summary>
        /// テナントID
        /// </summary>
        public long TenantId { get; set; }
        /// <summary>
        /// テナント名
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// コンテナ名
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 理由
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// エラーか否か
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// 初回登録日時
        /// </summary>
        public string FirstTimestamp { get; set; }

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public string LastTimestamp { get; set; }
    }
}

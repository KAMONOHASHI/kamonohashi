using System.ComponentModel.DataAnnotations;


namespace Nssol.Platypus.Models
{
    /// <summary>
    /// EKSの登録情報
    /// </summary>
    public class Eks : ModelBase
    {
        /// <summary>
        /// 識別名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// ホスト名
        /// </summary>
        [Required]
        public string HostName { get; set; }

        /// <summary>
        /// 待ち状態のコンテナ数がいくつ以上の場合にEKSを利用するか
        /// </summary>
        public long UsablePendingJobCounts { get; set; }

        /// <summary>
        /// ポート番号
        /// </summary>
        [Required]
        public string PortNumber { get; set; }

        /// <summary>
        /// API key (EKSの認証キー)
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}

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
        /// ポート番号
        /// </summary>
        [Required]
        public int PortNumber { get; set; }

        /// <summary>
        /// API key (k8sの認証キー)
        /// </summary>
        [Required]
        public string ApiKey { get; set; }
    }
}

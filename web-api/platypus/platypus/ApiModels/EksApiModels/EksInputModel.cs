using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace Nssol.Platypus.ApiModels.EksApiModels
{
    /// <summary>
    /// EKSの情報登録モデル
    /// </summary>
    public class EksInputModel
    {
        /// <summary>
        /// 登録名 (システム上一意であることが必要)
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        /// <summary>
        /// メモ (EKSの情報等)
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// EKSへのアクセストークン
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Token { get; set; }

        /// <summary>
        /// EKSのAPIエンドポイント
        /// </summary>
        [Required]
        [MinLength(1)]
        public string HostName { get; set; }

        /// <summary>
        /// EKSのAPIエンドポイントのポート番号
        /// 空欄で入力された場合は、登録時に80番ポートで補完する
        /// </summary>
        public long? PortNumber { get; set; }

        /// <summary>
        /// 利用可能テナントのID
        /// </summary>
        public IEnumerable<long> AnableTenantsId { get; set; }
    }
}
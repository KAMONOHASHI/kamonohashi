using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// ユーザー, テナント, EKSのマッピングを行うクラス
    /// サービスアカウントのトークンをDBに記録するために使用する
    /// </summary>
    public class UserTenantEksMap: ModelBase
    {
        /// <summary>
        /// ユーザーのID
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// テナントとEKSのマッピングのID
        /// </summary>
        [Required]
        public long TenantEksMapId { get; set; }

        /// <summary>
        /// サービスアカウントのトークン
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// ユーザー
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// テナントとEKSのマッピング
        /// </summary>
        [ForeignKey(nameof(TenantEksMapId))]
        public virtual TenantEksMap TenantEksMap { get; set; }
    }
}

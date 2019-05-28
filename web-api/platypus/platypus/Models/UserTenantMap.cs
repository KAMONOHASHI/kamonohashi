using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// ユーザーとテナントをマッピングするテーブル
    /// </summary>
    public class UserTenantMap : ModelBase
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// テナントID
        /// </summary>
        [Required]
        public long TenantId { get; set; }

        /// <summary>
        /// クラスタトークン。
        /// ユーザごとにトークンを付けることで、クラスタ管理システム側でアクセスユーザを区別できるようにする。
        /// </summary>
        public string ClusterToken { get; set; }

        /// <summary>
        /// ユーザー
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// テナント
        /// </summary>
        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }
    }
}

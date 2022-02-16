using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        /// <summary>
        /// 元々KQI上で紐づけあったか。
        /// </summary>
        /// <remarks>
        /// true : KQI上での紐づけあり。
        /// false: KQI上での紐づけなし。
        /// </remarks>
        [Required]
        public bool IsOrigin { get; set; }

        /// <summary>
        /// ユーザグループテナントマップID
        /// </summary>
        /// <remarks>
        /// どのユーザグループテナントマップに該当するのかをカンマ区切りで列挙する。
        /// </remarks>
        public string UserGroupTenantMapIds { get; set; }

        /// <summary>
        /// ユーザグループテナントマップID(リスト形式)
        /// </summary>
        /// <remarks>
        /// どのユーザグループテナントマップに該当するのかをリストで保持する。
        /// </remarks>
        [NotMapped]
        public List<long> UserGroupTenantMapIdList { get { return GetUserGroupTenantMapIdList(); } }

        /// <summary>
        /// <see cref="UserGroupTenantMapIds"/>をリスト形式で取得する。
        /// </summary>
        public List<long> GetUserGroupTenantMapIdList()
        {
            if (UserGroupTenantMapIds == null)
            {
                return new List<long>();
            }
            return JsonConvert.DeserializeObject<List<long>>(UserGroupTenantMapIds);
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// ユーザとロールの紐づけ
    /// </summary>
    public class UserRoleMap : ModelBase
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// ロールID
        /// </summary>
        [Required]
        public long RoleId { get; set; }

        /// <summary>
        /// UserTenantMap ID。
        /// <see cref="Role"/>がシステムロールの場合、この値はNULLになって、テナントに関係なくそのロールが必ず付与される。
        /// <see cref="Role"/>がテナントロールの場合、この値は非NULLになる。
        /// その場合、<see cref="UserId"/>と<see cref="TenantMap"/>のUserIdは一致している必要があり、
        /// <see cref="Role"/>のテナントIDが非NULLなら<see cref="TenantMap"/>のテナントIDと一致している必要がある。
        /// </summary>
        public long? TenantMapId { get; set; }

        /// <summary>
        /// ユーザ
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// ロール
        /// </summary>
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }

        /// <summary>
        /// テナントとのマッピング情報
        /// </summary>
        [ForeignKey(nameof(TenantMapId))]
        public virtual UserTenantMap TenantMap { get; set; }

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
        public List<long> UserGroupTenantMapIdList { get; set; }

        /// <summary>
        /// <see cref="UserGroupTenantMapIds"/>をリスト形式で取得する。
        /// </summary>
        public List<long> GetUserGroupTenantMapIdList()
        {
            if (UserGroupTenantMapIds == null)
            {
                return new List<long>();
            }
            UserGroupTenantMapIdList = JsonConvert.DeserializeObject<List<long>>(UserGroupTenantMapIds);
            return UserGroupTenantMapIdList;
        }

        /// <summary>
        /// リスト形式から文字列に変換して<see cref="UserGroupTenantMapIds"/>にセットする。
        /// </summary>
        public void SetUserGroupTenantMapIdList(List<long> list)
        {
            UserGroupTenantMapIds = JsonConvert.SerializeObject(list);
        }
    }
}

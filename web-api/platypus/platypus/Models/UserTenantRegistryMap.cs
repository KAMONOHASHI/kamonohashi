using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    public class UserTenantRegistryMap : ModelBase
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// TenantRegistryMap Id
        /// </summary>
        [Required]
        public long TenantRegistryMapId { get; set; }

        /// <summary>
        /// 接続用ユーザ。
        /// GitLabの場合、ここは無視される。
        /// </summary>
        public string RegistryUserName { get; set; }
        /// <summary>
        /// 接続用パスワード。
        /// GitLabの場合、ここにトークンを入れる
        /// </summary>
        public string RegistryPassword { get; set; }

        /// <summary>
        /// ユーザ
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// テナント・レジストリとの紐づけ情報
        /// </summary>
        [ForeignKey(nameof(TenantRegistryMapId))]
        public virtual TenantRegistryMap TenantRegistryMap { get; set; }

        /// <summary>
        /// レジストリ
        /// </summary>
        public Registry Registry
        {
            get
            {
                return TenantRegistryMap?.Registry;
            }
        }

        /// <summary>
        /// クラスタ管理サービスに登録される、このMap用の認証トークン名。
        /// 認証情報が未設定の場合はnullになる。
        /// </summary>
        public string RegistryTokenKey
        {
            get
            {
                if (string.IsNullOrEmpty(RegistryPassword))
                {
                    return null;
                }
                return $"registry-{Registry.Name}-{UserId}";
            }
        }

        /// <summary>
        /// 元々KQI上で紐づけあったか。
        /// true : KQI上での紐づけあり。
        /// false: KQI上での紐づけなし。
        /// </summary>
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

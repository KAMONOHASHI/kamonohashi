using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
    }
}

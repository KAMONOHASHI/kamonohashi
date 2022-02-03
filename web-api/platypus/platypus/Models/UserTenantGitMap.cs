﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    public class UserTenantGitMap : ModelBase
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// TenantGitMap Id
        /// </summary>
        [Required]
        public long TenantGitMapId { get; set; }

        /// <summary>
        /// API実行用の認証トークン
        /// </summary>
        public string GitToken { get; set; }

        /// <summary>
        /// ユーザ
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// テナント・Gitとの紐づけ情報
        /// </summary>
        [ForeignKey(nameof(TenantGitMapId))]
        public virtual TenantGitMap TenantGitMap { get; set; }

        /// <summary>
        /// レジストリ
        /// </summary>
        public Git Git
        {
            get
            {
                return TenantGitMap?.Git;
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

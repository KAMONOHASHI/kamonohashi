using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// 現在ログイン中のユーザ情報
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 表示名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ユーザコードとなるユーザの別名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 認証サービス種別
        /// </summary>
        public AuthServiceType ServiceType { get; set; }

        /// <summary>
        /// 現在選択中のテナント情報
        /// </summary>
        public Tenant SelectedTenant { get; private set; }

        /// <summary>
        /// 所属しているシステムロール
        /// </summary>
        public IEnumerable<Role> SystemRoles { get; set; }

        /// <summary>
        /// 現在選択中のテナントのロール情報
        /// </summary>
        public List<Role> SelectedTenantRoles
        {
            get
            {
                return TenantDic[SelectedTenant];
            }
        }

        /// <summary>
        /// リクエストID
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// カルチャ
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// 言語設定
        /// </summary>
        public string Locale
        {
            get
            {
                if (Culture == null)
                {
                    return "en";
                }
                return Culture.Name.Substring(0, 2);
            }
        }

        /// <summary>
        /// 既定で選択されるテナント
        /// </summary>
        public virtual Tenant DefaultTenant { get; set; }

        /// <summary>
        /// 所属するテナントと、そのテナントでのロール。
        /// </summary>
        public Dictionary<Tenant, List<Role>> TenantDic { get; set; }

        /// <summary>
        /// 指定したテナントIDのテナントを選択させる
        /// </summary>
        public void SelectTenant(long? tenantId)
        {
            if (tenantId == null)
            {
                // テナントが指定されていない場合にはデフォルトテナント、それすらない場合は一番上にあるテナントを取得する
                if (DefaultTenant == null)
                {
                    SelectedTenant = TenantDic.Keys.FirstOrDefault();
                }
                else
                {
                    SelectedTenant = DefaultTenant;
                }
            }
            else
            {
                var tenant = TenantDic.Keys.FirstOrDefault(t => t.Id == tenantId);
                if (tenant == null)
                {
                    //テナントIDが明示的に指定されていて、そのテナントへのアクセス権がない（＝トークンを取った後でテナントを外された）
                    throw new UnauthorizedAccessException("You have lost a role for the access.");
                }
                SelectedTenant = tenant;
            }
        }

        /// <summary>
        /// Slackの送信先URL
        /// </summary>
        public string SlackUrl { get; set; }

        /// <summary>
        /// Slackメッセージのメンション
        /// </summary>
        public string Mention { get; set; }
    }
}

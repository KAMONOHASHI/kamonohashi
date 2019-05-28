using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class AccountOutputModel
    {
        /// <summary>
        /// ログインユーザID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// ログインユーザ名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// パスワード変更が可能か
        /// </summary>
        public bool PasswordChangeEnabled { get; set; }

        /// <summary>
        /// 選択中のテナント
        /// </summary>
        public TenantInfo SelectedTenant { get; set; }

        /// <summary>
        /// デフォルトテナント
        /// </summary>
        public TenantInfo DefaultTenant { get; set; }

        /// <summary>
        /// テナント名と表示名のDictionary
        /// </summary>
        public List<TenantInfo> Tenants { get; set; }
    }
}

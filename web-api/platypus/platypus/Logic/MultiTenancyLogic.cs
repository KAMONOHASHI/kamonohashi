using Microsoft.AspNetCore.Http;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// ログインユーザの認証・認可情報を
    /// 同一セッションの内で、クラス間のデータ授受やDIを仲介するための特殊ロジッククラス。
    /// Controller/Feature/Logic/Service全てのクラスがこのクラスをDIする。
    /// 
    /// DataAccessだけはこのクラスがDIするので、DIが循環しないように、参照されてはならない。
    /// また、<see cref="PlatypusLogicBase"/>も内部でこのクラスをDIしているので、継承不可。
    /// </summary>
    /// <seealso cref="Nssol.Platypus.Logic.Interfaces.IMultiTenancyLogic" />
    public class MultiTenancyLogic : IMultiTenancyLogic
    {
        private ITenantRepository tenantRepository;
        private IUserRepository userRepository;
        private IHttpContextAccessor accessor;

        private UserInfo currentUserInfo;

        /// <summary>
        /// ログイン中のユーザ情報
        /// </summary>
        public UserInfo CurrentUserInfo { get {
                if(currentUserInfo == null)
                {
                    SetUserInfo();
                }
                return currentUserInfo;
            } }

        /// <summary>
        /// 現在選択中のテナントIDを取得します。
        /// </summary>
        public long TenantId
        {
            get
            {
                if(CurrentUserInfo?.SelectedTenant == null)
                {
                    //テナントに所属していないユーザがテナント用メニューを使った場合に到達。不到達コード。
                    throw new InvalidOperationException("テナント未所属！");
                }
                return CurrentUserInfo.SelectedTenant.Id;
            }
        }

        /// <summary>
        /// 現在選択中のテナント名をDBから取得します。
        /// 取得できなかった場合、空文字列を返します。
        /// </summary>
        public string TenantName
        {
            get
            {
                return CurrentUserInfo?.SelectedTenant?.DisplayName;
            }
        }

        /// <summary>
        /// 現在選択中のテナント情報を取得します。
        /// 取得できなかった場合、NULLを返します。
        /// </summary>
        public Tenant Tenant
        {
            get
            {
                return CurrentUserInfo?.SelectedTenant;
            }
        }

        /// <summary>
        /// ユーザ名を取得します。
        /// 未ログインの場合は ハイフン(-) を返します。 
        /// </summary>
        public string UserName
        {
        get
            {
                return CurrentUserInfo?.Name ?? "-";
            }
        }

        /// <summary>
        /// リクエストID
        /// </summary>
        public string RequestId
        {
            get
            {
                return CurrentUserInfo?.RequestId;
            }
        }

        /// <summary>
        /// クレーム
        /// </summary>
        public IEnumerable<Claim> Claims
        {
            get
            {
                var identity = this.accessor.HttpContext.GetClaims();
                if(identity != null)
                {
                    return identity.Claims;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MultiTenancyLogic(
            ITenantRepository tenantRepository,
            IUserRepository userRepository,
            IHttpContextAccessor accessor)
        {
            this.tenantRepository = tenantRepository;
            this.userRepository = userRepository;
            this.accessor = accessor;
        }

        private void SetUserInfo()
        {
            var identity = this.accessor.HttpContext.GetClaims();
            if (identity != null && identity.IsAuthenticated && identity.Claims.Count() > 0)
            {
                currentUserInfo = userRepository.GetUserInfoAsync(identity.Name).Result;

                if (currentUserInfo != null)
                {
                    currentUserInfo.SelectTenant(identity.GetTenantId());
                    currentUserInfo.RequestId = accessor.HttpContext?.TraceIdentifier;
                    currentUserInfo.Culture = accessor.HttpContext?.Features.Get<Microsoft.AspNetCore.Localization.IRequestCultureFeature>()?.RequestCulture?.Culture;
                }
            }
        }
    }
}

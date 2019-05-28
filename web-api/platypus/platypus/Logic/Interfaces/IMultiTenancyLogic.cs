using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// 現在のユーザに紐づくテナント情報を取得するためのロジッククラスのインターフェイス。
    /// Controller以外でも使用されうるため、ロジック層で実装。
    /// </summary>
    public interface IMultiTenancyLogic
    {
        /// <summary>
        /// ログイン中のユーザ情報を取得します。
        /// </summary>
        UserInfo CurrentUserInfo { get; }

        /// <summary>
        /// 現在選択中のテナントIDを取得します。
        /// </summary>
        long TenantId { get; }

        /// <summary>
        /// 現在選択中のテナント名を取得します。
        /// 取得できなかった場合、空文字列を返します。
        /// </summary>
        string TenantName { get; }

        /// <summary>
        /// 現在選択中のテナント情報を取得します。
        /// 取得できなかった場合、NULLを返します。
        /// </summary>
        Tenant Tenant { get; }

        /// <summary>
        /// ユーザ名を取得します。
        /// 未ログインの場合は ハイフン(-) を返します。 
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// リクエストID
        /// </summary>
        string RequestId { get; }

        /// <summary>
        /// クレーム
        /// </summary>
        IEnumerable<System.Security.Claims.Claim> Claims { get; }
    }
}

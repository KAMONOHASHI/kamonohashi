using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure.Types
{
    /// <summary>
    /// メニュー種別
    /// </summary>
    public enum MenuType
    {
        /// <summary>エラー用</summary>
        Unknown = 0,
        /// <summary>ロール不要。ログインユーザなら全員参照可能。</summary>
        Internal = 1,
        /// <summary>ユーザ向け</summary>
        Tenant = 2,
        /// <summary>管理者向け</summary>
        System = 3,
        /// <summary>未ログインユーザにも表示される</summary>
        Public = 4
    }
}

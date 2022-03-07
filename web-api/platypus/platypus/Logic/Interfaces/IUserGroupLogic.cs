using Novell.Directory.Ldap;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// ユーザグループロジックのインターフェイス
    /// </summary>
    public interface IUserGroupLogic
    {
        /// <summary>
        /// LDAPサーバに問い合わせを行い、認証情報を取得する。
        /// </summary>
        /// <param name="user">ユーザ</param>
        /// <param name="LdapUserName">LDAPサーバ認証用ユーザ名</param>
        /// <param name="password">LDAPサーバ認証用パスワード</param>
        Result<LdapEntry, string> Authenticate(User user, string LdapUserName, string password);

        /// <summary>
        /// 所属しているLdapグループから所属テナントを更新する
        /// </summary>
        /// <param name="entry">LDAPエントリ</param>
        /// <param name="user">ユーザ</param>
        /// <param name="LdapUserName">LDAPサーバ認証用ユーザ名</param>
        /// <param name="password">LDAPサーバ認証用パスワード</param>
        Task AddTenantFromGroup(LdapEntry entry, User user, string LdapUserName, string password);
    }
}

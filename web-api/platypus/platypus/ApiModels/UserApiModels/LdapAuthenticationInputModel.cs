namespace Nssol.Platypus.ApiModels.UserApiModels
{
    /// <summary>
    /// LDAP認証情報入力モデル
    /// </summary>
    public class LdapAuthenticationInputModel
    {
        /// <summary>
        /// LDAP接続用ユーザ名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// LDAP接続用パスワード
        /// </summary>
        public string Password { get; set; }
    }
}

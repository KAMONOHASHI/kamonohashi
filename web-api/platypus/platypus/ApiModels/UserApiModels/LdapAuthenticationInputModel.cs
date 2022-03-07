namespace Nssol.Platypus.ApiModels.UserApiModels
{
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

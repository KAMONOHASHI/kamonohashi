namespace Nssol.Platypus.Infrastructure.Options
{
    /// <summary>
    /// 認証に使用するActiveDirectoryの接続情報を保持するクラス
    /// </summary>
    public class ActiveDirectoryOptions
    {
        /// <summary>
        /// ADサーバ を取得します。
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// ADサーバーのポート
        /// </summary>
        public int Port { get; set; } = 389;

        /// <summary>
        /// ドメイン情報 を取得します。
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 認証に使用するOU
        /// </summary>
        public string BaseOu { get; set; }

        /// <summary>
        /// 認証に使用するDn
        /// </summary>
        public string BaseDn { get; set; }

        /// <summary>
        /// Ldap検索用のフィルタ（ユーザ検索）
        /// </summary>
        public string LdapFilter { get; set; }

        /// <summary>
        /// Ldapグループ検索用のフィルタ（ユーザ検索）
        /// </summary>
        public string LdapGroupFilter { get; set; }

        public string AuthBaseDn
        {
            get
            {
                if (string.IsNullOrEmpty(BaseOu))
                {
                    return $"OU = { Domain }";
                }
                else
                {
                    return $"OU = { BaseOu },{ Domain }";
                }
            }
        }
    }
}

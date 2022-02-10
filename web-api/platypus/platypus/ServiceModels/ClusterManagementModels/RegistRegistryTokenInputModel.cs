namespace Nssol.Platypus.ServiceModels.ClusterManagementModels
{
    /// <summary>
    /// クラスタ管理サービスにコンテナレジストリを登録するためのモデルクラス
    /// </summary>
    public class RegistRegistryTokenInputModel
    {
        /// <summary>
        /// テナント名
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// 登録名。
        /// これで一意性を識別する
        /// </summary>
        public string RegistryTokenKey { get; set; }

        /// <summary>
        /// 登録するレジストリのアクセスユーザ名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登録するレジストリのアクセスパスワード
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 登録するレジストリのURL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// base64エンコードする前のdockercfgの中身。
        /// { "サーバURL": {***}} ←この***に対応するもの
        /// </summary>
        public string DockerCfgAuthString { get; set; }
    }
}

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// アプリケーション内の定数を管理するクラス。
    /// </summary>
    public static class ApplicationConst
    {
        /// <summary>
        /// コピーライト
        /// </summary>
        public const string Copyright = "Copyright (C) 2016-2019 NS Solutions Corporation, All Rights Reserved.";

        /// <summary>
        /// 初期構築時に作成される初期Adminユーザ
        /// </summary>
        public const string DefaultFirstAdminUserName = "admin";

        /// <summary>
        /// 初期構築時に作成される初期テナント名
        /// </summary>
        public const string DefaultFirstTenantName = "sandbox";

        /// <summary>
        /// 初期構築時に作成される初期テナント表示
        /// </summary>
        public const string DefaultFirstTenantDisplayName = "Sandbox";

        /// <summary>
        /// 初期構築時に作成される初期ストレージ名
        /// </summary>
        public const string DefaultFirstStorageName = "Default-storage";
    }
}

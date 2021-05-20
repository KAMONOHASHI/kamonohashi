namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// テンプレートのアクセスレベル。
    /// </summary>
    public enum TemplateAccessLevel
    {
        /// <summary>
        /// 全テナントで利用できない
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// 許可されたテナント以外は利用できない
        /// </summary>
        Private = 1,
        /// <summary>
        /// 全テナントで利用可能
        /// </summary>
        Public = 2
    }
}

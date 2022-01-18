namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// ノードのアクセスレベル。
    /// </summary>
    public enum NodeAccessLevel
    {
        /// <summary>
        /// <see cref="Nssol.Platypus.Models.NodeTenantMap"/> の値に関わらず全テナントで利用できない
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// <see cref="Nssol.Platypus.Models.NodeTenantMap"/> で許可されたテナント以外は利用できない
        /// </summary>
        Private = 1,
        /// <summary>
        /// <see cref="Nssol.Platypus.Models.NodeTenantMap"/> の値に関わらず全テナントで利用可能
        /// </summary>
        Public = 2
    }
}

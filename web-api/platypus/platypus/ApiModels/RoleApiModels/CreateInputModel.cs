namespace Nssol.Platypus.ApiModels.RoleApiModels
{
    /// <summary>
    /// テナント用ロールでも設定可能な項目は<see cref="CreateForTenantInputModel"/>で管理。
    /// Adminのみが設定可能な項目（テナント用ロールでは自動設定される項目）のみ、このクラスで管理。
    /// </summary>
    public class CreateInputModel : CreateForTenantInputModel
    {
        /// <summary>
        /// 管理者用ロールか
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// 紐づけられているテナントID。
        /// </summary>
        /// <remarks>
        /// <see cref="IsSystemRole"/>がTrueの場合は、必ずNULL
        /// </remarks>
        public long? TenantId { get; set; }
    }
}

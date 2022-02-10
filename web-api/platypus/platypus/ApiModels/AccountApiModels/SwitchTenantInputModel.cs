namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    /// <summary>
    /// テナント切替用入力モデル
    /// </summary>
    public class SwitchTenantInputModel
    {
        /// <summary>
        /// 有効期限(秒)。省略時はシステムの既定値。
        /// </summary>
        public int? ExpiresIn { get; set; }
    }
}

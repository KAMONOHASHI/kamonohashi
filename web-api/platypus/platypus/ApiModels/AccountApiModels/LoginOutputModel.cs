namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class LoginOutputModel
    {
        /// <summary>
        /// トークン
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// ユーザ名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// テナントID
        /// </summary>
        public long TenantId { get; set; }
        /// <summary>
        /// テナント名
        /// </summary>
        public string TenantName { get; set; }
        /// <summary>
        /// 有効期限(秒)
        /// </summary>
        public long ExpiresIn { get; set; }
    }
}

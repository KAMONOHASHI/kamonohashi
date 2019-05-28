using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class LoginInputModel
    {
        /// <summary>
        /// ユーザ名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// パスワード
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// テナントID。省略時はデフォルトテナント。
        /// </summary>
        public long? TenantId { get; set; }

        /// <summary>
        /// 有効期限(秒)。省略時はシステムの既定値。
        /// </summary>
        public int? ExpiresIn { get; set; }
    }
}

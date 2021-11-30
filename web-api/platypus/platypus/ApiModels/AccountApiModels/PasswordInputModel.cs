using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class PasswordInputModel
    {
        /// <summary>
        /// 現在のパスワード
        /// </summary>
        [Required]
        public string CurrentPassword { get; set; }
        /// <summary>
        /// 変更後のパスワード
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

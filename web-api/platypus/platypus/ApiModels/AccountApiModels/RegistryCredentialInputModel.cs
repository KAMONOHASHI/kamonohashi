using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class RegistryCredentialInputModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// 認証ユーザ名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// パスワードあるいはトークン
        /// </summary>
        public string Password { get; set; }
    }
}

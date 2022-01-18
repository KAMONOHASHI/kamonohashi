using System.ComponentModel.DataAnnotations;

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

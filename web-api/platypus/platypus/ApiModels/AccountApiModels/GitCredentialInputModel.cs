using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class GitCredentialInputModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// トークン
        /// </summary>
        public string Token { get; set; }
    }
}

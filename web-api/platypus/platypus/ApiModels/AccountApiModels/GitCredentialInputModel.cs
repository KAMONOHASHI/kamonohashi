using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class DisplayNameInputModel
    {
        /// <summary>
        /// ユーザ表示名
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

    }
}

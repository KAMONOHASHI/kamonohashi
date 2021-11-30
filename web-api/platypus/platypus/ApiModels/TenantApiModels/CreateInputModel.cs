using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.TenantApiModels
{
    public class CreateInputModel : EditInputModel
    {
        /// <summary>
        /// テナント名
        /// </summary>
        /// <remarks>
        /// k8sの制約で英小文字のみを許可。
        /// </remarks>
        [Required]
        [Controllers.Util.CustomValidation(Controllers.Util.CustomValidationType.Alphanumeric)]
        public string TenantName { get; set; }
    }
}

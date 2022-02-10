using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.RoleApiModels
{
    public class DetailsForTenantOutputModel : IndexOutputModel
    {
        public DetailsForTenantOutputModel(Role role) : base(role)
        {
        }

        /// <summary>
        /// 全テナント共通のロールか。
        /// Trueの場合、編集できない。
        /// </summary>
        public bool IsShared { get; set; }
    }
}

using Nssol.Platypus.Infrastructure.Infos;

namespace Nssol.Platypus.ApiModels.ResourceApiModels
{
    public class ContainerDetailsOutputModel : ContainerDetailsForTenantOutputModel
    {
        public ContainerDetailsOutputModel(ContainerDetailsInfo info) : base(info)
        {
        }

        /// <summary>
        /// テナントID
        /// </summary>
        public long TenantId { get; set; }
        /// <summary>
        /// テナント名
        /// </summary>
        public string TenantName { get; set; }
        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { get; set; }
    }
}

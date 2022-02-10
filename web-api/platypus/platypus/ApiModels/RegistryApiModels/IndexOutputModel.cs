using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.RegistryApiModels
{
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(Registry registry) : base(registry)
        {
            Id = registry.Id;
            Name = registry.Name;
            RegistryPath = registry.RegistryPath;
            ProjectName = registry.ProjectName;
            ServiceType = registry.ServiceType;
        }

        /// <summary>
        /// Regisry ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Regisry識別名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// レジストリパス。
        /// </summary>
        public string RegistryPath { get; set; }

        /// <summary>
        /// レジストリアクセスユーザ
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Registry種別
        /// </summary>
        public RegistryServiceType ServiceType { get; set; }
    }
}

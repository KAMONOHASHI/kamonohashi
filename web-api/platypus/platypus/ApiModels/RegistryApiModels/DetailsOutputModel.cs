using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.RegistryApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Registry registry) : base(registry)
        {
            Host = registry.Host;
            PortNo = registry.PortNo;
            ApiUrl = registry.ApiUrl;
            RegistryUrl = registry.RegistryUrl;
            IsNotEditable = registry.IsNotEditable;
        }

        /// <summary>
        /// Registryホストサーバ名
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Registryホストサーバポート
        /// </summary>
        public int PortNo { get; set; }

        /// <summary>
        /// Registry API URL
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// Registry Url
        /// </summary>
        public string RegistryUrl { get; set; }

        /// <summary>
        /// 編集不可
        /// </summary>
        /// <remarks>
        /// true：編集不可　false：編集可
        /// </remarks>
        public bool IsNotEditable { get; set; }
    }
}

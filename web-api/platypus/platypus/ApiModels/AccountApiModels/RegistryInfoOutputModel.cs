using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    public class RegistryInfoOutputModel
    {
        /// <summary>
        /// デフォルトのレジストリ
        /// </summary>
        public long? DefaultRegistryId { get; set; }
        /// <summary>
        /// レジストリ認証情報一覧
        /// </summary>
        public IEnumerable<RegistryCredentialOutputModel> Registries { get; set; }
    }
}

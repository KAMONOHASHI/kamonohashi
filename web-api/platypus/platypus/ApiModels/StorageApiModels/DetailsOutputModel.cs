using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.StorageApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Storage storage) : base(storage)
        {
            AccessKey = storage.AccessKey;
            SecretKey = storage.SecretKey;
        }

        /// <summary>
        /// オブジェクトストレージのアクセスキー
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// オブジェクトストレージのシークレットキー
        /// </summary>
        public string SecretKey { get; set; }
    }
}

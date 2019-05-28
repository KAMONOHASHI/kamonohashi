using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.ApiModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

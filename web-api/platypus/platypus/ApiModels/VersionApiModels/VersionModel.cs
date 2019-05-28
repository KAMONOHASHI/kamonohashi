using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.VersionApiModels
{
    public class VersionModel
    {
        public VersionModel(string version)
        {
            Version = version;
        }
        /// <summary>
        /// version番号
        /// </summary>
        public string Version { get; set; }
    }
}

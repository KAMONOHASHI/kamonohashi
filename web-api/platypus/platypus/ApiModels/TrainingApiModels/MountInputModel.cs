using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    public class MountInputModel
    {
        /// <summary>
        /// ステータス
        /// </summary>
        public IEnumerable<string> Status { get; set; }
    }
}

using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    public class DataFilesOutputModel
    {
        /// <summary>
        /// データID
        /// </summary>
        public long Id { get; set; }
        public List<DataFileOutputModel> Files { get; set; }
    }
}

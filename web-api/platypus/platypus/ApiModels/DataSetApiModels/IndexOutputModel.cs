using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.Infrastructure;

namespace Nssol.Platypus.ApiModels.DataSetApiModels
{
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(DataSet dataSet) : base(dataSet)
        {
            Id = dataSet.Id;
            DisplayId = dataSet.DisplayId;
            Name = dataSet.Name;
            Memo = dataSet.Memo;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 表示用ID
        /// </summary>
        public long? DisplayId { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }
    }
}

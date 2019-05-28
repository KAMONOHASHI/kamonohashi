using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.Components
{
    public class OutputModelBase
    {
        public OutputModelBase(ModelBase model)
        {
            CreatedBy = model.CreatedBy;
            CreatedAt = model.CreatedAt.ToFormatedString();
            ModifiedBy = model.ModifiedBy;
            ModifiedAt = model.ModifiedAt.ToFormatedString();
        }
        public OutputModelBase()
        {
        }

        /// <summary>
        /// 登録者
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 登録日
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 更新日
        /// </summary>
        public string ModifiedAt { get; set; }
    }
}

using Nssol.Platypus.ApiModels.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.UserApiModels
{
    public class EditInputModel
    {
        /// <summary>
        /// 最初に付与するシステムロールID
        /// </summary>
        public IEnumerable<long> SystemRoles { get; set; }

        /// <summary>
        /// 最初に所属させるテナント
        /// </summary>
        [Required]
        public List<TenantInfoModel> Tenants { get; set; }

        public class TenantInfoModel
        {
            [Required]
            public long? Id { get; set; }

            public Boolean Default { get; set; }

            public IEnumerable<long> Roles { get; set; }
        }
    }
}

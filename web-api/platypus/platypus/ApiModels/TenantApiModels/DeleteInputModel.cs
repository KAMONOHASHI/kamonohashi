using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.TenantApiModels
{
    public class DeleteInputModel
    {
        public bool? IgnoreMinioBucketDeletion { get; set; }
    }
}

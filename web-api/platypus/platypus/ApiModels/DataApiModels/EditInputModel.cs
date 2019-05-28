using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Controllers.Util;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    public class EditInputModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// タグ。
        /// </summary>
        [ValidInputAsTag]
        public IEnumerable<string> Tags { get; set; }
    }
}

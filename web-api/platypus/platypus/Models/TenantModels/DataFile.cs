using Nssol.Platypus.Controllers.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    public class DataFile : TenantModelBase
    {
        /// <summary>
        /// アップロード時に指定したファイル名
        /// </summary>
        [Required]
        public string FileName { get; set; }
        /// <summary>
        /// 保存先パス（GUIDのファイル名）拡張子は変更なし
        /// </summary>
        [Required]
        public string StoredPath { get; set; }
    }
}

using Nssol.Platypus.Controllers.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    public class CreateInputModel
    {
        /// <summary>
        /// データ名
        /// </summary>
        [Required]
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

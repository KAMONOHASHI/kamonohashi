using Nssol.Platypus.Controllers.Util;
using System.Collections.Generic;

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

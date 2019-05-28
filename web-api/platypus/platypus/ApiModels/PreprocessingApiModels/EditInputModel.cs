using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Controllers.Util;

namespace Nssol.Platypus.ApiModels.PreprocessingApiModels
{
    public class EditInputModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        [ValidInputAsTag]
        public string Name { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// CPUコア数のデフォルト値
        /// </summary>
        public int? Cpu { get; set; }
        /// <summary>
        /// メモリ容量（GiB）のデフォルト値
        /// </summary>
        public int? Memory { get; set; }
        /// <summary>
        /// GPU数のデフォルト値
        /// </summary>
        public int? Gpu { get; set; }
    }
}

using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Infrastructure;

namespace Nssol.Platypus.ApiModels.PreprocessingApiModels
{
    /// <summary>
    /// 前処理情報のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(Preprocess preprocessing) : base(preprocessing)
        {
            Id = preprocessing.Id;
            //DisplayId = preprocessing.DisplayId;
            Name = preprocessing.Name;
            Memo = preprocessing.Memo;
            Cpu = preprocessing.Cpu;
            Memory = preprocessing.Memory;
            Gpu = preprocessing.Gpu;
        }

        /// <summary>
        /// 前処理ID
        /// </summary>
        public long Id { get; set; }
        ///// <summary>
        ///// 表示用ID
        ///// </summary>
        //public long? DisplayId { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// CPUコア数のデフォルト値
        /// </summary>
        public int Cpu { get; set; }
        /// <summary>
        /// メモリ容量（GiB）のデフォルト値
        /// </summary>
        public int Memory { get; set; }
        /// <summary>
        /// GPU数のデフォルト値
        /// </summary>
        public int Gpu { get; set; }
    }
}

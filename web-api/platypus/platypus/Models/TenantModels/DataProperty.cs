using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    public class DataProperty : TenantModelBase
    {
        /// <summary>
        /// 紐づくデータID
        /// </summary>
        [Required]
        public long DataId { get; set; }

        /// <summary>
        /// プロパティのキー。
        /// </summary>
        /// <remarks>
        /// 現状はファイル名と一致。ファイル以外の情報を扱う際の拡張として、残している。
        /// </remarks>
        [Required]
        public string Key { get; set; }

        /// <summary>
        /// 文字列情報。
        /// <see cref="DataFileId"/>とどちらかのみ指定可能。
        /// </summary>
        public string DataString { get; set; }

        /// <summary>
        /// ファイル情報。
        /// <see cref="DataString"/>とどちらかのみ指定可能。
        /// </summary>
        public long? DataFileId { get; set; }

        /// <summary>
        /// データ本体
        /// </summary>
        [ForeignKey(nameof(DataId))]
        public virtual Data Data { get; set; }

        /// <summary>
        /// データファイル
        /// </summary>
        [ForeignKey(nameof(DataFileId))]
        public virtual DataFile DataFile { get; set; }
    }
}

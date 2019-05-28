using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models
{
    public class MenuRoleMap : ModelBase
    {
        /// <summary>
        /// メニューコード
        /// </summary>
        [Required]
        public string MenuCode { get; set; }

        /// <summary>
        /// ロールID
        /// </summary>
        [Required]
        public long RoleId { get; set; }

        /// <summary>
        /// ロール
        /// </summary>
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }
    }
}

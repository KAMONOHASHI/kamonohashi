using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure.Infos
{
    public class RoleInfo
    {
        public RoleInfo()
        {

        }

        public RoleInfo(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            DisplayName = role.DisplayName;
            IsCustomed = role.TenantId != null;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ロール名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ロール表示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// カスタムロールか
        /// </summary>
        public bool IsCustomed { get; set; }
    }
}

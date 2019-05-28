using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.MenuApiModels
{
    public class MenuForAdminOutputModel
    {
        /// <summary>
        /// メニューID
        /// </summary>
        /// <remarks>
        /// 今はメニュー管理がDBではないのでCodeという名前にしているが、将来的にDBになるかもしれない＆他のAPIとそろえるために、入出力モデルではIDと呼ぶ。
        /// </remarks>
        public MenuCode Id { get; set; }

        /// <summary>
        /// メニュー名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// メニュー種別
        /// </summary>
        public MenuType MenuType { get; set; }

        /// <summary>
        /// アクセス可能なロール情報
        /// </summary>
        public IEnumerable<RoleModel> Roles { get; set; }

        public class RoleModel
        {
            /// <summary>
            /// ロールID
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// ロール表示名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// システムロールか
            /// </summary>
            public bool IsSystemRole { get; set; }
        }
    }
}

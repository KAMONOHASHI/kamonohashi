using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.ApiModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.NodeApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Node node) : base(node)
        {
        }

        /// <summary>
        /// <see cref="IndexOutputModel.AccessLevel"/>が<see cref="NodeAccessLevel.Private"/>の時、このノードを使用できるテナントの一覧。
        /// </summary>
        public IEnumerable<AssignedTenant> AssignedTenants { get; set; }

        public class AssignedTenant
        {
            /// <summary>
            /// テナントID
            /// </summary>
            public long Id { get; set; }
            /// <summary>
            /// テナント名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// テナント表示名
            /// </summary>
            public string DisplayName { get; set; }
        }
    }
}

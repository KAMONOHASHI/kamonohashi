using System;
using System.Collections.Generic;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// リソースモニタサンプル
    /// </summary>
    public class ResourceSample : ModelBase
    {
        /// <summary>
        /// サンプル日時
        /// </summary>
        public DateTime SampledAt { get; set; }

        /// <summary>
        /// リソースモニタノード
        /// </summary>
        public IEnumerable<ResourceNode> ResourceNodes { get; set; }
    }
}

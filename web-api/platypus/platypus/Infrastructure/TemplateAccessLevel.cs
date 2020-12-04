using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// テンプレートのアクセスレベル。
    /// </summary>
    public enum TemplateAccessLevel
    {
        /// <summary>
        /// <see cref="Nssol.Platypus.Models.TemplateTenantMap"/> の値に関わらず全テナントで利用できない
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// <see cref="Nssol.Platypus.Models.TemplateTenantMap"/> で許可されたテナント以外は利用できない
        /// </summary>
        Private = 1,
        /// <summary>
        /// <see cref="Nssol.Platypus.Models.TemplateTenantMap"/> の値に関わらず全テナントで利用可能
        /// </summary>
        Public = 2
    }
}

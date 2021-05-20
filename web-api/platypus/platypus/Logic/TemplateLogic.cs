using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// テンプレート処理のクラス
    /// </summary>
    public class TemplateLogic : PlatypusLogicBase, ITemplateLogic
    {
        public TemplateLogic(ICommonDiLogic commonDiLogic)
            : base(commonDiLogic) { }

        public bool Accessible(Template template, Tenant tenant)
            => template.AccessLevel == TemplateAccessLevel.Private && template.CreaterTenantId == tenant.Id
            || (template.AccessLevel == TemplateAccessLevel.Public);

        public bool IsCreatedTenant(Template template, Tenant tenant)
            => template.CreaterTenantId == tenant.Id;

    }
}

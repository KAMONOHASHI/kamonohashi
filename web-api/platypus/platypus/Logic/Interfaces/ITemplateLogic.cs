using Nssol.Platypus.Models;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// テンプレート処理のインターフェース
    /// </summary>
    public interface ITemplateLogic
    {
        /// <summary>
        /// テンプレートが指定されたテナントからアクセス可能かどうかを返す
        /// </summary>
        bool Accessible(Template template, Tenant tenant);

        /// <summary>
        /// テンプレートが指定されたテナントで作成されたかを返す
        /// </summary>
        bool IsCreatedTenant(Template template, Tenant tenant);

    }
}

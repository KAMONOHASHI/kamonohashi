using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// テンプレートマップにアクセスするためのリポジトリインターフェース
    /// </summary>
    public interface ITemplateTenantMapRepository : IRepository<TemplateTenantMap>
    {
        /// <summary>
        /// 指定されたテナントがプライベートアクセス可能なノードの一覧を取得
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        /// <returns>ノード一覧</returns>
        IEnumerable<TemplateTenantMap> GetPrivateAccessibleTemplateList(long tenantId);
    }
}

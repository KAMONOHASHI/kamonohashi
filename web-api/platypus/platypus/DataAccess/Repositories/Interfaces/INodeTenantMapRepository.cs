using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System.Collections.Generic;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// ノードマップにアクセスするためのリポジトリインターフェース
    /// </summary>
    public interface INodeTenantMapRepository : IRepository<NodeTenantMap>
    {
        /// <summary>
        /// 指定されたテナントがプライベートアクセス可能なノードの一覧を取得
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        /// <returns>ノード一覧</returns>
        IEnumerable<NodeTenantMap> GetPrivateAccessibleNodeList(long tenantId);
    }
}

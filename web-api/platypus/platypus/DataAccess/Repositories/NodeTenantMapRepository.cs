using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.DataAccess.Repositories
{
    /// <summary>
    /// ノードテナントテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    /// <seealso cref="Nssol.Platypus.DataAccess.Repositories.Interfaces.INodeTenantMapRepository" />
    public class NodeTenantMapRepository : RepositoryBase<NodeTenantMap>, INodeTenantMapRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">データにアクセスするためのDBコンテキスト</param>
        public NodeTenantMapRepository(CommonDbContext context) : base(context)
        { }

        /// <summary>
        /// 指定されたテナントがプライベートアクセス可能なノードの一覧を取得
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        /// <returns>ノード一覧</returns>
        public IEnumerable<NodeTenantMap> GetPrivateAccessibleNodeList(long tenantId)
        {
            return this.GetAll()
                .Where(m => m.TenantId == tenantId).Include(m => m.Node)
                .Where(m => m.Node.AccessLevel == NodeAccessLevel.Private);
        }
    }
}

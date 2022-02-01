using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories
{
    /// <summary>
    /// ノードテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    /// <seealso cref="Nssol.Platypus.DataAccess.Repositories.Interfaces.INodeRepository" />
    public class NodeRepository : RepositoryBase<Node>, INodeRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">データにアクセスするためのDBコンテキスト</param>
        public NodeRepository(CommonDbContext context) : base(context)
        {

        }

        /// <summary>
        /// 指定したノード名のノードを取得する
        /// </summary>
        public async Task<Node> GetByNameAsync(string name)
        {
            return await GetAll().FirstOrDefaultAsync(n => n.Name == name);
        }

        /// <summary>
        /// 指定したパーティションが有効な値かどうか。
        /// </summary>
        /// <param name="partition">パーティション値</param>
        /// <param name="includeDisableNode">Disableなノードも検索対象に含めるか</param>
        public async Task<bool> IsEnablePartitionAsync(string partition, bool includeDisableNode)
        {
            if (includeDisableNode)
            {
                return await ExistsAsync(n => n.Partition == partition);
            }
            else
            {
                return await ExistsAsync(n => n.Partition == partition && n.Enable);
            }
        }

        /// <summary>
        /// 指定したノードにアクセス可能なテナント一覧を返す。
        /// ノードIDの存在チェック、およびアクセスレベル確認は行わない。
        /// </summary>
        public IEnumerable<Tenant> GetAssignedTenants(long nodeId)
        {
            return FindModelAll<NodeTenantMap>(map => map.NodeId == nodeId).Include(map => map.Tenant).Select(map => map.Tenant);
        }

        /// <summary>
        /// 指定したノードに対するテナントのアサイン状況をリセットする。
        /// </summary>
        public void ResetAssinedTenants(long nodeId)
        {
            DeleteModelAll<NodeTenantMap>(map => map.NodeId == nodeId);
        }

        /// <summary>
        /// 指定したノードにテナントをアサインする。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        public void AssignTenants(Node node, IEnumerable<long> tenantIds, bool isCreate)
        {
            if (node.AccessLevel != Infrastructure.NodeAccessLevel.Private)
            {
                throw new UnauthorizedAccessException("An only private access level node is allowed to manage which tenants assigned.");
            }
            foreach (long tenantId in tenantIds)
            {
                var map = new NodeTenantMap()
                {
                    TenantId = tenantId
                };
                if (isCreate)
                {
                    map.Node = node;
                }
                else
                {
                    map.NodeId = node.Id;
                }
                AddModel<NodeTenantMap>(map);
            }
        }

        /// <summary>
        /// 指定したテナントがアクセス可能なノード一覧を取得する。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        public IEnumerable<Node> GetAccessibleNodes(long tenantId)
        {
            //プライベートノードで、そのテナントがアクセス可能なノードID一覧を取得
            var privateNodeIds = FindModelAll<NodeTenantMap>(map => map.TenantId == tenantId).Select(map => map.NodeId).ToList();

            //Publicか、あるいはアクセス可能なプライベートノードか
            return GetAll().Where(n => n.AccessLevel == Infrastructure.NodeAccessLevel.Public ||
                (n.AccessLevel == Infrastructure.NodeAccessLevel.Private && privateNodeIds.Contains(n.Id)));
        }
    }
}

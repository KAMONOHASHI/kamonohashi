using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// ノードにアクセスするためのリポジトリインターフェース
    /// </summary>
    public interface INodeRepository : IRepository<Node>
    {
        /// <summary>
        /// 指定したノード名のノードを取得する
        /// </summary>
        Task<Node> GetByNameAsync(string name);

        /// <summary>
        /// 指定したパーティションが有効な値かどうか。
        /// </summary>
        /// <param name="partition">パーティション値</param>
        /// <param name="includeDisableNode">Disableなノードも検索対象に含めるか</param>
        Task<bool> IsEnablePartitionAsync(string partition, bool includeDisableNode);

        /// <summary>
        /// 指定したノードにアクセス可能なテナント一覧を返す。
        /// ノードIDの存在チェック、およびアクセスレベル確認は行わない。
        /// </summary>
        IEnumerable<Tenant> GetAssignedTenants(long nodeId);

        /// <summary>
        /// 指定したノードに対するテナントのアサイン状況をリセットする。
        /// </summary>
        void ResetAssinedTenants(long nodeId);

        /// <summary>
        /// 指定したノードにテナントをアサインする。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        void AssignTenants(Node node, IEnumerable<long> tenantIds, bool isCreate);

        /// <summary>
        /// 指定したテナントがアクセス可能なノード一覧を取得する。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        IEnumerable<Node> GetAccessibleNodes(long tenantId);
    }
}

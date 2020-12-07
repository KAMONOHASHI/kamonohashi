using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// テンプレートテーブルにアクセスするためのリポジトリインターフェース
    /// </summary>
    public interface ITemplateRepository : IRepository<ModelTemplate>
    {
        /// <summary>
        /// 指定されたテンプレートIDのテンプレートエンティティ（外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">テンプレートID</param>
        Task<ModelTemplate> GetIncludeAllAsync(long id);


        /// <summary>
        /// 指定したテンプレートにアクセス可能なテナント一覧を返す。
        /// テンプレートIDの存在チェック、およびアクセスレベル確認は行わない。
        /// </summary>
        IEnumerable<Tenant> GetAssignedTenants(long templateId);

        /// <summary>
        /// 指定したテンプレートに対するテナントのアサイン状況をリセットする。
        /// </summary>
        void ResetAssinedTenants(long templateId);

        /// <summary>
        /// 指定したテンプレートにテナントをアサインする。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        void AssignTenants(ModelTemplate modelTemplate, IEnumerable<long> tenantIds, bool isCreate);

        /// <summary>
        /// 指定したテナントがアクセス可能なテンプレート一覧を取得する。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        IEnumerable<ModelTemplate> GetAccessibleTemplates(long tenantId);
    }
}

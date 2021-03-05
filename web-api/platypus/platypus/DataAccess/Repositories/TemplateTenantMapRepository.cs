using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories
{
    /// <summary>
    /// ノードテナントテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    /// <seealso cref="Nssol.Platypus.DataAccess.Repositories.Interfaces.ITemplateTenantMapRepository" />
    public class TemplateTenantMapRepository : RepositoryBase<TemplateTenantMap>, ITemplateTenantMapRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">データにアクセスするためのDBコンテキスト</param>
        public TemplateTenantMapRepository(CommonDbContext context) : base(context)
        { }

        /// <summary>
        /// 指定されたテナントがプライベートアクセス可能なノードの一覧を取得
        /// </summary>
        /// <param name="tenantId">テナントID</param>
        /// <returns>テンプレート一覧</returns>
        public IEnumerable<TemplateTenantMap> GetPrivateAccessibleTemplateList(long tenantId)
        {
            return this.GetAll()
                .Where(m => m.TenantId == tenantId).Include(m => m.Template)
                .Where(m => m.Template.AccessLevel == TemplateAccessLevel.Private);
        }
    }
}

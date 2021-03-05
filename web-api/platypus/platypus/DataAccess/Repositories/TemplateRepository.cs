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
    /// テンプレートテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    /// <seealso cref="Nssol.Platypus.DataAccess.Repositories.Interfaces.ITemplateRepository" />
    public class TemplateRepository : RepositoryBase<ModelTemplate>, ITemplateRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">データにアクセスするためのDBコンテキスト</param>
        public TemplateRepository(CommonDbContext context) : base(context)
        {
            
        }


        /// <summary>
        /// 指定されたテンプレートIDのテンプレートエンティティ（外部参照を含む）を取得します。
        /// </summary>
        /// <param name="id">テンプレートID</param>
        public async Task<ModelTemplate> GetIncludeAllAsync(long id)
        {
            return await FindAll(t => t.Id == id)
                .Include(t => t.PreprocessContainerRegistry)
                .Include(t => t.TrainingContainerRegistry)
                .SingleOrDefaultAsync();
        }
        /// <summary>
        /// 指定したテンプレートにアクセス可能なテナント一覧を返す。
        /// テンプレートIDの存在チェック、およびアクセスレベル確認は行わない。
        /// </summary>
        public IEnumerable<Tenant> GetAssignedTenants(long templateId)
        {
            return FindModelAll<TemplateTenantMap>(map => map.TemplateId == templateId).Include(map => map.Tenant).Select(map => map.Tenant);
        }

        /// <summary>
        /// 指定したテンプレートに対するテナントのアサイン状況をリセットする。
        /// </summary>
        public void ResetAssinedTenants(long templateId)
        {
            DeleteModelAll<TemplateTenantMap>(map => map.TemplateId == templateId);
        }

        /// <summary>
        /// 指定したテンプレートにテナントをアサインする。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        public void AssignTenant(ModelTemplate template, long tenantId, bool isCreate)
        {
            if (template.AccessLevel == Infrastructure.TemplateAccessLevel.Disabled )
            {
                throw new UnauthorizedAccessException("Private or public access level templates are allowed to manage which tenants assigned.");
            }
            var map = new TemplateTenantMap()
            {
                TenantId = tenantId
            };
            if (isCreate)
            {
                map.Template = template;
            }
            else
            {
                map.TemplateId = template.Id;
            }
            AddModel<TemplateTenantMap>(map);
        }

        /// <summary>
        /// 指定したテナントがアクセス可能なテンプレート一覧を取得する。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        public IEnumerable<ModelTemplate> GetAccessibleTemplates(long tenantId)
        {
            //プライベートのテンプレートで、そのテナントがアクセス可能なテンプレートID一覧を取得
            var privateTemplateIds = FindModelAll<TemplateTenantMap>(map => map.TenantId == tenantId).Select(map => map.TemplateId).ToList();

            //Publicか、あるいはアクセス可能なプライベートテンプレートか
            return GetAll().Where(n => n.AccessLevel == Infrastructure.TemplateAccessLevel.Public ||
                (n.AccessLevel == Infrastructure.TemplateAccessLevel.Private && privateTemplateIds.Contains(n.Id)));
        }

    }
}

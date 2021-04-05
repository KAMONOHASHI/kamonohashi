using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.DataAccess.Repositories
{
    /// <summary>
    /// テンプレートテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class TemplateRepository : RepositoryBase<Template>, ITemplateRepository
    {
        public TemplateRepository(CommonDbContext context) : base(context) { }
    }
}

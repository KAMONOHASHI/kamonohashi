using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.DataAccess.Repositories
{
    /// <summary>
    /// テンプレートバージョンテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class TemplateVersionRepository : RepositoryBase<TemplateVersion>, ITemplateVersionRepository
    {
        public TemplateVersionRepository(CommonDbContext context) : base(context) { }
    }
}

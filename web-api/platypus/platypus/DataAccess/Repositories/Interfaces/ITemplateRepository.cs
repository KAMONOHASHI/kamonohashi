using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// テンプレート関連テーブルにアクセスするためのリポジトリインターフェース
    /// </summary>
    public interface ITemplateRepository : IRepository<Template>
    {
        /// <summary>
        /// テンプレートバージョンを取得する
        /// </summary>
        /// <param name="templateId">テンプレートID</param>
        /// <param name="templateVersionId">テンプレートバージョンID</param>
        /// <returns>テンプレートバージョン</returns>
        Task<TemplateVersion> GetTemplateVersionAsync(long templateId, long templateVersionId);

        /// <summary>
        /// 新規エントリを追加する
        /// </summary>
        /// <param name="model">追加するエントリ</param>
        void Add<T>(T model) where T : ModelBase;
    }
}

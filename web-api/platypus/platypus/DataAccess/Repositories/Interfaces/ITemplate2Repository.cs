using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// テンプレート関連テーブルにアクセスするためのリポジトリインターフェース
    /// </summary>
    public interface ITemplate2Repository : IRepository<Template>
    {
        /// <summary>
        /// テンプレートとそのバージョンを取得する
        /// </summary>
        /// <param name="id">テンプレートID</param>
        /// <returns>テンプレート</returns>
        Task<Template> GetTemplateWithVersionsAsync(long id);

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

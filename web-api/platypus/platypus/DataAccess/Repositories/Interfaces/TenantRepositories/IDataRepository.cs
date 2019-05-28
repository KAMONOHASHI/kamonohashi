using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.CustomModels;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// データテーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IDataRepository : IRepositoryForTenant<Data>
    {
        /// <summary>
        /// 全データをタグ情報付きで取得する。
        /// ソート順はIDの逆順。
        /// </summary>
        IQueryable<Data> GetAllIncludeTag();

        /// <summary>
        /// DataのIndex情報をViewから取得する
        /// </summary>
        IQueryable<DataIndex> GetDataIndex();

        /// <summary>
        /// 指定したデータIDのデータ（データファイル、タグの外部参照を含む）を取得します。
        /// </summary>
        Task<Data> GetDataIncludeAllAsync(long id);

        /// <summary>
        /// 指定したデータに新規ファイルを追加する
        /// </summary>
        DataProperty AddFile(Data data, string fileName, string storedPath);

        /// <summary>
        /// 既存プロパティを取得する
        /// </summary>
        DataProperty GetDataProperty(long dataId, string key);

        /// <summary>
        /// 既存プロパティを全て取得する
        /// </summary>
        IEnumerable<DataProperty> GetAllDataProperty(long dataId);

        /// <summary>
        /// データを削除する。
        /// 紐づいているファイルもすべて削除する。
        /// </summary>
        void DeleteData(Data data);
    }
}

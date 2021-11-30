using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// データ種別テーブルにアクセスするためのリポジトリクラス
    /// </summary>
    public class DataTypeRepository : RepositoryForTenantBase<DataType>, IDataTypeRepository
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// データ種別エンティティリストをキャッシュします。
        /// </remarks>
        public DataTypeRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
        }

        /// <summary>
        /// 名前を元にデータ種別エンティティを取得します。
        /// </summary>
        /// <param name="name">種別名</param>
        /// <returns>
        /// データ種別エンティティ
        /// </returns>
        public DataType GetByName(string name)
        {
            return Find(m => m.Name == name);
        }
    }
}

using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// データ種別テーブルにアクセスするためのリポジトリインターフェイス
    /// </summary>
    public interface IDataTypeRepository : IRepositoryForTenant<DataType>
    {
        /// <summary>
        /// 名前を元にデータ種別エンティティを取得します。
        /// </summary>
        /// <param name="name">種別名</param>
        /// <returns>データ種別エンティティ</returns>
        DataType GetByName(string name);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Core
{
    /// <summary>
    /// まとめて反映するための UnitOfWork のインターフェイス
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// データベースへコミットします。
        /// </summary>
        /// <returns>影響を受けた行数</returns>
        int Commit();

        /// <summary>
        /// データベースへコミットします。
        /// </summary>
        /// <param name="user">更新者</param>
        /// <returns>
        /// 影響を受けた行数
        /// </returns>
        int Commit(string user);
    }
}

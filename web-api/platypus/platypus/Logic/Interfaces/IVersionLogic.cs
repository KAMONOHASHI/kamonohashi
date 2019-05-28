using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// バージョン情報しに対する共通処理をまとめたインターフェース。
    /// </summary>
    public interface IVersionLogic
    {
        /// <summary>
        /// バージョン番号を取得する
        /// </summary>
        string GetVersion();
    }
}

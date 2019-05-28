using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// 別のレイヤの処理を呼びだす際に、正常終了時と異常終了時で異なる型を返したいときに利用するクラス。
    /// </summary>
    /// <typeparam name="T">正常終了時の結果の型</typeparam>
    /// <typeparam name="U">異常終了時の結果の型</typeparam>
    public class Result<T,U>
    {
        /// <summary>
        /// 結果の成否
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return Error == null;
            }
        }

        /// <summary>
        /// 不整合が起こらないように、コンストラクタを秘匿する。
        /// </summary>
        protected Result()
        {
        }

        /// <summary>
        /// ロジック実行結果。
        /// </summary>
        public T Value { get; protected set; }

        /// <summary>
        /// ロジック実行時のエラー内容。
        /// この値がNULLでなければ、失敗と見なされる。
        /// </summary>
        public U Error { get; protected set; }

        /// <summary>
        /// 正常終了結果を作成する。
        /// </summary>
        public static Result<T, U> CreateResult(T value)
        {
            Result<T, U> response = new Result<T, U>()
            {
                Value = value
            };
            return response;
        }

        /// <summary>
        /// 異常終了結果を作成する。
        /// </summary>
        public static Result<T, U> CreateErrorResult(U error)
        {
            Result<T, U> response = new Result<T, U>()
            {
                Error = error
            };
            return response;
        }
    }
}

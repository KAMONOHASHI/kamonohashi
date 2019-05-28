using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// レイヤ―を跨いで使用されるユーティリティクラス。
    /// レイヤ固有のものが必要な場合、別途クラスを作る。
    /// </summary>
    public static class Util
    {
        public static string GenerateHash(string sourceString, string salt)
        {
            System.Security.Cryptography.HashAlgorithm hash = new System.Security.Cryptography.SHA256Managed();

            // compute hash of the password prefixing password with the salt
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(salt + sourceString);
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);

            string hashValue = Convert.ToBase64String(hashBytes);
            return hashValue;
        }


        /// <summary>
        /// 日時を共通フォーマットで文字列化する。
        /// </summary>
        public static string ToFormatedString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// UTC日時を、ローカル時刻の共通フォーマットで文字列化する。
        /// </summary>
        public static string ToLocalFormatedString(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 指定された条件でページングされたデータを取得する
        /// </summary>
        /// <typeparam name="T">データモデル</typeparam>
        /// <param name="data">クエリ</param>
        /// <param name="page">ページ番号。スタートは1。</param>
        /// <param name="perPage">表示件数</param>
        public static IQueryable<T> Paging<T>(this IQueryable<T> data, int page, int perPage) where T : Models.ModelBase
        {
            int skip = perPage * (page - 1);
            return data.Skip(skip).Take(perPage);
        }

        /// <summary>
        /// <see cref="IQueryable{T}"/>に、文字列の部分一致条件を追加する。
        /// <paramref name="query"/>が未指定だった場合は<paramref name="data"/>をそのまま返す。
        /// </summary>
        /// <param name="data">元のクエリ</param>
        /// <param name="keySelector">比較する文字列を選択する関数</param>
        /// <param name="query">検索文字列（部分一致）</param>
        public static IQueryable<T> SearchString<T>(this IQueryable<T> data, Func<T, string> keySelector, string query) where T : Models.ModelBase
        {
            if (string.IsNullOrEmpty(query))
            {
                return data;
            }
            if (query.StartsWith("!"))
            {
                return data.Where((d) => string.IsNullOrEmpty(keySelector(d)) || keySelector(d).Contains(query.Substring(1)) == false);
            }
            return data.Where((d) => keySelector(d) != null && keySelector(d).Contains(query));
        }

        /// <summary>
        /// <see cref="IQueryable{T}"/>に、数値の比較条件を追加する。
        /// <paramref name="query"/>が未指定だった場合は<paramref name="data"/>をそのまま返す。
        /// </summary>
        /// <param name="data">元のクエリ</param>
        /// <param name="keySelector">比較する数値を選択する関数</param>
        /// <param name="query">比較文字列＋数値の形式</param>
        public static IQueryable<T> SearchLong<T>(this IQueryable<T> data, Func<T, long> keySelector, string query) where T : Models.ModelBase
        {
            if (string.IsNullOrEmpty(query))
            {
                return data;
            }
            if (query.StartsWith(">="))
            {
                if (long.TryParse(query.Substring(2), out long target))
                {
                    return data.Where(d => keySelector(d) >= target);
                }
            }
            else if (query.StartsWith(">"))
            {
                if (long.TryParse(query.Substring(1), out long target))
                {
                    return data.Where(d => keySelector(d) > target);
                }
            }
            else if (query.StartsWith("<="))
            {
                if (long.TryParse(query.Substring(2), out long target))
                {
                    return data.Where(d => keySelector(d) <= target);
                }
            }
            else if (query.StartsWith("<"))
            {
                if (long.TryParse(query.Substring(1), out long target))
                {
                    return data.Where(d => keySelector(d) < target);
                }
            }
            else if (query.StartsWith("="))
            {
                if (long.TryParse(query.Substring(1), out long target))
                {
                    return data.Where(d => keySelector(d) == target);
                }
            }
            else
            {
                if (long.TryParse(query, out long target))
                {
                    return data.Where(d => keySelector(d) == target);
                }
            }

            // パースに失敗した場合は、検索結果0件として返す
            return data.Where(d => keySelector(d) == 0);
        }

        /// <summary>
        /// <see cref="IQueryable{T}"/>に、時刻の比較条件を追加する。
        /// <paramref name="query"/>が未指定だった場合は<paramref name="data"/>をそのまま返す。
        /// 
        /// e.g.（比較文字列は半角でOK）
        /// "＞2018/01/01" → 2018/01/01 00:00:00 より前
        /// "2018/01/01" → 2018/01/01 00:00:00 以降、2018/01/02 00:00:00 より前
        /// "＜2018/01/01" → 2018/01/01 00:00:00 より後
        /// </summary>
        /// <param name="data">元のクエリ</param>
        /// <param name="keySelector">比較する時刻を選択する関数</param>
        /// <param name="query">比較文字列＋時刻の形式</param>
        public static IQueryable<T> SearchTime<T>(this IQueryable<T> data, Func<T, DateTime> keySelector, string query) where T : Models.ModelBase
        {
            if (string.IsNullOrEmpty(query))
            {
                return data;
            }
            if (query.StartsWith(">="))
            {
                if (DateTime.TryParse(query.Substring(2), out DateTime createdSince))
                {
                    return data.Where(d => keySelector(d) >= createdSince);
                }
            }
            if (query.StartsWith(">"))
            {
                if (DateTime.TryParse(query.Substring(1), out DateTime createdSince))
                {
                    return data.Where(d => keySelector(d) > createdSince);
                }
            }
            else if (query.StartsWith("<="))
            {
                if (DateTime.TryParse(query.Substring(2), out DateTime createdBy))
                {
                    return data.Where(d => keySelector(d) <= createdBy);
                }
            }
            else if (query.StartsWith("<"))
            {
                if (DateTime.TryParse(query.Substring(1), out DateTime createdBy))
                {
                    return data.Where(d => keySelector(d) < createdBy);
                }
            }
            else if (query.StartsWith("="))
            {
                if (DateTime.TryParse(query.Substring(1), out DateTime createdAt))
                {
                    return data.Where(d => keySelector(d).Date == createdAt.Date);
                }
            }
            else
            {
                if (DateTime.TryParse(query, out DateTime createdAt))
                {
                    return data.Where(d => keySelector(d).Date == createdAt.Date);
                }
            }
            return data;
        }
    }
}

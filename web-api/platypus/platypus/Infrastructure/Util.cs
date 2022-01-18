using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
        /// 日付を共通フォーマットで文字列化する。
        /// </summary>
        /// <param name="dateTime">日時</param>
        public static string ToFormattedDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 日時を共通フォーマットで文字列化する。
        /// </summary>
        /// <param name="dateTime">日時</param>
        public static string ToFormatedString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// UTC日時を、ローカル時刻の共通フォーマットで文字列化する。
        /// </summary>
        /// <param name="dateTime">UTC日時</param>
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
        public static IEnumerable<T> Paging<T>(this IEnumerable<T> data, int page, int perPage) where T : Models.ModelBase
        {
            int skip = perPage * (page - 1);
            return data.Skip(skip).Take(perPage);
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>に、文字列の部分一致条件を追加する。
        /// <paramref name="query"/>が未指定だった場合は<paramref name="data"/>をそのまま返す。
        /// </summary>
        /// <param name="data">元のクエリ</param>
        /// <param name="keySelector">比較する文字列を選択する関数</param>
        /// <param name="query">検索文字列（部分一致）</param>
        public static IEnumerable<T> SearchString<T>(this IEnumerable<T> data, Func<T, string> keySelector, string query) where T : Models.ModelBase
        {
            if (string.IsNullOrEmpty(query))
            {
                return data;
            }
            if (query.StartsWith("!", StringComparison.CurrentCulture))
            {
                return data.Where((d) => string.IsNullOrEmpty(keySelector(d)) || keySelector(d).Contains(query.Substring(1), StringComparison.CurrentCulture) == false);
            }
            return data.Where((d) => keySelector(d) != null && keySelector(d).Contains(query, StringComparison.CurrentCulture));
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>に、数値の比較条件を追加する。
        /// <paramref name="query"/>が未指定だった場合は<paramref name="data"/>をそのまま返す。
        /// </summary>
        /// <param name="data">元のクエリ</param>
        /// <param name="keySelector">比較する数値を選択する関数</param>
        /// <param name="query">比較文字列＋数値の形式</param>
        public static IEnumerable<T> SearchLong<T>(this IEnumerable<T> data, Func<T, long> keySelector, string query) where T : Models.ModelBase
        {
            if (string.IsNullOrEmpty(query))
            {
                return data;
            }
            if (query.StartsWith(">=", StringComparison.CurrentCulture))
            {
                if (long.TryParse(query.Substring(2), out long target))
                {
                    return data.Where(d => keySelector(d) >= target);
                }
            }
            else if (query.StartsWith(">", StringComparison.CurrentCulture))
            {
                if (long.TryParse(query.Substring(1), out long target))
                {
                    return data.Where(d => keySelector(d) > target);
                }
            }
            else if (query.StartsWith("<=", StringComparison.CurrentCulture))
            {
                if (long.TryParse(query.Substring(2), out long target))
                {
                    return data.Where(d => keySelector(d) <= target);
                }
            }
            else if (query.StartsWith("<", StringComparison.CurrentCulture))
            {
                if (long.TryParse(query.Substring(1), out long target))
                {
                    return data.Where(d => keySelector(d) < target);
                }
            }
            else if (query.StartsWith("=", StringComparison.CurrentCulture))
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
        /// <see cref="IEnumerable{T}"/>に、時刻の比較条件を追加する。
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
        public static IEnumerable<T> SearchTime<T>(this IEnumerable<T> data, Func<T, DateTime> keySelector, string query) where T : Models.ModelBase
        {
            if (string.IsNullOrEmpty(query))
            {
                return data;
            }
            if (query.StartsWith(">=", StringComparison.CurrentCulture))
            {
                if (DateTime.TryParse(query.Substring(2), out DateTime createdSince))
                {
                    return data.Where(d => keySelector(d) >= createdSince);
                }
            }
            if (query.StartsWith(">", StringComparison.CurrentCulture))
            {
                if (DateTime.TryParse(query.Substring(1), out DateTime createdSince))
                {
                    return data.Where(d => keySelector(d) > createdSince);
                }
            }
            else if (query.StartsWith("<=", StringComparison.CurrentCulture))
            {
                if (DateTime.TryParse(query.Substring(2), out DateTime createdBy))
                {
                    return data.Where(d => keySelector(d) <= createdBy);
                }
            }
            else if (query.StartsWith("<", StringComparison.CurrentCulture))
            {
                if (DateTime.TryParse(query.Substring(1), out DateTime createdBy))
                {
                    return data.Where(d => keySelector(d) < createdBy);
                }
            }
            else if (query.StartsWith("=", StringComparison.CurrentCulture))
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

        /// <summary>
        /// 指定した長さのランダム文字列を生成する。
        /// 使用する文字は英小文字＋数字
        /// </summary>
        public static string GenerateRandamString(int size)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();

            byte[] data = new byte[size];

            // 厳密な乱数（64bit値）を作成。Randomクラスだと速いが予測可能になってしまう。
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// 2つのfloat型の和を求める。
        /// </summary>
        /// <param name="fp1">float型の値1</param>
        /// <param name="fp2">float型の値2</param>
        public static float SumOfFloat(float fp1, float fp2)
        {
            return (float)((decimal)fp1 + (decimal)fp2);
        }
    }
}

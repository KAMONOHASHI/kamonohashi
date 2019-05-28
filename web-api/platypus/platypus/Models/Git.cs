using Nssol.Platypus.Infrastructure.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// Gitリポジトリ。
    /// </summary>
    public class Git : ModelBase
    {
        /// <summary>
        /// 識別名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gitサービス種別
        /// </summary>
        [Required]
        public GitServiceType ServiceType { get; set; }

        #region API用
        /// <summary>
        /// GitのAPI実行に使うURL。
        /// 想定形式：{プロトコル}://{アクセスユーザ}@{ホスト名}:{ポート番号}
        /// </summary>
        [Required]
        public string ApiUrl { get; set; }

        /// <summary>
        /// API実行用の認証トークン
        /// </summary>
        [Obsolete]
        public string Token { get; set; }
        #endregion

        #region Gitコマンド用
        /// <summary>
        /// Gitコマンドでアクセスする際のURL。
        /// 想定形式：{プロトコル}://{アクセスユーザ}@{ホスト名}:{ポート番号}/{リポジトリオーナー}
        /// </summary>
        [Required]
        public string RepositoryUrl { get; set; }
        #endregion

        /// <summary>
        /// このGit情報の概略を返す
        /// </summary>
        public override string ToString()
        {
            string name = string.IsNullOrEmpty(Name) ? RepositoryUrl : Name;
            return $"{name}({ServiceType})";
        }
    }
}

using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// ストレージ情報
    /// </summary>
    public class Storage : ModelBase
    {
        /// <summary>
        /// 識別名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// オブジェクトストレージのサーバ名+ポート番号
        /// </summary>
        [Required]
        public string ServerAddress { get; set; }

        /// <summary>
        /// オブジェクトストレージのアクセスキー
        /// </summary>
        [Required]
        public string AccessKey { get; set; }

        /// <summary>
        /// オブジェクトストレージのシークレットキー
        /// </summary>
        [Required]
        public string SecretKey { get; set; }

        /// <summary>
        /// NFSサーバホスト名。
        /// </summary>
        /// <remarks>
        /// オブジェクトストレージとNFSでエンドポイントが異なる可能性があるため、<see cref="ServerAddress"/>とは別で保持
        /// </remarks>
        [Required]
        public string NfsServer { get; set; }

        /// <summary>
        /// NFSサーバ本体の共有ディレクトリパス。
        /// 実際にマウントする際はこの配下のテナント名ディレクトリをマウントする。
        /// </summary>
        [Required]
        public string NfsRoot { get; set; }

        /// <summary>
        /// NFSサーバ本体の共有ディレクトリパス。
        /// 実際にマウントする際はこの配下のテナント名ディレクトリをマウントする。
        /// "/"で終わることを保証する。
        /// </summary>
        [NotMapped]
        public string NfsRootPath
        {
            get
            {
                return NfsRoot.EndsWith("/") ? NfsRoot : NfsRoot + "/";
            }
        }
    }
}

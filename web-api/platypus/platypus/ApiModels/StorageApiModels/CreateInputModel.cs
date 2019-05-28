using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.Infrastructure.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.StorageApiModels
{
    public class CreateInputModel
    {
        /// <summary>
        /// 識別名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// オブジェクトストレージのサーバ名
        /// </summary>
        [Required]
        public string ServerUrl { get; set; }

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
        /// オブジェクトストレージとNFSでエンドポイントが異なる可能性があるため、<see cref="ServerUrl"/>とは別で保持
        /// </remarks>
        [Required]
        public string NfsServer { get; set; }

        /// <summary>
        /// NFSサーバ本体の共有ディレクトリパス
        /// 実際にマウントする際はこの配下のテナント名ディレクトリをマウントする
        /// </summary>
        [Required]
        public string NfsRoot { get; set; }
    }
}

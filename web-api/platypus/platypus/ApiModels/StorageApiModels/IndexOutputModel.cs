using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;

namespace Nssol.Platypus.ApiModels.StorageApiModels
{
    /// <summary>
    /// テナント情報のうち、Indexで表示する最低情報だけを保持する
    /// </summary>
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(Storage storage) : base(storage)
        {
            Id = storage.Id;
            Name = storage.Name;
            ServerUrl = storage.ServerAddress;
            NfsServer = storage.NfsServer;
            NfsRoot = storage.NfsRoot;
        }
        /// <summary>
        /// Git ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 識別名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// オブジェクトストレージのサーバURL
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// NFSサーバホスト名。
        /// </summary>
        /// <remarks>
        /// オブジェクトストレージとNFSでエンドポイントが異なる可能性があるため、<see cref="ServerUrl"/>とは別で保持
        /// </remarks>
        public string NfsServer { get; set; }

        /// <summary>
        /// NFSサーバ本体の共有ディレクトリパス
        /// 実際にマウントする際はこの配下のテナント名ディレクトリをマウントする
        /// </summary>
        public string NfsRoot { get; set; }
    }
}

using Nssol.Platypus.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.NodeApiModels
{
    public class CreateInputModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        /// <remarks>
        /// FQDNやIPアドレスを想定した入力規則を付与。
        /// </remarks>
        [Required]
        [Controllers.Util.CustomValidation(Controllers.Util.CustomValidationType.Fqdn)]
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// パーティション
        /// </summary>
        [Controllers.Util.CustomValidation(Controllers.Util.CustomValidationType.Fqdn)]
        public string Partition { get; set; }

        /// <summary>
        /// アクセスレベル
        /// </summary>
        [Required]
        public NodeAccessLevel? AccessLevel { get; set; }


        /// <summary>
        /// このノードを使用できるテナントのID。
        /// <see cref="AccessLevel"/>が<see cref="NodeAccessLevel.Private"/>の時のみ指定可能。
        /// それ以外の場合は無視される。
        /// </summary>
        public IEnumerable<long> AssignedTenantIds { get; set; }

        /// <summary>
        /// TensorBoardコンテナの実行可否
        /// </summary>
        public bool TensorBoardEnabled { get; set; }

        /// <summary>
        /// Notebookコンテナの実行可否
        /// </summary>
        public bool NotebookEnabled { get; set; }
    }
}

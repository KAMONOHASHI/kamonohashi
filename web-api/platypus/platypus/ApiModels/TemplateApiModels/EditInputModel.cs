using Nssol.Platypus.Infrastructure;
using System.Collections.Generic;
using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.Controllers.Util;

namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// テンプレート編集の入力モデル
    /// </summary>
    public class EditInputModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// アクセスレベル
        /// </summary>
        public TemplateAccessLevel? AccessLevel { get; set; }

        /// <summary>
        /// このテンプレートを使用できるテナントのID。
        /// <see cref="AccessLevel"/>が<see cref="TemplateAccessLevel.Private"/>の時のみ指定可能。
        /// それ以外の場合は無視される。
        /// </summary>
        public IEnumerable<long> AssignedTenantIds { get; set; }

    }
}

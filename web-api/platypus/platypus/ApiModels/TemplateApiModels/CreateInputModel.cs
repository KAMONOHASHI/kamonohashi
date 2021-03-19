using Nssol.Platypus.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// テンプレート作成の入力モデル
    /// </summary>
    public class CreateInputModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// アクセスレベル
        /// </summary>
        public TemplateAccessLevel AccessLevel { get; set; }
    }
}

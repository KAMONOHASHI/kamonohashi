using Nssol.Platypus.Infrastructure;

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
    }
}

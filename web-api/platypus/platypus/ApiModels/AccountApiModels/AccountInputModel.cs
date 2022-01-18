using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    /// <summary>
    /// ユーザ情報の変更モデル。
    /// </summary>
    /// <remarks>
    /// パラメタが一つの場合はモデル作成不要だが、将来的に拡張される可能性が高いので、事前にモデルを作っている
    /// </remarks>
    public class AccountInputModel
    {
        /// <summary>
        /// デフォルトテナント
        /// </summary>
        [Required]
        public string DefaultTenant { get; set; }
    }
}

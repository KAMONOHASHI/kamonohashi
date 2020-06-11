using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.RoleApiModels
{
    /// <summary>
    /// テナントロール管理用詳細出力モデル
    /// </summary>
    public class DetailsForTenantOutputModel : IndexOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="role"></param>
        public DetailsForTenantOutputModel(Role role) : base(role)
        {
        }

        /// <summary>
        /// 全テナント共通のロールか。
        /// Trueの場合、編集できない。
        /// </summary>
        public bool IsShared { get; set; }
    }
}

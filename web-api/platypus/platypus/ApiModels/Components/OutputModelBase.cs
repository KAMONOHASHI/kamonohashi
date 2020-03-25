using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.Components
{
    /// <summary>
    /// 共通の出力モデル
    /// </summary>
    public class OutputModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="model">共通モデル</param>
        public OutputModelBase(ModelBase model)
        {
            CreatedBy = model.CreatedBy;
            CreatedAt = model.CreatedAt.ToFormatedString();
            ModifiedBy = model.ModifiedBy;
            ModifiedAt = model.ModifiedAt.ToFormatedString();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OutputModelBase()
        {
        }

        /// <summary>
        /// 登録者
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 登録日
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 更新日
        /// </summary>
        public string ModifiedAt { get; set; }
    }
}

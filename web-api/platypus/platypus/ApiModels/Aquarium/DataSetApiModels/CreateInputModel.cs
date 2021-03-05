using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.Aquarium.DataSetApiModels
{
    /// <summary>
    /// アクアリウムデータセット作成の入力モデル
    /// </summary>
    public class CreateInputModel
    {
        /// <summary>
        /// データセット名
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}

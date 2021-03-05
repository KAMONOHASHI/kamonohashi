using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.Aquarium.DataSetApiModels
{
    /// <summary>
    /// アクアリウムデータセットバージョン作成の入力モデル
    /// </summary>
    public class VersionCreateInputModel
    {
        /// <summary>
        /// データセットID
        /// </summary>
        [Required]
        public long DataSetId { get; set; }
    }
}

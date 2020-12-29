using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// TensorBoard起動モデル
    /// </summary>
    public class TensorBoardInputModel
    {
        /// <summary>
        /// コンテナの生存期間(秒)
        /// </summary>
        public int? ExpiresIn { get; set; }

    }
}

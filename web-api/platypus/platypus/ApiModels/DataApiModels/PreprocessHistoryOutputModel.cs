using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.DataApiModels
{
    public class PreprocessHistoryOutputModel
    {
        public PreprocessHistoryOutputModel(PreprocessHistory history)
        {
            Id = history.Id;
            InputDataId = history.InputDataId;
            InputDataName = history.InputData?.Name;
            PreprocessId = history.PreprocessId;
            PreprocessName = history.Preprocess?.Name;
        }

        /// <summary>
        /// 前処理履歴ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 入力データID
        /// </summary>
        public long InputDataId { get; set; }

        /// <summary>
        /// 入力データ名
        /// </summary>
        public string InputDataName { get; set; }

        /// <summary>
        /// 前処理ID
        /// </summary>
        public long? PreprocessId { get; set; }

        /// <summary>
        /// 前処理名
        /// </summary>
        public string PreprocessName { get; set; }
    }
}

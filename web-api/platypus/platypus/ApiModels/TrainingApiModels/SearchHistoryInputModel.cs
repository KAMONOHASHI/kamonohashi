using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.Controllers.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    public class SearchHistoryInputModel
    {
        /// <summary>
        /// 検索履歴の名前
        /// </summary>
        [Required]
        [MinLength(4)]
        public string Name { set; get; }

        public SearchDetailInputModel searchDetailInputModel { set; get; }
    }
}

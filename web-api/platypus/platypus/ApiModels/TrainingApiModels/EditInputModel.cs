using Nssol.Platypus.Controllers.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// 学習履歴編集モデル
    /// </summary>
    public class EditInputModel
    {
        /// <summary>
        /// 識別名
        /// </summary>
        [MinLength(1)]
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// お気に入り
        /// </summary>
        public bool? Favorite { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        [ValidInputAsTag]
        public IEnumerable<string> Tags { get; set; }
    }
}

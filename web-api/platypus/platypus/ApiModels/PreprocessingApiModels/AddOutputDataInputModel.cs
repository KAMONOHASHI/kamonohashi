using Nssol.Platypus.ApiModels.Components;
using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.PreprocessingApiModels
{
    public class AddOutputDataInputModel
    {
        /// <summary>
        /// 前処理結果のデータ名。
        /// 省略した場合は「{元データ名}_{前処理名}」になる。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// タグ。
        /// 省略した場合は「前処理名」一つが付与される。
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// 前処理結果に登録するファイル群
        /// </summary>
        public IEnumerable<AddFileInputModel> Files { get; set; }
    }
}

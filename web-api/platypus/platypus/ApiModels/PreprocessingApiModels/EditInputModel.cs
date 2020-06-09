using Nssol.Platypus.Controllers.Util;

namespace Nssol.Platypus.ApiModels.PreprocessingApiModels
{
    /// <summary>
    /// 前処理編集の入力モデル
    /// </summary>
    public class EditInputModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        [ValidInputAsTag]
        public string Name { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// CPUコア数のデフォルト値
        /// </summary>
        public int? Cpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）のデフォルト値
        /// </summary>
        public int? Memory { get; set; }

        /// <summary>
        /// GPU数のデフォルト値
        /// </summary>
        public int? Gpu { get; set; }
    }
}

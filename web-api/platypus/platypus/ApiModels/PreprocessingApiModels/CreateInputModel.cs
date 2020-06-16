using Nssol.Platypus.ApiModels.Components;
using Nssol.Platypus.Controllers.Util;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.PreprocessingApiModels
{
    /// <summary>
    /// 前処理作成の入力モデル
    /// </summary>
    /// <remarks>
    /// 前処理はKAMONOHASHIで実行不能なものも設定できる（履歴管理用）ので、ジョブでは必須入力であったエントリポイントなども必須としない。
    /// </remarks>
    public class CreateInputModel
    {
        /// <summary>
        /// 識別名
        /// </summary>
        [Required]
        [ValidInputAsTag]
        public string Name { get; set; }

        /// <summary>
        /// エントリポイント
        /// </summary>
        public string EntryPoint { get; set; }

        /// <summary>
        /// コンテナ情報
        /// </summary>
        public ContainerImageInputModel ContainerImage { get; set; }

        /// <summary>
        /// 前処理ソースコードGit情報
        /// </summary>
        public GitCommitNullableInputModel GitModel { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// CPUコア数のデフォルト値
        /// </summary>
        public int Cpu { get; set; }

        /// <summary>
        /// メモリ容量（GB）のデフォルト値
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// GPU数のデフォルト値
        /// </summary>
        public int Gpu { get; set; }
    }
}

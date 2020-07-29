﻿using Nssol.Platypus.ApiModels.Components;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.NotebookApiModels
{
    /// <summary>
    /// ノートブック再起動モデル
    /// </summary>
    public class RerunInputModel
    {
        /// <summary>
        /// データセットID
        /// </summary>
        public long? DataSetId { get; set; }

        /// <summary>
        /// 親学習履歴ID
        /// </summary>
        public IEnumerable<long> ParentIds { get; set; }

        /// <summary>
        /// 親推論履歴ID
        /// </summary>
        public IEnumerable<long> InferenceIds { get; set; }

        /// <summary>
        /// コンテナ情報
        /// </summary>
        public ContainerImageInputModel ContainerImage { get; set; }

        /// <summary>
        /// ノートブックモデルGit情報
        /// </summary>
        public GitCommitInputModel GitModel { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        [Required]
        public int? Cpu { get; set; }

        /// <summary>
        /// メモリ数(GB)
        /// </summary>
        [Required]
        public int? Memory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        [Required]
        public int? Gpu { get; set; }

        /// <summary>
        /// コンテナの生存期間(秒)
        /// </summary>
        [Required]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// データセットをローカルコピーするか否か。
        /// true：ローカルコピーする　false：ローカルコピーしない(シンボリックリンクを作成する)
        /// </summary>
        public bool LocalDataSet { get; set; }

        /// <summary>
        /// エントリポイント。
        /// ノートブック起動時に実行されるスクリプト。
        /// </summary>
        public string EntryPoint { get; set; }
    }
}

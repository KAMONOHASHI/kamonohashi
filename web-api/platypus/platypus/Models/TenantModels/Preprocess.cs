using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 前処理
    /// </summary>
    public class Preprocess : TenantModelBase
    {
        /// <summary>
        /// 表示用ID
        /// </summary>
        public long? DisplayId { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// エントリポイント。
        /// </summary>
        public string EntryPoint { get; set; }
        /// <summary>
        /// Git
        /// </summary>
        /// <remarks>前処理はGitを必須としない</remarks>
        public long? RepositoryGitId { get; set; }

        /// <summary>
        /// リポジトリ名
        /// </summary>
        public string RepositoryName { get; set; }
        /// <summary>
        /// リポジトリオーナー
        /// </summary>
        public string RepositoryOwner { get; set; }
        /// <summary>
        /// ブランチ。
        /// </summary>
        public string RepositoryBranch { get; set; }
        /// <summary>
        /// コミットID
        /// </summary>
        public string RepositoryCommitId { get; set; }
        /// <summary>
        /// Dockerリポジトリ。
        /// </summary>
        public long? ContainerRegistryId { get; set; }
        /// <summary>
        /// コンテナ
        /// </summary>
        public string ContainerImage { get; set; }
        /// <summary>
        /// コンテナタグ（＝バージョン）
        /// </summary>
        public string ContainerTag { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// CPUコア数のデフォルト値
        /// </summary>
        public int Cpu { get; set; }
        /// <summary>
        /// メモリ容量（GiB）のデフォルト値
        /// </summary>
        public int Memory { get; set; }
        /// <summary>
        /// GPU数のデフォルト値
        /// </summary>
        public int Gpu { get; set; }

        /// <summary>
        /// コンテナレジストリ
        /// </summary>
        [ForeignKey(nameof(ContainerRegistryId))]
        public virtual Registry ContainerRegistry { get; set; }

    }
}

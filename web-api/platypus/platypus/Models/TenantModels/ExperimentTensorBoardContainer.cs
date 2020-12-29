using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 既存データから実験のTensorBoardを表示するためのコンテナのモデル
    /// </summary>
    public class ExperimentTensorBoardContainer : TenantModelBase
    {
        /// <summary>
        /// コンテナ名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// ホスト
        /// </summary>
        /// <remarks>
        /// 立てた直後はホスト名が決まらないことがあるので、NULLを許可する
        /// </remarks>
        public string Host { get; set; }

        /// <summary>
        /// ポート番号
        /// </summary>
        /// <remarks>
        /// 立てた直後はホスト名が決まらないことがあるので、NULLを許可する
        /// </remarks>
        public int? PortNo { get; set; }

        /// <summary>
        /// 実験履歴ID。
        /// </summary>
        [Required]
        public long ExperimentHistoryId { get; set; }
        /// <summary>
        /// ステータス
        /// </summary>
        [Required]
        public string Status { get; set; }
        /// <summary>
        /// 実行開始日時
        /// </summary>
        [Required]
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// コンテナの生存期間(秒)
        /// </summary>
        [Required]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// 実験履歴
        /// </summary>
        [ForeignKey(nameof(ExperimentHistoryId))]
        public virtual ExperimentHistory ExperimentHistory { get; set; }

        /// <summary>
        /// コンテナ情報の文字列表現を返す。
        /// </summary>
        public override string ToString()
        {
            return $"{Id}({Host}:{PortNo}):{Name}:{Status}";
        }
    }
}

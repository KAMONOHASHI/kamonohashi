using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// ノートブック履歴
    /// </summary>
    public class NotebookHistory : TenantModelBase
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
        /// データセットID
        /// </summary>
        public long? DataSetId { get; set; }

        /// <summary>
        /// GitID
        /// </summary>
        public long? ModelGitId { get; set; }

        /// <summary>
        /// リポジトリ
        /// </summary>
        public string ModelRepository { get; set; }

        /// <summary>
        /// リポジトリオーナー
        /// </summary>
        public string ModelRepositoryOwner { get; set; }

        /// <summary>
        /// ブランチ
        /// </summary>
        public string ModelBranch { get; set; }

        /// <summary>
        /// コミットID
        /// </summary>
        public string ModelCommitId { get; set; }

        /// <summary>
        /// エントリポイント。
        /// ノートブック起動時に実行されるスクリプト。
        /// </summary>
        public string EntryPoint { get; set; }

        /// <summary>
        /// Dockerレジストリ
        /// </summary>
        public long? ContainerRegistryId { get; set; }

        /// <summary>
        /// コンテナイメージ名
        /// </summary>
        [Required]
        public string ContainerImage { get; set; }

        /// <summary>
        /// コンテナタグ（＝バージョン）
        /// </summary>
        [Required]
        public string ContainerTag { get; set; }

        /// <summary>
        /// ユーザが定義可能なKeyValue値のJson。
        /// Keyが環境変数名、Valueがその値になる。
        /// </summary>
        /// <remarks>
        /// 例：
        /// {
        ///   "MODEL_TYPE" : "hoge",
        ///   "VALID_ARGUMENT" : "--num_classes=3"
        /// }
        /// </remarks>
        public string Options { get; set; }

        /// <summary>
        /// <see cref="Options"/> のディクショナリ表現。
        /// </summary>
        /// <remarks>
        /// <see cref="DataAccess.Repositories.TenantRepositories.NotebookHistoryRepository.Add(NotebookHistory)"/> の実行時、
        /// <see cref="Options"/> への変換を行う。更新はされないので、そこだけでいいはず。
        /// </remarks>
        [NotMapped]
        public Dictionary<string, string> OptionDic { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        [Required]
        public int Cpu { get; set; }

        /// <summary>
        /// メモリ容量（GiB）
        /// </summary>
        [Required]
        public int Memory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        [Required]
        public int Gpu { get; set; }
        
        /// <summary>
        /// パーティション
        /// </summary>
        public string Partition { get; set; }

        /// <summary>
        /// コンテナの設定値（起動したときのJson）
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// コンテナが実行されたノード名
        /// </summary>
        public string Node { get; set; }

        /// <summary>
        /// 起動日時
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// 停止日時
        /// エラーなどで中断した際も、その時刻が記録される。
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// お気に入り
        /// </summary>
        public bool Favorite { get; set; }

        /// <summary>
        /// コンテナの生存期間(秒)
        /// </summary>
        [Required]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// データセット
        /// </summary>
        [ForeignKey(nameof(DataSetId))]
        public virtual DataSet DataSet { get; set; }

        /// <summary>
        /// コンテナレジストリ
        /// </summary>
        [ForeignKey(nameof(ContainerRegistryId))]
        public virtual Registry ContainerRegistry { get; set; }

        /// <summary>
        /// 親学習履歴のマッピング
        /// </summary>
        public virtual ICollection<NotebookHistoryParentTrainingMap> ParentTrainingMaps { get; set; }

        /// <summary>
        /// コンテナ起動時に使用する名前
        /// </summary>
        public string Key
        {
            get
            {
                //IDを接尾辞に付けて一意性を担保する
                return $"notebook-{Id}";
            }
        }

        /// <summary>
        /// ノートブック履歴の文字列表現
        /// </summary>
        public override string ToString()
        {
            return $"{Id}:{Name}";
        }

        /// <summary>
        /// 環境変数のディクショナリ表現
        /// </summary>
        /// <returns>環境変数</returns>
        public Dictionary<string, string> GetOptionDic()
        {
            if(Options == null)
            {
                return new Dictionary<string, string>();
            }
            OptionDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(Options);
            return OptionDic;
        }

        /// <summary>
        /// コンテナステータスの取得
        /// </summary>
        /// <returns>コンテナのステータス詳細</returns>
        public ContainerStatus GetStatus()
        {
            return ContainerStatus.Convert(Status);
        }
    }
}

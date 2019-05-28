using Newtonsoft.Json;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 学習履歴
    /// </summary>
    public class TrainingHistory : TenantModelBase
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
        [Required]
        public long DataSetId { get; set; }
        /// <summary>
        /// 学習モデルGit
        /// </summary>
        [Required]
        public long ModelGitId { get; set; }
        /// <summary>
        /// 学習モデルリポジトリ
        /// </summary>
        [Required]
        public string ModelRepository { get; set; }
        /// <summary>
        /// 学習モデルリポジトリオーナー
        /// </summary>
        [Required]
        public string ModelRepositoryOwner { get; set; }
        /// <summary>
        /// 学習モデルブランチ。
        /// </summary>
        public string ModelBranch { get; set; }
        /// <summary>
        /// 学習モデルコミットID
        /// </summary>
        [Required]
        public string ModelCommitId { get; set; }
        /// <summary>
        /// エントリポイント.
        /// 学習開始時に実行されるコマンド。
        /// </summary>
        [Required]
        public string EntryPoint { get; set; }
        /// <summary>
        /// Dockerリポジトリ。
        /// </summary>
        [Required]
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
        /// <see cref="DataAccess.Repositories.TenantRepositories.TrainingHistoryRepository.Add(TrainingHistory)"/> の実行時、
        /// <see cref="Options"/> への変換を行う。更新はされないので、そこだけでいいはず。
        /// </remarks>
        [NotMapped]
        public Dictionary<string, string> OptionDic { get; set; }
        /// <summary>
        /// 親学習履歴ID
        /// </summary>
        public long? ParentId { get; set; }
        /// <summary>
        /// CPUコア数
        /// </summary>
        public int Cpu { get; set; }
        /// <summary>
        /// メモリ容量（GiB）
        /// </summary>
        public int Memory { get; set; }
        /// <summary>
        /// GPU数
        /// </summary>
        public int Gpu { get; set; }
        /// <summary>
        /// パーティション
        /// </summary>
        public string Partition { get; set; }
        /// <summary>
        /// コンテナの設定値（起動したときのJson）
        /// </summary>
        //[Required] TODO:学習実行が自動化されるまではNULLを許可する
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
        /// 実行開始日時
        /// </summary>
        public DateTime? StartedAt { get; set; }
        /// <summary>
        /// 学習完了日時。
        /// エラーなどで中断した際も、その時刻が記録される。
        /// </summary>
        public DateTime? CompletedAt { get; set; }
        /// <summary>
        /// ログ要約。
        /// ユーザが学習中に任意の値を記述できる。
        /// </summary>
        public string LogSummary { get; set; }
        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// お気に入り
        /// </summary>
        public bool Favorite { get; set; }

        /// <summary>
        /// データセット
        /// </summary>
        [ForeignKey(nameof(DataSetId))]
        public virtual DataSet DataSet { get; set; }
        /// <summary>
        /// 親学習履歴
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public virtual TrainingHistory Parent { get; set; }
        /// <summary>
        /// コンテナレジストリ
        /// </summary>
        [ForeignKey(nameof(ContainerRegistryId))]
        public virtual Registry ContainerRegistry { get; set; }

        /// <summary>
        /// 学習履歴添付ファイル
        /// </summary>
        public virtual ICollection<TrainingHistoryAttachedFile> TrainingHistoryAttachedFile { get; set; }

        public string Key
        {
            get
            {
                //IDを接尾辞に付けて一意性を担保する
                return $"training-{Id}";
            }
        }

        /// <summary>
        /// 学習履歴の文字列表現
        /// </summary>
        public override string ToString()
        {
            return $"{Id}:{Name}";
        }

        public Dictionary<string, string> GetOptionDic()
        {
            if(Options == null)
            {
                return new Dictionary<string, string>();
            }
            OptionDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(Options);
            return OptionDic;
        }

        public ContainerStatus GetStatus()
        {
            return ContainerStatus.Convert(Status);
        }
    }
}

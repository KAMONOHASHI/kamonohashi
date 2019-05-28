using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 前処理履歴
    /// </summary>
    public class PreprocessHistory : TenantModelBase
    {
        /// <summary>
        /// データID
        /// </summary>
        [Required]
        public long InputDataId { get; set; }
        /// <summary>
        /// 前処理方法ID
        /// </summary>
        public long? PreprocessId { get; set; }
        /// <summary>
        /// 実行開始日時
        /// </summary>
        public DateTime? StartedAt { get; set; }
        /// <summary>
        /// 実行完了日時
        /// </summary>
        public DateTime? CompletedAt { get; set; }
        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// コンテナ識別子
        /// </summary>
        [Obsolete]
        public string ContainerIdentifier { get; set; }

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
        /// <see cref="DataAccess.Repositories.TenantRepositories.PreprocessHistoryRepository.Add(PreprocessHistory)"/> の実行時、
        /// <see cref="Options"/> への変換を行う。更新はされないので、そこだけでいいはず。
        /// </remarks>
        [NotMapped]
        public Dictionary<string, string> OptionDic { get; set; }

        /// <summary>
        /// CPUコア数
        /// </summary>
        public int? Cpu { get; set; }
        /// <summary>
        /// メモリ容量（GiB）
        /// </summary>
        public int? Memory { get; set; }
        /// <summary>
        /// GPU数
        /// </summary>
        public int? Gpu { get; set; }
        /// <summary>
        /// パーティション
        /// </summary>
        public string Partition { get; set; }

        /// <summary>
        /// データ
        /// </summary>
        [ForeignKey(nameof(InputDataId))]
        public virtual Data InputData { get; set; }
        /// <summary>
        /// 前処理方法
        /// </summary>
        [ForeignKey(nameof(PreprocessId))]
        public virtual Preprocess Preprocess { get; set; }
        
        /// <summary>
        /// データセットエントリ
        /// </summary>
        public virtual ICollection<PreprocessHistoryOutput> PreprocessHistoryOutputs { get; set; }

        /// <summary>
        /// 前処理履歴名。コンテナ名にも利用される。
        /// </summary>
        public string Name
        {
            get
            {
                //前処理履歴IDはユニークなので、それをもとに名前を決める
                return $"preproc-{Id}";
            }
        }

        public Dictionary<string, string> GetOptionDic()
        {
            if (Options == null)
            {
                return new Dictionary<string, string>();
            }
            OptionDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(Options);
            return OptionDic;
        }
    }
}

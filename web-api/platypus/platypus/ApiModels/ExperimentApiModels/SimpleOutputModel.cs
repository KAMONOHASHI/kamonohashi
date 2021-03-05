using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    /// <summary>
    /// 学習履歴のうち、コスト最小で取得できる情報だけを保持する
    /// </summary>
    public class SimpleOutputModel : Components.OutputModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="history">実験履歴</param>
        public SimpleOutputModel(ExperimentHistory history) : base(history)
        {
            Id = history.Id;
            Name = history.Name;
            Status = history.GetStatus().ToString();
            FullName = $"{Id}:{Name}";
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 表示用ID
        /// </summary>
        public long? DisplayId { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }


        /// <summary>
        /// 表示用
        /// </summary>
        public string FullName { get; set; }
    }
}

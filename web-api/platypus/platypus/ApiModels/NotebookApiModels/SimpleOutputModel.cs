using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.NotebookApiModels
{
    /// <summary>
    /// ノートブック履歴のうち、コスト最小で取得できる情報だけを保持する
    /// </summary>
    public class SimpleOutputModel : Components.OutputModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="history">ノートブック履歴</param>
        public SimpleOutputModel(NotebookHistory history) : base(history)
        {
            Id = history.Id;
            DisplayId = history.DisplayId;
            Name = history.Name;
            Memo = history.Memo;
            Status = history.GetStatus().ToString();
            FullName = $"{Id}:{Name}";
            Favorite = history.Favorite;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 表示用ID
        /// </summary>
        public long? DisplayId { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// お気に入り
        /// </summary>
        public bool? Favorite { get; set; }

        /// <summary>
        /// 表示用
        /// </summary>
        public string FullName { get; set; }
    }
}

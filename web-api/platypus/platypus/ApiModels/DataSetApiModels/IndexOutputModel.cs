using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.DataSetApiModels
{
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(DataSet dataSet) : base(dataSet)
        {
            Id = dataSet.Id;
            DisplayId = dataSet.DisplayId;
            Name = dataSet.Name;
            Memo = dataSet.Memo;
            IsFlat = dataSet.IsFlat;
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
        /// メモ
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 展開時にデータ種別を無視する
        /// </summary>
        public bool IsFlat { get; set; }
    }
}

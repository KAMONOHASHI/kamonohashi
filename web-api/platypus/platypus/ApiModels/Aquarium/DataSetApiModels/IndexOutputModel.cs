using Nssol.Platypus.Models.TenantModels.Aquarium;

namespace Nssol.Platypus.ApiModels.Aquarium.DataSetApiModels
{
    /// <summary>
    /// アクアリウムデータセットの出力モデル
    /// </summary>
    public class IndexOutputModel : Components.OutputModelBase
    {
        public IndexOutputModel(DataSet dataSet) : base(dataSet)
        {
            Id = dataSet.Id;
            Name = dataSet.Name;
            LatestVersion = dataSet.LatestVersion;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 最新バージョン番号
        /// </summary>
        public long LatestVersion { get; set; }
    }
}

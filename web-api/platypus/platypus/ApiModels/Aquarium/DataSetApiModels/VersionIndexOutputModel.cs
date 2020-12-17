using Nssol.Platypus.Models.TenantModels.Aquarium;

namespace Nssol.Platypus.ApiModels.Aquarium.DataSetApiModels
{
    /// <summary>
    /// アクアリウムデータセットバージョンの出力モデル
    /// </summary>
    public class VersionIndexOutputModel : Components.OutputModelBase
    {
        public VersionIndexOutputModel(DataSetVersion dataSetVesion) : base(dataSetVesion)
        {
            Id = dataSetVesion.Id;
            Version = dataSetVesion.Version;
            DataSetId = dataSetVesion.DataSetId;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// バージョン番号
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// データセットID
        /// </summary>
        public long DataSetId { get; set; }
    }
}

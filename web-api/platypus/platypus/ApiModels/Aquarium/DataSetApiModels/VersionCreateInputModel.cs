using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.Aquarium.DataSetApiModels
{
    /// <summary>
    /// アクアリウムデータセットバージョン作成の入力モデル
    /// </summary>
    public class VersionCreateInputModel
    {
        public class Entry
        {
            /// <summary>
            /// データID
            /// </summary>
            [Required]
            public long Id { get; set; }
        }

        /// <summary>
        /// データエントリ
        /// </summary>
        [Required]
        public List<Entry> Entries { get; set; }
    }
}

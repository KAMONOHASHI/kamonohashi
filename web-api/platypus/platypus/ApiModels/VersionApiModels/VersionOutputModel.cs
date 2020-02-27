using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.VersionApiModels
{
    /// <summary>
    /// バージョン情報出力モデル
    /// </summary>
    public class VersionOutputModel
    {
        /// <summary>
        /// バージョン番号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// メッセージ
        /// </summary>
        public List<string> Messages { get; set; }
    }
}

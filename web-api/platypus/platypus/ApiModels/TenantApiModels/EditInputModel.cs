using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.TenantApiModels
{
    public class EditInputModel
    {
        /// <summary>
        /// テナント表示名
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Git ID。
        /// </summary>
        public long? DefaultGitId { get; set; }

        /// <summary>
        /// Git IDs
        /// </summary>
        [Required]
        public IEnumerable<long> GitIds { get; set; }

        /// <summary>
        /// Registry ID。
        /// </summary>
        public long? DefaultRegistryId { get; set; }

        /// <summary>
        /// Registry IDs
        /// </summary>
        [Required]
        public IEnumerable<long> RegistryIds { get; set; }

        /// <summary>
        /// Storage ID。
        /// </summary>
        [Required]
        public long? StorageId { get; set; }

        /// <summary>
        /// ノートブック無期限利用可否フラグ
        /// </summary>
        public bool AvailableInfiniteTimeNotebook { get; set; }
    }
}

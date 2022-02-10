using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// テナントとGitをマッピングするテーブル
    /// </summary>
    public class TenantGitMap : ModelBase
    {
        /// <summary>
        /// テナントID
        /// </summary>
        [Required]
        public long TenantId { get; set; }


        /// <summary>
        /// Git ID
        /// </summary>
        [Required]
        public long GitId { get; set; }

        /// <summary>
        /// 編集可否
        /// </summary>
        /// <remarks>
        /// 将来的にテナント管理者へGit情報の編集権限を与えられるようにした場合、
        /// 管理者側で編集可能なものと負荷の物を区別するために用いる予定
        /// </remarks>
        public bool IsEditable { get; set; }

        /// <summary>
        /// テナント
        /// </summary>
        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }

        /// <summary>
        /// Git
        /// </summary>
        [ForeignKey(nameof(GitId))]
        public virtual Git Git { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    public class UserTenantGitMap : ModelBase
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// TenantGitMap Id
        /// </summary>
        [Required]
        public long TenantGitMapId { get; set; }

        /// <summary>
        /// API実行用の認証トークン
        /// </summary>
        public string GitToken { get; set; }

        /// <summary>
        /// ユーザ
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// テナント・Gitとの紐づけ情報
        /// </summary>
        [ForeignKey(nameof(TenantGitMapId))]
        public virtual TenantGitMap TenantGitMap { get; set; }

        /// <summary>
        /// レジストリ
        /// </summary>
        public Git Git
        {
            get
            {
                return TenantGitMap?.Git;
            }
        }
    }
}

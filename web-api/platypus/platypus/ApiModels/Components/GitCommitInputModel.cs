using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.Components
{
    public class GitCommitInputModel
    {
        /// <summary>
        /// Git ID。
        /// 未指定の場合はテナントのデフォルトが使用される。
        /// </summary>
        public long? GitId { get; set; }

        /// <summary>
        /// リポジトリ名
        /// </summary>
        [Required]
        public string Repository { get; set; }

        /// <summary>
        /// リポジトリのオーナー名
        /// </summary>
        [Required]
        public string Owner { get; set; }

        /// <summary>
        /// ブランチ名。指定しない場合はmasterになる。
        /// <see cref="CommitId"/>を指定した場合は無視される。
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// コミットID。指定しない場合は<see cref="Branch"/>のHEADコミットになる。
        /// </summary>
        public string CommitId { get; set; }
    }
}

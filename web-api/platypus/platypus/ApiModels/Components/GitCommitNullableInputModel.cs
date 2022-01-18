namespace Nssol.Platypus.ApiModels.Components
{
    /// <summary>
    /// <see cref="GitCommitInputModel"/>からRequiredを消したもの。
    /// </summary>
    public class GitCommitNullableInputModel
    {
        /// <summary>
        /// Git ID
        /// </summary>
        public long? GitId { get; set; }

        /// <summary>
        /// リポジトリ名
        /// </summary>
        public string Repository { get; set; }

        /// <summary>
        /// リポジトリのオーナー名
        /// </summary>
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

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Repository))
            {
                //リポジトリが空だった場合はOK
                return true;
            }
            //リポジトリが指定されているのにオーナーが未指定だったらNG
            return string.IsNullOrEmpty(Owner) == false;
        }
    }
}

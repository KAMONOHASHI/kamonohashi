namespace Nssol.Platypus.ServiceModels.Git.GitHubModels
{
    /// <summary>
    /// ブランチ一覧取得（/repos/:owner/:repository/branches）の結果モデル
    /// </summary>
    public class GetBranchModel
    {
        public class Commit
        {
            public string sha { get; set; }
            public string url { get; set; }
        }

        public string name { get; set; }
        public Commit commit { get; set; }
    }
}
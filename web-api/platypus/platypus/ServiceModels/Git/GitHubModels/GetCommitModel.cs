using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.Git.GitHubModels
{
    /// <summary>
    /// コミット一覧取得（/repos/:owner/:repository/commits）の結果モデル
    /// </summary>
    public class GetCommitModel
    {
        public class Author
        {
            public string name { get; set; }
            public string email { get; set; }
            public DateTime date { get; set; }
        }

        public class Committer
        {
            public string name { get; set; }
            public string email { get; set; }
            public DateTime date { get; set; }
        }

        public class Tree
        {
            public string sha { get; set; }
            public string url { get; set; }
        }

        public class Verification
        {
            public bool verified { get; set; }
            public string reason { get; set; }
            public string signature { get; set; }
            public string payload { get; set; }
        }

        public class Commit
        {
            public Author author { get; set; }
            public Committer committer { get; set; }
            public string message { get; set; }
            public Tree tree { get; set; }
            public string url { get; set; }
            public int comment_count { get; set; }
            public Verification verification { get; set; }
        }

        public class Author2
        {
            public string login { get; set; }
            public int id { get; set; }
            public string avatar_url { get; set; }
            public string gravatar_id { get; set; }
            public string url { get; set; }
            public string html_url { get; set; }
            public string followers_url { get; set; }
            public string following_url { get; set; }
            public string gists_url { get; set; }
            public string starred_url { get; set; }
            public string subscriptions_url { get; set; }
            public string organizations_url { get; set; }
            public string repos_url { get; set; }
            public string events_url { get; set; }
            public string received_events_url { get; set; }
            public string type { get; set; }
            public bool site_admin { get; set; }
        }

        public class Committer2
        {
            public string login { get; set; }
            public int id { get; set; }
            public string avatar_url { get; set; }
            public string gravatar_id { get; set; }
            public string url { get; set; }
            public string html_url { get; set; }
            public string followers_url { get; set; }
            public string following_url { get; set; }
            public string gists_url { get; set; }
            public string starred_url { get; set; }
            public string subscriptions_url { get; set; }
            public string organizations_url { get; set; }
            public string repos_url { get; set; }
            public string events_url { get; set; }
            public string received_events_url { get; set; }
            public string type { get; set; }
            public bool site_admin { get; set; }
        }

        public string sha { get; set; }
        public Commit commit { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string comments_url { get; set; }
        public Author2 author { get; set; }
        public Committer2 committer { get; set; }
        public List<object> parents { get; set; }
    }
}
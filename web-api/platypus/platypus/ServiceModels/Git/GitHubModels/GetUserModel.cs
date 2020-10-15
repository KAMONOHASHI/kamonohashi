using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.Git.GitHubModels
{
    /// <summary>
    /// ユーザー情報取得（/user）の結果モデル
    /// </summary>
    public class GetUserModel
    {

        public string login { get; set; }

    }
}
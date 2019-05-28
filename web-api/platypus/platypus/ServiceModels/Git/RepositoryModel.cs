using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.Git
{
    /// <summary>
    /// リポジトリを表すクラス
    /// </summary>
    public class RepositoryModel
    {
        /// <summary>
        /// リポジトリのオーナー。
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// リポジトリ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// リポジトリの表示名
        /// </summary>
        public string FullName { get; set; }
    }
}

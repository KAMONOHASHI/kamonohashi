using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    public interface IGitRepository : IRepository<Git>
    {
        /// <summary>
        /// 全Git情報を取得する。
        /// </summary>
        IEnumerable<Git> GetGitAll();

        /// <summary>
        /// 指定したテナントに紐づく全Gitを取得する
        /// </summary>
        IEnumerable<Git> GetGitAll(long tenantId);

        /// <summary>
        /// テナントにGitを紐づける。
        /// 既に紐づけされていたらなにもせずfalseを返す。
        /// </summary>
        Task<IEnumerable<UserTenantGitMap>> AttachGitToTenantAsync(Tenant tenant, Git git, bool isCreate);

        /// <summary>
        /// テナントのGit紐づけを解除する
        /// </summary>
        void DetachGitFromTenant(Tenant tenant, Git git);

        /// <summary>
        /// 指定したテナント・ユーザに対応するマッピング情報を取得する
        /// </summary>
        IEnumerable<UserTenantGitMap> GetUserTenantGitMapAll(long tenantId, long userId);

        /// <summary>
        /// 指定したテナント・ユーザ・Gitに対応するマッピング情報を取得する
        /// </summary>
        UserTenantGitMap GetUserTenantGitMap(long userId, long tenantId, long gitId);


        /// <summary>
        /// 指定したGitを使用しているテナントを最大一件返す。
        /// </summary>
        Tenant GetTenant(long gitId);
    }
}

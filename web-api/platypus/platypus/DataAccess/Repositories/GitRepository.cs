using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories
{
    public class GitRepository : RepositoryBase<Git>, IGitRepository
    {
        private ILogger<GitRepository> logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GitRepository(CommonDbContext context,
            ILogger<GitRepository> logger) : base(context)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 全Git情報を取得する。
        /// </summary>
        public IEnumerable<Git> GetGitAll()
        {
            return base.GetAllWithOrderby(r => r.Id, true).ToList();
        }

        /// <summary>
        /// 指定したテナントに紐づく全Gitを取得する
        /// </summary>
        public IEnumerable<Git> GetGitAll(long tenantId)
        {
            return GetModelAll<TenantGitMap>().Include(map => map.Git).Where(map => map.TenantId == tenantId).Select(map => map.Git);
        }

        /// <summary>
        /// テナントにGitを紐づける。
        /// 結果として、作成したすべての<see cref="UserTenantGitMap"/>を返す。
        /// テナントの新規作成時であれば、<paramref name="isCreate"/>をTrueにする。
        /// </summary>
        public async Task<IEnumerable<UserTenantGitMap>> AttachGitToTenantAsync(Tenant tenant, Git git, bool isCreate)
        {
            if (git == null)
            {
                //指定がなければ何もしない
                return null;
            }

            TenantGitMap map = new TenantGitMap()
            {
                Git = git
            };

            List<UserTenantGitMap> maps = null;
            if (isCreate == false) //テナント新規作成時はIDが0の状態なので、判定しない。ユーザも未参加なので何もしない。
            {
                map.TenantId = tenant.Id;

                //既に紐づいていたら何もしない
                bool exist = await ExistsModelAsync<TenantGitMap>(m => m.TenantId == tenant.Id && m.GitId == git.Id);
                if (exist)
                {
                    return null;
                }

                maps = new List<UserTenantGitMap>();

                //テナントに紐づいているすべてのユーザを取得
                var userMaps = FindModelAll<UserTenantMap>(m => m.TenantId == tenant.Id).ToList();
                foreach (var userMap in userMaps)
                {
                    UserTenantGitMap utgMap = new UserTenantGitMap()
                    {
                        TenantGitMap = map,
                        UserId = userMap.UserId,
                    };

                    //認証情報が空欄のものをはじく
                    var existMap = GetModelAll<UserTenantGitMap>().Include(m => m.TenantGitMap)
                        .Where(m => m.UserId == userMap.UserId && m.TenantGitMap.GitId == git.Id && !String.IsNullOrEmpty(m.GitToken)).FirstOrDefault();
                    if (existMap != null && !String.IsNullOrEmpty(existMap.GitToken))
                    {
                        utgMap.GitToken = existMap.GitToken;
                    }
                    AddModel<UserTenantGitMap>(utgMap);
                    maps.Add(utgMap);
                }
            }
            else
            {
                map.Tenant = tenant;
            }

            AddModel<TenantGitMap>(map);
            return maps;
        }

        /// <summary>
        /// テナントのGit紐づけを解除する
        /// </summary>
        public void DetachGitFromTenant(Tenant tenant, Git git)
        {
            //UserTenantGitMapはcascade deleteされるはずなので、無視。
            DeleteModelAll<TenantGitMap>(map => map.TenantId == tenant.Id && map.GitId == git.Id);
        }

        /// <summary>
        /// 指定したテナント・ユーザに対応するマッピング情報を取得する
        /// </summary>
        public IEnumerable<UserTenantGitMap> GetUserTenantGitMapAll(long tenantId, long userId)
        {
            return GetModelAll<UserTenantGitMap>().Include(map => map.TenantGitMap).ThenInclude(map => map.Git)
                .Where(map => map.UserId == userId && map.TenantGitMap.TenantId == tenantId)
                .OrderBy(map => map.TenantGitMap.GitId);
        }

        /// <summary>
        /// 指定したテナント・ユーザ・Gitに対応するマッピング情報を取得する
        /// </summary>
        public UserTenantGitMap GetUserTenantGitMap(long userId, long tenantId, long gitId)
        {
            return GetModelAll<UserTenantGitMap>().Include(map => map.TenantGitMap).ThenInclude(map => map.Git).FirstOrDefault(map =>
                map.UserId == userId &&
                map.TenantGitMap.TenantId == tenantId &&
                map.TenantGitMap.GitId == gitId);
        }

        /// <summary>
        /// 指定したGitを使用しているテナントを最大一件返す。
        /// </summary>
        public Tenant GetTenant(long gitId)
        {
            return FindModelAll<TenantGitMap>(map => map.GitId == gitId).Include(map => map.Tenant).Select(map => map.Tenant).FirstOrDefault();
        }
    }
}

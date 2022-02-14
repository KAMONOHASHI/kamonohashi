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
    public class RegistryRepository : RepositoryBase<Registry>, IRegistryRepository
    {
        private ILogger<RegistryRepository> logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RegistryRepository(CommonDbContext context,
            ILogger<RegistryRepository> logger) : base(context)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 全レジストリ情報を取得する。
        /// </summary>
        public IEnumerable<Registry> GetRegistryAll()
        {
            return base.GetAllWithOrderby(r => r.Id, true).ToList();
        }

        /// <summary>
        /// 指定したテナントに紐づく全レジストリを取得する
        /// </summary>
        public IEnumerable<Registry> GetRegistryAll(long tenantId)
        {
            return GetModelAll<TenantRegistryMap>().Include(map => map.Registry).Where(map => map.TenantId == tenantId).Select(map => map.Registry);
        }

        /// <summary>
        /// テナントにレジストリを紐づける。
        /// 結果として、作成したすべての<see cref="UserTenantRegistryMap"/>を返す。
        /// テナントの新規作成時であれば、<paramref name="isCreate"/>をTrueにする。
        /// </summary>
        public async Task<IEnumerable<UserTenantRegistryMap>> AttachRegistryToTenantAsync(Tenant tenant, Registry registry, bool isCreate)
        {
            if (registry == null)
            {
                //指定がなければ何もしない
                return null;
            }

            TenantRegistryMap map = new TenantRegistryMap()
            {
                Registry = registry
            };

            List<UserTenantRegistryMap> maps = null;
            if (isCreate == false) //テナント新規作成時はIDが0の状態なので、判定しない。ユーザも未参加なので何もしない。
            {
                map.TenantId = tenant.Id;

                //既に紐づいていたら何もしない
                bool exist = await ExistsModelAsync<TenantRegistryMap>(m => m.TenantId == tenant.Id && m.RegistryId == registry.Id);
                if (exist)
                {
                    return null;
                }

                maps = new List<UserTenantRegistryMap>();

                //テナントに紐づいているすべてのユーザを取得
                var userMaps = FindModelAll<UserTenantMap>(m => m.TenantId == tenant.Id).ToList();
                foreach (var userMap in userMaps)
                {
                    UserTenantRegistryMap utrMap = new UserTenantRegistryMap()
                    {
                        TenantRegistryMap = map,
                        UserId = userMap.UserId,
                        IsOrigin = userMap.IsOrigin,
                        UserGroupTenantMapIds = userMap.UserGroupTenantMapIds
                    };

                    //認証情報が空欄のものをはじく
                    var existMap = GetModelAll<UserTenantRegistryMap>().Include(m => m.TenantRegistryMap)
                        .Where(m => m.UserId == userMap.UserId && m.TenantRegistryMap.RegistryId == registry.Id && !String.IsNullOrEmpty(m.RegistryPassword)).FirstOrDefault();
                    if (existMap != null && !String.IsNullOrEmpty(existMap.RegistryPassword))
                    {
                        utrMap.RegistryUserName = existMap.RegistryUserName;
                        utrMap.RegistryPassword = existMap.RegistryPassword;
                    }
                    else
                    {
                        utrMap.RegistryUserName = registry.ProjectName;
                    }
                    AddModel<UserTenantRegistryMap>(utrMap);
                    maps.Add(utrMap);
                }
            }
            else
            {
                map.Tenant = tenant;
            }

            AddModel<TenantRegistryMap>(map);
            return maps;
        }

        /// <summary>
        /// テナントのレジストリ紐づけを解除する
        /// </summary>
        public void DetachRegistryFromTenant(Tenant tenant, Registry registry)
        {
            //UserTenantRegistryMapはcascade deleteされるはずなので、無視。
            DeleteModelAll<TenantRegistryMap>(map => map.TenantId == tenant.Id && map.RegistryId == registry.Id);
        }

        /// <summary>
        /// 指定したテナント・ユーザに対応するマッピング情報を取得する
        /// </summary>
        public IEnumerable<UserTenantRegistryMap> GetUserTenantRegistryMapAll(long tenantId, long userId)
        {
            return GetModelAll<UserTenantRegistryMap>().Include(map => map.TenantRegistryMap).ThenInclude(map => map.Registry)
                .Where(map => map.UserId == userId && map.TenantRegistryMap.TenantId == tenantId)
                .OrderBy(map => map.TenantRegistryMap.RegistryId);
        }

        /// <summary>
        /// 指定したテナント・ユーザ・レジストリに対応するマッピング情報を取得する
        /// </summary>
        public UserTenantRegistryMap GetUserTenantRegistryMap(long userId, long tenantId, long registryId)
        {
            return GetModelAll<UserTenantRegistryMap>().Include(map => map.TenantRegistryMap).ThenInclude(map => map.Registry).FirstOrDefault(map =>
                map.UserId == userId &&
                map.TenantRegistryMap.TenantId == tenantId &&
                map.TenantRegistryMap.RegistryId == registryId);
        }

        /// <summary>
        /// 指定したレジストリを使用しているテナントを最大一件返す。
        /// </summary>
        public Tenant GetTenant(long registryId)
        {
            return FindModelAll<TenantRegistryMap>(map => map.RegistryId == registryId).Include(map => map.Tenant).Select(map => map.Tenant).FirstOrDefault();
        }

        /// <summary>
        /// ユーザ・テナント・レジストリに対応する全てのマッピング情報を取得する
        /// </summary>
        public IEnumerable<UserTenantRegistryMap> GetUserTenantRegistryMapAll()
        {
            return GetModelAll<UserTenantRegistryMap>().Include(map => map.TenantRegistryMap).ThenInclude(map => map.Registry)
                .OrderBy(map => map.Id);
        }
    }
}

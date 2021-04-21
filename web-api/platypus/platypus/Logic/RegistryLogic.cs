using Microsoft.AspNetCore.Http;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using Nssol.Platypus.Services;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// レジストリ管理を行うロジック
    /// </summary>
    public class RegistryLogic : PlatypusLogicBase, IRegistryLogic
    {
        // For DI
        private readonly ITenantRepository tenantRepository;
        private readonly IRegistryRepository registryRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RegistryLogic(
            ITenantRepository tenantRepository,
            IRegistryRepository registryRepository,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.tenantRepository = tenantRepository;
            this.registryRepository = registryRepository;
        }

        /// <summary>
        /// クラスタ管理サービスに登録するdockercfgを作る。
        /// エラーが発生したらnullが返る。
        /// <see cref="ServiceModels.ClusterManagementModels.RegistRegistryTokenInputModel.DockerCfgAuthString"/>に格納される。
        /// </summary>
        public string GetDockerCfgAuthString(UserTenantRegistryMap userRegistryMap)
        {
            Registry registry = userRegistryMap.Registry;
            IRegistryService registryService = GetRegistryService(registry);
            if (registryService != null)
            {
                return registryService.GetDockerCfgAuthString(userRegistryMap);
            }
            else
            {
                LogError($"Undefined registry: {registry.Name}({registry.ServiceType})");
                return null;
            }
        }

        /// <summary>
        /// クラスタ管理サービスに登録するdockercfgを作る。
        /// エラーが発生したらnullが返る。
        /// <see cref="ServiceModels.ClusterManagementModels.RegistRegistryTokenInputModel.DockerCfgAuthString"/>に格納される。
        /// </summary>
        public string GetDockerCfgAuthString(Registry registry, string userName, string password)
        {
            IRegistryService registryService = GetRegistryService(registry);
            if (registryService != null)
            {
                return registryService.GetDockerCfgAuthString(userName, password);
            }
            else
            {
                LogError($"Undefined registry: {registry.Name}({registry.ServiceType})");
                return null;
            }
        }

        /// <summary>
        /// 全てのイメージのリストを取得
        /// </summary>
        /// <returns>全イメージのリスト。エラーの場合はNULL。</returns>
        public async Task<List<string>> GetAllImageListAsync(long registryId)
        {
            UserTenantRegistryMap registryMap = GetCurrentRegistryMap(registryId);
            if (registryMap == null)
            {
                LogError($"registry {registryId} does NOT map to the current user");
                return null;
            }
            Registry registry = registryMap.Registry;
            IRegistryService registryService = GetRegistryService(registry);
            if(registryService != null)
            {
                return await registryService.GetAllImageListAsync(registryMap);
            }
            else
            {
                LogError($"Undefined registry: {registry.Name}({registry.ServiceType})");
                return null;
            }
        }

        /// <summary>
        /// 指定されたイメージのタグを取得
        /// </summary>
        /// <param name="registryId">レジストリID</param>
        /// <param name="imageName">イメージ名</param>
        /// <returns>タグのリスト</returns>
        public async Task<Result<List<string>, string>> GetAllTagListAsync(long registryId, string imageName)
        {
            UserTenantRegistryMap registryMap = GetCurrentRegistryMap(registryId);
            if(registryMap == null)
            {
                return Result<List<string>, string>.CreateErrorResult($"registry {registryId} does NOT map to the current user");
            }
            Registry registry = registryMap.Registry;
            IRegistryService registryService = GetRegistryService(registry);
            if (registry != null)
            {
                string image = imageName.StartsWith("/") ? imageName.Substring(1) : imageName;
                return await registryService.GetAllTagListAsync(registryMap, image);
            }
            else
            {
                return Result<List<string>, string>.CreateErrorResult($"undefined registry: {registry.Name}({registry.ServiceType})");
            }
        }

        /// <summary>
        /// 現在のテナントに紐付くレジストリ情報を取得する
        /// </summary>
        public UserTenantRegistryMap GetCurrentRegistryMap(long registryId)
        {
            long userId = CurrentUserInfo.Id;
            long tenantId = CurrentUserInfo.SelectedTenant.Id;
            return registryRepository.GetUserTenantRegistryMap(userId, tenantId, registryId);
        }

        /// <summary>
        /// 現在のテナントに紐付くレジストリ情報を取得する
        /// </summary>
        /// <returns></returns>
        private IRegistryService GetRegistryService(Registry registry)
        {
            if(registry != null)
            {
                if(registry.ServiceType == RegistryServiceType.DockerHub)
                {
                    return CommonDiLogic.DynamicDi<DockerHubRegistryService>();
                }
                else if (registry.ServiceType == RegistryServiceType.GitLab)
                {
                    return CommonDiLogic.DynamicDi<GitLabRegistryService>();
                }
                else if (registry.ServiceType == RegistryServiceType.PrivateDockerRegistry)
                {
                    return CommonDiLogic.DynamicDi<PrivateDockerRegistryService>();
                }
                else if (registry.ServiceType == RegistryServiceType.NvidiaGPUCloud)
                {
                    return CommonDiLogic.DynamicDi<NvidiaGPUCloudRegistryService>();
                }
            }
            return null;
        }
    }
}

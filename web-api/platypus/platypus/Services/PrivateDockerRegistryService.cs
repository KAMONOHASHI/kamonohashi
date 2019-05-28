using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services
{
    /// <summary>
    /// レジストリAPIを呼び出すサービス
    /// (Private Docker Registry用)
    /// </summary>
    public class PrivateDockerRegistryService : PlatypusServiceBase, IRegistryService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PrivateDockerRegistryService(Logic.Interfaces.ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
        }

        /// <summary>
        /// クラスタ管理サービスに登録するdockercfgを作る。
        /// エラーが発生したらnullが返る。
        /// <see cref="ServiceModels.ClusterManagementModels.RegistRegistryTokenInputModel.DockerCfgAuthString"/>に格納される。
        /// </summary>
        public string GetDockerCfgAuthString(UserTenantRegistryMap userRegistryMap)
        {
            string userName = userRegistryMap.RegistryUserName;
            if (string.IsNullOrEmpty(userName))
            {
                LogWarning($"User {userRegistryMap.UserId}'s userName is empty. UserTenantRegistryMapID = {userRegistryMap.Id}");
                return null;
            }
            string password = userRegistryMap.RegistryPassword;
            if (string.IsNullOrEmpty(password))
            {
                LogWarning($"User {userRegistryMap.UserId}'s password is empty. UserTenantRegistryMapID = {userRegistryMap.Id}");
                return null;
            }
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
            return $"\"username\":\"{userName}\",\"password\":\"{password}\",\"auth\":\"{auth}\"";
        }

        /// <summary>
        /// 全てのイメージのリストを取得
        /// </summary>
        /// <param name="userRegistryMap">ユーザとレジストリのマッピング情報</param>
        /// <returns>全イメージのリスト。エラーの場合はNULL。</returns>
        public async Task<List<string>> GetAllImageListAsync(UserTenantRegistryMap userRegistryMap)
        {
            // API呼び出しパラメータ作成
            RequestParam param = new RequestParam()
            {
                BaseUrl = userRegistryMap.Registry.ApiUrl,
                ApiPath = $"/v2/_catalog",
                UserName = userRegistryMap.RegistryUserName,
                Password = userRegistryMap.RegistryPassword
            };
            // API 呼び出し
            var response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var images = ConvertResult<GetImagesApiModel>(response).Repositories;
                return images;
            }
            else
            {
                LogError("イメージ一覧の取得に失敗：" + response.Error);
                return null;
            }
        }

        /// <summary>
        /// 指定されたイメージのタグを取得
        /// </summary>
        /// <param name="userRegistryMap">ユーザとレジストリのマッピング情報</param>
        /// <param name="imageName">イメージ名</param>
        /// <returns>タグのリスト</returns>
        public async Task<Result<List<string>, string>> GetAllTagListAsync(UserTenantRegistryMap userRegistryMap, string imageName)
        {
            // API呼び出しパラメータ作成
            RequestParam param = new RequestParam()
            {
                BaseUrl = userRegistryMap.Registry.ApiUrl,
                ApiPath = $"/v2/{imageName}/tags/list",
                UserName = userRegistryMap.RegistryUserName,
                Password = userRegistryMap.RegistryPassword
            };

            // API 呼び出し
            var response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var images = ConvertResult<GetTagsApiModel>(response).Tags;
                return Result<List<string>, string>.CreateResult(images);
            }
            else
            {
                return Result<List<string>, string>.CreateErrorResult(response.Error);
            }
        }

        private class GetImagesApiModel
        {
            public List<string> Repositories { get; set; }
        }

        private class GetTagsApiModel
        {
            public string Name { get; set; }
            public List<string> Tags { get; set; }
        }
    }
}

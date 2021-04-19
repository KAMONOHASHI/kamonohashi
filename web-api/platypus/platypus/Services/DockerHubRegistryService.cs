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
    /// (DockerHub用)
    /// </summary>
    public class DockerHubRegistryService : PlatypusServiceBase, IRegistryService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DockerHubRegistryService(Logic.Interfaces.ICommonDiLogic commonDiLogic) : base(commonDiLogic)
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
            return GetDockerCfgAuthString(userName, password);
        }

        public string GetDockerCfgAuthString(string userName, string password)
        {
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
            await Task.CompletedTask;

            // DockerHubはAPIを公開していない。
            // そのため必ず空のリストを返す。
            return new List<string>();
        }

        /// <summary>
        /// 指定されたイメージのタグを取得
        /// </summary>
        /// <param name="userRegistryMap">ユーザとレジストリのマッピング情報</param>
        /// <param name="imageName">イメージ名</param>
        /// <returns>タグのリスト</returns>
        public async Task<Result<List<string>, string>> GetAllTagListAsync(UserTenantRegistryMap userRegistryMap, string imageName)
        {
            if (string.IsNullOrEmpty(userRegistryMap.RegistryPassword))
            {
                //DockerHubの場合、ユーザ名・パスワードが必須なので、未入力だったら空を返すs
                return Result<List<string>, string>.CreateResult(new List<string>());
            }

            string token;
            if (string.IsNullOrEmpty(userRegistryMap.RegistryUserName))
            {
                //ユーザ名が未設定の場合、パスワードにトークンが入っているとみなす
                token = userRegistryMap.RegistryPassword;
            }
            else
            {
                //トークンではなく、ユーザ名・パスワードが指定されている場合、まずはOAuthトークンを取得する
                var tokenResult = await GetAuthTokenAsync(userRegistryMap, imageName);

                if (tokenResult.IsSuccess == false)
                {
                    return Result<List<string>, string>.CreateErrorResult(tokenResult.Error);
                }
                token = tokenResult.Value;
            }

            // API呼び出しパラメータ作成
            RequestParam param = new RequestParam()
            {
                BaseUrl = userRegistryMap.Registry.ApiUrl,
                ApiPath = $"/v2/{imageName}/tags/list",
                Token = token
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

        private async Task<Result<string, string>> GetAuthTokenAsync(UserTenantRegistryMap userRegistryMap, string imageName)
        {
            string scope = $"repository:{imageName}:pull";

            // API呼び出しパラメータ作成
            RequestParam param = new RequestParam()
            {
                BaseUrl = "https://auth.docker.io/",
                ApiPath = $"/token",
                UserName =  userRegistryMap.RegistryUserName,
                Password = userRegistryMap.RegistryPassword,
                QueryParams = new Dictionary<string, string>
                {
                    { "service", "registry.docker.io" },
                    { "scope", $"repository:{imageName}:pull" },
                    //{ "offline_token", "true" }
                }
            };

            // API 呼び出し
            var response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var tokenModel = ConvertResult<GetAuthTokenApiModel>(response);
                return Result<string, string>.CreateResult(tokenModel.Token);
            }
            else
            {
                LogError("認証トークンの取得に失敗：" + response.Error);
                return Result<string, string>.CreateErrorResult(response.Error);
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

        private class GetAuthTokenApiModel
        {
            public string Token { get; set; }
            public int ExpiresIn { get; set; }
            public DateTime IssuedAt { get; set; }
        }
    }
}

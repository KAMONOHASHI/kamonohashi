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
    /// GitLabのレジストリAPIを呼び出すサービス
    /// </summary>
    public class GitLabRegistryService : PlatypusServiceBase, IRegistryService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GitLabRegistryService(
            Logic.Interfaces.ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
        }

        /// <summary>
        /// クラスタ管理サービスに登録するdockercfgを作る。
        /// エラーが発生したらnullが返る。
        /// <see cref="ServiceModels.ClusterManagementModels.RegistRegistryTokenInputModel.DockerCfgAuthString"/>に格納される。
        /// </summary>
        public string GetDockerCfgAuthString(UserTenantRegistryMap userRegistryMap)
        {
            //GitLabではtokenはパスワードとして管理されている
            string token = userRegistryMap.RegistryPassword;
            if (string.IsNullOrEmpty(token))
            {
                LogWarning($"User {userRegistryMap.UserId}'s token is empty. UserTenantRegistryMapID = {userRegistryMap.Id}");
                return null;
            }

            //GitLabの場合、トークンを使うならuserNameは任意の文字列（空文字除く）を与えられる。
            //なのでダミー文字列としてkqiを入れる。
            //ユーザ名はシークレット名にも使われるので、ここでは扱わない
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"kqi:{token}"));
            return $"\"auth\":\"{auth}\"";
        }

        /// <summary>
        /// 全てのイメージのリストを取得
        /// </summary>
        /// <remarks>
        /// GitLabでは権限の絡みがあるためか通常のAPIが公開されていない。
        /// https://forum.gitlab.com/t/how-to-get-container-registry-information-using-the-api/10664/2
        /// </remarks>
        /// <param name="userRegistryMap">ユーザとレジストリのマッピング情報</param>
        /// <returns>全イメージのリスト。エラーの場合はNULL。</returns>
        public async Task<List<string>> GetAllImageListAsync(UserTenantRegistryMap userRegistryMap)
        {
            var images = await GetContainerRegistryJsonAsync(userRegistryMap.Registry, userRegistryMap.Registry.ProjectName, userRegistryMap.RegistryPassword);

            if (images != null)
            {
                //イメージのPathにはリポジトリ名も入ってしまっているが、GitLabだとイメージ名が空（/owner/repos:tagの形式）が許されているので、trimせずにそのまま返す
                return images.Select(i => i.Path).ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 全てのイメージのリストを取得
        /// </summary>
        private async Task<IEnumerable<GetImagesApiModel>> GetContainerRegistryJsonAsync(Registry registry, string project, string token)
        {
            // API呼び出しパラメータ作成
            RequestParam param = new RequestParam()
            {
                BaseUrl = registry.ApiUrl,
                ApiPath = $"{project}/container_registry.json",
                Headers = new Dictionary<string, string>()
                {
                    {"Private-Token", token } //GitLabはトークンの形式がBearerではないので、ヘッダに独自で追加
                }
            };
            // API 呼び出し
            var response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var images = ConvertResult<IEnumerable<GetImagesApiModel>>(response);
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
            Registry registry = userRegistryMap.Registry;
            var images = await GetContainerRegistryJsonAsync(registry, userRegistryMap.Registry.ProjectName, userRegistryMap.RegistryPassword);

            if (images == null)
            {
                return Result<List<string>, string>.CreateErrorResult("Failed to get image info.");
            }
            var image = images.FirstOrDefault(i => i.Path == imageName);
            if (image == null)
            {
                return Result<List<string>, string>.CreateErrorResult($"Image {imageName} is not Found.");
            }
            
            // API呼び出しパラメータ作成
            RequestParam param = new RequestParam()
            {
                BaseUrl = registry.ApiUrl,
                ApiPath = image.TagsPath,
                Headers = new Dictionary<string, string>()
                {
                    {"Private-Token", userRegistryMap.RegistryPassword } //GitLabはトークンの形式がBearerではないので、ヘッダに独自で追加
                }
            };

            // API 呼び出し
            var response = await this.SendGetRequestAsync(param);

            if (response.IsSuccess)
            {
                var tags = ConvertResult<IEnumerable<GetTagsApiModel>>(response);

                return Result<List<string>, string>.CreateResult(tags.Select(t => t.Name).ToList());
            }
            else
            {
                return Result<List<string>, string>.CreateErrorResult(response.Error);
            }
        }

        private class GetImagesApiModel
        {
            public int Id { get; set; }
            public string Path { get; set; }
            public string Location { get; set; }
            [JsonProperty("tags_path")]
            public string TagsPath { get; set; }
        }

        private class GetTagsApiModel
        {
            public string Name { get; set; }
            public string Location { get; set; }
            public string Revision { get; set; }
        }
    }
}

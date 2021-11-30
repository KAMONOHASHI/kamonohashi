using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services
{
    /// <summary>
    /// レジストリAPIを呼び出すサービスクラス
    /// (Nvidia GPU Cloud用)
    /// </summary>
    public class NvidiaGPUCloudRegistryService : PlatypusServiceBase, IRegistryService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commonDiLogic">DIロジック</param>
        public NvidiaGPUCloudRegistryService(Logic.Interfaces.ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
        }

        /// <summary>
        /// 全てのイメージのリストを取得
        /// </summary>
        /// <param name="userRegistryMap">ユーザとレジストリのマッピング情報</param>
        /// <returns>全イメージのリスト。エラーの場合はNULL。</returns>
        public async Task<List<string>> GetAllImageListAsync(UserTenantRegistryMap userRegistryMap)
        {
            await Task.CompletedTask;

            // NGCはユーザに手入力で設定させる。
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
            await Task.CompletedTask;

            // NGCはユーザに手入力で設定させる。
            // そのため必ず空のリストを返す。
            return Result<List<string>, string>.CreateResult(new List<string>());
        }

        /// <summary>
        /// クラスタ管理サービスに登録するdockercfgを作る。
        /// エラーが発生したらnullが返る。
        /// <see cref="ServiceModels.ClusterManagementModels.RegistRegistryTokenInputModel.DockerCfgAuthString"/>に格納される。
        /// </summary>
        public string GetDockerCfgAuthString(UserTenantRegistryMap userRegistryMap)
        {
            //ユーザ名
            string userName = "$oauthtoken";

            //API Key
            string password = userRegistryMap.RegistryPassword;
            if (string.IsNullOrEmpty(password))
            {
                LogWarning($"User {userRegistryMap.UserId}'s password is empty. UserTenantRegistryMapID = {userRegistryMap.Id}");
                return null;
            }
            return GetDockerCfgAuthString(UserName, password);
        }

        public string GetDockerCfgAuthString(string userName, string password)
        {
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
            return $"\"username\":\"{userName}\",\"password\":\"{password}\",\"auth\":\"{auth}\"";
        }
    }
}

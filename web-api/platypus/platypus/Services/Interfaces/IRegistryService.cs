using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services.Interfaces
{
    /// <summary>
    /// レジストリAPIを呼び出すサービス
    /// </summary>
    public interface IRegistryService
    {
        /// <summary>
        /// クラスタ管理サービスに登録するdockercfgを作る。
        /// エラーが発生したらnullが返る。
        /// <see cref="ServiceModels.ClusterManagementModels.RegistRegistryTokenInputModel.DockerCfgAuthString"/>に格納される。
        /// </summary>
        string GetDockerCfgAuthString(UserTenantRegistryMap userRegistryMap);

        /// <summary>
        /// クラスタ管理サービスに登録するdockercfgを作る。
        /// エラーが発生したらnullが返る。
        /// </summary>
        string GetDockerCfgAuthString(string userName, string password);

        /// <summary>
        /// 全てのイメージのリストを取得
        /// </summary>
        /// <param name="userRegistryMap">ユーザとレジストリのマッピング情報</param>
        /// <returns>全イメージのリスト。エラーの場合はNULL。</returns>
        Task<List<string>> GetAllImageListAsync(UserTenantRegistryMap userRegistryMap);

        /// <summary>
        /// 指定されたイメージのタグを取得
        /// </summary>
        /// <param name="userRegistryMap">ユーザとレジストリのマッピング情報</param>
        /// <param name="imageName">イメージ名</param>
        /// <returns>タグのリスト</returns>
        Task<Result<List<string>, string>> GetAllTagListAsync(UserTenantRegistryMap userRegistryMap, string imageName);
    }
}

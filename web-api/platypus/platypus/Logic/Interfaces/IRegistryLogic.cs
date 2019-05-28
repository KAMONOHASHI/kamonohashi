using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// レジストリ管理を行うロジック
    /// </summary>
    public interface IRegistryLogic
    {

        /// <summary>
        /// 現在のテナントに紐付くレジストリ情報を取得する
        /// </summary>
        UserTenantRegistryMap GetCurrentRegistryMap(long registryId);

        /// <summary>
        /// クラスタ管理サービスに登録するdockercfgを作る。
        /// エラーが発生したらnullが返る。
        /// <see cref="ServiceModels.ClusterManagementModels.RegistRegistryTokenInputModel.DockerCfgAuthString"/>に格納される。
        /// </summary>
        string GetDockerCfgAuthString(UserTenantRegistryMap userRegistryMap);

        /// <summary>
        /// 全てのイメージのリストを取得
        /// </summary>
        /// <returns>全イメージのリスト。エラーの場合はNULL。</returns>
        Task<List<string>> GetAllImageListAsync(long registryId);


        /// <summary>
        /// 指定されたイメージのタグを取得
        /// </summary>
        /// <param name="registryId">レジストリID</param>
        /// <param name="imageName">イメージ名</param>
        /// <returns>タグのリスト</returns>
        Task<Result<List<string>, string>> GetAllTagListAsync(long registryId, string imageName);
    }
}

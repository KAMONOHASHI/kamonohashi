using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System.Threading.Tasks;


namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// Inference系の操作で、<see cref="Controllers.spa.InferenceController"/>以外からも使用したい処理をまとめたロジック
    /// </summary>
    public interface IInferenceLogic
    {
        /// <summary>
        /// 推論履歴コンテナを削除し、ステータスを変更する。
        /// </summary>
        /// <param name="inferenceHistory">対象推論履歴</param>
        /// <param name="status">変更後のステータス</param>
        /// <param name="force">他テナントに対する変更を許可するか</param>
        Task ExitAsync(InferenceHistory inferenceHistory, ContainerStatus status, bool force);
    }
}

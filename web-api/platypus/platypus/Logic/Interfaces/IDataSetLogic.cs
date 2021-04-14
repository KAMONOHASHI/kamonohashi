using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// データセットに対する共通処理をまとめたインターフェース
    /// </summary>
    public interface IDataSetLogic
    {
        /// <summary>
        /// データセットのロックを解放する
        /// </summary>
        /// <param name="dataSetId">データセットID</param>
        /// <returns>ロックが解放されたらtrue</returns>
        /// <remarks>返値はロック状態ではない。ロックが解放されたかどうか。</remarks>
        Task<bool> ReleaseLockAsync(long dataSetId);
    }
}

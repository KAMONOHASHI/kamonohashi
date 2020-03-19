namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// バージョン情報に対する共通処理をまとめたインターフェース
    /// </summary>
    public interface IVersionLogic
    {
        /// <summary>
        /// バージョン番号を取得する
        /// </summary>
        /// <returns>バージョン番号</returns>
        string GetVersion();
    }
}

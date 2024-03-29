﻿using Nssol.Platypus.Models;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// リソースモニタロジックインターフェース
    /// </summary>
    public interface IResourceMonitorLogic
    {
        /// <summary>
        /// ジョブ実行履歴を追加する
        /// </summary>
        /// <remarks>コミットは呼び出し側で行う</remarks>
        /// <param name="resourceJob">リソースモニタジョブ履歴</param>
        void AddJobHistory(ResourceJob resourceJob);
    }
}

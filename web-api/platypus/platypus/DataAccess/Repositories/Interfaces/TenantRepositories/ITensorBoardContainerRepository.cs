﻿using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories
{
    /// <summary>
    /// TensorBoardコンテナテーブルにアクセスするためのリポジトリ
    /// </summary>
    public interface ITensorBoardContainerRepository : IRepository<TensorBoardContainer>
    {
        /// <summary>
        /// テナント横断で全データ（外部参照を含む）をすべて取得する。
        /// ソートはIDの逆順。
        /// </summary>
        Task<IEnumerable<TensorBoardContainer>> GetAllIncludePortAndTenantAsync();

        /// <summary>
        /// 検索条件に合致するデータを一件取得します。
        /// </summary>
        TensorBoardContainer Find(Expression<Func<TensorBoardContainer, bool>> where, bool force);

        /// <summary>
        /// 現在のテナント内で、学習履歴IDが等しく、Disable状態じゃないコンテナを取得します。
        /// </summary>
        TensorBoardContainer GetAvailableContainer(long trainingHistoryId);
        
        /// <summary>
        /// 指定したIDのTensorBoardコンテナのステータスを更新する。
        /// </summary>
        bool UpdateStatus(long id, string status, bool force);

        /// <summary>
        /// 既存エントリを削除
        /// </summary>
        void Delete(TensorBoardContainer entity, bool force);
    }
}

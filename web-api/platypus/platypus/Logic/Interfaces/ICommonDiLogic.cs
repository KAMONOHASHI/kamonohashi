using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    public interface ICommonDiLogic
    {

        /// <summary>
        /// ロジック層/サービス層/Feature層のクラスを動的にDIするための処理。
        /// コントローラ場合は <see cref="Microsoft.AspNetCore.Mvc.FromServicesAttribute"/> を使用すること。
        /// </summary>
        T DynamicDi<T>();

        /// <summary>
        /// ロガーを動的にDIするための処理
        /// </summary>
        Microsoft.Extensions.Logging.ILogger DynamicLoggerDi(object sender);

        /// <summary>
        /// <see cref="IUnitOfWork"/>を動的にDIするための処理
        /// </summary>
        IUnitOfWork DynamicUnitOfWorkDi();

        /// <summary>
        /// <see cref="IMultiTenancyLogic"/>を動的にDIするための処理
        /// </summary>
        IMultiTenancyLogic DynamicMultiTenancyLogicDi();

        /// <summary>
        /// テナント用テーブルの初期化。
        /// Commitはしない。
        /// </summary>
        void InitializeTenant(Tenant tenant);
    }
}

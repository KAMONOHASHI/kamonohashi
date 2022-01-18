using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.Logic
{
    public class CommonDiLogic : ICommonDiLogic
    {
        /// <summary>
        /// HttpContextやDI用のServiceProviderを使用するためのアクセサ
        /// </summary>
        private readonly IHttpContextAccessor accessor;
        /// <summary>
        /// <see cref="ILogger"/>を動的にDIするための Factory
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonDiLogic(
            IHttpContextAccessor accessor,
            ILoggerFactory loggerFactory)
        {
            this.accessor = accessor;
            this.loggerFactory = loggerFactory;
        }

        /// <summary>
        /// ロジック層クラスを動的にDIするための処理
        /// </summary>
        public T DynamicDi<T>()
        {
            // Controller を経由せずに DI されると accessor.HttpContext が null になる。よって default() で返却。
            if (accessor.HttpContext == null)
            {
                return default(T);
            }
            var provider = accessor.HttpContext.RequestServices;
            T instance = (T)provider.GetService(typeof(T));
            return instance;
        }

        /// <summary>
        /// ロガーを動的にDIするための処理
        /// </summary>
        public ILogger DynamicLoggerDi(object sender)
        {
            // loggerFactory 経由でロガーを生成して返却
            return loggerFactory.CreateLogger(sender.GetType());
        }

        /// <summary>
        /// <see cref="IUnitOfWork"/>を動的にDIするための処理
        /// </summary>
        public IUnitOfWork DynamicUnitOfWorkDi()
        {
            // 現状、IUnitOfWork は accessor 経由で生成して返却する。
            // Controller を経由せずに DI された時に、この IUnitOfWork を利用するなら、
            // コンストラクタの引数で実態を受け取るようソースを修正すること。
            return (IUnitOfWork)accessor.HttpContext.RequestServices.GetService(typeof(IUnitOfWork));
        }

        /// <summary>
        /// <see cref="IMultiTenancyLogic"/>を動的にDIするための処理。
        /// Controller を経由せずに DI された時は null を返却する。
        /// </summary>
        public IMultiTenancyLogic DynamicMultiTenancyLogicDi()
        {
            return DynamicDi<IMultiTenancyLogic>();
        }

        #region DB初期化

        /// <summary>
        /// テナント用テーブルの初期化。
        /// Commitはしない。
        /// </summary>
        public void InitializeTenant(Tenant tenant)
        {
            Seed seed = (Seed)accessor.HttpContext.RequestServices.GetService(typeof(Seed));
            seed.InitTenant(tenant);
        }
        #endregion
    }
}

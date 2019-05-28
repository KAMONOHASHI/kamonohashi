using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.Infrastructure;
using System;
using System.Linq;
using System.Security.Claims;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Nssol.Platypus.Logic.Interfaces;
using System.Collections.Generic;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// ロジッククラスに汎用的な処理を追加するための共通親クラス。
    /// メンバは基本的にはprotected。
    /// </summary>
    public class PlatypusLogicBase
    {
        /// <summary>
        /// ログ出力を行うためのロガーインスタンス。
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// ログイン中のユーザ情報
        /// </summary>
        protected UserInfo CurrentUserInfo
        {
            get
            {
                // Controller を経由せずに DI されると multiTenancyLogic が null となる。よって CurrentUserInfo も null を返却する。
                return multiTenancyLogic?.CurrentUserInfo;
            }
        }

        protected IEnumerable<Claim> Claims
        {
            get
            {
                // Controller を経由せずに DI されると multiTenancyLogic が null となる。よって Claims も null を返却する。
                return multiTenancyLogic?.Claims;
            }
        }

        /// <summary>
        /// アドホックにDIするためのロジックインスタンス
        /// </summary>
        protected ICommonDiLogic CommonDiLogic { get; private set; }

        private IMultiTenancyLogic multiTenancyLogic;

        /// <summary>
        /// ログイン中のユーザ名
        /// </summary>
        protected string UserName
        {
            get
            {
                return string.IsNullOrEmpty(CurrentUserInfo?.Name) ? "-" : CurrentUserInfo.Name;
            }
        }

        /// <summary>
        /// ログイン中のユーザが所属するテナントの名前
        /// </summary>
        protected string TenantName
        {
            get
            {
                return CurrentUserInfo?.SelectedTenant?.Name;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlatypusLogicBase(ICommonDiLogic commonDiLogic)
        {
            //ロガーのDI
            logger = commonDiLogic.DynamicLoggerDi(this);

            // Controller を経由せずに DI されると commonDiLogic.DynamicMultiTenancyLogicDi() は null を返却するので注意すること。
            this.multiTenancyLogic = commonDiLogic.DynamicMultiTenancyLogicDi();

            this.CommonDiLogic = commonDiLogic;
        }

        #region ログメソッド
        /// <summary>
        /// Errorログを出力する
        /// </summary>
        protected void LogError(string message, Exception e = null)
        {
            if (e == null)
            {
                LogUtil.WriteLLog(logger.LogError, UserName, CurrentUserInfo?.RequestId, message);
            }
            else
            {
                LogUtil.WriteErrorLLog(logger, UserName, CurrentUserInfo?.RequestId, message, e);
            }
        }
        /// <summary>
        /// Warningログを出力する
        /// </summary>
        protected void LogWarning(string message, params object[] args)
        {
            LogUtil.WriteLLog(logger.LogWarning, UserName, CurrentUserInfo?.RequestId, string.Format(message, args));
        }
        /// <summary>
        /// Infomationログを出力する
        /// </summary>
        protected void LogInformation(string message)
        {
            LogUtil.WriteLLog(logger.LogInformation, UserName, CurrentUserInfo?.RequestId, message);
        }
        /// <summary>
        /// Debugログを出力する
        /// </summary>
        protected void LogDebug(string message)
        {
            LogUtil.WriteLLog(logger.LogDebug, UserName, CurrentUserInfo?.RequestId, message);
        }
        #endregion
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Core
{
    /// <summary>
    /// まとめて確定するためのUnitOfWorkの実装クラス
    /// </summary>
    /// <seealso cref="Nssol.Platypus.DataAccess.Core.IUnitOfWork" />
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// ログ出力を行うためのロガーインスタンス。
        /// </summary>
        private ILogger<UnitOfWork> logger;
        
        private CommonDbContext context;

        /// <summary>
        /// ユーザ名
        /// </summary>
        private string userName;
        /// <summary>
        /// トレース用リクエストID
        /// </summary>
        private string requestId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnitOfWork(CommonDbContext context,
            ILogger<UnitOfWork> logger,
            Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
        {
            this.context = context;
            this.logger = logger;

            var identity = accessor.HttpContext.GetClaims();
            if (identity != null)
            {
                userName = identity.Name;
            }
            else
            {
                // Controller を経由せずに DI されると accessor.HttpContext.GetClaims() が null を返却する。よって、ユーザ名を暫定的に設定する。
                userName = "system";
            }
            requestId = accessor.HttpContext?.TraceIdentifier;
        }

        /// <summary>
        /// データベースへコミットします。
        /// </summary>
        /// <param name="user">更新者</param>
        /// <returns>
        /// 影響を受けた行数
        /// </returns>
        public int Commit(string user)
        {
            var result = context.SaveChanges(user);
            if (result > 0)
            {
                LogUtil.WriteLLog(logger.LogInformation, userName, requestId, $"commited transaction. entries = {result}");
            }
            return result;
        }

        /// <summary>
        /// データベースへコミットします。
        /// </summary>
        /// <returns>
        /// 影響を受けた行数
        /// </returns>
        public int Commit()
        {
            return Commit(userName);
        }

        /// <summary>
        /// オブジェクトをクリーンアップします。
        /// </summary>
        /// <remarks>
        /// Disposeパターンの実装
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// DBコンテキストがあればDisposeします。
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }
    }
}

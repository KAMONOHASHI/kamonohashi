using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Filters
{
    /// <summary>
    /// 認可用フィルタ、メソッド単位でアクセス可能なメニューを指定。
    /// 
    /// 指定した場合、該当メニューのアクセス権限のあるユーザのみアクセス可能
    /// 指定しない場合、全ユーザからアクセス可能
    /// </summary>
    /// <example>
    /// <code>
    /// [HttpGet("{id}")]
    /// [PermissionFilter(MenuCode.TrainingHistory)]
    ///  public async Task{IActionResult} GetDetail(long? id)
    ///  { ....
    /// 
    /// [HttpGet("total")]
    /// [PermissionFilter(MenuCode.DataSet, MenuCode.TrainingHistory)]
    /// public IActionResult GetTotal()
    ///  { ....
    ///  
    /// </code>
    /// </example>
    public sealed class PermissionFilterAttribute : TypeFilterAttribute
    {
        public MenuCode[] Menus { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="menus">アクセス許可するメニュー</param>
        public PermissionFilterAttribute(params MenuCode[] menus)
            : base(typeof(PermissionFilterAttributeImpl))
        {
            Menus = menus;
            Arguments = new[] { menus };
        }

        /// <summary>
        /// DI用の内部クラス
        /// </summary>
        private sealed class PermissionFilterAttributeImpl : Attribute, IAsyncActionFilter
        {
            // for di
            private IMenuLogic menuLogic;

            private MenuCode[] menus;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="menuLogic">メニュー操作用ロジック</param>
            /// <param name="menus">メニューコード</param>
            public PermissionFilterAttributeImpl(IMenuLogic menuLogic, MenuCode[] menus)
            {
                this.menuLogic = menuLogic;
                this.menus = menus;
            }

            /// <summary>
            /// アクション実行時に呼ばれる
            /// </summary>
            /// <param name="context">コンテキスト</param>
            /// <param name="next">フィルタ</param>
            /// <returns></returns>
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                // before the action executes
                
                // ログイン済みの場合、権限チェックを実施
                if (context.HttpContext?.User?.Identity?.IsAuthenticated ?? false)
                {
                    if (this.menus != null && this.menus.Length > 0)
                    {
                        bool accessible = await this.menuLogic.IsAccessibleMenuAsync(this.menus);
                        if(!accessible)
                        {
                            // アクセス許可のないメニューにアクセスしようとした場合、403に飛ばす
                            context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                            context.HttpContext.Response.Headers.Add("X-Permit-MenuCode", string.Join(",", menus));
                            return;
                        }
                    }
                }

                await next();

                // after the action executes
            }
        }
    }
}

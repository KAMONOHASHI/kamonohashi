using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// .NET Core および ASP.NET Coreの内部クラスの拡張
    /// </summary>
    public static class NetCoreExtensions
    {
        /// <summary>
        /// HttpContextのクレームを取得する
        /// </summary>
        public static ClaimsIdentity GetClaims(this HttpContext context)
        {
            return context?.User?.Identity as ClaimsIdentity;
        }

        /// <summary>
        /// テナントIDを取得する
        /// </summary>
        public static long GetTenantId(this ClaimsIdentity identity)
        {
            if (identity == null)
            {
                // Controller を経由せずに DI されると identity が null となる。この場合は TanantId を -1 と見做して返却する。
                return -1;
            }
            string idStr = identity.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.GroupSid)?.Value;
            if (long.TryParse(idStr, out long result))
            {
                return result;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// IApplicationBuilder.UserWhen を拡張するメソッド。
        /// 条件によってアプリケーションの実行を制御する。
        /// </summary>
        /// <remarks>
        /// リクエストパイプラインはリニアに処理が進んでいき、実行の条件を定義することはできない。
        /// ミドルウェアを自作すれば条件分岐を入れられるが、できればコストを抑えるために既存のミドルウェアを利用したい。
        /// そこで、既存のミドルウェアを条件付きで利用するために、
        /// 別のパイプラインを実行するためのミドルウェアを作成し、そのパイプライン内で既存のミドルウェアを利用する。
        /// 
        /// </remarks>
        /// <param name="app">アプリケーション</param>
        /// <param name="condition">条件デリゲート</param>
        /// <param name="configuration">条件が合致した際に実行するデリゲート（内部でミドルウェアの登録を行う）</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static IApplicationBuilder UseWhen(IApplicationBuilder app,
            Func<HttpContext, bool> condition,
            Action<IApplicationBuilder> configuration)
        {
            //引数チェック
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            //プロパティを共有した新しい子ApplicationBuilder（≒パイプライン）を作成
            IApplicationBuilder builder = app.New();
            //作成した子ApplicationBuilderにミドルウェアを登録
            configuration(builder);

            //メインの親パイプラインに「条件を満たした場合に新しく作成した子ApplicationBuilderを実行する」ミドルウェアを登録
            return app.Use(next =>
            {
                //ここの処理はStartup.Configureメソッドが終了した後で、一度だけ実行される（リクエスト毎に呼ばれるわけではない）

                //親子でnextを共有させる
                builder.Run(next);
                //子ApplicationBuilderをビルドしてRequestDelegateを作成する
                //nextの解決等を先んじてやっておくことで、パフォーマンスを上げる
                RequestDelegate branch = builder.Build();

                return context =>
                {
                    //この処理はリクエストパイプラインがミドルウェアに到達するたびに実行される

                    //条件を満たすか判定
                    if (condition(context))
                    {
                        //満たしていれば子パイプラインを実行
                        return branch(context);
                    }
                    //満たしていなければ、親パイプラインの次の処理へ移行
                    return next(context);
                };
            });
        }
    }
}
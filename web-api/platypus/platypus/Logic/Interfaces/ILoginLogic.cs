using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.LogicModels;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// ログインロジックのインターフェイス
    /// </summary>
    public interface ILoginLogic
    {
        /// <summary>
        /// 指定された情報で認証・認可を行います。
        /// 認証に成功した場合は、ユーザ名や認可情報を含んだ<see cref="Claim"/>のリストを返します。
        /// 失敗した場合はエラーメッセージを返します。
        /// </summary>
        /// <param name="userName">ユーザ名</param>
        /// <param name="password">パスワード</param>
        /// <param name="tenantId">テナントID。省略時はデフォルトテナント。</param>
        Task<Result<List<Claim>, string>> SignInAsync(string userName, string password, long? tenantId = null);

        /// <summary>
        /// 指定されたテナントでの認可を行い、結果を詰め込んだクレームを返す。
        /// 認証は行わない。
        /// テナントが指定されていない場合にはデフォルトテナントを使う。
        /// </summary>
        /// <param name="userName">アカウント名</param>
        /// <param name="baseClaims">外部サービスから取得した情報の入ったクレーム。このクレームを出力の基にする。Nameはこのメソッドで詰めるので不要。</param>
        /// <param name="tenantId">接続テナントID</param>
        Task<Result<List<Claim>, string>> AuthorizeAsync(string userName, IEnumerable<Claim> baseClaims, long? tenantId = null);

        /// <summary>
        /// 新規に認証用のトークンを生成する。
        /// 現在認証中のユーザのクレーム情報を再利用してトークンに含める。
        /// 認証ユーザのクレーム情報は変更しない。
        /// </summary>
        /// <param name="expiresIn">有効期限（秒）</param>
        JwtToken GenerateToken(int? expiresIn = null);


        /// <summary>
        /// 新規に認証用のトークンを生成する。
        /// 引数で受け取ったクレーム情報を拡張してトークンに含める（＝引数の内容が変わる）ため注意。
        /// 引数のクレームに<see cref="ClaimTypes.Name"/>が含まれていない場合は<see cref="ArgumentException"/>が発生する。
        /// </summary>
        /// <param name="claims">拡張対象のクレーム情報</param>
        /// <param name="expiresIn">有効期限（秒）</param>
        JwtToken GenerateToken(List<Claim> claims, int? expiresIn = null);
    }
}

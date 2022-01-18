using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Novell.Directory.Ldap;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.LogicModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// ログインロジッククラス。
    /// <para>
    /// ADアクセスは以下理由からサービス層には切り出さず、ロジック層内で行う。
    /// <list type="bullet">
    /// <item>認証系をAD以外に変更する予定がない</item>
    /// <item>処理が少ない</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <seealso cref="Nssol.Platypus.Logic.Interfaces.ILoginLogic" />
    public class LoginLogic : PlatypusLogicBase, ILoginLogic
    {
        private readonly IUserRepository userRepository;
        private readonly ISettingRepository settingRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ActiveDirectoryOptions adOptions;
        private readonly WebSecurityOptions webSecurityOptions;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LoginLogic(
            IUserRepository userRepository,
            ISettingRepository settingRepository,
            IUnitOfWork unitOfWork,
            ICommonDiLogic commonDiLogic,
            IOptions<ActiveDirectoryOptions> adOptions,
            IOptions<WebSecurityOptions> webSecurityOptions) : base(commonDiLogic)
        {
            this.userRepository = userRepository;
            this.settingRepository = settingRepository;
            this.unitOfWork = unitOfWork;
            this.adOptions = adOptions.Value;
            this.webSecurityOptions = webSecurityOptions.Value;
        }

        #region サインイン

        /// <summary>
        /// 指定された情報で認証・認可を行います。
        /// 認証に成功した場合は、ユーザ名や認可情報を含んだ<see cref="Claim"/>のリストを返します。
        /// 失敗した場合はエラーメッセージを返します。
        /// </summary>
        /// <param name="userName">ユーザ名</param>
        /// <param name="password">パスワード</param>
        /// <param name="tenantId">テナントID。省略時はデフォルトテナント。</param>
        public async Task<Result<List<Claim>, string>> SignInAsync(string userName, string password, long? tenantId = null)
        {
            var user = userRepository.GetUser(userName);

            List<Claim> claims = null;
            if (user != null && user.ServiceType == AuthServiceType.Local)
            {
                //ローカルアカウントの場合
                if (user.Password != Util.GenerateHash(password, userName))
                {
                    return Result<List<Claim>, string>.CreateErrorResult("User name or password is incorrect.");
                }

                //ローカル認証の場合、UIDやGIDは使えないので、空のクレームを作る
                claims = new List<Claim>();
            }
            else
            {
                //ローカルアカウントがない場合、LDAP認証と判断して、ユーザ自身の権限で、ユーザ情報を取得する
                Result<List<Claim>, string> result = Authenticate(userName, password);
                if (!result.IsSuccess)
                {
                    //ログインに失敗した
                    LogError($"Can't get the user infomation for user {userName}");
                    return Result<List<Claim>, string>.CreateErrorResult(result.Error);
                }

                if (user == null)
                {
                    //ログインに成功したが、ユーザ存在しない（＝LDAPの新規ログイン＝ユーザを作成する）
                    userRepository.AddLdapUser(userName);
                    unitOfWork.Commit(userName);
                }

                claims = result.Value;
            }
            return await AuthorizeAsync(userName, claims, tenantId);
        }

        /// <summary>
        /// 指定されたテナントでの認可を行い、結果を詰め込んだクレームを返す。
        /// 認証は行わない。
        /// テナントが指定されていない場合にはデフォルトテナントを使う。
        /// </summary>
        /// <param name="userName">アカウント名</param>
        /// <param name="baseClaims">外部サービスから取得した情報の入ったクレーム。このクレームを出力の基にする。Nameはこのメソッドで詰めるので不要。</param>
        /// <param name="tenantId">接続テナントID</param>
        public async Task<Result<List<Claim>, string>> AuthorizeAsync(string userName, IEnumerable<Claim> baseClaims = null, long? tenantId = null)
        {
            List<Claim> claims = new List<Claim>();
            if (baseClaims != null)
            {
                claims.AddRange(baseClaims);
            }
            claims.Add(new Claim(ClaimTypes.Name, userName));

            UserInfo userInfo = await userRepository.GetUserInfoAsync(userName);

            if (userInfo?.TenantDic == null || userInfo.TenantDic.Count() == 0)
            {
                //テナントが1つもついていない場合はエラー
                LogError($"User {userName} belongs no tenant");
                string errorMessage = "You belong no tenant. Please contact the administrator.";
                return Result<List<Claim>, string>.CreateErrorResult(errorMessage);
            }

            //テナント名が非Nullだった場合、アクセス権を確認
            if (tenantId.HasValue && userInfo.TenantDic.Keys.FirstOrDefault(t => t.Id == tenantId) == null)
            {
                //指定したテナントに所属していない
                LogError($"User {userName} does NOT have a permission to tenant {tenantId}");
                return Result<List<Claim>, string>.CreateErrorResult($"Invalid tenant ID. Current user does not have a permission to tenant {tenantId}.");
            }

            userInfo.SelectTenant(tenantId);
            //テナントIDをクレームに追加
            claims.Add(new Claim(ClaimTypes.GroupSid, userInfo.SelectedTenant.Id.ToString()));

            return Result<List<Claim>, string>.CreateResult(claims);

        }

        /// <summary>
        /// LDAP認証。併せて、ADから取得可能な情報をクレームに詰めて返す。
        /// </summary>
        /// <remarks>原因に寄らず、認証に失敗したらfalseが返る。システムエラー、ユーザの入力ミスが区別されない</remarks>
        private Result<List<Claim>, string> Authenticate(string userName, string password)
        {
            try
            {
                List<Claim> claims = new List<Claim>();
                using (var conn = new LdapConnection())
                {
                    conn.Connect(adOptions.Server, adOptions.Port);
                    var loginDN = $"{userName}@{adOptions.Domain}";
                    conn.Bind(loginDN, password);

                    string searchFilter = string.Format(adOptions.LdapFilter, userName);
                    var result = conn.Search(
                            this.adOptions.BaseDn,
                            LdapConnection.SCOPE_SUB,
                            searchFilter,
                            Array.Empty<string>(), //特に追加で情報は取らない
                            false
                    );
                    LogDebug($"Login succeeded - {userName}");
                }
                return Result<List<Claim>, string>.CreateResult(claims);
            }
            catch (LdapException e)
            {
                //サーバへ接続失敗したときも、パスワードが間違っていた時もここに到達してしまう
                string errorMessage = "Invalid user name or password.";
                LogError(errorMessage, e);
                return Result<List<Claim>, string>.CreateErrorResult(errorMessage);
            }
        }
        #endregion

        #region トークン再発行

        /// <summary>
        /// 新規に認証用のトークンを生成する。
        /// 現在認証中のユーザのクレーム情報を再利用してトークンに含める。
        /// 認証ユーザのクレーム情報は変更しない。
        /// </summary>
        /// <param name="expiresIn">有効期限（秒）</param>
        public JwtToken GenerateToken(int? expiresIn = null)
        {
            List<Claim> newClaims = new List<Claim>();
            foreach (Claim claim in this.Claims)
            {
                newClaims.Add(new Claim(claim.Type, claim.Value));
            }
            return GenerateToken(newClaims, expiresIn);
        }

        /// <summary>
        /// 新規に認証用のトークンを生成する。
        /// 引数で受け取ったクレーム情報を拡張してトークンに含める（＝引数の内容が変わる）ため注意。
        /// 引数のクレームに<see cref="ClaimTypes.Name"/>が含まれていない場合は<see cref="ArgumentException"/>が発生する。
        /// </summary>
        /// <param name="claims">拡張対象のクレーム情報</param>
        /// <param name="expiresIn">有効期限（秒）</param>
        public JwtToken GenerateToken(List<Claim> claims, int? expiresIn = null)
        {
            string userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName))
            {
                throw new UnauthorizedAccessException("引数にユーザ名が含まれていません。未認証ユーザからのトークンの発行要求が発生しました。");
            }

            var now = DateTime.UtcNow;
            //JWT ID（トークン生成ごとに一意になるようなトークンのID）。ランダムなGUIDを採用する。
            string jwtId = Guid.NewGuid().ToString();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName)); //Subject ユーザ名を指定する
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jwtId));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64)); //Issured At 発行日時

            //期限が切れる時刻
            DateTime expireDate;
            //期限が切れるまでの秒数
            long expireSec;
            if (expiresIn == null)
            {
                //期限が指定されていなかったら、設定ファイルの値を使う
                expireDate = now.AddSeconds(webSecurityOptions.ApiJwtExpirationSec);
                expireSec = webSecurityOptions.ApiJwtExpirationSec;
            }
            else
            {
                TimeSpan expiredSpan = TimeSpan.FromSeconds(expiresIn.Value);
                expireDate = now + expiredSpan;
                expireSec = (long)expiredSpan.TotalSeconds;
            }

            var key = settingRepository.GetApiJwtSigningKey();

            // Json Web Tokenを生成
            var jwt = new JwtSecurityToken(
                webSecurityOptions.ApiJwtIssuer, //発行者(iss)
                webSecurityOptions.ApiJwtAudience, //トークンの受け取り手（のリスト）
                claims, //付与するクレーム(sub,jti,iat)
                now, //開始時刻(nbf)（not before = これより早い時間のトークンは処理しちゃいけない）
                expireDate, //期限(exp)
                new SigningCredentials(key, SecurityAlgorithms.HmacSha256) //署名に使うCredential
                );
            //トークンを作成（トークンは上記クレームをBase64エンコードしたものに署名をつけただけ）
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            LogInformation($"{userName} のアクセストークン {jwtId} を発行しました。");

            return new JwtToken()
            {
                AccessToken = encodedJwt,
                ExpiresIn = expireSec,
                Id = jwtId
            };
        }

        /// <summary>
        /// 現在時刻をepochタイムスタンプに変更します。
        /// </summary>
        /// <param name="date">対象の時刻</param>
        /// <returns>Unix epochからの秒</returns>
        private static long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }
        #endregion
    }
}

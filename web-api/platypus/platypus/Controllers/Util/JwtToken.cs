using Microsoft.IdentityModel.Tokens;
using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nssol.Platypus.LogicModels
{
    /// <summary>
    /// JsonWebTokenによるトークン認証で扱うトークンを表すクラス。
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// トークン
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 期限
        /// </summary>
        [JsonProperty(PropertyName = "expires_in")]
        public long ExpiresIn { get; set; }

        /// <summary>
        /// トークンのID。
        /// Jsonには含まれないように属性で制限をかける。
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; }
    }
}

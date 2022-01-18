using System;

namespace Nssol.Platypus.ServiceModels.KubernetesModels
{
    /// <summary>
    /// シークレット取得した時の返り値
    /// </summary>
    public class GetSercretOutputModel
    {
        /// <summary>
        /// トークンをBase64エンコードした結果を返す
        /// </summary>
        public string DecodedToken
        {
            get
            {
                var token_bytes = Convert.FromBase64String(Data.Token);
                var token = System.Text.Encoding.GetEncoding("utf-8").GetString(token_bytes);
                return token;
            }
        }

        public DataModel Data { get; set; }

        public class DataModel
        {
            public string Token { get; set; }
        }
    }
}

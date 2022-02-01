namespace Nssol.Platypus.Infrastructure.Options
{
    /// <summary>
    /// ObjectStorage設定情報をappsettings.jsonからデシリアライズするためのクラス。
    /// </summary>
    public class ObjectStorageOptions
    {

        /// <summary>
        /// 署名付URLで発行する署名の期限（秒）
        /// </summary>
        public int PreSignedUrlExpirationSec { get; set; }
    }
}

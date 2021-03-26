namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// コンテナ情報の出力モデル
    /// </summary>
    public class ContainerImageOutputModel : Components.ContainerImageOutputModel
    {
        /// <summary>
        /// レジストリトークン
        /// </summary>
        public string Token { get; set; }
    }
}

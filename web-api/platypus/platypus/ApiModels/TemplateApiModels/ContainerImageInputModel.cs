namespace Nssol.Platypus.ApiModels.TemplateApiModels
{
    /// <summary>
    /// コンテナ情報の入力モデル
    /// </summary>
    public class ContainerImageInputModel : Components.ContainerImageInputModel
    {
        /// <summary>
        /// レジストリトークン
        /// </summary>
        public string Token { get; set; }
    }
}
